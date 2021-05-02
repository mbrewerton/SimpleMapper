using System;

namespace SimpleMapper.Benchmark.Models
{
    public class SimpleOutput
    {
        public SimpleOutput()
        {
            
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Decimal { get; set; }
        public DateTime Date { get; set; }
    }
}