using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Core.Entities
{
    public class Product:BaseEntity
    {
        public string Name { get;  set; }
        public DateTime CreationDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }
        public int? CategoryId { get;  set; }
        public virtual Category Category { get; set; }
    }
}
