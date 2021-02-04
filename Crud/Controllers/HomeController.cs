using Crud.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Crud.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

       public ActionResult GetData()
        {
            using (DBModel db = new DBModel())
            {
                List<Employee> employe = db.Employee.ToList<Employee>();
                return Json(new { data = employe }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (DBModel db = new DBModel())
            {
                Employee emp = db.Employee.Where(x => x.EmployeeID == id).FirstOrDefault<Employee>();
                db.Employee.Remove(emp);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult Ajouter(int id = 0)
        {
            if (id == 0)
                return View(new Employee());
            else
            {
                using (DBModel db = new DBModel())
                {
                    return View(db.Employee.Where(x => x.EmployeeID == id).FirstOrDefault<Employee>());
                }
            }

            
        }

        [HttpPost]
        public ActionResult Ajouter(Employee emp)
        {
            using (DBModel db = new DBModel())
            {
                if (emp.EmployeeID == 0)
                {
                    db.Employee.Add(emp);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Add Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(emp).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Modified" }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public void Exporter()

        {
            using (DBModel db = new DBModel())
            {
                StringWriter client = new StringWriter();
                client.WriteLine("\"EmployeeID\",\"Name\",\"Office\",\"Position\",\"Salary\"");
                Response.ClearContent();
                Response.AddHeader("content-disposition", string.Format("attachement;filename=Liste Personnels Sayna {0}.txt", DateTime.Now));
                Response.ContentType = "text/txt";
                var listClients = db.Employee.OrderBy(x => x.EmployeeID);
                foreach (var news in listClients)
                {
                    client.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\"",
                        news.EmployeeID, news.Name, news.Office, news.Position, news.Salary));
                }
                Response.Write(client.ToString());
                Response.End();
            }
        }
    }
}