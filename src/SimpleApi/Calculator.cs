using System;
using System.Linq;

namespace SimpleApi
{
    public class Calculator
    {
        public int Add(params int[] numbers)
        {
            return numbers.Aggregate(0, (sum, number) => sum + number);
        }
        public int Subtract(params int[] numbers)
        {
            return numbers.Aggregate(0, (sum, number) => sum + number);
        }
    }
}
