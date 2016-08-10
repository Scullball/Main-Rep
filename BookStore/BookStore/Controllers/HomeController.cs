using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;
using System.Collections.Specialized;
using System.Text;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        private BookStoreContext db = new BookStoreContext();


        //Рендер главной страницы.
        //Вывод ошибок регистрации(перебрасывает объект model state после редиректа (/Account/Register)).
        //Принимает cookie и обновляет счетчики общих цены и кол-ва в корзине.
        public ActionResult Index()
        {
            if (TempData.ContainsKey("ModelState"))
                ModelState.Merge((ModelStateDictionary)TempData["ModelState"]);

            if (Request.Cookies["cart"] != null)
            {
                Session["totalBooksQuantity"] = Request.Cookies["cart"]["totalBooksQuantity"];
                Session["totalPrice"] = Request.Cookies["cart"]["totalPrice"];
            }
            else
            {
                Session["totalBooksQuantity"] = 0;
                Session["totalPrice"] = 0;
            }


            ViewBag.Books = db.Books.ToList();
            return View();
        }

        public ActionResult About(int id)
        {
            ViewBag.Books = db.Books.Where(b => b.Id == id).ToList();
            return View();
        }

        //Строка поиска (_LogotypePartial).
        //Поиск в бд и рендер представления about.cshtml со всем найденным
        [HttpPost]
        public ActionResult Search(string query)
        {
            query = query.Trim();
            if (!String.IsNullOrEmpty(query))
            {
                var result = db.Books.Where(b => b.Title.StartsWith(query + " ") ||
                                            b.Title.EndsWith(" " + query) ||
                                            b.Title.Contains(" " + query + " ") ||
                                            b.Title.ToLower().Equals(query.ToLower()) ||
                                                            b.Authors.Any(a => a.AuthorName.StartsWith(query + " ")) ||
                                                            b.Authors.Any(a => a.AuthorName.EndsWith(" " + query)) ||
                                                            b.Authors.Any(a => a.AuthorName.Contains(" " + query + " ")) ||
                                                            b.Authors.Any(a => a.AuthorName.ToLower().Equals(query.ToLower())) ||
                                                                                b.Genres.Any(g => g.GenreName.StartsWith(query + " ")) ||
                                                                                b.Genres.Any(g => g.GenreName.EndsWith(" " + query)) ||
                                                                                b.Genres.Any(g => g.GenreName.Contains(" " + query + " ")) ||
                                                                                b.Genres.Any(g => g.GenreName.ToLower().Equals(query.ToLower()))).ToList();
                if (result.Any())
                {
                    Book b = result.First();
                    if (b.Authors.Any(a => a.AuthorName.ToLower().Contains(query.ToLower())))
                    {
                        foreach(Author a in b.Authors)
                        {
                            if (a.AuthorName.ToLower().Contains(query.ToLower()))
                            {
                                ViewBag.searchTitle = a.AuthorName;
                                ViewBag.searchDesc = a.AuthorDescription;
                            }
                        }
                    }
                    if (b.Genres.Any(g => g.GenreName.ToLower().Contains(query.ToLower())))
                    {
                        foreach(Genre g in b.Genres)
                        {
                            if (g.GenreName.ToLower().Contains(query.ToLower()))
                            {
                                ViewBag.searchTitle = g.GenreName;
                                ViewBag.searchDesc = g.GenreDescription;
                            }
                        }
                    }  
                }
                ViewBag.Books = result;
            }                                                        
            return View("About");
        }

        //Рендер представления cart.cshtml
        public ActionResult Cart()
        {
            if (Request.Cookies["cart"] != null)
            {
                Dictionary<Book, int> booksDictionary = new Dictionary<Book, int>();
                Dictionary<string, string> TaggedBooks = new Dictionary<string, string>();
                TaggedBooks = GetCookie("cart");
                foreach (KeyValuePair<string, string> item in TaggedBooks)
                {
                    int id;
                    bool result = int.TryParse(item.Key, out id);
                    if (result)
                    {
                        Book book = db.Books.Find(id);
                        if (book != null)
                        {
                            int bookQuantity = int.Parse(item.Value);
                            booksDictionary.Add(book, bookQuantity);
                        }
                    }
                }
                if(booksDictionary.Count == 0)
                {
                    ViewBag.Books = null;
                }
                else
                {
                    ViewBag.Books = booksDictionary;
                }
                
            }
            return View();
        }

        //Срабаьывает при нажатии кнопки "в корзину"(ajax) на index.cshtml и about.cshtml
        //Добавляет в корзину по одной книге за вызов.
        public ActionResult InCart(int id, int price)
        {

            if (Request.Cookies["cart"] == null)
            {
                Dictionary<string, string> TaggedBooks = new Dictionary<string, string>();
                string sId = id.ToString();
                TaggedBooks.Add(sId, "1");
                SetCookie("cart", TaggedBooks, price.ToString());
                Session["totalBooksQuantity"] = 1;
                Session["totalPrice"] = price;
            }
            else
            {
                Dictionary<string, string> TaggedBooks = new Dictionary<string, string>();
                TaggedBooks = GetCookie("cart");
                string sId = id.ToString();
                if (TaggedBooks.ContainsKey(sId))
                {
                    int val = int.Parse(TaggedBooks[sId]);
                    val++;
                    TaggedBooks[sId] = val.ToString();
                }
                else
                {
                    TaggedBooks.Add(sId, "1");
                }
                int total = int.Parse(TaggedBooks["totalPrice"]);
                total = total + price;
                string totalPrice = total.ToString();
                Session["totalPrice"] = total;
                int count = int.Parse(TaggedBooks["totalBooksQuantity"]);
                count++;
                Session["totalBooksQuantity"] = count;
                string counter = count.ToString();
                SetCookie("cart", TaggedBooks, totalPrice, counter);
            }          
            return PartialView("_CartPartial");
        }
        //Срабатывает при вводе в поле input на cart.cshtml.(AJAX)
        //Добавляет в корзину по указанное кол-во книг за вызов и обновляет счетчики общей цены и количества.
        public ActionResult InCartFromInput(int id, int price, int BookQuantity)
        {
            Dictionary<string, string> TaggedBooks = new Dictionary<string, string>();
            TaggedBooks = GetCookie("cart");
            string sId = id.ToString();
            int OldBookQuantity = int.Parse(TaggedBooks[sId]);
            TaggedBooks[sId] = BookQuantity.ToString();
            int total = int.Parse(TaggedBooks["totalPrice"]);
            total = total + price*(BookQuantity - OldBookQuantity);
            string totalPrice = total.ToString();
            Session["totalPrice"] = total;
            int count = int.Parse(TaggedBooks["totalBooksQuantity"]);
            count = count + (BookQuantity - OldBookQuantity);
            Session["totalBooksQuantity"] = count;
            string counter = count.ToString();
            SetCookie("cart", TaggedBooks, totalPrice, counter);
            return PartialView("_CartPartial");
        }

        //Срабатывает при клике на соответствующую ссылку в Cart.cshtml
        //Удаляет указанное кол-во книг из корзины Cart.cshtml и обновляет счетчики общей цены и количества
        public ActionResult DeleteFromCart(int id, int price)
        {
            Dictionary<string, string> TaggedBooks = new Dictionary<string, string>();
            TaggedBooks = GetCookie("cart");
            int BookQuantity = int.Parse(TaggedBooks[id.ToString()]);
            int totalQuantity = int.Parse(TaggedBooks["totalBooksQuantity"]);
            totalQuantity = totalQuantity - BookQuantity;
            TaggedBooks["totalBooksQuantity"] = totalQuantity.ToString();
            Session["totalBooksQuantity"] = totalQuantity;
            int total = int.Parse(TaggedBooks["totalPrice"]);
            total = total - (price * BookQuantity);
            TaggedBooks["totalPrice"] = total.ToString();
            Session["totalPrice"] = total;
            TaggedBooks.Remove(id.ToString());
            SetCookie("cart", TaggedBooks, total.ToString(), totalQuantity.ToString());
            return RedirectToAction("Cart", "Home");
        }
        //Срабатывает при клике на кнопку "оформить заказ" в Cart.cshtml
        //Записывает в бд список заказанных книг ввиде строки "id книги"="количество"
        public ActionResult Buy()
        {
            string customerName = User.Identity.Name;
            Dictionary<string, string> TaggedBooks = new Dictionary<string, string>();
            TaggedBooks = GetCookie("cart");
            StringBuilder builder = new StringBuilder();
            int result = 0;
            foreach (KeyValuePair<string, string> item in TaggedBooks)
            {
                if (int.TryParse(item.Key, out result))
                {
                    builder.Append(item.Key).Append("=").Append(item.Value).Append(" ");
                }
            }
            string purchasedBooks = builder.ToString();
            Purchase model = new Purchase();
            model.Customer = customerName;
            model.PurchasedBooks = purchasedBooks;
            model.Date = DateTime.Now;
            db.Purshases.Add(model);
            //db.SaveChanges();
            return View("ThankForPurchase");
        }

        //Отсылает файл Cookie где ключ - id книги в базе, а значение - количество.
        private void SetCookie(string cookieName, Dictionary<string, string> dictionary, string total, string counter = "1")
        {
            HttpCookie cookie = new HttpCookie(cookieName);
            foreach (KeyValuePair<string, string> item in dictionary)
            {
                cookie[item.Key] = item.Value;
            }
            cookie["totalPrice"] = total;
            cookie["totalBooksQuantity"] = counter;
            cookie.Expires = DateTime.Now.AddHours(1);
            Response.Cookies.Add(cookie);
        }

        //Принимает файл Cookie и распаковывает его в словарь: ключ-id, значение-количество.
        private Dictionary<string, string> GetCookie(string cookieName)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            if (Request.Cookies[cookieName] != null)
            {
                HttpCookie cookie = Request.Cookies[cookieName];
                NameValueCollection nameValueCollection = cookie.Values;
                foreach (string key in nameValueCollection.AllKeys)
                {
                    dictionary.Add(key, cookie[key]);
                }
            }
            return dictionary;
        }    
    }
}