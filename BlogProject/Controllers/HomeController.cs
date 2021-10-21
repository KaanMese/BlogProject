using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogProject.Models;
using BlogProject.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogProject.Controllers
{
    public class HomeController : Controller
    {
        BlogDbContext dbContext;
        public HomeController(BlogDbContext _dbContenxt)
        {
            dbContext = _dbContenxt;
        }
        public IActionResult Index()
        {
            // take t-sqldeki top ifadesidir...
            // select top 10 * from Contents;


            // Include => ilişkili verileri getirmek için kullanılır
            var result = dbContext.Contents.Include(c => c.Comments).OrderByDescending(c => c.ViewCount).Take(10)
                .ToList();



            return View(result);
        }

        public IActionResult PostSubscribe(string semail1)
        {
            JsonResponse response = new JsonResponse();
            response.Code = OperationStatu.Success;
            response.Message = "Tebrik ederiz. Artı sizde bizim bir abonemizsiniz...";

            try
            {
                if (String.IsNullOrWhiteSpace(semail1)) // değer null veya boşluk ise...
                {
                    response.Code = OperationStatu.Error;
                    response.Message = "Lütfen Email Alanını KOntrol ediniz..";
                    return Json(response); //metodu terket..
                }

                // Email var mi? bakacaz...
                Subscribe abone = dbContext.Subscribe.FirstOrDefault(c => c.EmailAdress == semail1.ToLower());
                if (abone != null)
                {
                    // kullanıcıya hata dön...
                    response.Code = OperationStatu.Error;
                    response.Message = "Bu Email Adresi zaten kayıtlı. Lütfen tekrar deneyin";
                }
                else
                {
                    abone = new Subscribe();
                    abone.CreDate = DateTime.Now;
                    abone.EmailAdress = semail1.ToLower();

                    dbContext.Subscribe.Add(abone);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                response.Code = OperationStatu.Error;
                response.Message = "Bir hata oluştu. Lütfen tekrar deneyiniz";
            }
            return Json(response);
        }
    }
}