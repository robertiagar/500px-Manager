using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _500pxManager.Api.Entities
{
    public class Collection
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Position { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Path { get; set; }
        public List<Photo> Photos { get; set; }
    }
}
