﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Xml;
using SitePerfTest.Models;
using System.IO;


namespace SitePerfTest.Controllers
{
    public class HomeController : Controller
    {
        private Context db = new Context();
        //рендер главной страницы
        public ActionResult Index()
        {
            ViewBag.data = db.GraphycsDatas.ToList();
            return View();
        }
        //тестовый запрос для проверки протокола
        public void TestCall(string url)
        {
            XmlDocument xml = new XmlDocument();
            try
            {
                xml.Load(url);
            }
            catch(FileNotFoundException e)
            {
                
            }            
        }
        
        //загружает xml с указанного урла и возвращает его
        public void GetXml(string url)
        {
            XmlDocument xml = new XmlDocument();
            try
            {
                xml.Load(url);
            }
            catch (FileNotFoundException e)
            {
                
            }
            Response.ContentType = "text/xml"; 
            Response.ContentEncoding = System.Text.Encoding.UTF8; 
            xml.Save(Response.Output);
        }   

       public string FirstRequest(string url)
        {
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            try
            {
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                return (resp.StatusCode).ToString();
            }
            catch(Exception e)
            {
                return e.Message;
            }
                    
        }
        public string SecondRequest(string url)
        {
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            try
            {
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                return (resp.StatusCode).ToString();
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public string ThirdRequest(string url)
        {
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            try
            {
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                return (resp.StatusCode).ToString();
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public string FourthRequest(string url)
        {
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            try
            {
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                return (resp.StatusCode).ToString();
            }
            catch (Exception e)
            {
                return e.Message;
            }
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