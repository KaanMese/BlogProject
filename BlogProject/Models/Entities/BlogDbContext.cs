using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogProject.Models.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogProject.Models.Entities
{
    public class BlogDbContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Content> Contents { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Subscribe> Subscribe { get; set; }
    }
}
