using Microsoft.AspNetCore.Identity;

namespace BlogProject.Models.Identity
{
    //public class AppUser : IdentityUser // default key string oluşacaktır...
    // User tablomuzun primary key'i int olması için...
    public class AppUser : IdentityUser<int>
    {
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string PicturePath { get; set; }
    }
}