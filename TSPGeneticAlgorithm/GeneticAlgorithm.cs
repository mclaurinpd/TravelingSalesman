using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Text;

namespace TSPGeneticAlgorithm
{
    class GeneticAlgorithm
    {

        public List<City> cityList { get; private set; }

        public GeneticAlgorithm()
        {
            cityList = new List<City>();
            cityList = ReadCities();
        }

        private List<City> ReadCities()
        {
            var cityList = new List<City>();
            XDocument xml = XDocument.Load("resources/cities.xml");
            var cities = xml.Descendants("city");

            foreach (var element in cities)
            {
                cityList.Add(new City(double.Parse(element.Attribute("x").Value), 
                                      double.Parse(element.Attribute("y").Value), 
                                      element.Attribute("name").Value));
            }

            return cityList;
        }

        private double DistanceBetweenTwoCities(City A, City B)
        {
            var distance = Math.Pow(A.X - B.X, 2) + Math.Pow(A.Y - B.Y, 2);
            distance = Math.Sqrt(distance);
            return distance;
        }

    }
}
