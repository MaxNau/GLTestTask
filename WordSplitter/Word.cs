using System;
using System.Collections.Generic;

namespace WordSplitter
{
    public class Word
    {
        public string Text { get; set; }
        public List<int> LineNumbers { get; set; }

        public Word()
        {
            LineNumbers = new List<int>();
        }
    }
}
