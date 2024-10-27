using Backend.Interfaces.Repositories;
using Backend.Models.Entities;

namespace Backend.Repositories
{
    public class SudokuRepository(SudokuDbContext dbContext) : 
        BaseRepository<SudokuBoard>(dbContext), 
        ISudokuRepository
    {
    }
}
