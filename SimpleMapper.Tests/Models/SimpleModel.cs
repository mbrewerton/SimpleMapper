using System;

namespace SimpleMapper.Tests.Models
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