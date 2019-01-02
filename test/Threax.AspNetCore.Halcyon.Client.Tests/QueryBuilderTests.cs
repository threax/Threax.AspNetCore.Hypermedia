using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xunit;

namespace Threax.AspNetCore.Halcyon.Client.Tests
{
    public class QueryBuilderTests
    {
        class SimpleQueryTest
        {
            public String Name { get; set; }

            public int Number { get; set; }
        }

        [Fact]
        public void BasicQuery()
        {
            var query = QueryBuilder.BuildQueryString(new SimpleQueryTest()
            {
                Name = "Bob",
                Number = 1
            });
            Assert.Equal("Name=Bob&Number=1", query);
        }

        [Fact]
        public void EncodedQuery()
        {
            var query = QueryBuilder.BuildQueryString(new SimpleQueryTest()
            {
                Name = "Bob Smith & The Crew / Other Peeps",
                Number = 1
            });
            Assert.Equal("Name=Bob%20Smith%20&%20The%20Crew%20/%20Other%20Peeps&Number=1", query);
        }

        [Fact]
        public void QueryWithNull()
        {
            var query = QueryBuilder.BuildQueryString(new SimpleQueryTest()
            {
                Name = null,
                Number = 1
            });
            Assert.Equal("Number=1", query);
        }

        class ArrayQueryTest
        {
            public String Name { get; set; }

            public int[] Numbers { get; set; }
        }

        [Fact]
        public void ArrayQuery()
        {
            var query = QueryBuilder.BuildQueryString(new ArrayQueryTest()
            {
                Name = "Bob",
                Numbers = new int[] { 1, 15, 20 }
            });
            Assert.Equal("Name=Bob&Numbers=1&Numbers=15&Numbers=20", query);
        }

        [Fact]
        public void ArrayQueryOneItem()
        {
            var query = QueryBuilder.BuildQueryString(new ArrayQueryTest()
            {
                Name = "Bob",
                Numbers = new int[] { 1 }
            });
            Assert.Equal("Name=Bob&Numbers=1", query);
        }

        [Fact]
        public void DictionaryQuery()
        {
            var query = QueryBuilder.BuildQueryString(new Dictionary<String, Object>()
            {
                { "Name", "Bob" },
                { "Numbers", new List<int>() { 1, 15, 20 } }
            });
            Assert.Equal("Name=Bob&Numbers=1&Numbers=15&Numbers=20", query);
        }

        class GeneratedClassTest
        {
            [JsonProperty("switchIds", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
            public List<Guid> SwitchIds { get; set; }
            //
            // Summary:
            //     Get the current status of the switches in the query results. Will take longer
            //     while the switch info is loaded.
            [JsonProperty("getStatus", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
            public bool GetStatus { get; set; }
            //
            // Summary:
            //     Lookup a @switch by id.
            [JsonProperty("switchId", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
            public Guid? SwitchId { get; set; }
            //
            // Summary:
            //     The number of pages (item number = Offset * Limit) into the collection to query.
            [JsonProperty("offset", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
            public int Offset { get; set; }
            //
            // Summary:
            //     The limit of the number of items to return.
            [JsonProperty("limit", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
            public int Limit { get; set; }
        }

        [Fact]
        public void GeneratedClassTestQuery()
        {
            var query = QueryBuilder.BuildQueryString(new GeneratedClassTest()
            {
                SwitchIds = new List<Guid>()
                {
                    Guid.Parse("5eaf4629-d7c5-4523-a386-71aac9d679af"),
                    Guid.Parse("a3e20767-e369-45ba-92bd-1c3251ff7b3b")
                },
                Limit = int.MaxValue
            });
            Assert.Equal("SwitchIds=5eaf4629-d7c5-4523-a386-71aac9d679af&SwitchIds=a3e20767-e369-45ba-92bd-1c3251ff7b3b&GetStatus=False&Offset=0&Limit=2147483647", query);
        }
    }
}
