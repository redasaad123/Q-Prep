namespace ProjectAPI.DTO
{
    public class UpdateQuestionInTestDTO
    {
        public string Q_Id { get; set; }
        public string? Qeuestion { get; set; }
        public string? A_1 { get; set; }
        public string? A_2 { get; set; }
        public string? A_3 { get; set; }
        public string? A_4 { get; set; }
        public string CorrectAnswers { get; set; }
    }
}
