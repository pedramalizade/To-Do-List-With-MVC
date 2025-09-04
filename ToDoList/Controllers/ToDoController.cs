using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class ToDoController : Controller
    {
        private readonly IToDoRepository _toDoRepository;

        public ToDoController(IToDoRepository toDoRepository)
        {
            _toDoRepository = toDoRepository;
        }

        public async Task<IActionResult> Index()
        {
            var toDos = await _toDoRepository.GetAll();
            return View(toDos);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ToDo toDo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _toDoRepository.Create(toDo);
                    TempData["SuccessMessage"] = "Task created successfully!";
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    TempData["ErrorMessage"] = "An error occurred while creating the task.";
                }
            }
            return View(toDo);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var toDo = await _toDoRepository.GetById(id.Value);
            if (toDo == null)
                return NotFound();

            return View(toDo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, ToDo toDo)
        {
            if (id == null || id != toDo.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _toDoRepository.Edit(id, toDo);
                    TempData["SuccessMessage"] = "Task updated successfully!";
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    TempData["ErrorMessage"] = "An error occurred while updating the task.";
                }
            }

            return View(toDo);
        }


        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return RedirectToAction("Index");

            var toDo = await _toDoRepository.GetById(id);
            if (toDo == null)
                return NotFound();

            return View(toDo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            if (id <= 0)
                return NotFound();

            try
            {
                await _toDoRepository.DeleteConfirm(id);
                TempData["SuccessMessage"] = "Task deleted successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the task.";
                return RedirectToAction("Index");
            }
        }
    }
}
