namespace genetic_algorithm.gen
{
    public class Individual
    {
        private readonly static int Length = 10; // (3 - (-3)) / 2 ^ 10 < 0.01
        private readonly static int MinX = -3;
        private readonly static int MaxX = 3;
        private readonly static int Range = (int)Math.Pow(2, Length); //1024

        private readonly List<int> chromosomeX;
        private readonly List<int> chromosomeY;
        private readonly double phenotypeX; // [-3,3]
        private readonly double phenotypeY; // [-3,3]
        private readonly double fitness;

        private static List<int> CreateChromosome()
        {
            List<int> chromosome = new();
            Random random = new Random();
            for(int i = 0; i < Length; i++)
            {
                if(random.Next(2) == 0)
                {
                    chromosome.Add(0);
                }
                else
                {
                    chromosome.Add(1);
                }
            }

            return chromosome;
        }

        private static int MapListToInt(List<int> chromosome)
        {
            string binaryArray = string.Join("", chromosome);
            return Convert.ToInt32(binaryArray, 2) % Range;
        }

        private static double CalcPhenotype(List<int> chromosome)
        {
            double min = 0d;
            double max = (double)Range;
            int value = MapListToInt(chromosome);
            return MinX + (MaxX - MinX) * ((value - min) / (max - min));
        }

        private double CalcFitness()
        {
            double result = 1.25d * Math.Pow(1 - phenotypeX, 2) * Math.Pow(Math.E, -Math.Pow(phenotypeX, 2) - Math.Pow(phenotypeY + 1, 2)) 
                - 10 * (phenotypeX - Math.Pow(phenotypeX, 5) - Math.Pow(phenotypeY, 5)) * Math.Pow(Math.E, -(Math.Pow(phenotypeX, 2) + Math.Pow(phenotypeY, 2)/0.9d))
                -0.2d * Math.Pow(Math.E, -(Math.Pow(phenotypeX + 1, 2) - Math.Pow(phenotypeY, 2)));
            return result;
        }

        public Individual()
        {
            chromosomeX = CreateChromosome();
            chromosomeY = CreateChromosome();
            phenotypeX = CalcPhenotype(chromosomeX);
            phenotypeY = CalcPhenotype(chromosomeY);
            fitness = CalcFitness();
        }

        public double GetFitness()
        {
            return fitness;
        }
    }
}
