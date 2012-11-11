using ShakrLabs.Choice.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShakrLabs.Choice.Web.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        static readonly IChoiceRepository _repository = new ChoiceRepository();

        public ActionResult Index()
        {            
            return View(_repository.GetAll());
        }

        //
        // GET: /Admin/Details/5

        public ActionResult Details(Guid id)
        {            
            return View(_repository.Get(id));
        }

        //
        // GET: /Admin/Create

        public ActionResult Create()
        {
           
            return View();
        }

        //
        // POST: /Admin/Create

        [HttpPost]
        public ActionResult Create(ChoiceViewModel model)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View(_repository.Add(model));
            }
        }

        //
        // GET: /Admin/Edit/5

        public ActionResult Edit(Guid id)
        {
            return View(_repository.Get(id));
        }

        //
        // POST: /Admin/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, ChoiceViewModel model)
        {
            try
            {
                // TODO: Add update logic here
                _repository.Update(model);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Admin/Delete/5

        public ActionResult Delete(Guid id)
        {
            return View();
        }

        //
        // POST: /Admin/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
