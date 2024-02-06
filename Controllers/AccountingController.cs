using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using samadApp.Models.DataTransportObject;
using samadApp.Models;
namespace samadApp.Controllers
{
    // [Route("[controller]")]
    public class AccountingController : Controller
    {
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;
        private readonly ILogger<AccountingController> _logger;

        public AccountingController(ILogger<AccountingController> logger,SignInManager<Users> signInManager,UserManager<Users> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult RegisterAction(){
            return View("Register");
        }
        [HttpPost]
        public IActionResult RegisterAction(Register register){
           
            if(ModelState.IsValid == false){
                return View("Register");
            }
            Users newUser= new Users(){
                Fname= register.Fname,
                Lname= register.Lname,
                Email = register.Email,
                UserName = register.Email,
                College = register.College
            };
           
            var result = _userManager.CreateAsync(newUser,register.Password).Result;
            if (result.Succeeded){
                return RedirectToAction("Index","Home");
            }
            string messageRegister = "";
            foreach (var item in result.Errors.ToList())
            {
                messageRegister += item.Description + Environment.NewLine;
            }
            TempData["MessageRegister"] = messageRegister;
            return View("Register");
        }


        [HttpGet]
        public IActionResult Login(){
            return View();
        } 

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto){
            if(!ModelState.IsValid){
                return View(loginDto);
            }
            var user = _userManager.FindByNameAsync(loginDto.UserName).Result;
            if(user == null){
                TempData["MessageLogin"] = "نام کاربری یا کلمه عبور اشتباه است";
            }
            _signInManager.SignOutAsync();
            
            //login proccess 
            var result = _signInManager.PasswordSignInAsync(loginDto.UserName,loginDto.Password,loginDto.RememmberMe,true).Result;
        
             
            if(result.Succeeded){
                ViewBag.LogedinUserName = user.Fname +" "+user.Lname;
                return RedirectToAction("Index","Home");
            }else{
                TempData["MessageLogin"] = "نام کاربری یا کلمه عبور اشتباه است";
                if(result.IsLockedOut){
                    ModelState.AddModelError(string.Empty," اکانت شما قفل شده است ");
                    TempData["MessageLogin"] = " اکانت شما قفل شده است ";
                } 
                ModelState.AddModelError(string.Empty,"مشکل در ورود");
                return View(loginDto);
               
            }

            
           

           

            //return View(loginDto);
            
        } 

        public IActionResult Logout(){
            _signInManager.SignOutAsync();
            //RedirectToAction("Login","Accounting");
            return View("Login");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}