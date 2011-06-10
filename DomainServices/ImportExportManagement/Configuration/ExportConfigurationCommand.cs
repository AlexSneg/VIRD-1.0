using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;
using TechnicalServices.Util.FileTransfer;
using Command=TechnicalServices.Common.Command;

namespace DomainServices.ImportExportClientManagement.Configuration
{
    internal class ExportConfigurationCommand : Command
    {
        private readonly IClientResourceCRUD<FilesGroup> _clientResourceCRUD;
        private readonly FilesGroup _filesGroup;

        public ExportConfigurationCommand(string commandName, string directory, FilesGroup filesGroup,
                                          IConfigurationTransfer service)
            : base(commandName)
        {
            _filesGroup = filesGroup;
            _clientResourceCRUD = new ClientSideConfigurationTransfer(directory, service);
            _clientResourceCRUD.OnPartTransmit += new EventHandler<PartSendEventArgs>(_clientResourceCRUD_OnPartTransmit);
            _clientResourceCRUD.OnComplete += new EventHandler<OperationStatusEventArgs<FilesGroup>>(_clientResourceCRUD_OnComplete);
        }

        public void Terminate()
        {
            if (!IsCanceled && !IsExecuted)
                _clientResourceCRUD.Terminate();
        }

        event EventHandler<PartSendEventArgs> OnPartTransmit;
        event EventHandler<OperationStatusEventArgs<FilesGroup>> OnComplete;

        protected override bool OnExecute()
        {
            if (!_clientResourceCRUD.GetSource(_filesGroup, false))
                throw new ApplicationException("Неизвестная ошибка при экспорте конфигурации");
            return true;
        }

        protected override void OnRollBack()
        {
            _clientResourceCRUD.RollBack();
        }

        protected override void OnCommit()
        {
            _clientResourceCRUD.Commit();
        }

        void _clientResourceCRUD_OnComplete(object sender, OperationStatusEventArgs<FilesGroup> e)
        {
            if (OnComplete != null)
                OnComplete(sender, e);
        }

        void _clientResourceCRUD_OnPartTransmit(object sender, PartSendEventArgs e)
        {
            if (OnPartTransmit != null)
                OnPartTransmit(sender, e);
        }

    }
}