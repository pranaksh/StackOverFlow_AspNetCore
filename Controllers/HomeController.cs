using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Stack.Models;
using Stack.ViewModels;
using System.Web.Helpers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using StackOverFlow.Services;
using StackOverFlowData.Models;
using Grpc.Net.Client;
using GrpcService1;

namespace Stack.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        IUserService _userService;
        IQuestionService _questionService;
        public HomeController(ILogger<HomeController> logger , IUserService userService , IQuestionService questionService)
        {
            _logger = logger;
            _userService = userService;
            _questionService = questionService;
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterViewModel rvm,IFormFile P)
        {
            if (ModelState.IsValid)
            {
                byte[] data = null;
                if(P!=null)
                {
                    byte[] p1 = null;
                    using (var fs1 = P.OpenReadStream())
                    using (var ms1 = new MemoryStream())
                    {
                        fs1.CopyTo(ms1);
                        p1 = ms1.ToArray();
                    }
                    data = p1;
                }
                var check = _userService.FindUser(rvm.Username);
                var passwordHash = Crypto.HashPassword(rvm.Password);
                if (check == null)
                {
                    var user = new User() { UName = rvm.Username, UEmail = rvm.Email, UMobile = rvm.Mobile, UPassword = passwordHash ,UPhoto=data};
                    _userService.SaveUser(user);
                    ViewBag.message = "Success";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.message = "User Already Registered";
                    return View();
                }
            }
            else
            {
                return View();
            }
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel lvm)
        {
            if (ModelState.IsValid)
            {
                var user = _userService.FindUser(lvm.Username,lvm.Password);
                if (user == null)
                {
                    ViewBag.message = "Can't Find You";
                    return View();
                }

            }
            var claims = new List<Claim> {
            new Claim(ClaimTypes.Name, lvm.Username),
            new Claim("UserDefined", "whatever")};

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                       principal,
                       new AuthenticationProperties { IsPersistent = true });
            return RedirectToAction("Index");
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var q = _userService.FindUser(id);
            return View(q);
        }


        [HttpPost]
        public IActionResult Edit(User q,IFormFile P)
        {
            var u = _userService.FindUser(q.UId);
            if (u.UName != q.UName)
            {
                var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, q.UName),
                new Claim("UserDefined", "whatever")};

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                           principal,
                           new AuthenticationProperties { IsPersistent = true });
            }

            _userService.Edit(q,P);


            return RedirectToAction("Profiles");
        }

        public IActionResult Index()
        {
            ViewBag.Title = "Index";
            return View();
        }

        public ActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Profiles()
        {
            var user=_userService.FindUser(User.Identity.Name);
            ViewBag.User = user;
            ViewBag.questions = _questionService.ShowAll();
            return View(user);
        }


        public void GetUsers()
        {
            var channel = GrpcChannel.ForAddress("https://localhost:7187");
            //var client= new Greeter.GreeterClient(channel);
            //var Request = new HelloRequest { Name = "ajay" };
            //var response = client.SayHello(Request);
            //Console.WriteLine(response.Message);
            //Console.ReadKey();
            var client = new crud.crudClient(channel);
            var Request = new request { };
            var response = client.Show(Request);
            foreach (var i in response.Listofusers)
            {
                Console.WriteLine(i.UName);
                Console.WriteLine(i.UEmail);
                Console.WriteLine(i.UMobile);
            }
        }
        
        public IActionResult Chat()
        {
            return View();
        }


        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
    }
}