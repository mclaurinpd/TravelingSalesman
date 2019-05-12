namespace TSPGeneticAlgorithm
{
    class City
    {

        public string Name { get; }
        public double X { get; }
        public double Y { get; }

        public City(double xcoord, double ycoord, string name)
        {
            Name = name;
            X = xcoord;
            Y = ycoord;
        }

    }
}
