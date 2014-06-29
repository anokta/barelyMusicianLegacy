using System;

namespace BarelyAPI
{
    public static class RandomNumber
    {
        static Random rand = new Random();

        public static float NextFloat(float min = 0.0f, float max = 1.0f)
        {
            return min + (max - min) * (float)(rand.NextDouble());
        }

        public static int NextInt(int min = 0, int max = 2)
        {
            return rand.Next(min, max);
        }
    }
}