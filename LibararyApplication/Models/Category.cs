﻿namespace LibararyApplication.Models

{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<Book> books { get; set; }
    }
}