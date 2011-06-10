
using System;
namespace TechnicalServices.Licensing
{
    internal class AgentCountValidationStrategy : ILicenseValidationStrategy
    {
        public AgentCountValidationStrategy(int agentCount)
        {
            _agentCount = agentCount;
        }

        #region ILicenseValidationStrategy Members

        public void ValidateLicense(string licenseData)
        {
            IsLicenseInvalid = (_agentCount > Convert.ToInt32(licenseData));
        }

        public bool IsLicenseInvalid
        {
            get; set;
        }
   
        #endregion

        private readonly int _agentCount;
    }
}
