using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

//using USLocations;

namespace Zipcode
{
    class Program
    {
        static void Main(string[] args)
        {
                USLocations test = new USLocations("zipcodes.tsv.txt");
            Console.WriteLine("OPTIONS"); 
            Console.WriteLine("Show all names associated with zipcode: 'lookup (zipcode)'");
            Console.WriteLine("Show distance between two zipcodes: 'distance (zipcode1) (zipcode2)'");
            Console.WriteLine("Show average tax return in state: 'taxes (2 letter state abbreviation)'");
            Console.WriteLine("Close Application: 'exit'");
            bool done = false;
            while (done == false)
            {
                Console.Write("USLocations> ");
                String[] input = Console.ReadLine().Split(' ');
                if (input[0] == "lookup")
                {
                    if (Regex.IsMatch(input[1], @"^[a-zA-Z]+$"))
                    {
                        Console.WriteLine("Enter valid zipcode");
                    }
                    else
                    {
                        List<string> zipNames = test.Lookup(Int32.Parse(input[1]));

                        foreach (string name in zipNames)
                        {
                            if (name.Contains('"'))
                            {
                                Console.WriteLine(name);
                            }
                        }
                    }
                }
                if (input[0] == "distance")
                {
                    double km = test.Distance(Int32.Parse(input[1]), Int32.Parse(input[2]));
                    double mi = km * 0.621371;
                    Console.WriteLine("The distance between " + input[1] + " and " + input[2] +
                        " is " + Math.Round(mi, 2) + " miles " + "(" + Math.Round(km,2) + " km)");
                }
                if (input[0] == "taxes")
                {
                    List<int> vals = test.finance(input[1]);
                    double res = vals[1] / vals[0];
                    Console.WriteLine("Average tax return: " + res);
                }
                if (input[0] == "exit")
                {
                    done = true;
                }
            }
        }
    }
}
