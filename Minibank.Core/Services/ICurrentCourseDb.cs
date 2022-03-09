namespace Minibank.Core.Services
{
    public interface ICurrentCourseDb
    {
        decimal GetRate(string currency);
    }
}