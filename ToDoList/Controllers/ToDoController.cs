using Microsoft.AspNetCore.Mvc;
using ToDoList.Data;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class ToDoController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public ToDoController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        // متد Index برای نمایش لیست وظایف
        public IActionResult Index()
        {
            var toDos = _appDbContext.ToDos.ToList();
            return View(toDos);
        }

        // متد Create (GET)
        public IActionResult Create()
        {
            return View();
        }

        // متد Create (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ToDo toDo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _appDbContext.ToDos.Add(toDo);
                    _appDbContext.SaveChanges();
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

        // متد Edit (GET)
        public IActionResult Edit(int? id)
        {
            if (id == null || _appDbContext.ToDos == null)
            {
                return NotFound();
            }

            var toDo = _appDbContext.ToDos.FirstOrDefault(x => x.Id == id);
            if (toDo == null)
            {
                return NotFound();
            }

            return View(toDo);
        }

        // متد Edit (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int? id, ToDo toDo)
        {
            if (id == null || id != toDo.Id)
            {
                return NotFound();
            }

            var existingToDo = _appDbContext.ToDos.FirstOrDefault(x => x.Id == id);
            if (existingToDo == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    existingToDo.Title = toDo.Title;
                    existingToDo.IsDone = toDo.IsDone;
                    existingToDo.Date = toDo.Date; // ذخیره زمان سررسید

                    _appDbContext.ToDos.Update(existingToDo);
                    _appDbContext.SaveChanges();

                    TempData["SuccessMessage"] = "Task updated successfully!";
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    TempData["ErrorMessage"] = "An error occurred while updating the task.";
                    return View(toDo);
                }
            }

            return View(toDo);
        }

        // متد Delete (GET)
        public IActionResult Delete(int id)
        {
            if (id <= 0 || _appDbContext.ToDos == null)
            {
                return RedirectToAction("Index");
            }

            var toDo = _appDbContext.ToDos.FirstOrDefault(x => x.Id == id);
            if (toDo == null)
            {
                return NotFound();
            }

            return View(toDo);
        }

        // متد Delete (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public IActionResult DeleteConfirm(int id)
        {
            if (id <= 0 || _appDbContext.ToDos == null)
            {
                return NotFound();
            }

            var toDo = _appDbContext.ToDos.FirstOrDefault(x => x.Id == id);
            if (toDo == null)
            {
                TempData["ErrorMessage"] = "Task not found.";
                return NotFound();
            }

            try
            {
                _appDbContext.ToDos.Remove(toDo);
                _appDbContext.SaveChanges();

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
