using System;

namespace UpBlazor.Core.Models
{
    public class RegisteredUser
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string GivenName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}