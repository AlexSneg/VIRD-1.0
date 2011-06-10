using System.Text;

using Aladdin.HASP;

namespace TechnicalServices.Licensing
{
    /// <summary>
    /// Инкапсулирует объект Hasp. Класс не предназначен для использования вне сборки (internal).
    /// </summary>
    internal class HaspSession
    {
        internal void Open()
        {
            Hasp = new Hasp(HaspFeature.Default); // Используется feature по умолчанию (ID=0), которая всегда есть на ключе.
            HaspStatus status = Hasp.Login(VendorCode);

            if (HaspStatus.StatusOk != status)
            {
                throw new HaspException(status, "Log in failed.");
            }
        }

        internal void Close()
        {
            HaspStatus status = Hasp.Logout();
            Hasp.Dispose();

            if (HaspStatus.StatusOk != status)
            {
                throw new HaspException(status, "Log out failed.");
            }
        }

        internal string ReadFeatures()
        {
            string info = null;

            HaspStatus status = Hasp.GetInfo(Scope, Format, VendorCode, ref info);

            if (HaspStatus.StatusOk != status)
            {
                throw new HaspException(status, "Hasp GetInfo failed");
            }

            return info;
        }

        internal string Read(int offset, int length)
        {
            byte[] data = new byte[length];

            HaspFile file = Hasp.GetFile(HaspFileId.ReadOnly);
            file.FilePos = offset;
            HaspStatus status = file.Read(data, 0, data.Length);

            if (HaspStatus.StatusOk != status)
            {
                throw new HaspException(status, "Hasp Read failed");
            }

            return _encoding.GetString(data);
        }

        private const string VendorCode = "Qk/LvFZ4NYjl2MEFk7QTLSLqHjM1OS0vlG4tTxKm3ovYP8fZoiMC1cFztJ5IEFBtqxLYJl6Rtf7A59nV" +
                                      "j5XSwawlQCKgFXR28MrAoNrWkBBgW/aQUqBOpbycuBE7PeoS0xAYrfMgdBcdhs0Gac6KGisf43G7wq/5" +
                                      "i/BmRPsHPYzyWNW+HkqMsC3E23BlAmScKb0cyCz8yEsvm45fhvmyjOhW2+RfcSDodRAJiB1O9LrWGFNg" +
                                      "MSOLwrITRf45d/VKpFtbp9hp/klnb2dyRXbS2FBDEChmvcSdpElYVRCnd0jTsapiuhDFZmGH4nkZ9fnW" +
                                      "whAn6iJSOjCrzxgcV3xL29ofPSR8mmqmNOykTJKDUA8DcZ2iuq8q5RQjSoHMKXm0Qn0mh1D0Zyv+cTsz" +
                                      "bgBV3rSHcq3mH5djEI4VDhVZSjUDyyP94PYQSHR2dlsLt9tR+T7o0o/BtFUG1+h1UHuq+pRtSXk5AawW" +
                                      "nWE7VwsEUZyFOfp3UruYEWDLDX9uLRCO0LOBJb7emBTJH77KuMaov2VY3aw0yqIAtfYuV2mhLOQjL24b" +
                                      "eKrNvuJKM89WwXySc+cEjA8krxfEAjMj7DVsnHSbKXM+YU3uOnUH76ECqNKNNqKAvS8qTtcnmBloRKl6" +
                                      "194+c1ivLUqKF3JMq7A/7omCsCqewh2sK+jnkRJkG8SrC6IdVXBXU/08FQ2lxhiH5YdJij53al8Ski5s" +
                                      "bZVAw2cDKNGJGRXmGCVP/lYmN5mo/BxwZ/JkwXl91yug6wlU0ph2p3tTAhNwfHCdyZtgVnDzu2F/WcBy" +
                                      "ZEbIUg+4IrbOe2mE0us7di/RLivasSIL3Zsq+37QC/3EH4RWeQq6O3vlW/IRDvOtJW0OkqzjtiwTUNe3" +
                                      "YgtncHpQGotdityEOWAvp+NCaMzrPvuWz3ErNS47SMBy2E2Fi85uHPHmHIQ=";

        private const string Scope =
            "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" +
            "<haspscope/>";

        private const string Format =
            "<haspformat root=\"hasp_info\">" +
            "    <feature>" +
            "       <attribute name=\"id\" />" +
            "       <attribute name=\"disabled\" />" +
            "       <attribute name=\"usable\" />" +
            "    </feature>" +
            "</haspformat>";

        private Hasp Hasp { get; set; }

        private static readonly ASCIIEncoding _encoding = new ASCIIEncoding();
    }
}
