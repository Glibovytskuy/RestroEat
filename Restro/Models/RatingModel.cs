
using System.Collections.Generic;


namespace Restro.Models
{
    public class RatingModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Mark { get; set; }
        public string UserName { get; set; }
        public virtual ICollection<PlaceModel> Places { get; set; }
        public RatingModel()
        {
            Places = new List<PlaceModel>();
        }
    }
}