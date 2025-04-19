using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public class ToDo
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is Required")]
        public string Title { get; set; } = string.Empty;
        [DataType(DataType.MultilineText)]
        public string Details { get; set; } = string.Empty;
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }
        [Display(Name = "Is Done")]
        public bool IsDone { get; set; }
    }
}
