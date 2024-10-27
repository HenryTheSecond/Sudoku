using Backend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend;

public class SudokuDbContext : DbContext
{
    public SudokuDbContext(DbContextOptions<SudokuDbContext> options) : base(options) { }

    public DbSet<SudokuBoard> SudokuBoards { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<SudokuBoard>().Property(x => x.Board).HasColumnType("varchar(256)");
    }
}
