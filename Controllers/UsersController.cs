using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shape.Contracts;
using Shape.Models;

namespace Shape.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserRepository userRepo, ILogger<UsersController> logger)
        {
            userRepository = userRepo;
            _logger = logger;
        }


        // GET: Users/
        //GET :
        public IActionResult Index()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("uid")))
                return RedirectToAction("Index", "Home");

            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("Firstname,Lastname,Email,Password,ConfirmPassword")] UserRegistration userRegistration)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    
                    User user = await userRepository.SignUpAsync(userRegistration.SetPasswordHash().ApplyTextualFormatting());
                   
                    HttpContext.Session.SetString("uid", user.Id.ToString());
                    HttpContext.Session.SetString("name", user.Firstname);

                    return RedirectToAction(nameof(Index), "Home");
                }
            }
            catch(Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("Cannot insert duplicate key row in object 'dbo.User' with unique index 'UniqueEmail'", StringComparison.OrdinalIgnoreCase))
                    ModelState.AddModelError("Email", "A user already exists for this email. Please use another email or try login.");
                else
                {
                    string mesage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    TempData["ErrorMessage"] = mesage;
                    _logger.LogError(ex, mesage, Array.Empty<object?>());
                }
            }
            return View(userRegistration);
        }
    }
}
