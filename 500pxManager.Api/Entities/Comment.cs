using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _500pxManager.Api.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ToWhomUserId { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ParentId { get; set; }
        public bool Flagged { get; set; }
        public int Rating { get; set; }
        public bool Voted { get; set; }
        public User User { get; set; }
    }
}
