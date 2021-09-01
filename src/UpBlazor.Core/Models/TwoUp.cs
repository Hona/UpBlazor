namespace UpBlazor.Core.Models
{
    public class TwoUp
    {
        public string MartenId => UserId1 + UserId2;
        public string UserId1 { get; set; }
        public string UserId2 { get; set; }
    }
}