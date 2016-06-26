using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;

namespace BookStore.Controllers
{
    public class AdminController : Controller
    {
        private BookStoreContext db = new BookStoreContext();

        [Authorize(Roles = "admin")]
        public ActionResult DeleteBook(int id)
        {
            Book result = db.Books.Find(id);
            if (result != null)
            {
                db.Books.Remove(result);
                db.SaveChanges();
                TempData["messageBox"] = "книга удалена";
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "admin")]
        public ActionResult AddBook()
        {
            if (TempData.ContainsKey("ModelState"))
                ModelState.Merge((ModelStateDictionary)TempData["ModelState"]);

            List<Author> author = db.Authors.ToList();
            List<Genre> genre = db.Genres.ToList();
            ViewBag.authors = author;
            ViewBag.genres = genre;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult AddBook(Book model)
        {
            var FormData = Request.Form;
            if (string.IsNullOrEmpty(FormData["Author"]))
            {
                ModelState.AddModelError("Author", "выберите автора/авторов");
            }
            else
            {
                string e = FormData["Author"];
                string[] elements = e.Split(',');
                int[] authorsIDs = Array.ConvertAll(elements, int.Parse);
                foreach (int item in authorsIDs)
                {
                    Author a = db.Authors.Find(item);
                    model.Authors.Add(a);
                }
            }
            if (string.IsNullOrEmpty(FormData["Genre"]))
            {
                ModelState.AddModelError("Genre", "выберите жанр/жанры");
            }
            else
            {
                string e = FormData["Genre"];
                string[] elements = e.Split(',');
                int[] genresIDs = Array.ConvertAll(elements, int.Parse);
                foreach (int item in genresIDs)
                {
                    Genre g = db.Genres.Find(item);
                    model.Genres.Add(g);
                }
            }
            if (ModelState.IsValid)
            {
                db.Books.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            List<Author> author = db.Authors.ToList();
            List<Genre> genre = db.Genres.ToList();
            ViewBag.authors = author;
            ViewBag.genres = genre;
            ViewBag.showErrorMessage = 1;
            return View(model);
        }

        [Authorize(Roles = "admin")]
        public ActionResult DeleteAuthor(int id)
        {
            Author result = db.Authors.Find(id);
            if (result != null)
            {
                db.Authors.Remove(result);
                db.SaveChanges();
                TempData["messageBox"] = "автор удален";
            }
            return RedirectToAction("AddBook", "Admin");
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult EditAuthor(Author model)
        {
            var FormData = Request.Form;
            if (string.IsNullOrEmpty(FormData["AuthorName"]))
            {
                ModelState.AddModelError("AuthorName", "введите имя автора");
            }
            if (string.IsNullOrEmpty(FormData["AuthorDescription"]))
            {
                ModelState.AddModelError("AuthorDescription", "краткое описание автора обязательно");
            }

            if (ModelState.IsValid)
            {
                Author author = db.Authors.Find(model.Id);
                author.AuthorName = model.AuthorName;
                author.AuthorDescription = model.AuthorDescription;
                db.SaveChanges();
                TempData["messageBox"] = "изменения выполнены";
                return RedirectToAction("AddBook", "Admin");
            }
            else
            {
                TempData["id"] = model.Id;
                TempData["name"] = model.AuthorName;
                TempData["desc"] = model.AuthorDescription;
                TempData["ModelState"] = ModelState;
                TempData["EditAuthorError"] = 1;
                return RedirectToAction("AddBook", "Admin");
            }
            
        }


        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult AddAuthor(Author model)
        {
            var FormData = Request.Form;
            if (string.IsNullOrEmpty(FormData["AuthorName"]))
            {
                ModelState.AddModelError("AuthorName", "введите имя автора");
            }
            if (string.IsNullOrEmpty(FormData["AuthorDescription"]))
            {
                ModelState.AddModelError("AuthorDescription", "краткое описание автора обязательно");
            }

            if (ModelState.IsValid)
            {
                Author author = new Author();
                author = model;
                db.Authors.Add(author);
                db.SaveChanges();
                TempData["messageBox"] = "изменения выполнены";
                return RedirectToAction("AddBook", "Admin");
            }
            else
            {
                TempData["ModelState"] = ModelState;
                TempData["AddAuthorError"] = 1;
                return RedirectToAction("AddBook", "Admin");
            }
        }

        [Authorize(Roles = "admin")]
        public ActionResult DeleteGenre(int id)
        {
            Genre result = db.Genres.Find(id);
            if (result != null)
            {
                db.Genres.Remove(result);
                db.SaveChanges();
                TempData["messageBox"] = "жанр удален";
            }
            return RedirectToAction("AddBook", "Admin");
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult EditGenre(Genre model)
        {
            var FormData = Request.Form;
            if (string.IsNullOrEmpty(FormData["GenreName"]))
            {
                ModelState.AddModelError("GenreName", "введите название жанра");
            }
            if (string.IsNullOrEmpty(FormData["GenreDescription"]))
            {
                ModelState.AddModelError("GenreDescription", "краткое описание для жанра обязательно");
            }

            if (ModelState.IsValid)
            {
                Genre genre = db.Genres.Find(model.Id);
                genre.GenreName = model.GenreName;
                genre.GenreDescription = model.GenreDescription;
                db.SaveChanges();
                TempData["messageBox"] = "изменения выполнены";
                return RedirectToAction("AddBook", "Admin");
            }
            else
            {
                TempData["id"] = model.Id;
                TempData["name"] = model.GenreName;
                TempData["desc"] = model.GenreDescription;
                TempData["ModelState"] = ModelState;
                TempData["EditGenreError"] = 1;
                return RedirectToAction("AddBook", "Admin");
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult AddGenre(Genre model)
        {
            var FormData = Request.Form;
            if (string.IsNullOrEmpty(FormData["GenreName"]))
            {
                ModelState.AddModelError("GenreName", "введите название жанра");
            }
            if (string.IsNullOrEmpty(FormData["GenreDescription"]))
            {
                ModelState.AddModelError("GenreDescription", "краткое описание для жанра обязательно");
            }

            if (ModelState.IsValid)
            {
                Genre genre = new Genre();
                genre = model;
                db.Genres.Add(genre);
                db.SaveChanges();
                TempData["messageBox"] = "изменения выполнены";
                return RedirectToAction("AddBook", "Admin");
            }
            else
            {
                TempData["ModelState"] = ModelState;
                TempData["AddGenreError"] = 1;
                return RedirectToAction("AddBook", "Admin");
            }
        }
    }
}