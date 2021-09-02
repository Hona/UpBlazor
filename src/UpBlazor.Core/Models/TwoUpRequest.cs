namespace UpBlazor.Core.Models
{
    public class TwoUpRequest
    {
        public string MartenId => RequesterId + RequesteeId;
        public string RequesterId { get; set; }
        public string RequesteeId { get; set; }
    }
}