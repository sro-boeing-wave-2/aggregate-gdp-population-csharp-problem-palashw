using System;
using Xunit;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using AggregateGDPPopulation;
using System.Threading.Tasks;

namespace AggregateGDPPopulation.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void test1()
        {
            //calling the function
            AggregatePopGDPSync.AggregateGdpPop(@"../../../../AggregateGDPPopulation/data/datafile.csv");

            //storing files for comparison
            string expected = File.ReadAllText(@"../../../expected-output.json");
            string actual = File.ReadAllText(@"../../../../AggregateGDPPopulation/data/output.json");

            //making and comparing json objects for output and expected output
            Dictionary<string, POPGDPObject> actualjson = JsonConvert.DeserializeObject<Dictionary<string, POPGDPObject>>(actual);
            Dictionary<string, POPGDPObject> expectedjson = JsonConvert.DeserializeObject<Dictionary<string, POPGDPObject>>(expected);
            foreach (var key in actualjson.Keys)
            {
                if (expectedjson.ContainsKey(key))
                {
                    Assert.Equal(actualjson[key].GDP_2012, expectedjson[key].GDP_2012);
                    Assert.Equal(actualjson[key].POPULATION_2012, expectedjson[key].POPULATION_2012);
                }
                else
                {
                    Assert.True(false);
                }
            }

        }

        [Fact]
        async public void test2()
        {
            // calling function to aggregate
            await AggregatePopGDPAsync.AggregateGdpPopAsync(@"../../../../AggregateGDPPopulation/data/datafile.csv");
           
            // creating actual and expected strings to compare
            string expecteddata = await AggregatePopGDPAsync.ReadfileAsync(@"../../../expected-output.json"); ;
            string actualdata = await AggregatePopGDPAsync.ReadfileAsync(@"../../../../AggregateGDPPopulation/data/output.json");
            
            Assert.Equal(expecteddata, actualdata);

        }
    }
}
