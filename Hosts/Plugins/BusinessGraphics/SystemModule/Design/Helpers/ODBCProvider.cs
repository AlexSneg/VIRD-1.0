using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data.Odbc;
using System.IO;

namespace Hosts.Plugins.BusinessGraphics.SystemModule.Design.Helpers
{
    /// <summary>прячет работу с ODBC, читает данные и возвращает xml  </summary>
    internal static class ODBCProvider
    {
        /// <summary>читает данные из базы и возвращает признак успешного/неуспешного чтения</summary>
        internal static bool ReadXml(string connectionString, string storedProcName, out XmlTextReader xmlResult)
        {
            string mess;
            return TestConnection(connectionString, storedProcName, out mess, out xmlResult);
        }

        /// <summary>валидирует коннекшен и хранимку</summary>
        internal static bool TestConnection(string connectionString, string storedProcName, out string errMessage, out XmlTextReader xmlResult)
        {
            errMessage = string.Empty;
            xmlResult = GetEmptyXml();
            try
            {
                using (OdbcConnection conn = OpenConnection(connectionString))
                {
                    try
                    {
                        using (OdbcCommand command = conn.CreateCommand())
                        {
                            command.CommandText = storedProcName;
                            using (OdbcDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    object xmlData = reader.GetValue(0);
                                    xmlResult = new XmlTextReader(new StringReader(xmlData.ToString()));
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        errMessage = "Не удалось получить данные из хранимой процедуры.";
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                errMessage = "Не удалось соединится с источником.";
                return false;
            }

            return true;
        }

        private static OdbcConnection OpenConnection(string connectionString)
        {
            OdbcConnectionStringBuilder csb = new OdbcConnectionStringBuilder();
            csb.ConnectionString = connectionString;
            OdbcConnection conn = new OdbcConnection(csb.ConnectionString);
            conn.Open();
            return conn;
        }

        internal static XmlTextReader GetEmptyXml()
        {
            return new XmlTextReader(new StringReader("<root></root>"));
        }
    }
}
