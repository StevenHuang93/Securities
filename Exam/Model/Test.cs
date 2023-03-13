using System;
using System.Collections.Generic;
using System.Text;

namespace Exam.Model
{
    public class Test
    {
        public int ExamID { get; set; }
        public string Question { get; set; }

        public string Answer { get; set; }

        public string YourAnswer { get; set; }

        public bool Point
        {
            get
            {
                return this.Answer == YourAnswer;
            }

            set { }
        }

        public int Year { get; set; }

        public int Quarter { get; set; }

        public string Name { get; set; }

        public int Type { get; set; }

        public string Note { get; set; }
    }
}
