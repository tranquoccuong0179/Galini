using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galini.Models.Payload.Response.UserInfo
{
    public class CreateUserInfoResponse
    {
        public Guid Id { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public Guid PremiumId { get; set; }
        public string Type { get; set; }
        public int Friend {  get; set; }
        public bool TimeLimit { get; set; }
        public int Match {  get; set; }
        public double Price { get; set; }
        public Guid AccountId { get; set; }
        public string Role { get; set; } 
        public string FullName { get; set; } 
        public string Email { get; set; } 
        public string Phone { get; set; } 
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } 
        public string? AvatarUrl { get; set; }
        public int? Duration { get; set; }
    }
}
