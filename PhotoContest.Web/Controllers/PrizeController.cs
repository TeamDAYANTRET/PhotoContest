using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhotoContest.Data;
using PhotoContest.Data.Contracts;
using PhotoContest.Models;
using PhotoContest.Web.Models.BindingModels;
using PhotoContest.Web.Models.ViewModels;

namespace PhotoContest.Web.Controllers
{
    public class PrizeController : BaseController
    {

        public PrizeController() : this(new PhotoContestData())
        {
        }

        public PrizeController(IPhotoContestData data)
            : base(data)
        {
        }

        [HttpPost]
        public ActionResult CreatePrize(CreatePrizeBindingModel model)
        {
            if (model == null)
            {
                ModelState.AddModelError("", "Incorect enered parameter");
            }
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Incorect enered parameter");
            }
            Prize prize = new Prize()
            {
                ContestId = model.ContestId,
                Description = model.Description,
                Name = model.Name,
                ForPlace = model.Place
            };

            this.Data.Prizes.Add(prize);
            this.Data.SaveChanges();

            return Json("Success", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Index(PrizeIndexBindingModel model)
        {
            PrizeIndexViewModel modelView = new PrizeIndexViewModel()
            {
                ContestId = model.ContestId,
                CountOfPrizes = model.CountOfPrizes
            };
            return View(modelView);
        }
    }
}