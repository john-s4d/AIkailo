using System;


namespace AIkailo.Executive
{
    public class RandomProvider
    {
        Random _random;

            public RandomProvider()
        {
            _random = new Random(); // TODO: Better random number generation
        }

        public double NextDouble()
        {
            return _random.NextDouble() * 2 - 1; // Returns within range [-1,1]
        }
    }
}
