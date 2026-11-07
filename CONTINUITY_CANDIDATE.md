# Experimental source-handoff candidate

This branch is experimental source code. It is not installed on the live server and is not a release.

The candidate isolates slow listeners with bounded per-client output queues and does not evict a listener merely because a different connection has the same IP address and user agent. A replacement source owns a generation-scoped lease: the previous authenticated source is closed, and an old source cannot append bytes or disconnect the replacement's listeners. For matching content types, existing listener sockets are retained for up to 15 seconds across a source interruption; an incompatible content type, explicit server stop, mount removal, or grace expiry closes them.

The test suite uses synthetic bytes over literal loopback sockets. It does not establish real AAC, HE-AAC or Ogg FLAC decoder behavior across restarts; player buffering, proxy timeouts, server restarts, actual production service binaries and long-duration performance remain unqualified. During any source interruption there may be no audio. The 03:00 shared server/proxy outage may still close every socket regardless of this code. Production rollout requires server administrator access, codec and device qualification, deployment and rollback planning, and an independent fallback origin.
