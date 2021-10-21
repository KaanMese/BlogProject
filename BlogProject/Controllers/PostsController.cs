using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlogProject.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BlogProject.Models.Identity;
using BlogProject.Models;

namespace BlogProject.Controllers
{
    public class PostsController : Controller
    {
        BlogDbContext dbContext;
        UserManager<AppUser> userManager;
        
        public PostsController(BlogDbContext _blogDbContext,UserManager<AppUser> _userManager)
        {
            dbContext = _blogDbContext;
            userManager = _userManager;
        }

        // KategoriID gönderecğeiz..
        public IActionResult Index(int ID)
        {
            var result = dbContext.Contents.Include(c => c.Comments).Where(c => c.CategoryID == ID).ToList();

            return View(result);
        }

        // ContentID'yi gönderecğeiz...
        public IActionResult Detail(int ID)
        {
            var result = dbContext.Contents
                .Include(c => c.Comments)
                .ThenInclude(c=>(c as Comment).User)
                .Include(c => c.User)
                .Where(c => c.ID == ID)
                .FirstOrDefault();
                //.FirstOrDefault(c => c.ID == ID); // ID'si gönderilen Cntent

            // okuma sayısını arttırılaım...
            result.ViewCount = result.ViewCount + 1;

            dbContext.SaveChanges(); // değişikliği kaydet...

            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> AddComment(string text,int contentId)
        {
            JsonResponse response = new JsonResponse();
            try
            {
                // kullanıcı bulunur...
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                Comment comment = new Comment();
                comment.UserID = user.Id;
                comment.Text = text;
                comment.CreDate = DateTime.Now;
                comment.ContentID = contentId;
                comment.IsApprove = false;

                dbContext.Comments.Add(comment);
                dbContext.SaveChanges();

                response.Code = OperationStatu.Success;
                response.Message = "Tebrikler yorumunuz eklendi. Yönetici tarafından onaylandıktan sonra yayınlanacaktır.";

            }
            catch(Exception)
            {
                response.Code = OperationStatu.Error;
                response.Message = "Yorum yaparken bir hata oluştu.Lğtfen tekrar gödnerin ..";
            }
            return Json(response);
        }
    }
}