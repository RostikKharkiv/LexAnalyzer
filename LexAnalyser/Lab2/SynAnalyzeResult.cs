using LexAnalyzer.Lab3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexAnalyzer.Lab2
{
    class SynAnalyzeResult
	{
		public bool Success { get; set; }
		public string ErrorMessage { get; set; }
		public List<PostfixEntry> EntryList { get; set; }
	}

}
