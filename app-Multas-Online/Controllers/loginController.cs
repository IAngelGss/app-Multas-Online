using app_Multas_Online.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace app_Multas_Online.Controllers
{
    public class loginController : Controller
    {
        // GET: login
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(csUsuario user)
        {
            // Validación simple (luego puedes conectar a BD)
            if (user.username == "admin" && user.password_hash == "1234")
            {
                Session["Usuario"] = user.username;
                return RedirectToAction("Index", "Home");

            }

            ViewBag.Mensaje = "Credenciales incorrectas";
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
