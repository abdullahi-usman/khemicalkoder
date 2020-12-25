using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using System;
using System.ComponentModel.DataAnnotations;

namespace KhemicalKoder.Models
{
    public class Article
    {
        [Required] [DataType(DataType.Text)] public string id { set; get; }

        [DataType(DataType.DateTime)] public DateTime Date { set; get; }
        [Required] [DataType(DataType.Text)]public string Title { set; get; }

        [Required] [DataType(DataType.Html)]  public string Story { set; get; }
    }
}