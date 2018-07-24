using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AggregateGDPPopulation
{
    public class POPGDPObject
    {
        public float POPULATION_2012 { get; set; }
        public float GDP_2012 { get; set; }

    };

    public class Class1
    {
        public static void AggregateGdpPop(string filename)
        {   //reading in csv data and making headers array
            string[] csvdata = File.ReadAllLines(filename);
            string[] headers = csvdata[0].Split(',');

            //storing indices of required fields
            int indexcountry = Array.IndexOf(headers, "\"Country Name\"");
            int indexgdp = Array.IndexOf(headers, "\"GDP Billions (USD) 2012\"");
            int indexpop = Array.IndexOf(headers, "\"Population (Millions) 2012\"");

            //reading country continent map text file and making dictionary for the mapping
            string[] countrymap = File.ReadAllLines(@"../../../../AggregateGDPPopulation/data/countriesmap.txt");
            Console.WriteLine(countrymap[0]);
            Dictionary<string, string> mapper = new Dictionary<string, string>();

            foreach (string str in countrymap)
            {
                string[] row = str.Split(',');
                mapper[row[0]] = row[1];
            }

            // declaring finalobjects dictionary to store final object format of output
            Dictionary<string, POPGDPObject> finalobjects = new Dictionary<string, POPGDPObject>();

            try
            {

                for (int i = 1; i < csvdata.Length - 1; i++)
                {
                    string[] datarow = csvdata[i].Replace("\"", "").Split(',');
                    
                    if (!finalobjects.ContainsKey(mapper[datarow[indexcountry]]))
                    {
                        finalobjects[mapper[datarow[indexcountry]]] = new POPGDPObject();
                        finalobjects[mapper[datarow[indexcountry]]]
                          .GDP_2012 = float.Parse(datarow[indexgdp]);
                        finalobjects[mapper[datarow[indexcountry]]]
                          .POPULATION_2012 = float.Parse(datarow[indexpop]);
                    }
                    else
                    {
                        finalobjects[mapper[datarow[indexcountry]]]
                          .GDP_2012 += float.Parse(datarow[indexgdp]);
                        finalobjects[mapper[datarow[indexcountry]]]
                          .POPULATION_2012 += float.Parse(datarow[indexpop]);
                    }

                }

            }
            catch (Exception){ }

            // converiting and writing to json
            var jsonString = JsonConvert.SerializeObject(finalobjects);
            File.WriteAllText(@"../../../../AggregateGDPPopulation/data/output.json", jsonString);
        }
    }

}
