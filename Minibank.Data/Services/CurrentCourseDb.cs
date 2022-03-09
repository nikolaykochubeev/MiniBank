using System;
using Minibank.Core.Services;

namespace Minibank.Data.Services
{
    public class CurrentCourseDb : ICurrentCourseDb
    {
        private static readonly Random Random = new();

        public decimal GetRate(string currency)
        {
            return currency switch
            {
                "USD" => Random.Next(),
                "EUR" => Random.Next(),
                _ => -1,
            };
        }
    }
}