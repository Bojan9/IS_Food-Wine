using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using bojan_recipe.Models;

namespace bojan_recipe.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<bojan_recipe.Models.Recipe>? Recipe { get; set; }
        public DbSet<bojan_recipe.Models.Tutorial>? Tutorial { get; set; }
        public DbSet<bojan_recipe.Models.Gallery>? Gallery { get; set; }
    }
}