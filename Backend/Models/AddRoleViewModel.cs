using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Backend.Models
{
    public class AddRoleViewModel
    {
        [Key]
        public int AddRoleViewModelId { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }
    }
}