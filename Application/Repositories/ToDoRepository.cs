using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.Models;

namespace Application.Repositories
{
    public class ToDoRepository : IToDoRepository
    {
        private readonly AppDbContext _appDbContext;
        public ToDoRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<List<ToDo>> GetAll()
        {
            return await _appDbContext.ToDos.ToListAsync();
        }

        public async Task<ToDo?> GetById(int id)
        {
            return await _appDbContext.ToDos.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task Create(ToDo toDo)
        {
            await _appDbContext.ToDos.AddAsync(toDo);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var toDo = await _appDbContext.ToDos.FirstOrDefaultAsync(x => x.Id == id);
            if (toDo == null)
                throw new KeyNotFoundException("Task not found.");

            _appDbContext.ToDos.Remove(toDo);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteConfirm(int id)
        {
            await Delete(id);
        }

        public async Task Edit(int? id, ToDo toDo)
        {
            if (id == null || id != toDo.Id)
                throw new ArgumentException("Invalid ID.");

            var existingToDo = await _appDbContext.ToDos.FirstOrDefaultAsync(x => x.Id == id);
            if (existingToDo == null)
                throw new KeyNotFoundException("Task not found.");

            existingToDo.Title = toDo.Title;
            existingToDo.IsDone = toDo.IsDone;
            existingToDo.Date = toDo.Date;

            _appDbContext.ToDos.Update(existingToDo);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
