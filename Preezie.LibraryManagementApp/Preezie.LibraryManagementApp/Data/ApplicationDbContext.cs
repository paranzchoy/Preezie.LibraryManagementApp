using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Preezie.LibraryManagementApp.Models;

namespace Preezie.LibraryManagementApp.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
    : IdentityDbContext<ApplicationUser>(options)
{
    public virtual DbSet<Book> Books { get; set; }
}
