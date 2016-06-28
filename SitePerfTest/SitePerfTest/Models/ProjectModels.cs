using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

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

  
}