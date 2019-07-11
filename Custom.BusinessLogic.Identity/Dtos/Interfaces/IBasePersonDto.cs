namespace Custom.BusinessLogic.Identity.Dtos.Interfaces
{
    /// <summary>
    /// Mehdi
    /// </summary>
    public interface IBasePersonDto
    {
        object Id { get; }
        object UserId { get; }
        bool IsDefaultId();
    }
}
