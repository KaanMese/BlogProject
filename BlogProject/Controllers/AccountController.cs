using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BlogProject.Models;
using BlogProject.Models.Entities;
using BlogProject.Models.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Controllers
{
    public class AccountController : Controller
    {
        UserManager<AppUser> userManager; // kullanıcı yönetimi  UserTablo
        RoleManager<AppRole> roleManager; // role yönetimi // Role tablosu
        SignInManager<AppUser> signInManager; // oturum yönetimi...
        IWebHostEnvironment environment;
        BlogDbContext blogDbContext;

        public AccountController(UserManager<AppUser> um, RoleManager<AppRole> rl, SignInManager<AppUser> sm, IWebHostEnvironment whe, BlogDbContext bdc)
        {
            userManager = um; // intance runtime'da startup dosyasındaki ConfigureServices meotundan alınarak otomatik olarak buraya gönderilir.. Bunun nedeni .net core'da dependency injection ve IOC mantığı vardır.. 
            roleManager = rl;
            signInManager = sm;
            environment = whe;
            blogDbContext = bdc;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AccountUserVM model)
        {

            // modelden gelen email ile kullanıcı buluyoruz...
            AppUser user = await userManager.FindByEmailAsync(model.Email); // email ile kullanıcı buluyoruz...

            if (user == null)
            {
                ViewData["Mesaj"] = "Email adresiniz hatalı";
                return View();
            }
            var result = await signInManager.PasswordSignInAsync(user, model.Password, false, false);

            if (result.Succeeded) // Başarılıysa...
            {
                return RedirectToAction("Index", "Home"); // anasayfaya yönlendir..
            }
            else
            {
                ViewData["Mesaj"] = "Şifreniz hatalı.";
            }

            return View();
        }

        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(AccountUserVM model)
        {

            // Rol ekleyeceğiz..
            if (!await roleManager.RoleExistsAsync("user")) // user rolü var mı?? veritabanından kontrol eder...
            {
                AppRole role = new AppRole();
                role.Name = "user";

                await roleManager.CreateAsync(role); // veritabanına rolü ekle.... //AspNetRoles tablosuna kayıt atar..
            }

            AppUser user = new AppUser();
            user.UserName = model.Email;
            user.Email = model.Email;
            user.FullName = model.FullName;

            var result = await userManager.CreateAsync(user, model.Password); // kullanıcıyı veritbanaına ekle... //AspNetUsers tablosuna kayıt atar..

            if (result.Succeeded) // kayıt yapıldıysa....
            {
                var roleResult = await userManager.AddToRoleAsync(user, "user"); // Kullanıcıya rol ekle.... //AspNetUserROles tablosuna kayıt atar...

                // View'a mesaj gönderiyoruz..
                // ViewData => Controllerden view'a veya viewdan view veri göndermek için kullanılır..Sadece bir sonraki request için çalışır..
                ViewData["Mesaj"] = "Kayıt İşleminiz Başarılı.";
            }
            else
            {
                ViewData["Mesaj"] = "Kayıt işleminiz yapılamadı.. Lütfen bilgilerinizi kontrol ediniz...";
            }
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync(); // kullanıcı oturumunu kapat
            return RedirectToAction("Index", "Home"); // anasayfaya yönlendir...
        }

        public IActionResult UpdateProfile()
        {
            return View();
        }

        // action'a formdan dosya verisi göndercekseniz parametre tipiniz IFormFile olmalıdır
        [HttpPost]
        public IActionResult UpdatePhoto(IFormFile file)
        {

            string fileName = Guid.NewGuid().ToString();  // guid bir değer veriyirouz
            string extension = Path.GetExtension(file.FileName); // dosya isminden uzantıyı buluyoruz...
            string newFileName = fileName + extension;

            // dosyayı fiziki klasöre yazıyoruz...
            var path = Path.Combine(environment.WebRootPath, "UploadProfilePicture/") + newFileName;
            FileStream st = new FileStream(path, FileMode.Create);
            file.CopyTo(st);

            // Dosyanın adını kullanıcın PicturePath alanına yazıyoruz...
            AppUser user = blogDbContext.Users.FirstOrDefault(c => c.Email == User.Identity.Name);
            user.PicturePath = newFileName;
            blogDbContext.SaveChanges();


            return View("UpdateProfile");
        }

        public ActionResult ProfilePath()
        {
            string filePicturePath = "/assets/images/profile.png";

            if (User.Identity.IsAuthenticated)
            {
                var user = blogDbContext.Users.FirstOrDefault(c => c.Email == User.Identity.Name);
                if (user.PicturePath != null) // profil resmi yüklediyse....
                {
                    filePicturePath = "UploadProfilePicture/" + user.PicturePath;
                }
            }

            ViewData["photoPath"] = filePicturePath;
            return PartialView("_profilePicture");
        }
    }
}