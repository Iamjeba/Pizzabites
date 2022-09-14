using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using Pizzabites.Models;

namespace Pizzabites.Controllers
{
    public class AdminController : Controller
    {
        PizzabitesEntities db = new PizzabitesEntities();

        // GET: Admin
        public ActionResult Index()
        {
            return RedirectToAction("AdminPanel");
        }
        public ActionResult AdminPanel()
        {

#pragma warning disable CS0252 // Possible unintended reference comparison; left hand side needs cast
            if (Session["CUSEmail"] == null || Session["CUSEmail"] != "Pizzabites@gmail.com")
            {
                ViewBag.AdminLogin = "Admin needs to log in";
            }
#pragma warning restore CS0252 // Possible unintended reference comparison; left hand side needs cast

            return View();
        }

        public ActionResult UserTable()
        {

#pragma warning disable CS0252 // Possible unintended reference comparison; left hand side needs cast
            if (Session["CUSEmail"] == null || Session["CUSEmail"] != "Pizzabites@gmail.com")
            {
                ViewBag.AdminLogin = "Admin needs to log in";
            }
#pragma warning restore CS0252 // Possible unintended reference comparison; left hand side needs cast

            List<Customer> customer = db.Customers.ToList();
            return View(customer);
        }

        public ActionResult AddItem()
        {

#pragma warning disable CS0252 // Possible unintended reference comparison; left hand side needs cast
            if (Session["CUSEmail"] == null || Session["CUSEmail"] != "Pizzabites@gmail.com")
            {
                ViewBag.AdminLogin = "Admin needs to log in";
            }
#pragma warning restore CS0252 // Possible unintended reference comparison; left hand side needs cast

            return View();
        }
        public ActionResult ContactUsTable()
        {

#pragma warning disable CS0252 // Possible unintended reference comparison; left hand side needs cast
            if (Session["CUSEmail"] == null || Session["CUSEmail"] != "Pizzabites@gmail.com")
            {
                ViewBag.AdminLogin = "Admin needs to log in";
            }
#pragma warning restore CS0252 // Possible unintended reference comparison; left hand side needs cast

            List<ContactU> contactUs = db.ContactUs.ToList();
            return View(contactUs);
        }

        public ActionResult RemoveUser(Customer cus)
        {

            Customer customer = db.Customers.Where(temp => temp.CUSEmail.Equals(cus.CUSEmail)).SingleOrDefault();
            db.Customers.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("UserTable");
        }

        public ActionResult RemoveContact(ContactU contact)
        {
            ContactU contactU = db.ContactUs.Where(temp => temp.ContactID.Equals(contact.ContactID)).SingleOrDefault();
            db.ContactUs.Remove(contactU);
            db.SaveChanges();
            return RedirectToAction("ContactUsTable");
        }

        [HttpPost]
        public ActionResult AdminContact(ContactU contact, FormCollection form)
        {
            string x = form["AdminReply"];
            ContactU contactU = db.ContactUs.Where(temp => temp.ContactID.Equals(contact.ContactID)).SingleOrDefault();
            db.ContactUs.Remove(contactU);
            db.SaveChanges();
            contactU.AdminReply = x;
            db.ContactUs.Add(contactU);
            db.SaveChanges();
            return RedirectToAction("ContactUsTable");
        }

        public ActionResult ItemAdd(Product pro)
        {
            string pic = System.IO.Path.GetFileName(pro.PRImage);
            //string path = Path.Combine(Server.MapPath("~/images"), pic);
            //pro.PRImage.SaveAs(path);
          
            Product product = new Product
            {
                PRName = pro.PRName,
                PRDescription = pro.PRDescription,
                PRPrize = pro.PRPrize,
                PRType = pro.PRType,
                PRImage = pro.PRImage
            };

            var sql = "Selete * from Product where PRName = '" + product.PRName + "'";

            if(db.Products.SqlQuery(sql).ToList() != null)
            {
                sql = "Update Product Set PRImage = '" + product.PRImage + "', PRType = '" + product.PRType + "', PRDescription = '" + product.PRDescription + "', PRPrize = " +product.PRPrize;
                db.Products.SqlQuery(sql);
            }
            else
            {
                db.Products.Add(product);
                db.SaveChanges();
            }           
            
            return RedirectToAction("AddItem");
        }
        public ActionResult ItemEdit(Product pro)
        {
            string pic = System.IO.Path.GetFileName(pro.PRImage);
            string path = Path.Combine(Server.MapPath("~/images"), pic);
            //pro.PRImage.SaveAs(path);

            Product product = new Product
            {
                PRName = pro.PRName,
                PRDescription = pro.PRDescription,
                PRPrize = pro.PRPrize,
                PRType = pro.PRType,
                PRImage = pro.PRImage
            };

            db.Products.Add(product);
            db.SaveChanges();
            return RedirectToAction("AddItem");
        }

        [HttpPost]
        public ActionResult ItemDelete(FormCollection form)
        {
            string prname = form["prName"];
            //string pic = System.IO.Path.GetFileName(pro.PRImage);
            var sql = "";
            //string path = Path.Combine(Server.MapPath("~/images"), pic);
            //pro.PRImage.SaveAs(path);

            /*Product product = new Product
            {
                PRName = pro.PRName
                //PRDescription = pro.PRDescription,
                //PRPrize = pro.PRPrize,
                //PRType = pro.PRType,
                //PRImage = pro.PRImage
          
            };*/
            if (prname != null)
            {
                sql = "DELETE FROM PRODUCT WHERE PRNAME = '" + prname + "'";
            }
            db.Products.SqlQuery(sql);
/*
            db.Products.Remove(product);
            db.SaveChanges();*/
            return RedirectToAction("AddItem");
        }
    }
}