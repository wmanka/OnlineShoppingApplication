﻿using OnlineShoppingApp.Models;
using OnlineShoppingApp.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using System.Net;
using OnlineShoppingApp.Interfaces;
using OnlineShoppingApp.Repositories;

namespace OnlineShoppingApp.Controllers
{
    public class ItemsController : Controller
    {
        //private ApplicationDbContext context;
        //public ItemsController()
        //{
        //    context = new ApplicationDbContext();
        //}

        private GenericUnitOfWork uow = null;

        public ItemsController()
        {
            uow = new GenericUnitOfWork();
        }

        public ItemsController(GenericUnitOfWork uow)
        {
            this.uow = uow;
        }

        public ActionResult Index()
        {
            var items = uow.Repository<Item>().GetAll().OrderBy(m => m.Name);
            var categories = uow.Repository<Category>().GetAll();

            var viewModel = new ItemsViewModel()
            {
                Items = items,
                Categories = categories
            };

            return View(viewModel);
        }

        public ActionResult Filtered(int? categoryId)
        {
            var items = uow.Repository<Item>().GetAll().Where(m => m.CategoryId == categoryId).OrderBy(m => m.Name);
            var categories = uow.Repository<Category>().GetAll();

            var viewModel = new ItemsViewModel()
            {
                Items = items,
                Categories = categories
            };

            return View("Index", viewModel);
        }

        //[Authorize]
        //public ActionResult MyItems()
        //{
        //    var userId = User.Identity.GetUserId();
        //    var user = context.Users.FirstOrDefault(u => u.Id == userId);
        //    var items = context.Items.Include(m => m.Category).ToList().Where(m => m.User == user);

        //    var viewModel = new ItemsViewModel()
        //    {
        //        Items = items,
        //        Categories = context.Categories.ToList()
        //    };

        //    return View(viewModel);
        //}

        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new ItemFormViewModel()
            {
                Item = new Item(),
                Categories = uow.Repository<Category>().GetAll()
            };

            return View("ItemForm", viewModel);
        }

        //[HttpPost]
        //[Authorize]
        //public ActionResult Save(Item item)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        var viewModel = new ItemFormViewModel()
        //        {
        //            Item = item,
        //            Categories = unitOfWork.Categories.GetAll()
        //        };

        //        return View("ItemForm", viewModel);
        //    }

        //    var userId = User.Identity.GetUserId();
        //    var currentUser = context.Users.Single(u => u.Id == userId);

        //    item.User = currentUser;

        //    if (item.Id == 0)
        //    {
        //        context.Items.Add(item);
        //    }
        //    else
        //    {
        //        var itemInDb = context.Items.Single(i => i.Id == item.Id);
        //        itemInDb.Name = item.Name;
        //        itemInDb.Description = item.Description;
        //        itemInDb.Price = item.Price;
        //        itemInDb.PictureUrl = item.PictureUrl;
        //    }

        //    context.SaveChanges();

        //    return RedirectToAction("Index", "Items");

        //}

        //[Authorize]
        //public ActionResult Edit(int id)
        //{
        //    var item = context.Items.SingleOrDefault(i => i.Id == id);

        //    var userId = User.Identity.GetUserId();
        //    var curentUser = context.Users.Single(u => u.Id == userId);

        //    if (!(item.User == curentUser))
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        //    if (item == null)
        //        return HttpNotFound();

        //    var viewModel = new ItemFormViewModel()
        //    {
        //        Item = item,
        //        Categories = context.Categories.ToList()
        //    };

        //    return View("ItemForm", viewModel);
        //}

        //public ActionResult Details(int? id)
        //{
        //    if(id == null)
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        //    var item = context.Items.Include(i => i.Category).SingleOrDefault(i => i.Id == id);

        //    if (item == null)
        //        return HttpNotFound();

        //    return View(item);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(string searchtext)
        {
            var items = uow.Repository<Item>().GetAll().Where(m => m.Name.Contains(searchtext) || m.Description.Contains(searchtext));

            var viewModel = new ItemsViewModel()
            {
                Categories = uow.Repository<Category>().GetAll(),
                Items = items
            };

            return View("Index", viewModel);
        }
    }
}