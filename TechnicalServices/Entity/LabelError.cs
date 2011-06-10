using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechnicalServices.Entity
{
    [Flags, Serializable]
    public enum LabelError
    {
        NoError = 0, //нет ошибок 
        LabelAlreadyExist = 1,// Такая метка уже существует *
        LockedAlready = 2,// //Метка заблокирована *
        SystemLabel = 4,// Это системная  метка, с которой запрещены какие - либо операции *
        NoName = 8,// отсутствует название метки *
        DeletedAlready = 16,// метка уже уделен *
        NoDeleted = 32, // Не удалось удалить *
        UnlockedAlready = 64 //,//уже разблокирована
        //UsedInPresenatation = 128 // метка используется в презентации
    }
}
