namespace Tools.Random
{
    public class SystemRandom
    {
        System.Random systemRandom;

        public SystemRandom(int seed)
        {
            this.systemRandom = new System.Random(seed);
        }

        public double Generate()
        {
            return this.systemRandom.NextDouble();
        }
    }
}
