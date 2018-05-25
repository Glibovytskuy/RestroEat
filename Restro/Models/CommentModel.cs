using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Restro.Models
{
    public class CommentModel
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int PlaceId  { get; set; }
        public DateTime Date { get; set; }
    }
}
