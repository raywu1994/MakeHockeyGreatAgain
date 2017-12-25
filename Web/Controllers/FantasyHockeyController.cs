using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Services.Models.FantasyHockeyService;
using Services;

namespace Web.Controllers
{
    public class FantasyHockeyController : Controller
    {
        FantasyHockeyService _service;
        public FantasyHockeyController()
        {
            _service = new FantasyHockeyService();
        }
        public ActionResult DraftKingsPickEm()
        {
            List<DraftKingsPlayerSelection> viewModel = _service.GetDraftKingsPlayerSelections();
           
            return View(viewModel);
        }
    }
}