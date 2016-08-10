using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        [Display(Name ="Название книги")]
        public string Title { get; set; }
        [Required]
        [Display(Name ="Издательство")]
        public string Publishing { get; set; }
        [Required]
        [Display(Name ="Краткое описание")]
        public string Description { get; set; }
        [Display(Name = "Цена")]
        public int Price { get; set; }
        [Display(Name = "Кол-во страниц")]
        public int Pages { get; set; }
        [Required]
        [Display(Name ="Миниатюра")]
        public string Image { get; set; }
        public virtual ICollection<Author> Authors { get; set; }
        public virtual ICollection<Genre> Genres { get; set; }
        public string GenresCommaOutput
        {
            get
            {
                return string.Join(", ", Genres.Select(g => g.GenreName));
            }
        }

        public Book()
        {
            Authors = new List<Author>();
            Genres = new List<Genre>();
        }
    }

    public class Author
    {
        public int Id { get; set; }
        public string AuthorName { get; set; }
        public string AuthorDescription { get; set; }

        public virtual ICollection<Book> Books { get; set; }

        public Author()
        {
            Books = new List<Book>();
        }
    }

    public class Genre
    {
        public int Id { get; set; }
        public string GenreName { get; set; }
        public string GenreDescription { get; set; }

        public virtual ICollection<Book> Books { get; set; }

        public Genre()
        {
            Books = new List<Book>();
        }
    }

    public class Purchase
    {
        public int Id { get; set; }
        public string Customer { get; set; }
        public string PurchasedBooks { get; set; }
        public DateTime Date { get; set; }
    }

    public class BookStoreContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Purchase> Purshases { get; set; }
    }
}