namespace System.Globalization
{
    /// <summary>
    /// Defines the contract for a date time converter from or to UTC and/or local times
    /// </summary>
    public interface IDateTimeConverter
    {
        DateTime Convert(DateTime dt);
    }
}