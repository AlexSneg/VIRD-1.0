namespace TechnicalServices.Licensing
{
    internal interface ILicenseValidationStrategy
    {
        void ValidateLicense(string licenseData);
        bool IsLicenseInvalid { get; set; }
    }
}
