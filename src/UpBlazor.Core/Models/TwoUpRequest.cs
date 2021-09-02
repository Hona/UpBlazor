using System;

namespace UpBlazor.Core.Models
{
    public class TwoUpRequest
    {
        public string MartenId => RequesterId + RequesteeId;
        public string RequesterId { get; set; }
        public string RequesterName { get; set; }
        public string RequesterMessage { get; set; }
        public string RequesteeId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}