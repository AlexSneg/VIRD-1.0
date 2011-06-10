using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechnicalServices.Entity
{
    [Flags,Serializable] 
    public enum UserError
    {
        NoError = 0, //нет ошибок*
        LoginAlreadyExist = 1,// Так логин уже существует*
        LockedAlready =2,// //Пользователь заблокирован*
        NoRole = 4,// отсутствуют роли*
        NoPassword = 8,// отсутствует пароль*
        NoLogin = 16,// отсутствует логин*
        DeletedAlready = 32,// // пользователь уже уделен*
        FIOAlreadyExist = 64,// // Такое ФИО уже существует*
        NoFIO = 128,// Отсутствует ФИО*
        NoDeleted = 256, // Не удалось удалить*
        UnlockedAlready = 512, //уже разблокирована
        WorkInSystem = 1024, // Пользователь работает в системе*
        NoMoreAdminAfterDelete = 2048, // Хотя бы один Администратор должен остаться после удаления*
        LastUserWithAdminRole = 4096 //последний пользователь с ролью Админ, нельзя снимать роль Админ*
    }
}
