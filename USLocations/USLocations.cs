using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

public class USLocations
{
    IDictionary<string, List<string>> lookupDict = new Dictionary<string, List<string>>();
    IDictionary<string, List<int>> financeDict = new Dictionary<string, List<int>>();
    List<int> states;
    List<string> names;
    String[] toks;

    public USLocations(string fileName)
    {
        using (StreamReader input = new StreamReader(fileName))
        {
            Task t = Task.Run(() =>
            {
                while (!input.EndOfStream)
            {
                names = new List<string>();
                states = new List<int>();
                string line = input.ReadLine();
                toks = line.Split('\t');
                names = new List<string>();
                if((Int32.TryParse(toks[16], out int val)) && (Int32.TryParse(toks[18], out int val2))){
                    states.Add(val);
                    states.Add(val2);
                }
                names.Add(toks[13]);
                if (!lookupDict.ContainsKey(toks[1]))
                {
                    names.Add(toks[6]);
                    names.Add(toks[7]);
                    lookupDict.Add(toks[1], names);
                }
                else
                {                  
                    lookupDict[toks[1]].Add(toks[13]);
                }
                if (states.Count > 0)
                {
                    if (!financeDict.ContainsKey(toks[4]))
                    {
                        financeDict.Add(toks[4], states);
                    }
                    else
                    {
                        if (!(lookupDict.ContainsKey(toks[1]))) {
                        List<int> vals = financeDict[toks[4]];
                                vals[0] += Int32.Parse(toks[16]);
                                vals[1] += Int32.Parse(toks[18]);
                                financeDict[toks[4]] = vals;
                        }
                    }
                }
            }
            });
            t.Wait();
        }
    }

    public double Distance(int zip1, int zip2)
    {
        string str1 = zip1.ToString();
        string str2 = zip2.ToString();
        List<string> list1 = lookupDict[str1];
        List<string> list2 = lookupDict[str2];

        double lat1 = Convert.ToDouble(list1[1]);
        double lat2 = Convert.ToDouble(list2[1]);
        double lon1 = Convert.ToDouble(list1[2]);
        double lon2 = Convert.ToDouble(list2[2]);

        double dLat = (Math.PI / 180) * (lat2 - lat1);
        double dLon = (Math.PI / 180) * (lon2 - lon1);

        lat1 = (Math.PI / 180) * (lat1);
        lat2 = (Math.PI / 180) * (lat2);

        double a = Math.Pow(Math.Sin(dLat / 2), 2) +
               Math.Pow(Math.Sin(dLon / 2), 2) *
               Math.Cos(lat1) * Math.Cos(lat2);
        double rad = 6371;
        double c = 2 * Math.Asin(Math.Sqrt(a));

        return rad * c;
    }

    public List<string> Lookup(int zip)
    {
        List<string> res = new List<string>();
        String str = zip.ToString();
        if (lookupDict.ContainsKey(str))
        {
            res = lookupDict[str];
        }
        else
        {
            res.Add("Zipcode not found");
        }
        return res;
    }

    public List<int> finance(string state)
    {
        List<int> res = new List<int>();
        String str = state;
        if (financeDict.ContainsKey(str))
        {
            res = financeDict[str];
        }
        else
        {
            Console.WriteLine("state not found or no data available");
            res.Add(0);
        }
        return res;
    }
}