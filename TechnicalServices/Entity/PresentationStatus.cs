namespace TechnicalServices.Entity
{
    /// <summary>
    /// статус блокировки
    /// </summary>
    public enum PresentationStatus
    {
        /// <summary>
        /// заблокирована презентация для редактирования
        /// </summary>
        LockedForEdit,
        /// <summary>
        /// заблокирована презентация для показа
        /// </summary>
        LockedForShow,
        /// <summary>
        /// заблокированы слайды презентации
        /// </summary>
        SlideLocked,
        /// <summary>
        /// удалена
        /// </summary>
        Deleted,
        /// <summary>
        /// есть и не заблокирована ни презентация ни один из ее слайдов
        /// </summary>
        ExistsAndUnLocked,
        /// <summary>
        /// чето непонятное - какая то фигня
        /// </summary>
        Unknown,

        /// <summary>
        /// сценарий открыт локально
        /// </summary>
        AlreadyLocallyOpened
    }
}