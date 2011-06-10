using System;
using System.Diagnostics;


namespace TechnicalServices.Licensing
{
    public class LicenseChecker
    {
        /// <summary>
        /// Проверяет наличие HASP ключа.
        /// </summary>
        /// <exception cref="HaspException">Исключение возникает в случае, если корректный HASP ключ отсутсвует.</exception>
        [Conditional("Security_Release")]
        public void CheckHasp()
        {
            _session.Open();
            _session.Close();
        }

        /// <summary>
        /// Проверяет наличие активной лицензии на функциональность.
        /// </summary>
        /// <param name="featureId">Идентификатор функциональности (уникальный ID плагина).</param>
        [Conditional("Security_Release")]
        public void CheckFeature(int featureId)
        {
            _session.Open();

            try
            {
                string features = _session.ReadFeatures();
                if (!HaspInfoParser.IsFeatureAvailable(featureId, features))
                {
                    throw new LicenseInvalidException(LicenseInvalidReason.Feature, "Состав плагинов не соответствует конфигурации ключа.");
                }
            }
            finally
            {
                _session.Close();
            }
        }

        /// <summary>
        /// Сопоставляет реальное количество агентов с количеством агентов в лицензии.
        /// </summary>
        /// <param name="agentCount">Реальное количество подключенных агентов (визуализаторов).</param>
        [Conditional("Security_Release")]
        public void CheckAgentCount(int agentCount)
        {
            ILicenseValidationStrategy strategy = new AgentCountValidationStrategy(agentCount);
            _session.Open();
         
            try
            {
                string licenseData = _session.Read(0, AgentCountLength);
                strategy.ValidateLicense(licenseData);
            }
            catch (FormatException ex)
            {
               // Ислючение при конвертации в Int32.
               throw new HaspException("Incorrect agent count data.", ex);
            }
            finally
            {
               _session.Close();
            }

            if (strategy.IsLicenseInvalid)
            {
                throw new LicenseInvalidException(LicenseInvalidReason.AgentCount, "Количество запущенных агентов превышает допустимое значение.");
            }
        }
       
        /// <summary>
        /// Сопоставляет ID подключенного Crestron c данными лицензии.
        /// </summary>
        /// <param name="hashedId">Хэш ID подключенного Crestron.</param>
        /// <param name="randomSeed">Случайная строка, использованная для генерации хэша.</param>
        /// <param name="decoder"></param>
        [Conditional("Security_Release")]
        public void CheckCrestronID(string hashedId, string randomSeed, Func<string, string> decoder)
        {
            CheckCrestronID(new CrestronIDValidationStrategy(hashedId, randomSeed, decoder));
        }

        /// <summary>
        /// Проверяет корректность лицензии при неподключенном Crestron.
        /// </summary>
        [Conditional("Security_Release")]
        public void CheckUnavailableCrestron()
        {
            CheckCrestronID(new CrestronUnavailableValidationStrategy());
        }

        private void CheckCrestronID(ILicenseValidationStrategy strategy)
        {
            _session.Open();

            try
            {
                string licenseData = _session.Read(AgentCountLength, CrestronIdLength).TrimEnd('\0');
                strategy.ValidateLicense(licenseData);
            }
            finally
            {
                _session.Close();
            }

            if (strategy.IsLicenseInvalid)
            {
                throw new LicenseInvalidException(LicenseInvalidReason.CrestronId, "ID Crestron в ключе не соответствует прошивке Crestron.");
            }
        }

        private readonly HaspSession _session = new HaspSession();
        private const int AgentCountLength = 3;
        private const int CrestronIdLength = 16; // 128 bit
    }
}
