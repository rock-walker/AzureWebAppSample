﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;
using log4net.Appender;
using log4net.Core;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        //private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly AzureTableAppender appender = new AzureTableAppender();
        BookContext db = new BookContext();

        public HomeController()
        {
            appender.ActivateOptions();
        }

	    public ActionResult Index()
        {
            appender.DoAppend(new LoggingEvent(
                    new LoggingEventData
                    {
                        Domain = "testDomain",
                        Identity = "testIdentity",
                        Level = Level.Critical,
                        LoggerName = "testLoggerName",
                        Message = "testMessage",
                        ThreadName = "testThreadName",
                        TimeStamp = DateTime.UtcNow,
                        UserName = "testUsername",
                        LocationInfo = new LocationInfo("className", "methodName", "fileName", "lineNumber")
                    }
                ));
            // получаем из бд все объекты Book
            IEnumerable<Book> books = db.Books;
            return View(books);
        }

        [HttpGet]
        public ActionResult Buy(int id)
        {
            ViewBag.BookId = id;
            ViewBag.Message = "Это вызов частичного представления из обычного";
            SelectList books = new SelectList(db.Books, "Author", "Name");
            return View(books);
        }

        [HttpGet]
        public ActionResult BookView(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Book book = db.Books.Find(id);
            if (book != null)
            {
                return View(book);
            }
            return HttpNotFound();
        }


        [HttpGet]
        public ActionResult EditBook(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Book book = db.Books.Find(id);
            if (book != null)
            {
                return View(book);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult EditBook(Book book)
        {
            db.Entry(book).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public string Buy(Purchase purchase)
        {
            purchase.Date = DateTime.Now;
            // добавляем информацию о покупке в базу данных
            db.Purchases.Add(purchase);
            // сохраняем в бд все изменения
            db.SaveChanges();
            return "Спасибо," + purchase.Person + ", за покупку!";
        }

        public ActionResult Partial()
        {
            ViewBag.Message = "Это вызов частичного представления из обычного";
            return PartialView();
        }


        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Book book)
        {
            db.Entry(book).State = EntityState.Added;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            Book b = db.Books.Find(id);
            if (b == null)
            {
                return HttpNotFound();
            }
            return View(b);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Book b = db.Books.Find(id);
            if (b == null)
            {
                return HttpNotFound();
            }
            db.Books.Remove(b);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

		protected override void Dispose(bool disposing)
		{
			if (db != null)
			{
				db.Dispose();
				db = null;
			}
			base.Dispose(disposing);
		}
	}
}