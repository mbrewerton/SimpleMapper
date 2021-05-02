using System;
using System.Collections.Generic;
using System.Linq;
using SimpleMapper.Tests.Models;
using Xunit;

namespace SimpleMapper.Tests
{
    public class MapCollectionTests
    {
        [Fact]
        public void Test()
        {
            var models = new SimpleModel[]
            {
                new SimpleModel
                {
                    Id = 1,
                    Date = DateTime.Now,
                    Decimal = 1,
                    Name = "Item 1"
                },
                new SimpleModel
                {
                    Id = 2,
                    Date = DateTime.Now,
                    Decimal = 2,
                    Name = "Item 2"
                }
            };

            SimpleOutput[] output = null;
            output = models.MapCollection<SimpleModel, SimpleOutput>().ToArray();
            Assert.NotNull(output);
        }
    }
}