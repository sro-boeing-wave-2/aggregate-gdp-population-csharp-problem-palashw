using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace AggregateGDPPopulation
{
    public class POPGDPObject
    {
        public float POPULATION_2012 { get; set; }
        public float GDP_2012 { get; set; }

    };

    public class AggregatePopGDPSync
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

                for (int i = 1; i < csvdata.Length; i++)
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

    public class AggregatePopGDPAsync
    {
        public static async Task<string> ReadfileAsync(string filepath)
        {
            string csvdata;
            using(StreamReader r = new StreamReader(filepath))
            {
                csvdata = await r.ReadToEndAsync();
            }
            return csvdata;
        }

        public static async Task WritefileAsync(string outputpath, Dictionary<string, POPGDPObject> finalobjects)
        {
            using (StreamWriter w = new StreamWriter(outputpath))
            {
                await w.WriteAsync(JsonConvert.SerializeObject(finalobjects));
            }
        }

        public static async Task AggregateGdpPopAsync(string filename)
        {
            Task<string> csvdatatask = ReadfileAsync(filename);
            Task<string> mapperdatatask = ReadfileAsync(@"../../../../AggregateGDPPopulation/data/countriesmap.txt");

            await Task.WhenAll(csvdatatask, mapperdatatask);

            string csvdata = csvdatatask.Result;
            string mapperdata = mapperdatatask.Result;
            
            // making mapper dictionary
            string[] countrymap = mapperdata.Split('\n');
            Dictionary<string, string> mapper = new Dictionary<string, string>();

            foreach (string str in countrymap)
            {
                string[] row = str.Split(',');
                mapper[row[0]] = row[1];
            }
            
            // making csv data
            string[] data = csvdata.Split('\n');
            string[] headers = data[0].Split(',');
            int indexcountry = Array.IndexOf(headers, "\"Country Name\"");
            int indexgdp = Array.IndexOf(headers, "\"GDP Billions (USD) 2012\"");
            int indexpop = Array.IndexOf(headers, "\"Population (Millions) 2012\"");

            Dictionary<string, POPGDPObject> finalobjects = new Dictionary<string, POPGDPObject>();

            // aggregating
            try
            {

                for (int i = 1; i < data.Length; i++)
                {
                    string[] datarow = data[i].Replace("\"", "").Split(',');

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
            catch (Exception) { }

            // writing to json
            await WritefileAsync(@"../../../../AggregateGDPPopulation/data/output.json", finalobjects);
            
        }


    }

}
