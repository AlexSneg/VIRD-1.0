using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Interfaces;
using TechnicalServices.Util.FileTransfer;

namespace Domain.PresentationDesign.Client
{
    public static class ClientFileTransferFactory
    {
        public static IResourceCRUD CreateClientFileTransfer(bool isStandAlone, IResourceRemoteCRUD contract, ISourceDAL sourceDAL)
        {
            if (isStandAlone)
                return new ClientSideStandAloneFileTransfer(sourceDAL);
            else
            {
                return new ClientSideFileTransfer(contract);
            }
        }
    }
}
