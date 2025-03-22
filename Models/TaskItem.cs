using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required]
        public string? Description { get; set; }

        public PriorityLevel Priority { get; set; } = PriorityLevel.Low;

        public bool IsRequired { get; set; } = false;

        public bool IsCompleted { get; set; } = false;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? DueDate { get; set; }

        public DateTime? CompletedDate { get; set; }
    }

    public enum PriorityLevel
    {
        Low = 0,
        Medium = 1,
        High = 2
    }
}
