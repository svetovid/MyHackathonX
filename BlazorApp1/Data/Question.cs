namespace HackathonX.Web.Data
{
    public class Question
    {
        public Question()
        {
            Answers = new List<Answer>();
        }

        public int Id { get; set; }
        public string Text { get; set; } = null!;
        public int Score { get; set; }
        public int? AnswerId { get; set; }

        public IEnumerable<Answer> Answers { get; set; }
    }
}