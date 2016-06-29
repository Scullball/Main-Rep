using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Text.RegularExpressions;


namespace SitePerfTest.Models
{
    public class GraphycsData
    {
        public int Id {get;set;}
        public string Title { get; set; }
        public string Data { get; set; }
        public DateTime Date { get; set; }
    }

    public class Context : DbContext
    {
        public DbSet<GraphycsData> GraphycsDatas { get; set; }
    }

    //класс нахоит все ссылки на переданной странице
    public static class LInkFinder
    {

        public static List<string> Find(string file)
        {
            List<string> list = new List<string>();
            //находит все теги a(anchor) в переданной строке
            MatchCollection match = Regex.Matches(file, @"(<a.*?>.*?</a>)", RegexOptions.Singleline);

            foreach(Match item in match)
            {
                string result = item.Groups[1].Value;
                //находит url в найденных выше ссылках
                Match url = Regex.Match(result, @"href=\""(.*?)\""", RegexOptions.Singleline);
                if (url.Success)
                {
                    //если получившийся url больше 5 знаков,добавляем в список.
                    if (url.Groups[1].Value.Length > 5)
                    {
                        list.Add(url.Groups[1].Value);
                    }
                    if (list.Count == 20) break;
                }               
            }           
            return list;
        }
    }
  
}