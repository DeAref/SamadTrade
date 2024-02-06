using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using samadApp.Models;
using samadApp.Models.DataTransportObject;
using ZXing;

//using ZXing.Common;
using System.Drawing;
using System;
//using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Globalization;
//using ZXing.QrCode;


namespace samadApp.Controllers;

public class HomeController : Controller
{
    private readonly MvcFprojectContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly UserManager<Users> _userManager;
    private readonly SignInManager<Users> _signInManager;
    private readonly ILogger<HomeController> _logger;
 
    public HomeController(ILogger<HomeController> logger,IWebHostEnvironment webHostEnvironment,MvcFprojectContext mvcFprojectContext,UserManager<Users> userManager)
    {
        _webHostEnvironment = webHostEnvironment;
        _context = mvcFprojectContext;
        _userManager = userManager;
        _logger = logger;
    }

    //genrate uniq name for pictures 
      public string GenerateUniqueImageName(string originalFileName)
    {
        string extension = originalFileName.Substring(originalFileName.LastIndexOf('.'));
        string uniqueName = Guid.NewGuid().ToString("N") + extension;
        return uniqueName;
    }


//convet jalali to miladi
    static string PersianToGregorian(string persianDate)
    {
        PersianCalendar persianCulture = new PersianCalendar();
        var parts = persianDate.Split('/');
        int year = Convert.ToInt32(parts[0]);
        int month = Convert.ToInt32(parts[1]);
        int day = Convert.ToInt32(parts[2]);
        DateTime dt = persianCulture.ToDateTime(year, month, day,0,0,0,0);

        return string.Join("-", dt.Year, dt.Month, dt.Day);
    }

    // convert Qrcode to string (decode)
    public string DecodeQrCode(string imagePath)
    {
        var reader = new BarcodeReaderGeneric();
        Bitmap image = (Bitmap)Image.FromFile(imagePath);
        using(image){
            LuminanceSource source;
            source = new ZXing.Windows.Compatibility.BitmapLuminanceSource(image);
            Result result = reader.Decode(source);
            if (result == null || string.IsNullOrEmpty(result.Text))
            {
                return "-1";
            }else{
                return result.Text;
            }
           
        }
    }

    public IActionResult Index()
    {
        if(User.Identity.IsAuthenticated){
            return View();
        }else{
            return RedirectToAction("Login","Accounting");
        }
        
    }
    
     public IActionResult Sell(){
        if(User.Identity.IsAuthenticated){
            return View();
        }else{
            return RedirectToAction("Login","Accounting");
        }
     }
    [HttpPost]
    public async Task<IActionResult> Sell(IFormFile ImageQrCode,SellDto sellDto){
        
        if(User.Identity.IsAuthenticated){
            Users user = await _userManager.GetUserAsync(User);
            if (ImageQrCode != null && ImageQrCode.Length > 0)
            {
                string File_Name = GenerateUniqueImageName(ImageQrCode.FileName);
                var path = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot/images",
                    File_Name);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await ImageQrCode.CopyToAsync(stream);
                }
                string QrCodeData = DecodeQrCode(path);
                if(QrCodeData != "-1"){
                    Sell newSell = new Sell(){
                    EmployeeUserId = user.Id,
                    FoodName = sellDto.FoodName,
                    QrCode = QrCodeData,
                    Date = sellDto.Date.Date,
                    College = sellDto.College  
                    };
                    await _context.AddAsync(newSell);
                    await _context.SaveChangesAsync();

                    //Delete Image after input on DB
                    FileInfo file = new FileInfo(path); 
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                    return RedirectToAction("Index","Home");
                }else{
                    TempData["errorQrcode"]="بارکد مشکل دارد (برسی کنید که بارکد در تصویر معلوم باشد یا تصویر ناقص نباشد)";
                    return View();
                }
               
            }else{
                TempData["errorPicture"] = "بارکد به درستی آپلود نشده است یا پسوند نامعتبر دارد";
                return View();
            }

            
        }else{
            return RedirectToAction("Login","Accounting");
        }
    }

  
    public IActionResult Buy(string day)
    {
        if(User.Identity.IsAuthenticated){
           
            if(day != null){
            // var convertedDate = PersianToGregorian(day);
                DateTime DateForCompire = DateTime.Parse(day);
                ViewBag.FoodDate = DateForCompire.ToString();
                var matchedData = _context.Sell
                    .Where(x => x.Date == DateForCompire)
                    .GroupBy(y =>  y.FoodName)
                    .Select(g=>new {FoodName = g.Key})
                    .ToList();
                ViewBag.FoodsForSell = matchedData;
            }
        
            return View();
        }else{
            return RedirectToAction("Login","Accounting");
        }
    }
    [HttpPost]
    public async Task<IActionResult> Pay(string NameOfFood,string DateOfFood){
        if(User.Identity.IsAuthenticated){
            //  if(ModelState.IsValid == false){
            //     return View("Buy");
            // }
            Users user = await _userManager.GetUserAsync(User);
            if(NameOfFood != null && DateOfFood != null){
                var queryoffood = _context.Sell
                                                .Where(x=>x.CustomerUserId == null)
                                                .Where(x=>x.Date == DateTime.Parse(DateOfFood))
                                                .Where(x=>x.FoodName == NameOfFood)
                                                .ToList();
                if(queryoffood.Count() != 0){
                    if(user.Wallet != null){
                        if(int.Parse(user.Wallet) >= 15000){
                            var selectedfood = queryoffood.First();
                            selectedfood.CustomerUserId = user.Id;
                            selectedfood.IsSold = true;
                            user.Wallet = (int.Parse(user.Wallet) - 15000).ToString();
                            _context.SaveChanges();
                            ViewBag.QrCode=selectedfood.QrCode;
                            TempData["buyResult"] = "خرید با موفقیت انجام شد (همینطور میتوانید در پروفایل خود مشاهده کنید)";
                        }else{
                            TempData["buyResult"] = "موجودی حساب شما کافی نیست (اقدام به شارژ حساب کنید)";
                        }
                    }else{
                        TempData["buyResult"] = "موجودی کافی نیست";
                    }
                }else{
                    TempData["buyResult"] = "این کد غذا به اتمام رسیده است ، لطفا بعد از چند دقیقه دوباره مراجعه کنید";
                    return View("Buy");
                }
            }else{
                TempData["buyResult"] = DateOfFood + NameOfFood;
                return View();
            }
            
        }else{
            return RedirectToAction("Login","Accounting");
        }
        return View();
    }
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
