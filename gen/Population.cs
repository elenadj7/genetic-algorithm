using System;

namespace genetic_algorithm.gen
{
    public class Population
    {
        private readonly static double MaxError = 6.251637526363513d;

        private readonly int size;
        private List<Individual> individuals = [];

        private int mutationCount = 0;
        private int recombinationCount = 0;
        private double meanSquareError;
        private double avgFitness;
        private Individual bestIndividual;

        private List<Individual> CreatePopulation()
        {
            List<Individual> population = new();
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
            individuals = CreatePopulation();
        }
    }
}
