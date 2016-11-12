using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskIt.Models.Enums;

namespace TaskIt.Models.TaskViewModels
{
    public class TaskViewModel
    {
        [Key]
        public int TaskId
        {
            get;
            set;
        }

        [Required]
        public string Title
        {
            get;
            set;
        }

        public string Body
        {
            get;
            set;
        }

        public DateTime Created
        {
            get;
            set;
        }

        public Status State
        {
            get;
            set;
        }

        [ForeignKey("CategoryId")]
        public int CategoryId
        {
            get;
            set;
        }

        public CategoryViewModel Category
        {
            get;
            set;
        }

        [ForeignKey("UserName")]
        public string UserName
        {
            get;
            set;
        }

        public ApplicationUser User
        {
            get;
            set;
        }
    }
}
