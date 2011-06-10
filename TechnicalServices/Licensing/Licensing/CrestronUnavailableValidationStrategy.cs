
using System;
namespace TechnicalServices.Licensing
{
    internal class CrestronUnavailableValidationStrategy : ILicenseValidationStrategy
    {
        #region ILicenseValidationStrategy Members
        
        public void ValidateLicense(string licenseData)
        {
            IsLicenseInvalid = (licenseData != String.Empty);
        }

        public bool IsLicenseInvalid
        {
            get; set;
        }

        #endregion
    }
}
