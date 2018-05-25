using Restro.Models;
using System.Collections.Generic;

namespace Restro.Models
{
    public class FeatureModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<PlaceModel> Places { get; set; }
        public FeatureModel()
        {
            Places = new List<PlaceModel>();
        }
    }
}