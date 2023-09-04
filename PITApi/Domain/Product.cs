using PTI.Utils;
using PTI.Utils.Enums;
using System;

namespace PTI.Domain
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public eTypes Type { get; set; }
  }
}
