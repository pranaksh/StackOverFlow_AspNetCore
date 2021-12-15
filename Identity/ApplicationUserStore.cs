using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Stack.Identity
{
    public class ApplicationUserStore : UserStore<ApplicationUser>
    {
        public ApplicationUserStore(ApplicationDbContext dbContext) : base(dbContext)
        {

        }
    }
}
