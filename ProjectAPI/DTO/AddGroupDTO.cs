namespace ProjectAPI.DTO
{
    public class AddGroupDTO
    {
        public string GroupName { get; set; }
        public IFormFile? Photo { get; set; }
        public string? Description { get; set; }

    }
}
