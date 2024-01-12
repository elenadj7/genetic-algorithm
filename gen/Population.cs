namespace genetic_algorithm.gen
{
    public class Population
    {
        private readonly static double MaxError = 6.251637526363513d;

        //private readonly int size;
        private List<Individual> individuals = new();

        private int mutation = 0;
        private int recombination = 0;
        private double meanSquareError;
        private double avgFitness;
        private Individual bestIndividual;

        /*private List<Individual> CreatePopulation()
        {
            List<Individual> population = new();
            for(int i = 0; i < size; i++)
            {
                population.Add(new Individual());
            }

            return population;
        }*/

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
            return individuals.Select(i => Math.Pow(i.GetFitness(), 2)).Average();
        }
    }
}
