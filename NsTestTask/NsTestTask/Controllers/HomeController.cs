using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NsTestTask.Models;
using Microsoft.AspNet.Identity;

namespace NsTestTask.Controllers
{
    public class HomeController : Controller
    {
        private IRepository rep;

        public HomeController(IRepository r)
        {
            rep = r;
        }

        public HomeController()
        {
            rep = new Repository();
        }
        public ActionResult Index()
        {
            if (TempData.ContainsKey("ModelState"))
                ModelState.Merge((ModelStateDictionary)TempData["ModelState"]);

            ViewBag.Users = rep.GetUsersList();
            ViewBag.Tasks = rep.GetJobsList();
            return View();
        }






        public ActionResult AddTask(Job model)
        {
            if (ModelState.IsValid)
            {
                string CurrentUserEmail = User.Identity.GetUserName();
                User CurrentUser = rep.GetUserByEmail(CurrentUserEmail);
                model.Users.Add(CurrentUser);
                model.Date = DateTime.Now;
                rep.AddTask(model);
                try
                {
                    rep.save();
                    TempData["ConsoleMessage"] = "New task created";
                }
                catch (Exception e)
                {
                    TempData["ConsoleMessage"] = e.Message;
                }

                return RedirectToAction("Index", "Home");
            }

            TempData["ModelState"] = ModelState;
            TempData["AddTaskError"] = 1;
            return RedirectToAction("Index", "Home");
        }






        public ActionResult EditTask(Job model)
        {
            if (ModelState.IsValid)
            {
                Job job = rep.FindJob(model.Id);
                job.Title = model.Title;
                job.Description = model.Description;
                try
                {
                    rep.save();
                    TempData["ConsoleMessage"] = "Task changed successfully";
                }
                catch (Exception e)
                {
                    TempData["ConsoleMessage"] = e.Message;
                }

                return RedirectToAction("Index", "Home");
            }
            TempData["ModelState"] = ModelState;
            TempData["EditTaskError"] = 1;
            return RedirectToAction("Index", "Home");
        }







        public ActionResult DeleteTask(int id)
        {
            Job result = rep.FindJob(id);
            if (result != null)
            {
                rep.DeleteJob(result);
                try
                {
                    rep.save();
                    TempData["ConsoleMessage"] = "Task deleted successfully";
                }
                catch (Exception e)
                {
                    TempData["ConsoleMessage"] = e.Message;
                }
            }
            return RedirectToAction("Index", "Home");
        }







        public ActionResult ShareTask()
        {
            var FormData = Request.Form;
            string taskId = FormData["hiddenShareTaskId"];
            Job task = rep.FindJob(int.Parse(taskId));
            if (string.IsNullOrEmpty(FormData["usersList"]))
            {
                ModelState.AddModelError("usersList", "please,choose user/users");
            }
            else
            {
                string s = FormData["usersList"];
                string[] elements = s.Split(',');
                int[] usersIDs = Array.ConvertAll(elements, int.Parse);
                foreach (int item in usersIDs)
                {
                    User user = rep.FindUser(item);
                    task.Users.Add(user);
                }
            }

            if (ModelState.IsValid)
            {

                try
                {
                    rep.save();
                    TempData["ConsoleMessage"] = "Task shared successfully";
                }
                catch (Exception e)
                {
                    TempData["ConsoleMessage"] = e.Message;
                }
                return RedirectToAction("Index", "Home");
            }
            TempData["ModelState"] = ModelState;
            TempData["ShareTaskError"] = 1;
            return RedirectToAction("Index", "Home");
        }






        public int TestStart()
        {
            Random rnd = new Random();
            int number = rnd.Next(1, 20);
            return number;
        }
    }
}