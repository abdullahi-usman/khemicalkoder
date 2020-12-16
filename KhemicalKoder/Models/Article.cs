using Microsoft.AspNetCore.Mvc;

using System;
using System.ComponentModel.DataAnnotations;

namespace KhemicalKoder.Models
{
    public class Article
    {
        [Required] public string Id { set; get; }

        public DateTime Date { set; get; }

        [Required] public string Title { set; get; }

        [Required] [DataType(DataType.Html)] public string Story { set; get; }
    }
}