using ApiCrud.Estudantes;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace ApiCrud.Data;

public class AppDbContext : DbContext
{
    private DbSet<Estudante> Estudantes { get; set; }

    public AppDbContext()
    {
        Batteries.Init();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=Banco.sqlite");
        base.OnConfiguring(optionsBuilder);
    }
}
