using IcecastStreamMonitor.RemoteControl.Forms;

namespace IcecastStreamMonitor.RemoteControl;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new RemoteLoginForm());
    }    
}