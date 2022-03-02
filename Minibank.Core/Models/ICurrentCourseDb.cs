namespace Minibank.Core.Models
{
    public interface ICurrentCourseDb
    {
        decimal GetRate(string currency);
    }
}