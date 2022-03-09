namespace Minibank.Core.Services
{
    public interface ICurrencyConverter
    {
        decimal Convert(decimal amount, string currency);
    }
}