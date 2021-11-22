using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexAnalyzer
{
    public enum LexType
    {
		If,
		Then,
		Else,
		End,
		And,
		Or,
		Output,
		Input,
		Delimiter,
		Rel,
		Ao,
		As,
		OpenPar,
		ClosePar,
		None
	}
}
