using System;
using Minibank.Core.Models;

namespace Minibank.Data.Services
{
    public class CurrentCourseDb : ICurrentCourseDb
    {
        private Random rand; 
        public CurrentCourseDb()
        {
            rand = new Random();
        }
        public decimal GetRate(string currency)
        {
            return currency switch
            {
                "USD" => rand.Next(),
                "EUR" => rand.Next(),
                _ => default,
            };
        }
    }
}