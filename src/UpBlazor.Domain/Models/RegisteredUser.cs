using System;

namespace UpBlazor.Domain.Models
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