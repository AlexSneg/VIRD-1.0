using System.Windows.Forms;

namespace DomainServices.EquipmentManagement.AgentCommon
{
    public interface IAgentManager
    {
        void OpenMessageView();
        void CloseMessageView();
        void WriteLine(string message);

        void SetBackgroudImage(string imageFileName);

        bool Wait(int tick);
        bool Wait();

        IWin32Window MainWindow { get; }
    }
}