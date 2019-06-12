using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.IO;
using System.Text;
using System.Linq;
using TSPHelperClasses;

namespace GeneticTSP
{
    public class GeneticAlgorithm
    {

        public List<City> CityList { get; private set; }
        public List<Trip> Population { get; private set; }
        public int Generation { get; private set; }
        public int MaxGenerations { get; private set; }
        public int MaxPopulation { get; private set; }
        public double MutateRate { get; private set; }
        public Random Rand { get; private set; }
        public string FilePath { get; private set; }
        public Trip MostFit { get; private set; }

        public GeneticAlgorithm(int maxPopulation, int maxGenerations, double mutateRate)
        {
            this.MaxPopulation = maxPopulation;
            this.MutateRate = mutateRate;
            this.MaxGenerations = maxGenerations;
            this.Rand = new Random();

            CityList = new List<City>();
            CityList = ReadCities();
            Population = new List<Trip>();
            Generation = 0;

            GeneratePopulation();
            InitializeDataFile();
            WriteStatsToFile();

            while (Generation < MaxGenerations)
            {
                AgeGeneration();
            }
        }

        private List<City> ReadCities()
        {
            List<City> cityList = new List<City>();
            XDocument xml = XDocument.Load("resources/cities.xml");
            var cities = xml.Descendants("city");

            foreach (var element in cities)
            {
                cityList.Add(new City(double.Parse(element.Attribute("x").Value), 
                                      double.Parse(element.Attribute("y").Value), 
                                      element.Attribute("name").Value));
            }

            return cityList.OrderBy(x => x.X).ToList();
        }

        private bool InitializeDataFile()
        {
            FilePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\GeneticAlgorithm{DateTime.Now.ToString("MMddyyy_Hmmss")}.txt";

            using (StreamWriter sw = new StreamWriter(FilePath))
            {
                var sb = new StringBuilder();
                sb.AppendLine("Genetic Algorithm");
                sb.AppendLine($"Max Population:  {MaxPopulation}");
                sb.AppendLine($"Max Generations: {MaxGenerations}");
                sb.AppendLine($"Mutation Rate: {MutateRate}");
                sw.Write(sb.ToString());
            }

            if (File.Exists(FilePath))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void GeneratePopulation()
        {
            for (int i=0; i < MaxPopulation; i++)
            {
                Population.Add(new Trip(CityList, Rand));
            }

            MostFit = Population[0];
        }

        public void AgeGeneration()
        {
            var nextGen = new List<Trip>();

            // Natural selection means that only the fittest will survive.
            Population = Population.OrderBy(x => x.TripLength).ToList();
            for (int i = 0; i < Population.Count; i++)
            {
                nextGen.Add(Crossover(Population[Rand.Next(Population.Count / 4)], Population[Rand.Next(Population.Count / 4)]));
            }

            Population = nextGen;
            Generation++;
            WriteStatsToFile();
        }

        public Trip Crossover(Trip a, Trip b)
        {
            var newRoute = new List<City>();
                        
            for (int i = 0; i < a.Route.Count / 2; i++)
            {
                newRoute.Add(a.Route[i]);
            }

            foreach (City city in b.Route)
            {
                if (!newRoute.Contains(city))
                {
                    newRoute.Add(city);
                }
            }

            var child = new Trip(newRoute);

            if (Rand.NextDouble() < MutateRate)
            {
                child.Mutate(Rand);
            }

            return child;
        }

        private void WriteStatsToFile()
        {
            Trip worst = Population[0];
            Trip best = Population[0];
            double sum = 0;

            foreach (Trip trip in Population)
            {
                sum += trip.TripLength;
                if (trip.TripLength > worst.TripLength)
                {
                    worst = trip;
                }
                if (trip.TripLength < best.TripLength)
                {
                    best = trip;
                    if (best.TripLength < MostFit.TripLength)
                    {
                        MostFit = best;
                    }
                }
            }

            double mean = sum / MaxPopulation;
            double populationStdDev = 0;
            double sampleStdDev = 0;

            foreach (Trip trip in Population)
            {
                populationStdDev += Math.Pow(trip.TripLength - mean, 2);
                sampleStdDev += Math.Pow(trip.TripLength - mean, 2);
            }

            populationStdDev = Math.Sqrt(populationStdDev / MaxPopulation);
            sampleStdDev = Math.Sqrt(sampleStdDev / (MaxPopulation - 1));

            using (StreamWriter sw = new StreamWriter(FilePath, append: true))
            {
                var sb = new StringBuilder();
                sb.AppendLine("-----------------------------------------------------------------------");
                sb.AppendLine($"Generation:                        {Generation}");
                sb.AppendLine($"Mean:                              {mean}");
                sb.AppendLine($"Population Standard Deviation:     {populationStdDev}");
                sb.AppendLine($"Sample Standard Deviation:         {sampleStdDev}");
                sb.AppendLine($"Best Trip of Generation:           {best.ToString()}    - Length: {best.TripLength}");
                sb.AppendLine($"Worst Trip of Generation:          {worst.ToString()}   - Length: {worst.TripLength}");
                sb.AppendLine($"Best Overall Trip:                 {MostFit.ToString()} - Length: {MostFit.TripLength}");
                sw.Write(sb.ToString());
            }
        }

    }
}
