using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexAnalyzer
{
    public class Lexeme
    {
        public Category Category { get; set; }
        public LexType Type { get; set; }
        public string Value { get; set; }

    }
}
