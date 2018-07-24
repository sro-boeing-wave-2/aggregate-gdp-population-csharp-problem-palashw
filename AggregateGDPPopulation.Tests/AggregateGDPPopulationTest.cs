using System;
using Xunit;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AggregateGDPPopulation.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void test1()
        {
            Class1.AggregateGdpPop(@"D:\workspace\CSharp\Assignments\aggregate-gdp-population-csharp-problem-palashw\AggregateGDPPopulation\data\datafile.csv");
            string actual = File.ReadAllText(@"D:\workspace\CSharp\Assignments\aggregate-gdp-population-csharp-problem-palashw\AggregateGDPPopulation.Tests\expected-output.json");
            string expected = File.ReadAllText(@"D:\workspace\CSharp\Assignments\aggregate-gdp-population-csharp-problem-palashw\AggregateGDPPopulation\data\output.json");
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
    }
}
