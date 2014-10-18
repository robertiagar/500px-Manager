using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _500pxManager.Api.Entities
{
    public class UserRoot
    {
        public User User { get; set; }
    }

    public class User
    {
        public int id { get; set; }
        public string username { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public object birthday { get; set; }
        public object sex { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string registration_date { get; set; }
        public string about { get; set; }
        public int usertype { get; set; }
        public string domain { get; set; }
        public bool fotomoto_on { get; set; }
        public string locale { get; set; }
        public bool show_nude { get; set; }
        public int allow_sale_requests { get; set; }
        public string fullname { get; set; }
        public string userpic_url { get; set; }
        public string userpic_https_url { get; set; }
        public int upgrade_status { get; set; }
        public bool store_on { get; set; }
        public int photos_count { get; set; }
        public int affection { get; set; }
        public int in_favorites_count { get; set; }
        public int friends_count { get; set; }
        public int followers_count { get; set; }
        public object analytics_code { get; set; }
        public string email { get; set; }
        public int upload_limit { get; set; }
        public string upload_limit_expiry { get; set; }
        public int upgrade_type { get; set; }
        public string upgrade_status_expiry { get; set; }
        public bool presubmit_for_licensing { get; set; }
        public Contacts contacts { get; set; }
        public Equipment equipment { get; set; }
    }

}
