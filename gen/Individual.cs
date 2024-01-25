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
            List<int> chromosome = [];
            Random random = new();
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
            double max = Range;
            int value = MapListToInt(chromosome);
            return MinX + (MaxX - MinX) * ((value - min) / (max - min));
        }

        private double CalcFitness()
        {
            double result = 1.25d * Math.Pow(1 - phenotypeX, 2) * Math.Pow(Math.E, -Math.Pow(phenotypeX, 2) - Math.Pow(phenotypeY + 1, 2)) 
                - 10 * (phenotypeX - Math.Pow(phenotypeX, 5) - Math.Pow(phenotypeY, 5)) * Math.Pow(Math.E, -(Math.Pow(phenotypeX, 2) + Math.Pow(phenotypeY, 2)/0.9d))
                -0.2d * Math.Pow(Math.E, -(Math.Pow(phenotypeX + 1, 2) - Math.Pow(phenotypeY, 2)));
            return result <= 0 ? 0 : result;
        }

        public static List<int> Mutate(List<int> chromosome) //random bit inversion
        {
            return chromosome.Select(bit => new Random().Next(2) == 1 ? 1 - bit : bit).ToList();
        }

        public Individual ExecuteMutation()
        {
            List<int> newChromosomeX = Mutate(chromosomeX);
            List<int> newChromosomeY = Mutate(chromosomeY);
            return new Individual(newChromosomeX, newChromosomeY);
        }

        /*first child = first part of first parent + second part of second parent
        second child = first part od second parent + second part of first parent*/
        public static Tuple<Individual, Individual> ExecuteRecombination(Tuple<Individual, Individual> parents)
        {
            int pivot = new Random().Next(1, Length);

            Individual firstChild = CrossoverFirstChild(parents.Item1.GetChromosomeX(), parents.Item1.GetChromosomeY(), parents.Item2.GetChromosomeX(), parents.Item2.GetChromosomeY(), pivot);
            Individual secondChild = CrossoverSecondChild(parents.Item1.GetChromosomeX(), parents.Item1.GetChromosomeY(), parents.Item2.GetChromosomeX(), parents.Item2.GetChromosomeY(), pivot);

            return Tuple.Create(firstChild, secondChild);
        }

        private static Individual CrossoverFirstChild(List<int> firstChromosomeX, List<int> firstChromosomeY, List<int> secondChromosomeX, List<int> secondChromosomeY, int pivot)
        {
            List<int> newChromosomeX = firstChromosomeX.Take(pivot).Concat(secondChromosomeX.Skip(pivot)).ToList();
            List<int> newChromosomeY = firstChromosomeY.Take(pivot).Concat(secondChromosomeY.Skip(pivot)).ToList();

            return new Individual(newChromosomeX, newChromosomeY);
        }

        private static Individual CrossoverSecondChild(List<int> firstChromosomeX, List<int> firstChromosomeY, List<int> secondChromosomeX, List<int> secondChromosomeY, int pivot)
        {
            List<int> newChromosomeX = secondChromosomeX.Take(pivot).Concat(firstChromosomeX.Skip(pivot)).ToList();
            List<int> newChromosomeY = secondChromosomeY.Take(pivot).Concat(firstChromosomeY.Skip(pivot)).ToList();

            return new Individual(newChromosomeX, newChromosomeY);
        }

        public Individual()
        {
            chromosomeX = CreateChromosome();
            chromosomeY = CreateChromosome();
            phenotypeX = CalcPhenotype(chromosomeX);
            phenotypeY = CalcPhenotype(chromosomeY);
            fitness = CalcFitness();
        }

        private Individual(List<int> chromosomeX, List<int> chromosomeY)
        {
            this.chromosomeX = chromosomeX;
            this.chromosomeY = chromosomeY;
            phenotypeX = CalcPhenotype(chromosomeX);
            phenotypeY = CalcPhenotype(chromosomeY);
            fitness = CalcFitness();
        }

        public double GetFitness()
        {
            return fitness;
        }

        public List<int> GetChromosomeX()
        {
            return chromosomeX;
        }

        public List <int> GetChromosomeY()
        {
            return chromosomeY;
        }
    }
}
