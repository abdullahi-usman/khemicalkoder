using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using System;
using System.ComponentModel.DataAnnotations;

namespace KhemicalKoder.Models
{
    public class Article
    {
        [Required] [JsonProperty(PropertyName = "id")] public string id { set; get; }

        [JsonProperty(PropertyName = "Date")] public DateTime Date { set; get; }
        [Required] [JsonProperty(PropertyName = "Title")] public string Title { set; get; }

        [Required] [DataType(DataType.Html)] [JsonProperty(PropertyName = "Story")] public string Story { set; get; }
    }
}