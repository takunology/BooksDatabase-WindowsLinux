using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class BooksFieldView
    {
        public List<Books> Books { get; set; }
        public string タイトル { get; set;}
        public string 著者 { get; set; }
        public string 出版社 { get; set; }

        [DataType(DataType.Date)]
        public DateTime 発行日 { get; set; }

        public SelectList 分野 { get; set; }
        public string リンク { get; set; }
        public string BookField { get; set; }
        public string Keywords { get; set; }
    }
}