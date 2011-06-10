using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.PresentationDesign.DesignCommon;
using TechnicalServices.Common;
using TechnicalServices.Entity;
using TechnicalServices.Interfaces;
using TechnicalServices.Util.FileTransfer;

namespace DomainServices.ImportExportClientManagement.Presentation
{
    internal class ExportPresentationSchemaFilesCommand : Command
    {
        private readonly IPresentationClient _presentationClient;
        private readonly IClientResourceCRUD<FilesGroup> _exportPresentationCRUD;

        public ExportPresentationSchemaFilesCommand(string commandName,
            IPresentationClient presentationClient, string directory)
            : base(commandName)
        {
            _presentationClient = presentationClient;
            _exportPresentationCRUD = _presentationClient.GetPresentationSchemaCrud();
        }

        #region Overrides of Command

        protected override bool OnExecute()
        {
            FilesGroup filesGroup = _presentationClient.GetPresentationSchemaFilesForExport();
            return _exportPresentationCRUD.GetSource(filesGroup, false);
        }

        protected override void OnRollBack()
        {
            if (_presentationClient != null)
                _exportPresentationCRUD.RollBack();
        }

        protected override void OnCommit()
        {
            if (_presentationClient != null)
                _exportPresentationCRUD.Commit();
        }

        #endregion
    }
}
