using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Books
    {
        public int Id { get; set; } //データベースのキーID
        public string タイトル { get; set; } //文献タイトル
        public string 著者 { get; set; } //文献の著者
        public string 出版社 { get; set; } //文献の出版社

        [DataType(DataType.Date)]
        public DateTime 発行日 { get; set; } //文献の発行日

        public string 分野 { get; set; } //文献の分類
        public string リンク { get; set; } //文献のリンク先
    }
}
