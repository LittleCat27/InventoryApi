using InventoryApi.Models.Clases;
using InventoryApi.Models;
using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace InventoryApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(ILogger<UsuarioController> logger)
        {
            _logger = logger;
        }

        [HttpPost("Login")]
        public JsonResult Login(Login login)
        {
            UsuarioCLS usuario = new UsuarioCLS();
            
            using(var db = new InventoryDBContext())
            {
                var userN = (from user in db.Usuarios
                             where (user.Username == login.user.ToLower() || user.Mail == login.user.ToLower())
                             && user.Password == login.password
                        select new UsuarioCLS
                        {
                            UserId = user.UserId,
                            Mail = user.Mail,
                            Username = user.Username,
                            LoginOK = 1
                        }).FirstOrDefault();

                if (userN != null) usuario = userN;
            }

            return new JsonResult(usuario);
        }

        [HttpPost("Register")]
        public ActionResult Register(Register r)
        {
            if (r == null) return StatusCode(401, "Register failed, null object");
            if (string.IsNullOrEmpty(r.user) || string.IsNullOrEmpty(r.mail) ||string.IsNullOrEmpty(r.password)) return StatusCode(401, "Register failed, some parameters are null or empty");
            try
            {
                using (var db = new InventoryDBContext())
                {

                    var userN = (from user in db.Usuarios
                                 where user.Username == r.user.ToLower() || user.Mail == r.mail.ToLower()
                                 select new UsuarioCLS
                                 {
                                     UserId = user.UserId,
                                     Mail = user.Mail,
                                     Username = user.Username,
                                     LoginOK = 1
                                 }).ToList();
                    if (userN == null || userN.Count > 0) return StatusCode(401,"Username or e-mail already in use");

                    Usuario usuario = new Usuario
                    {
                        Mail = r.mail.ToLower(),
                        Password = r.password,
                        Username = r.user.ToLower()

                    };
                    db.Usuarios.Add(usuario);
                    db.SaveChanges();
                }

                return Ok("Succesfull Registry");
            }
            catch (Exception ex)
            {
                return StatusCode(401, ex.Message);
            }
            
        }
    }
}