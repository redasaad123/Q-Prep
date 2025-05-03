namespace ProjectAPI.DTO
{
    public class UpdatePostDTO
    {
        public string? header { get; set; }
        public string? Text { get; set; }
        public List<IFormFile>? Images { get; set; }
    }
}
