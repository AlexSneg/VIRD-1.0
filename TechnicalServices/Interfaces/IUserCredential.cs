namespace TechnicalServices.Interfaces
{
    public interface IUserCredential
    {
        bool GetUserCredential(out string loginName, out string password);
        void FailedLogin();
        void FailedRole(string role);
    }
}