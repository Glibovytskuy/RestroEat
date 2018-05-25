using Restro.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Restro.Models
{
    public class PlaceModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Вкажіть назву закладу")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Вкажіть тип закладу")]
        public string Type { get; set; }
        [Required(ErrorMessage = "Вкажіть адресу закладу")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Вкажіть контакти закладу")]
        public string Contacts { get; set; }
        public byte[] Image { get; set; }

        public virtual ICollection<RatingModel> Ratings { get; set; }
        public virtual ICollection<KitchenModel> Kitchens { get; set; }
        public virtual ICollection<FeatureModel> Features { get; set; }
        public virtual ICollection<DayModel> Days { get; set; }
        public virtual ICollection<CommentModel> Comments { get; set; }

        public PlaceModel()
        {
            Ratings = new List<RatingModel>();
            Kitchens = new List<KitchenModel>();
            Features = new List<FeatureModel>();
            Days = new List<DayModel>();
            Comments = new List<CommentModel>();
        }
    }
}