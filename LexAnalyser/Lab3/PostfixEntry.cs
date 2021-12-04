using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LexAnalyzer.Lab3.Enums;

namespace LexAnalyzer.Lab3
{
    public class PostfixEntry
	{
		public int Index { get; set; }
		public EntryType EntryType { get; set; }
		public Cmd? Cmd { get; set; }
		public string Value { get; set; }
		public int? CmdPtr { get; set; }
		public int? CurrentValue { get; set; }
	}
}
