using System;
using System.Collections.Generic;
using Extensions;

namespace TSPGeneticAlgorithm
{
    class Trip
    {

        public List<City> route { get; private set; }

        /// <summary>
        /// Creates a route for a trip with the cities passed in. Uses Fisher-Yates shuffle to randomize the route.
        /// </summary>
        /// <param name="cities"></param>
        public Trip(List<City> cities, Random rand)
        {
            route = new List<City>(cities);
            ShuffleRoute(rand);
        }

        private void ShuffleRoute(Random rand)
        {
            route.Shuffle(rand);
        }
    }
}
