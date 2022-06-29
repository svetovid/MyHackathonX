using System;
using System.Collections.Generic;

namespace HackathonX.DB.Model
{
    public partial class Answer
    {
        public long Id { get; set; }
        public int QuestionId { get; set; }
        public string Text { get; set; } = null!;
        public bool IsCorrect { get; set; }

        public virtual Question Question { get; set; } = null!;
    }
}
