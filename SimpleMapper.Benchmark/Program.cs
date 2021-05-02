using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using SimpleMapper.Benchmark.Models;

namespace SimpleMapper.Benchmark
{
    [SimpleJob(RuntimeMoniker.CoreRt31)]
    public class SimpleBenchmark
    {
        private SimpleModel[] _models;
        private int _count = 10000;

        [GlobalSetup]
        public void Setup()
        {
            var dateTime = DateTime.Now;
            _models = new SimpleModel[_count];
            for (var c = 0; c < _count; c++)
            {
                _models[c] = new SimpleModel
                {
                    Id = c,
                    Date = dateTime,
                    Decimal = c,
                    Name = $"Item {c}"
                };
            }
        }

        [Benchmark]
        public void MapSimpleModels() => Consume(_models.MapCollection<SimpleModel, SimpleOutput>());

        private readonly Consumer _consumer = new Consumer();
        private void Consume(IEnumerable<SimpleOutput> outputs)
        {
            foreach (var simpleOutput in outputs)
            {
                _consumer.Consume(simpleOutput);
            }
        }
    }
    
    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<SimpleBenchmark>();
        }
    }
}