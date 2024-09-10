using ApiCrud.Estudantes;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace ApiCrud.Data;

public class AppDbContext : DbContext
{
    public DbSet<Estudante> Estudantes { get; set; }

    public AppDbContext()
    {
        Batteries.Init();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=Banco.sqlite");
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
        optionsBuilder.EnableSensitiveDataLogging();
        
        base.OnConfiguring(optionsBuilder);
    }
}
