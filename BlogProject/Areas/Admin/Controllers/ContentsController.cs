using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogProject.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using BlogProject.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System.Net.Mail;
using System.Net;

namespace BlogProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize] // Sadece üyeler autorize olabilir
    public class ContentsController : Controller
    {
        private readonly BlogDbContext _context;

        public UserManager<AppUser> _userManager;
        public ContentsController(BlogDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Admin/Contents
        public async Task<IActionResult> Index()
        {
            var blogDbContext = _context.Contents.Include(c => c.Category).Include(c => c.User);
            return View(await blogDbContext.ToListAsync());
        }

        // GET: Admin/Contents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var content = await _context.Contents
                .Include(c => c.Category)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (content == null)
            {
                return NotFound();
            }

            return View(content);
        }

        // GET: Admin/Contents/Create
        public IActionResult Create()
        {
            ViewData["CategoryID"] = new SelectList(_context.Categories, "ID", "Name");
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Admin/Contents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Title,ShortText,Text,CategoryID")] Content content)
        {
            if (ModelState.IsValid)
            {
                // context sınıfı üzerinden kullanıcı bilgilerini çektikk..
                //AppUser user = _context.Users.FirstOrDefault(c => c.UserName == User.Identity.Name);

                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                content.UserID = user.Id;  // Contetin UserID alanına User'ın ID'sini set ediyoruz...

                content.CreDate = DateTime.Now;
                _context.Add(content);
                await _context.SaveChangesAsync();

                // Email Gönder....
                // Email gönderilecek kullanıcı listesini bul...
                List<Subscribe> subs = _context.Subscribe.ToList(); // email listesi...
                foreach (var item in subs)
                {
                    string icerik = "<h1>Fırından Taze çıktı</h1><br /> <a href='http://blogprojectbt.com/Posts/'> " + content.Title + " </a>";
                    MailMessage message = new MailMessage("btblogproject1@gmail.com", item.EmailAdress, "Yeni İçerik", icerik);
                    message.IsBodyHtml = true;

                    // makaleleri listele..
                    using (SmtpClient smtp = new SmtpClient())
                    {
                        smtp.Host = "smtp.gmail.com";
                        smtp.EnableSsl = true;
                        NetworkCredential NetworkCred = new NetworkCredential("btblogproject1@gmail.com", "bt1234bt");
                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = NetworkCred;
                        smtp.Port = 587;
                        smtp.Send(message);
                    }
                }

                return RedirectToAction(nameof(Index)); // index actionına yönlendirmişş....
            }

            ViewData["CategoryID"] = new SelectList(_context.Categories, "ID", "Name", content.CategoryID);

            return View(content);
        }

        // GET: Admin/Contents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var content = await _context.Contents.FindAsync(id);
            if (content == null)
            {
                return NotFound();
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "ID", "Name", content.CategoryID);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", content.UserID);
            return View(content);
        }

        // POST: Admin/Contents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,ShortText,Text,CategoryID")] Content content)
        {
            if (id != content.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(content);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContentExists(content.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "ID", "Name", content.CategoryID);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", content.UserID);
            return View(content);
        }

        // GET: Admin/Contents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var content = await _context.Contents
                .Include(c => c.Category)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (content == null)
            {
                return NotFound();
            }

            return View(content);
        }

        // POST: Admin/Contents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var content = await _context.Contents.FindAsync(id);
            _context.Contents.Remove(content);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContentExists(int id)
        {
            return _context.Contents.Any(e => e.ID == id);
        }
    }
}
