using System;

namespace SimpleMapper.Benchmark.Models
{
    public class SimpleModel
    {
        public SimpleModel()
        {
            
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Decimal { get; set; }
        public DateTime Date { get; set; }
    }
}