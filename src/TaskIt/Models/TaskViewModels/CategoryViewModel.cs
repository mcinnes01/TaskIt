using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskIt.Models.TaskViewModels
{
    [Bind(new string[] {"CategoryId", "Title", "Colour", "Tasks", "UserName", "User"})]
    public class CategoryViewModel
    {
        [Key]
        public int CategoryId
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string Colour
        {
            get;
            set;
        }

        public List<TaskViewModel> Tasks
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
