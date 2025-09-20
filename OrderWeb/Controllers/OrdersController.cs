using Microsoft.AspNetCore.Mvc;
using OrderWeb.Data;
using OrderWeb.Models;
using System;
using System.Linq;


namespace OrderWeb.Controllers
{
    public class OrdersController : Controller
    {
        private readonly OrderDbContext _db;
        public OrdersController(OrderDbContext db) { _db = db; }

        public IActionResult Index()
        {
            var list = _db.Orders.OrderByDescending(o => o.CreatedAt).ToList();
            return View(list);
        }

        [HttpGet]
        public IActionResult Create() => View(new Order());

        [HttpPost]
        public IActionResult Create(Order model)
        {
            if (!ModelState.IsValid) return View(model);
            model.CreatedAt = DateTime.Now;
            model.Status = "Pending";
            _db.Orders.Add(model);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult MarkReady(int id)
        {
            var order = _db.Orders.Find(id);
            if (order != null)
            {
                order.Status = "Ready";
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
