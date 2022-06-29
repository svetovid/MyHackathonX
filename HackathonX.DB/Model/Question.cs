using System;
using System.Collections.Generic;

namespace HackathonX.DB.Model
{
    public partial class Question
    {
        public Question()
        {
            Answers = new HashSet<Answer>();
        }

        public int Id { get; set; }
        public string Text { get; set; } = null!;
        public int Score { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }
    }
}
