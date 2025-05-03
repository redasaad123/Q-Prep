namespace ProjectAPI.DTO
{
    public class GetSaveQuestionDTO
    {
        public string Id { get; set; }
        
        public string questionId { get; set; }
        public string UserId { get; set; }



        public string Question { get; set; }

        public string? Answer { get; set; }
   
    }
}
