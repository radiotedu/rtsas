#!/usr/bin/env python3
"""Synthetic loopback test: existing AAC/Ogg-FLAC listener across source reconnect."""
import array,json,math,os,pathlib,queue,select,socket,subprocess,sys,tempfile,threading,time,urllib.error,urllib.request

if len(sys.argv)!=2:raise SystemExit('Pass the compiled lab host DLL')
root=pathlib.Path(tempfile.mkdtemp(prefix='rtsas-synthetic-codecs-'))

def run(args):
 p=subprocess.run(args,stdout=subprocess.PIPE,stderr=subprocess.PIPE,timeout=45)
 if p.returncode:raise RuntimeError('Fixture generation failed: '+p.stderr.decode('utf8','replace')[-1200:])

def make_fixture(codec,freq,path):
 args=['ffmpeg','-hide_banner','-loglevel','error','-y','-f','lavfi','-i',f'sine=frequency={freq}:sample_rate=48000:duration=2','-ac','2','-ar','48000']
 args+=['-c:a','aac','-b:a','128k','-f','adts'] if codec=='aac' else ['-c:a','flac','-f','ogg']
 run(args+[str(path)])

def readline_timeout(pipe,seconds):
 q=queue.Queue();threading.Thread(target=lambda:q.put(pipe.readline()),daemon=True).start()
 try:return q.get(timeout=seconds)
 except queue.Empty:raise TimeoutError('Loopback server did not start')

def header(sock):
 result=bytearray()
 while not result.endswith(b'\r\n\r\n'):
  b=sock.recv(1)
  if not b:raise EOFError('Fixture HTTP header ended early')
  result+=b
  if len(result)>8192:raise ValueError('Fixture HTTP header too long')
 if not result.startswith(b'HTTP/1.0 200'):raise ValueError('Fixture source rejected: '+result[:120].decode('latin1'))

def source(port,mount,content):
 sock=socket.create_connection(('127.0.0.1',port),timeout=10);sock.settimeout(10)
 request=f'SOURCE {mount} HTTP/1.0\r\nContent-Type: {content}\r\nice-password: loopback-fixture-only\r\n\r\n'
 sock.sendall(request.encode('latin1'));header(sock);return sock

def stats(port,mount):
 try:
  with urllib.request.urlopen(f'http://127.0.0.1:{port}/status-json.xsl',timeout=2) as r:obj=json.load(r)
  sources=obj['icestats']['source'];sources=[sources] if isinstance(sources,dict) else sources
  return next((x for x in sources if x.get('listenurl','').endswith(mount)),None)
 except (OSError,ValueError,KeyError):return None

def wait_for(predicate,seconds,label):
 deadline=time.monotonic()+seconds
 while time.monotonic()<deadline:
  result=predicate()
  if result:return result
  time.sleep(.05)
 raise TimeoutError(label)

def score(values,freq,offset):
 count=len(values);omega=2*math.pi*freq/48000
 a=sum(v*math.sin(omega*(i+offset)) for i,v in enumerate(values));b=sum(v*math.cos(omega*(i+offset)) for i,v in enumerate(values))
 return math.hypot(a,b)/count

def attempt(port,codec,mode):
 mount='/next-codec-'+codec+'-'+mode;content='application/ogg' if codec=='oggflac' else 'audio/aac'
 if codec=='heaac':
  fixtures=pathlib.Path(__file__).resolve().parent/'fixtures'
  a=fixtures/'synthetic-he-aac-v2-440.aac';b=fixtures/'synthetic-he-aac-v2-880.aac'
 else:
  a=root/(codec+'-'+mode+'-a');b=root/(codec+'-'+mode+'-b');make_fixture(codec,440,a);make_fixture(codec,880,b)
 first=source(port,mount,content);second=None;player=None
 try:
  wait_for(lambda:stats(port,mount),10,'first source did not become online')
  out=root/(codec+'-'+mode+'-listener.pcm');err=root/(codec+'-'+mode+'-listener.err')
  with err.open('wb') as diag:
   player=subprocess.Popen(['ffmpeg','-hide_banner','-loglevel','error','-y','-user_agent','ContinuityCodecLab/1.0','-icy','0','-probesize','32768','-analyzeduration','1000000','-i',f'http://127.0.0.1:{port}{mount}','-ac','1','-ar','48000','-f','s16le',str(out)],stdout=subprocess.DEVNULL,stderr=diag)
   wait_for(lambda:(s if (s:=stats(port,mount)) and s['listeners']==1 else None),10,'listener did not join')
   firstBytes=a.read_bytes();first.sendall(firstBytes)
   wait_for(lambda:(s if (s:=stats(port,mount)) and s['total_bytes']>=len(firstBytes) else None),10,'first source not ingested')
   time.sleep(.3)
   if mode=='gap':
    first.shutdown(socket.SHUT_RDWR);first.close();first=None
    wait_for(lambda:stats(port,mount) is None,10,'old source did not end')
    time.sleep(1)
    if player.poll() is not None:raise RuntimeError(codec+' listener exited during short source gap')
    second=source(port,mount,content);wait_for(lambda:stats(port,mount),10,'replacement not online')
   else:
    second=source(port,mount,content)
    def old_closed():
     readable,_,_=select.select([first],[],[],0)
     if not readable:return False
     try:return first.recv(1,socket.MSG_PEEK)==b''
     except (ConnectionError,OSError):return True
    wait_for(old_closed,10,'superseded source was not closed')
    first.close();first=None;time.sleep(.3)
    wait_for(lambda:stats(port,mount),10,'replacement not online')
   after=stats(port,mount)
   if after['listeners']!=1:raise RuntimeError(codec+' listener was not retained')
   secondBytes=b.read_bytes();second.sendall(secondBytes)
   wait_for(lambda:(s if (s:=stats(port,mount)) and s['total_bytes']>=len(firstBytes)+len(secondBytes) else None),10,'replacement not ingested')
   # Chained Ogg streams can reset timestamps. Measure actual decoded PCM
   # rather than requiring ffmpeg's timestamp-based -t to reach three seconds.
   deadline=time.monotonic()+20;required=int(2.7*48000)*2
   while time.monotonic()<deadline:
    size=out.stat().st_size if out.exists() else 0
    if size>=required:break
    if player.poll() is not None:raise RuntimeError(codec+' decoder exited before enough PCM; bytes='+str(size))
    time.sleep(.1)
   else:
    size=out.stat().st_size if out.exists() else 0
    player.terminate();player.wait(timeout=5)
    raise TimeoutError(codec+' decoder produced only '+str(size)+' PCM bytes after reconnect: '+err.read_text(errors='replace')[-1000:])
   player.terminate()
   try:player.wait(timeout=5)
   except subprocess.TimeoutExpired:player.kill();player.wait(timeout=5)
  pcm=array.array('h');pcm.frombytes(out.read_bytes())
  if sys.byteorder!='little':pcm.byteswap()
  if len(pcm)<int(2.7*48000):raise RuntimeError(codec+' listener produced insufficient decoded audio')
  width=int(.35*48000);start=int(.25*48000);last=len(pcm)-width
  first440=score(pcm[start:start+width],440,start);first880=score(pcm[start:start+width],880,start)
  last440=score(pcm[last:],440,last);last880=score(pcm[last:],880,last)
  if not first440>first880*2 or not last880>last440*2:raise RuntimeError(codec+' listener did not decode both source tones')
  return {'codec':codec,'mode':mode,'sameListenerRetained':True,'decodedSeconds':round(len(pcm)/48000,3),'firstToneHz':440,'secondToneHz':880,'firstScoreRatio':round(first440/max(first880,.001),2),'secondScoreRatio':round(last880/max(last440,.001),2)}
 finally:
  if first is not None:first.close()
  if second is not None:second.close()
  if player is not None and player.poll() is None:
   player.kill();player.wait(timeout=5)

server=subprocess.Popen(['dotnet',sys.argv[1]],stdin=subprocess.PIPE,stdout=subprocess.PIPE,stderr=subprocess.PIPE,text=True)
try:
 line=readline_timeout(server.stdout,30);port=json.loads(line)['port']
 if not 0<port<65536:raise ValueError('Invalid ephemeral port')
 result=[]
 for codec in ('aac','heaac','oggflac'):
  for mode in ('gap','overlap'):
   case=attempt(port,codec,mode);result.append(case);print(json.dumps({'case':case}),flush=True)
 print(json.dumps({'scope':'synthetic real decoders; literal loopback only','cases':result,'heAacTested':True,'proxyTested':False,'serverRestartTested':False}),flush=True)
finally:
 if server.poll() is None:
  try:server.stdin.write('\n');server.stdin.flush();server.wait(timeout=15)
  except (OSError,subprocess.TimeoutExpired):server.kill();server.wait(timeout=5)
