using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace NsTestTask.Models
{
    public class Job
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Task Name")]
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public string UsersCommaOutput
        {
            get
            {
                return string.Join(", ", Users.Select(u => u.Email));
            }
        }

        public Job()
        {
            Users = new List<User>();
        }
    }

    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public virtual ICollection<Job> Tasks { get; set; }

        public User()
        {
            Tasks = new List<Job>();
        }
    }

    public class NsTestTaskContext : DbContext
    {
        public DbSet<Job> Jobs { get; set; }
        public DbSet<User> Users { get; set; }
    }

    public interface IRepository : IDisposable
    {
        List<User> GetUsersList();
        List<Job> GetJobsList();
        User GetUserByEmail(string Email);
        void AddTask(Job model);
        Job FindJob(int id);
        User FindUser(int id);
        void DeleteJob(Job obj);
        void save();
    }

        class Repository : IRepository
    {
        private NsTestTaskContext db = new NsTestTaskContext();

        public List<User> GetUsersList()
        {
            return db.Users.ToList();
        }
        public List<Job> GetJobsList()
        {
            return db.Jobs.ToList();
        }
        public User GetUserByEmail(string Email)
        {
            return db.Users.Where(u => u.Email == Email).FirstOrDefault();
        }
        public void AddTask(Job model)
        {
            db.Jobs.Add(model);
        }
        public Job FindJob(int id)
        {
            return db.Jobs.Find(id);
        }
        public User FindUser(int id)
        {
            return db.Users.Find(id);
        }
        public void DeleteJob(Job obj)
        {
            db.Jobs.Remove(obj);
        }
        public void save()
        {
            db.SaveChanges();
        }
        public void Dispose()
        {
            db.Dispose();
        }
    }
}