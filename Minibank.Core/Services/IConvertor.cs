namespace Minibank.Core.Services
{
    public interface IConvertor
    {
        decimal Convert(decimal amount, string currency);
    }
}