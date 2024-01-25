namespace genetic_algorithm.gen
{
    public class Algorithm(int maxGenerations, int populationSize, double mutationProb, double recombinationProb)
    {
        private static int Count = 1;

        private readonly int id = Count++;
        private readonly List<Population> history = [];
        private readonly int populationSize = populationSize;
        private readonly int maxGenerations = maxGenerations;
        private Population current = new(populationSize);
        private readonly double mutationProb = mutationProb;
        private readonly double recombinationProb = recombinationProb;

        public void Start()
        {
            Random random = new();

            for(int g = 0; g < maxGenerations; g++)
            {
                List<Individual> parents = current.ExecuteRoulette();
                List<Individual> nextGen = [];

                for(int parentIndex = 0; parentIndex < parents.Count - 1; parentIndex += 2)
                {
                    int nextParentIndex = parentIndex + 1;

                    if(random.NextDouble() < recombinationProb && nextParentIndex < parents.Count)
                    {
                        (Individual firstChild, Individual secondChild) = Individual.ExecuteRecombination(Tuple.Create(parents[parentIndex], parents[nextParentIndex]));
                        nextGen.AddRange(new[] { firstChild, secondChild });
                        current.IncrementRecombinationCount();
                    }
                    else
                    {
                        nextGen.AddRange(parents.Skip(parentIndex).Take(2));
                    }
                }

                for(int i = 0; i < nextGen.Count; i++)
                {
                    Individual child = nextGen[i];
                    if(random.NextDouble() < mutationProb)
                    {
                        Individual mutated = child.ExecuteMutation();
                        nextGen[i] = mutated;
                        current.IncrementMutationCount();
                    }
                }

                history.Add(current);
                current = new Population(populationSize, nextGen);
            }
        }

        public void Print()
        {
            using StreamWriter 
                fitness = new($"{id}-fitness.csv"),
                bestIndividual = new($"{id}-best-individual.csv"),
                mutationCount = new($"{id}-mutation-count.csv"),
                recombinationCount = new($"{id}-recombination-count.csv"),
                error = new($"{id}-error.csv");
            for (int i = 0; i < history.Count; ++i)
            {
                Population currentPopulation = history[i];

                fitness.WriteLine($"{i},{currentPopulation.AvgFitness}");
                bestIndividual.WriteLine($"{i},{currentPopulation.BestIndividual.GetFitness()}");
                mutationCount.WriteLine($"{i},{currentPopulation.MutationCount}");
                recombinationCount.WriteLine($"{i},{currentPopulation.RecombinationCount}");
                error.WriteLine($"{i},{currentPopulation.MeanSquareError}");
            }
        }
    }
}
