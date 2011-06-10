namespace DomainServices.EquipmentManagement.AgentManagement
{
    public interface IAgentManager
    {
        void OpenMessageView();
        void CloseMessageView();
        void WriteLine(string message);

        bool Wait(int tick);
        bool Wait();
    }
}