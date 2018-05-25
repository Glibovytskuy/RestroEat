using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;
using Restro.Models;

namespace Restro.Controllers
{
    public class PlacesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Places
        public ActionResult Index(string currentFilter, string searchString, int? page, string typefilter,
            string kitchenfilter, string featurefilter, List<int> names, string sortorder, string datefilter)
        {
            ViewBag.IsAdmin = HttpContext.User.IsInRole("admin");
            ViewBag.Search = searchString;
            ViewBag.KitchenSelect = kitchenfilter;
            ViewBag.TypeSelect = typefilter;
            ViewBag.FetureSelect = featurefilter;
            ViewBag.Dateselect = (datefilter == null)? "all": datefilter;







            var places = from s in db.Places.OrderBy(i => i.Name) select s;



            var daynow = System.DateTime.Now.DayOfWeek.ToString();
            switch (datefilter)
            {
                case "open":
                    var hour = DateTime.Now.TimeOfDay;
                    places = places.Where(x =>
                        x.Days.Any(y =>
                            !(y.IsWeeknd.HasValue && y.IsWeeknd.Value)
                            && y.Name == daynow
                            && (hour > y.From && hour < y.To
                                || y.IsRoundClock.HasValue && y.IsRoundClock.Value)
                    ));
                    break;
                case "all_day":
                    places = places.Where(s => s.Days.Any(x => x.Name == daynow && x.IsRoundClock == true));
                    break;
                default:
                    places = places.OrderBy(i => i.Name);
                    break;
            }

            switch (sortorder)
            {
                case "up":
                    places = places.OrderByDescending(s => s.Ratings.Average(x => x.Mark));
                    break;
                case "down":
                    places = places.OrderBy(s => s.Ratings.Average(x => x.Mark));
                    break;
                default:
                    places = places.OrderBy(i => i.Name);
                    break;
            }

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                places = places.Where(s => s.Name.Contains(searchString) || s.Type.Contains(searchString));
                places = places.OrderBy(s => s.Name);
            }

            if (!String.IsNullOrEmpty(typefilter))
            {
                places = places.Where(s => s.Type == typefilter);
            }
            if (!String.IsNullOrEmpty(kitchenfilter))
            {
                places = places.Where(s => s.Kitchens.Any(x => x.Name == kitchenfilter));
            }
            if (names != null)
            {
                places = places.Where(s => s.Features.Select(x => x.Id).Intersect(names).Count() == names.Count);
            }
            var types = db.Places.Select(m => m.Type).Distinct().ToList();
            var typelist = types.Select(x => new SelectListItem() { Value = x, Text = x }).Distinct().ToList();
            typelist.Insert(0, new SelectListItem { Value = "", Text = "Тип закладу" });
            ViewBag.Types = typelist;


            var kitchens = db.Kitchens.Select(m => m.Name).Distinct().ToList();
            var kitchenlist = kitchens.Select(x => new SelectListItem() { Value = x, Text = x }).Distinct().ToList();
            kitchenlist.Insert(0, new SelectListItem { Value = "", Text = "Кухня" });
            ViewBag.Kitchens = kitchenlist;


            ViewBag.Days = db.Days.Select(m => m.Name).Distinct().ToList();

            ViewBag.Features = db.Features.ToList();


            ViewBag.SelectedFeatures = names;

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(places.ToPagedList(pageNumber, pageSize));
        }

        // GET: Places/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.IsAdmin = HttpContext.User.IsInRole("admin");
            ViewBag.IsAthorized = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;

            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());

            if(currentUser!= null)
            {
                ViewBag.UserID = currentUser.Id;
                ViewBag.UserName = currentUser.Email.Substring(0, currentUser.Email.IndexOf('@'));
            }
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlaceModel placeModel = db.Places.Find(id);

            //var a = placeModel.Ratings.GroupBy(x => x.Name);

            //var b = placeModel.Ratings.GroupBy(x => x.Name)
            //  .Select(x => new
            //  {
            //      Average = x.Average(p => p.Mark),
            //  });

            double RatingAverage1 = placeModel.Ratings.Where(r => r.Name == "Інтер'єр").Average(r => r.Mark);
            double RatingAverage2 = placeModel.Ratings.Where(r => r.Name == "Обслуговування").Average(r => r.Mark);
            double RatingAverage3 = placeModel.Ratings.Where(r => r.Name == "Кухня").Average(r => r.Mark);
            double RatingAverage4 = placeModel.Ratings.Where(r => r.Name == "Ціни").Average(r => r.Mark);




            placeModel.Ratings.Clear();
            var ratings = placeModel.Ratings;
            ratings.Add(new RatingModel { Name = "Інтер'єр", Mark = (int)RatingAverage1 });
            ratings.Add(new RatingModel { Name = "Обслуговування", Mark = (int)RatingAverage2 });
            ratings.Add(new RatingModel { Name = "Кухня", Mark = (int)RatingAverage3 });
            ratings.Add(new RatingModel { Name = "Ціни", Mark = (int)RatingAverage4 });


            if (placeModel == null)
            {
                return HttpNotFound();
            }
            return View(placeModel);
        }


        // POST: Places/Details
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(int? id, RatingModel ratingModel, CommentModel commentModel)
        {
            PlaceModel placeModel = db.Places.Find(id);

            ViewBag.IsAdmin = HttpContext.User.IsInRole("admin");
            ViewBag.IsAthorized = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;

            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());

            if (currentUser != null)
            {
                ViewBag.UserID = currentUser.Id;
                ViewBag.UserName = currentUser.Email.Substring(0, currentUser.Email.IndexOf('@'));
            }

            if (commentModel.Comment!=null)
            {
                commentModel.Date = System.DateTime.Now;
                commentModel.UserId = currentUser.Id;
                commentModel.UserName = currentUser.Email.Substring(0, currentUser.Email.IndexOf('@')); 
                placeModel.Comments.Add(commentModel);
                db.SaveChanges();
                return RedirectToAction("Details");
            }




            if (ratingModel.Name!=null)
            {
                var a = placeModel.Ratings.Where(x => x.UserName == currentUser.Id && x.Name == ratingModel.Name);
         
            

                placeModel.Ratings.Where(x => x.UserName == currentUser.Id && x.Name == ratingModel.Name)
                .ToList()
                .ForEach(y => y.Mark=ratingModel.Mark);

                ratingModel.UserName = currentUser.Id;

                if(a.Count() == 0)
                {
                   placeModel.Ratings.Add(ratingModel);
                }


                db.SaveChanges();
                return RedirectToAction("Details");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            if (placeModel == null)
            {
                return HttpNotFound();
            }
            if (commentModel.Comment == null)
            {
                return RedirectToAction("Details");
            }
            return View(placeModel);
        }


        [Authorize(Roles = "admin")]
        // GET: Places/Create
        public ActionResult Create(PlaceModel placeModel)
        {
            ViewBag.Days = db.Days.Distinct().ToList();
            ViewBag.Kitchens = db.Kitchens.ToList();
            ViewBag.Features = db.Features.ToList();
            ViewBag.Ratings = db.Ratings.ToList();
            return View(placeModel);
        }
        [Authorize(Roles = "admin")]
        // POST: Places/Create
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create(PlaceModel placeModel, int[] selectedKitchens, int[] selectedFeatures, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid)
            {
                if (selectedKitchens != null)
                {
                    foreach (var item in db.Kitchens.Where(x => selectedKitchens.Contains(x.Id)))
                    {
                        placeModel.Kitchens.Add(item);
                    }
                }
                if (selectedFeatures != null)
                {
                    foreach (var item in db.Features.Where(x => selectedFeatures.Contains(x.Id)))
                    {
                        placeModel.Features.Add(item);
                    }
                }
                placeModel.Image = DisplayImage(uploadImage);
                var ratings = placeModel.Ratings;
                ratings.Add(new RatingModel { Name = "Інтер'єр", Mark = 100});
                ratings.Add(new RatingModel { Name = "Обслуговування", Mark = 100 });
                ratings.Add(new RatingModel { Name = "Кухня", Mark = 100  });
                ratings.Add(new RatingModel { Name = "Ціни", Mark = 100 });
                db.Places.Add(placeModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(placeModel);
        }

        [Authorize(Roles = "admin")]
        // GET: Places/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.Kitchens = db.Kitchens.ToList();
            ViewBag.Features = db.Features.ToList();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlaceModel placeModel = db.Places.Find(id);
            if (placeModel == null)
            {
                return HttpNotFound();
            }
            return View(placeModel);
        }
        [Authorize(Roles = "admin")]
        // POST: Places/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PlaceModel placeModel, int[] selectedKitchens, int[] selectedFeatures, HttpPostedFileBase uploadImage)
        {
            PlaceModel place = db.Places.Where(x => x.Id == placeModel.Id).FirstOrDefault();

            if (ModelState.IsValid)
            {
                place.Id = placeModel.Id;
                place.Name = placeModel.Name;
                place.Type = placeModel.Type;
                place.Address = placeModel.Address;
                place.Description = placeModel.Description;
                place.Contacts = placeModel.Contacts;
                if (uploadImage != null)
                {
                    place.Image = DisplayImage(uploadImage);
                }
                place.Kitchens.Clear();
                if (selectedKitchens != null)
                {
                    foreach (var item in db.Kitchens.Where(x => selectedKitchens.Contains(x.Id)))
                    {
                        place.Kitchens.Add(item);
                    }
                }
                place.Features.Clear();
                if (selectedFeatures != null)
                {
                    foreach (var item in db.Features.Where(x => selectedFeatures.Contains(x.Id)))
                    {
                        place.Features.Add(item);
                    }
                }
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(placeModel);
        }
        [Authorize(Roles = "admin")]
        // GET: Places/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlaceModel placeModel = db.Places.Find(id);
            if (placeModel == null)
            {
                return HttpNotFound();
            }
            return View(placeModel);
        }
        [Authorize(Roles = "admin")]
        // POST: Places/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PlaceModel placeModel = db.Places.Find(id);
            db.Places.Remove(placeModel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        private byte[] DisplayImage(HttpPostedFileBase uploadImage)
        {
            byte[] imageData = null;
            // считываем переданный файл в массив байтов
            if (uploadImage != null)
            {
                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }
            }
            return (imageData);
        }

    }
}
