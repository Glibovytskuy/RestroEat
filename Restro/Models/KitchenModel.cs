using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Restro.Models
{
    public class KitchenModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<PlaceModel> Places { get; set; }
        public KitchenModel()
        {
            Places = new List<PlaceModel>();
        }
    }
}