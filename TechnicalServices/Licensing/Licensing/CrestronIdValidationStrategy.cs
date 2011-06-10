
using System;
using System.Globalization;

namespace TechnicalServices.Licensing
{
    internal class CrestronIDValidationStrategy : ILicenseValidationStrategy 
    {
        public CrestronIDValidationStrategy(string hashedId, string randomSeed, Func<string, string> decoder)
        {
            _hashedId = hashedId;
            _randomSeed = randomSeed;
            _decoder = decoder;
        }

        #region ILicenseValidationStrategy Members

        public void ValidateLicense(string licenseData)
        {
            string value = _hashCalculator.CalculateHash(licenseData, _randomSeed);
            if (_decoder != null) value = _decoder(value);
            //IsLicenseInvalid = (_hashedId != value);
            IsLicenseInvalid = String.Compare(_hashedId, value, true, CultureInfo.InvariantCulture) != 0;
        }

        public bool IsLicenseInvalid
        {
           get; set;
        }

        #endregion

        private readonly string _hashedId;
        private readonly string _randomSeed;
        private readonly Func<string, string> _decoder;
        private readonly HashCalculator _hashCalculator = new HashCalculator();
    }
}
