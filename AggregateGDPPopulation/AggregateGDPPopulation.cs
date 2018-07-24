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

            int indexcountry = Array.IndexOf(headers, "\"Country Name\"");
            int indexgdp = Array.IndexOf(headers, "\"GDP Billions (USD) 2012\"");
            int indexpop = Array.IndexOf(headers, "\"Population (Millions) 2012\"");

            string[] countrymap = File.ReadAllLines(@"D:\workspace\CSharp\Assignments\aggregate-gdp-population-csharp-problem-palashw\AggregateGDPPopulation\data\countriesmap.txt");
            Dictionary<string, string> mapper = new Dictionary<string, string>();

            foreach (string str in countrymap)
            {
                string[] row = str.Split(',');
                mapper[row[0]] = row[1];
            }

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

            var jsonString = JsonConvert.SerializeObject(finalobjects);
            File.WriteAllText(@"D:\workspace\CSharp\Assignments\aggregate-gdp-population-csharp-problem-palashw\AggregateGDPPopulation\data\output.json", jsonString);
            //string oppath = @"D:\workspace\CSharp\Assignments\aggregate - gdp - population - csharp - problem - palashw\AggregateGDPPopulation\data\output";
            //try
            //{
            //    File.WriteAllText(oppath+@"\output.json", jsonString);
            //}
            //catch (Exception e)
            //{
            //    Directory.CreateDirectory(oppath);
            //    File.WriteAllText(oppath + @"\output.json", jsonString);
            //}
        }
    }

}
