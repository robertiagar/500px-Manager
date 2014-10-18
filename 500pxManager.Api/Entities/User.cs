using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _500pxManager.Api.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Fullname { get; set; }
        public string UserpicUrl { get; set; }
        public int UpgradeStatus { get; set; }
        public DateTime? Birthday { get; set; }
        public int Sex { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string About { get; set; }
        public string Domain { get; set; }
        public bool FotomotoOn { get; set; }
        public string Locale { get; set; }
        public bool ShowNude { get; set; }
        public bool StoreOn { get; set; }
        public Contacts contacts { get; set; }
        public Equipment equipment { get; set; }
        public string Fullname { get; set; }
        public string UserpicUrl { get; set; }
        public string Email { get; set; }
        public int PhotosCount { get; set; }
        public int Affection { get; set; }
        public int InFavoritesCount { get; set; }
        public int FriendsCount { get; set; }
        public int FollowersCount { get; set; }
        public int UploadLimit { get; set; }
        public DateTime UploadLimitExpiry { get; set; }
        public DateTime UpgradeStatusExpiry { get; set; }
    }
}
