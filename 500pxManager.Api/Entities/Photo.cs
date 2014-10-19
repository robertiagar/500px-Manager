using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _500pxManager.Api.Extensions;

namespace _500pxManager.Api.Entities
{
    public class Photo
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string camera { get; set; }
        public object lens { get; set; }
        public object focal_length { get; set; }
        public string iso { get; set; }
        public object shutter_speed { get; set; }
        public object aperture { get; set; }
        public int times_viewed { get; set; }
        public double rating { get; set; }
        public int status { get; set; }
        public string created_at { get; set; }
        public int category { get; set; }
        public string Category { get { return ((Category)category).ToString().ToWords(); } }
        public object location { get; set; }
        public bool privacy { get; set; }
        public object latitude { get; set; }
        public object longitude { get; set; }
        public string taken_at { get; set; }
        public int hi_res_uploaded { get; set; }
        public bool for_sale { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int votes_count { get; set; }
        public int favorites_count { get; set; }
        public int comments_count { get; set; }
        public int sales_count { get; set; }
        public object for_sale_date { get; set; }
        public double highest_rating { get; set; }
        public object highest_rating_date { get; set; }
        public int license_type { get; set; }
        public int converted { get; set; }
        public string image_url { get; set; }
        public IList<string> tags { get; set; }
    }
}
