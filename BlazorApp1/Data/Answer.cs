namespace HackathonX.Web.Data
{
    public class Answer
    {
        public long Id { get; set; }
        public int QuestionId { get; set; }
        public string Text { get; set; } = null!;
        public bool IsCorrect { get; set; }
    }
}