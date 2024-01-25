namespace genetic_algorithm.gen
{
    public class Population
    {
        private readonly static double MaxError = 6.251637526363513d;

        private readonly int size;
        private readonly List<Individual> individuals = [];

        public int MutationCount { get; private set; }
        public int RecombinationCount {  get; private set; }
        public double MeanSquareError { get; private set; }
        public double AvgFitness { get; private set; }
        public Individual BestIndividual { get; private set; }

        private List<Individual> CreatePopulation()
        {
            List<Individual> population = [];
            for(int i = 0; i < size; i++)
            {
                population.Add(new Individual());
            }

            return population;
        }

        private double CalcAvgFitness()
        {
            return CalcTotalFitness() / individuals.Count;
        }

        private double CalcTotalFitness()
        {
            return individuals.Select(i => i.GetFitness()).Sum();
        }

        private double CalcMeanSquareError()
        {
            return individuals.Select(i => Math.Pow(MaxError - i.GetFitness(), 2)).Average();
        }

        private Individual CalcBestIndividual()
        {
            return individuals.MaxBy(i => i.GetFitness());
        }

        public List<Individual> ExecuteRoulette()
        {
            double totalFitness = CalcTotalFitness();
            Random random = new();

            try
            {
                List<double> spins = Enumerable.Range(0, individuals.Count).Select(_ => random.NextDouble() * totalFitness).ToList();

                List<Individual> parents = spins.Select(spin =>
                {
                    double sum = 0.0d;
                    return individuals.First(i =>
                    {
                        sum += i.GetFitness();
                        return sum >= spin;
                    });

                }).ToList();

                return parents;
            }
            catch (DivideByZeroException)
            {
                return individuals;
            }
        }

        public Population(int size)
        {
            this.size = size;
            MutationCount = 0;
            RecombinationCount = 0;
            individuals = CreatePopulation();
            AvgFitness = CalcAvgFitness();
            MeanSquareError = CalcMeanSquareError();
            BestIndividual = CalcBestIndividual();
        }

        public Population(int size, List<Individual> individuals)
        {
            this.size = size;
            MutationCount = 0;
            RecombinationCount = 0;
            this.individuals = individuals;
            AvgFitness = CalcAvgFitness();
            MeanSquareError = CalcMeanSquareError();
            BestIndividual = CalcBestIndividual();
        }

        public void IncrementRecombinationCount()
        {
            RecombinationCount++;
        }

        public void IncrementMutationCount()
        {
            MutationCount++;
        }
    }
}
