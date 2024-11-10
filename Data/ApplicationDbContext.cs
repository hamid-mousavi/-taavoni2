using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Taavoni.Models;
using Taavoni.Models.Entities;


namespace Taavoni.Data;

public  class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
        public DbSet<Debt> Debts { get; set; }  // اضافه کردن DbSet
        public DbSet<FixedDebt> FixedDebts { get; set; }  // اضافه کردن DbSet
        public DbSet<Payment> Payments { get; set; }


            
    protected override void OnModelCreating(ModelBuilder builder)
    {

        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>()
            .HasMany(u => u.Debts)
            .WithOne(d => d.User)
            .HasForeignKey(d => d.UserId);
            
    // رابطه بین User و Payment
    builder.Entity<Payment>()
        .HasOne(p => p.User)
        .WithMany(u => u.payments)
        .HasForeignKey(p => p.UserId);

        
    // رابطه بین Debt و Payment
    builder.Entity<Payment>()
        .HasOne(p => p.Debt)
        .WithMany(d => d.Payments)
        .HasForeignKey(p => p.DebtId);
            
    }
}
