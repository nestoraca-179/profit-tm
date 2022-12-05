using ProfitTM.Models;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ProfitTM.Controllers
{
    public class EmpresaController : Controller
    {
        public ActionResult Index(string message = "")
        {
            ViewBag.user = Session["USER"];
            ViewBag.connect = Session["CONNECT"];
            ViewBag.modules = Session["MODULES"];
            ViewBag.product = "Administrativo";

            if (ViewBag.user == null)
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", "Home", new { message = "Debes iniciar sesión" });
            }
            else if (ViewBag.connect == null)
            {
                return RedirectToAction("Logout", "Account", new { msg = "Debes elegir una empresa" });
            }
            else
            {
                ViewBag.message = message;
                ViewBag.data_conn = Session["DATA_CONN"].ToString();
                ViewBag.bran_conn = Session["BRAN_CONN"].ToString();
                ViewBag.conn = Connection.GetConnByID(Session["ID_CONN"].ToString());

                return View();
            }
        }

        [HttpPost]
        public ActionResult GuardarDatos(string name, string rif, string phone, string address, HttpPostedFileBase image)
        {
            Connections conn = Connection.GetConnByID(Session["ID_CONN"].ToString());

            conn.Name = name;
            conn.RIF = rif;
            conn.Phone = phone;
            conn.Address = address;

            if (image != null)
            {
                string ext = Path.GetExtension(image.FileName).ToLower();

                if (ext == ".jpg" || ext == ".jpeg" || ext == ".png")
                {
                    string folder = Server.MapPath("~") + "images\\" + conn.ID + "\\";
                    string img = image.FileName;
                    string path = folder + img;

                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);
                    
                    image.SaveAs(path);
                    conn.Image = "\\images\\" + conn.ID + "\\" + img;
                }
                else
                {
                    string msg = "Debes subir un archivo de extensión .jpg o .png";
                    return RedirectToAction("Index", new { message = msg });
                }
            }

            ProfitTMResponse result = Connection.Edit(conn);

            if (result.Status == "OK")
            {
                string msg = "OK";
                return RedirectToAction("Index", new { message = msg });
            }
            else
            {
                string msg = "Ha ocurrido un error al modificar la conexión => " + result.Message;
                return RedirectToAction("Index", new { message = msg });
            }
        }
    }
}