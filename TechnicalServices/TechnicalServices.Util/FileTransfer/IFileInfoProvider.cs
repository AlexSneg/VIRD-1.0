using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalServices.Entity;

namespace TechnicalServices.Util.FileTransfer
{
    public interface IFileInfoProvider<TProperty> : IFileInfoProvider, IEnumerable<TProperty>
        where TProperty : class 
    {
        string GetResourceId(TProperty property);
    }

    public interface IFileInfoProvider
    {
        string Identity { get; }
        string UniqueName { get; }
        string GetFileName(string fileId);
        void SaveFile(UserIdentity userIdentity, Dictionary<string, string> fileDic);
        int GetNumberOfParts(string resourceId);
        int GetNumberOfParts();
    }
}
