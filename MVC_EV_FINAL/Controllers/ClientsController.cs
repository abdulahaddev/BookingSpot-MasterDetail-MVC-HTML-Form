using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC_EV_FINAL.Models;
using MVC_EV_FINAL.ViewModels;
using Newtonsoft.Json;

namespace MVC_EV_FINAL.Controllers
{
    public class ClientsController : Controller
    {
        private BookingDbContext db = new BookingDbContext();

        // GET: Clients
        public ActionResult Index()
        {
            var clients = db.Clients.Include(c => c.bookingEntries.Select(b => b.Spot)).OrderByDescending(x => x.ClientId).ToList();
            return View(clients);
        }

        public ActionResult AddNewSpot(int? id)
        {
            ViewBag.spots = new SelectList(db.Spots.ToList(), "SpotId", "SpotName", (id != null) ? id.ToString() : "");
            return PartialView("_addNewSpot");
        }
        public ActionResult Create()
        {
                return View();
        }

        [HttpPost]
        public ActionResult Create(ClientVM clientVM, int[] SpotId)
        {
            if (ModelState.IsValid)
            {
                Client client = new Client()
                {
                    ClientName = clientVM.ClientName,
                    BirthDate = clientVM.BirthDate,
                    Age = clientVM.Age,
                    MaritalStatus = clientVM.MaritalStatus
                };

                HttpPostedFileBase file = clientVM.PictureFile;
                if (file != null)
                {
                    string filePath = Path.Combine("/Images/", Guid.NewGuid().ToString() + Path.GetExtension(file.FileName));
                    file.SaveAs(Server.MapPath(filePath));
                    client.Picture = filePath;
                }

                //Save all spot from SpotId[]
                foreach (var item in SpotId)
                {
                    BookingEntry bookingEntry = new BookingEntry()
                    {
                        Client = client,
                        ClientId = client.ClientId, //client will be created and refer the ClientId
                        SpotId = item
                    };
                    db.BookingEntries.Add(bookingEntry);
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }

        public ActionResult Edit(int? id)
        {
            Client client = db.Clients.First(x => x.ClientId == id);
            var clientSpots = db.BookingEntries.Where(x => x.ClientId == id).ToList();

            ClientVM clientVM = new ClientVM()
            {
                ClientId = client.ClientId,
                ClientName = client.ClientName,
                Age = client.Age,
                BirthDate = client.BirthDate,
                Picture = client.Picture,
                MaritalStatus = client.MaritalStatus
            };
            if (clientSpots.Count() > 0)
            {
                foreach (var item in clientSpots)
                {
                    clientVM.SpotList.Add(item.SpotId);
                }
            }
            return View(clientVM);
        }

        [HttpPost]
        public ActionResult Edit(ClientVM clientVM, int[] SpotId)
        {
            if (ModelState.IsValid)
            {
                Client client = new Client()
                {
                    ClientId = clientVM.ClientId,
                    ClientName = clientVM.ClientName,
                    BirthDate = clientVM.BirthDate,
                    Age = clientVM.Age,
                    MaritalStatus = clientVM.MaritalStatus
                };

                HttpPostedFileBase file = clientVM.PictureFile;
                if (file != null)
                {
                    string filePath = Path.Combine("/Images/", Guid.NewGuid().ToString() + Path.GetExtension(file.FileName));
                    file.SaveAs(Server.MapPath(filePath));
                    client.Picture = filePath;
                }
                else
                {
                    client.Picture = "";
                }
                var existsSpotEntry = db.BookingEntries.Where(x => x.ClientId == client.ClientId).ToList();

                //Delete all spot from exist booking
                foreach (var bookingEntry in existsSpotEntry)
                {
                    db.BookingEntries.Remove(bookingEntry);
                }

                //Save all spot from SpotId[]
                foreach (var item in SpotId)
                {
                    BookingEntry bookingEntry = new BookingEntry()
                    {
                        ClientId = client.ClientId,
                        SpotId = item
                    };
                    db.BookingEntries.Add(bookingEntry);
                }

                db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View();
        }
        public ActionResult Delete(int? id)
        {
            var client = db.Clients.Find(id);
            var existsSpotEntry = db.BookingEntries.Where(x => x.ClientId == id).ToList();

            foreach (var bookingEntry in existsSpotEntry)
            {
                db.BookingEntries.Remove(bookingEntry);
            }
            db.Entry(client).State = EntityState.Deleted;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
