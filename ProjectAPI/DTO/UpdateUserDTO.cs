namespace ProjectAPI.DTO
{
    public class UpdateUserDTO : UserDTO
    {
        public IFormFile? Photo { get; set; }
    }
}
