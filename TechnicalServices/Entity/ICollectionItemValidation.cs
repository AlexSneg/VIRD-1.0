namespace TechnicalServices.Entity
{
    /// <summary>
    /// Интерфейс проверки всех элементов коллекции.
    /// </summary>
    public interface ICollectionItemValidation
    {
        bool ValidateItem(out string errorMessage);
    }
}