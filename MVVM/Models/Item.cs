using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MVVM.Models
{
    public class Item
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
    }
}
