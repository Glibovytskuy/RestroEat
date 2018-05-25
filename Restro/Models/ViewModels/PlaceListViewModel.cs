using System.Collections.Generic;
using System.Web.Mvc;

namespace Restro.Models.ViewModels
{
    public class PlaceListViewModel
    {
        public IEnumerable<PlaceModel> Places { get; set; }
        public SelectList Kitchens { get; set; }
        public SelectList Types { get; set; }       
    }
}