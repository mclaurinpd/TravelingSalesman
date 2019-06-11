using System;
using System.Collections.Generic;
using Extensions;

namespace TSPHelperClasses
{
    public class Trip
    {

        public List<City> Route { get; private set; }
        public double TripLength { get; private set; }

        /// <summary>
        /// Creates a route for a trip with the cities passed in. Uses Fisher-Yates shuffle to randomize the route.
        /// </summary>
        /// <param name="cities"></param>
        public Trip(List<City> cities, Random rand)
        {
            Route = new List<City>(cities);
            ShuffleRoute(rand);
            TripLength = 0;
            CalculateLength();
        }

        public Trip(List<City> route)
        {
            this.Route = route;
            TripLength = 0;
            CalculateLength();
        }

        private void CalculateLength()
        {
            for (int i = 0; i < Route.Count; i++)
            {
                if (i + 1 < Route.Count)
                {
                    TripLength += DistanceBetweenTwoCities(Route[i], Route[i + 1]);
                }
                else
                {
                    TripLength += DistanceBetweenTwoCities(Route[i], Route[0]);
                }
            }
        }

        public override string ToString()
        {
            string str = string.Empty;
            foreach (City city in Route)
            {
                str = str + city.ToString();
            }
            string.Concat(str, $" | Length: {TripLength}");
            return str;
        }

        private void ShuffleRoute(Random rand)
        {
            Route.Shuffle(rand);
        }

        private double DistanceBetweenTwoCities(City A, City B)
        {
            var distance = Math.Pow(A.X - B.X, 2) + Math.Pow(A.Y - B.Y, 2);
            distance = Math.Sqrt(distance);
            return distance;
        }

        public void Mutate(Random rand)
        {
            int pos1 = rand.Next(Route.Count);
            int pos2 = rand.Next(Route.Count);
            while (pos1 == pos2)
            {
                pos2 = rand.Next(Route.Count);
            }
            var temp = Route[pos1];
            Route[pos1] = Route[pos2];
            Route[pos2] = temp;
        }
    }
}
