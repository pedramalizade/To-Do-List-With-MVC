using ToDoList.Models;

namespace Domain.Interfaces
{
    public interface IToDoRepository
    {
        Task<List<ToDo>> GetAll();
        Task<ToDo?> GetById(int id);
        Task Create(ToDo toDo);
        Task Edit(int? id, ToDo toDo);
        Task Delete(int id);
        Task DeleteConfirm(int id);
    }
}
