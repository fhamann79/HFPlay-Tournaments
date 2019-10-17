using Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Backend.Models
{
    public class NewsItemView : NewsItem
    {
        [Display(Name = "Imagen")]
        public HttpPostedFileBase PictureFile { get; set; }
    }
}