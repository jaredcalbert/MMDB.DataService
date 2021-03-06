﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MMDB.DataService.Data.Metadata;

namespace MMDB.DataService.Web.Controllers
{
    public class ViewManagerController : Controller
    {
		private IDataServiceViewManager ViewManager { get; set; }

		public ViewManagerController(IDataServiceViewManager viewManager)
		{
			this.ViewManager = viewManager;
		}
        //
        // GET: /ViewManager/

        public ActionResult Index()
        {
			var list = this.ViewManager.GetViewList();
			return View(list);
        }

		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Create(string objectTypeName, string viewName, string resourceAssemblyName, string resourceIdentifier)
		{
			this.ViewManager.CreateView(objectTypeName, viewName, resourceAssemblyName, resourceIdentifier);
			System.Web.HttpContext.Current.Cache.Remove(DataServicePathProvider.CacheKey);
			return RedirectToAction("Index");
		}

		public ActionResult Edit(int id)
		{
			var item = this.ViewManager.GetView(id);
			System.Web.HttpContext.Current.Cache.Remove(DataServicePathProvider.CacheKey);
			return View(item);
		}

		[HttpPost]
		public ActionResult Edit(int id, string objectTypeName, string viewName, string resourceAssemblyName, string resourceIdentifier)
		{
			this.ViewManager.UpdateView(id, objectTypeName, viewName, resourceAssemblyName, resourceIdentifier);
			System.Web.HttpContext.Current.Cache.Remove(DataServicePathProvider.CacheKey);
			return RedirectToAction("Index");
		}
	}
}
