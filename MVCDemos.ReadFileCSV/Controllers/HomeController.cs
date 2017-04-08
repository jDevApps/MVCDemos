using MVCDemos.ReadFileCSV.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace MVCDemos.ReadFileCSV.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(new List<CustomerModel>());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase postedFile)
        {
            var customers = new List<CustomerModel>();
            string filePath = string.Empty;

            if (postedFile != null)
            {
                string path = Server.MapPath("~/Uploads/");
                // Se valida si existe el diretorio y sino se crea
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                filePath = Path.Combine(path, Path.GetFileName(postedFile.FileName));
                string extension = Path.GetExtension(postedFile.FileName);
                postedFile.SaveAs(filePath);

                string csvData = System.IO.File.ReadAllText(filePath);
                var dataCollection = csvData.Split('\n');

                foreach (var row in dataCollection)
                {
                    if (!string.IsNullOrEmpty(row))
                    {
                        customers.Add(new CustomerModel
                        {
                            Id = Convert.ToInt32(row.Split(',')[0]),
                            Name = row.Split(',')[1],
                            Country = row.Split(',')[2]
                        });
                    }
                }
            }

            return View(customers);
        }
    }
}