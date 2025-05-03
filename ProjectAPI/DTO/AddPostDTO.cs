namespace ProjectAPI.DTO
{
    public class AddPostDTO
    {
        public string? header { get; set; }
        public string? TypeOfBody { get; set; }
        public string groupId { get; set; }
        public string? Text { get; set; }
        public List< IFormFile>? Images { get; set; }
    }
}
