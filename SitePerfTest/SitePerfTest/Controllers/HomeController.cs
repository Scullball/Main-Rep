using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Xml;
using SitePerfTest.Models;

namespace SitePerfTest.Controllers
{
    public class HomeController : Controller
    {
        private Context db = new Context();

        public ActionResult Index()
        {
            ViewBag.data = db.GraphycsDatas.ToList();
            return View();
        }   

       public string SaveToBd(string title, string data)
        {
            GraphycsData model = new GraphycsData();
            model.Title = title;
            model.Data = data;
            model.Date = DateTime.Now;
            db.GraphycsDatas.Add(model);
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                return e.Message;
            }            
            return "saved sucsessful";
        }
    }

    
}