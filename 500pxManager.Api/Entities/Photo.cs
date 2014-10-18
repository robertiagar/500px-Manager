using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _500pxManager.Api.Entities
{
    public class Photo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? TimesViewed { get; set; }
        public double Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public Category Category { get; set; }
        public bool? Privacy { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public int VotesCount { get; set; }
        public int? FavoritesCount { get; set; }
        public int CommentsCount { get; set; }
        public bool NSFW { get; set; }
        public string ImageUrl { get; set; }
        public User User { get; set; }
    }
}
