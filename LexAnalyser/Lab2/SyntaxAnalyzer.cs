using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LexAnalyzer;

namespace LexAnalyzer.Lab2
{
    class SyntaxAnalyzer
	{
		private List<Lexeme> _lexems;
		private IEnumerator<Lexeme> _lexReader;
		private string _errorMsg;
		private bool isElse = false;
		private int parCount = 0;
		private Stack<Lexeme> par = new Stack<Lexeme>();

		public SyntaxAnalyzer(List<Lexeme> lexemeList)
		{
			_lexems = lexemeList;
		}

		public SynAnalyzeResult Go()
		{
			_errorMsg = string.Empty;
			_lexReader = _lexems.GetEnumerator();

			var syntaxResult = IsWhileStatement();

			return new SynAnalyzeResult
			{
				Success = syntaxResult,
				ErrorMessage = syntaxResult ? string.Empty : _errorMsg,
			};
		}

		private bool IsWhileStatement()
		{

			if (_lexems.Count == 0) return Error("Lexems list is empty", 0);

			if (!_lexReader.MoveNext() || _lexReader.Current.Type != LexType.If)
			{
				return Error("Expected keyword \"If\"", _lexems.IndexOf(_lexReader.Current));
			}

			_lexReader.MoveNext();

			if (!IsCondition()) return false;

			if (_lexReader.Current is null || _lexReader.Current.Type != LexType.Then)
			{
				return Error("Expected keyword \"Then\"", _lexems.IndexOf(_lexReader.Current));
			}

			_lexReader.MoveNext();

			while (IsStatement()) ;
			if (_errorMsg != string.Empty)
			{
				return false;
			}

			if ((_lexReader.Current is null || (_lexReader.Current.Type != LexType.Else && isElse)))
			{
				return Error("Expected keyword \"Else\"", _lexems.IndexOf(_lexReader.Current));
			}
			else if (_lexReader.Current.Type == LexType.Else && isElse)
            {
				_lexReader.MoveNext();

				while (IsStatement()) ;
				if (_errorMsg != string.Empty)
				{
					return false;
				}
			}

            foreach (var item in par)
            {

            }

			if (parCount != 0)
            {
				int[] positions = new int[par.Count];

				for (int i = 0; i < par.Count; i++)
                {
					positions[i] = _lexems.IndexOf(par.Peek());
					par.Pop();
                }

				StringBuilder positionOutput = new StringBuilder();

                foreach (var position in positions.Reverse())
                {
					positionOutput.Append(position);
					positionOutput.Append("; ");
                }

				return Error("Incorrect count of parenthesis", positionOutput.ToString());
			}

            if (_lexReader.Current is null || _lexReader.Current.Type != LexType.End)
            {
                return Error("Expected keyword \"End\"", _lexems.IndexOf(_lexReader.Current));
            }

            return true;
		}

		private bool IsCondition()
		{
			while (_lexReader.Current?.Type == LexType.OpenPar)
			{
				parCount++;
				par.Push(_lexReader.Current);
				_lexReader.MoveNext();
			}

			if (!IsRelationalExpression()) return false;
			while (_lexReader.Current != null &&
				(_lexReader.Current.Type == LexType.Or || _lexReader.Current.Type == LexType.And))
			{
				_lexReader.MoveNext();

				while (_lexReader.Current?.Type == LexType.OpenPar)
				{
					parCount++;
					par.Push(_lexReader.Current);
					_lexReader.MoveNext();
				}

				if (!IsRelationalExpression()) return false;
			}

			while (_lexReader.Current?.Type == LexType.ClosePar)
			{
				parCount--;
				if (par.Count == 0)
                {
					return Error("Incorrect count of parenthesis", _lexems.IndexOf(_lexReader.Current));
				}
				par.Pop();
				_lexReader.MoveNext();
			}

			return true;
		}

		private bool IsRelationalExpression()
		{
			while (_lexReader.Current?.Type == LexType.OpenPar)
			{
				parCount++;
				par.Push(_lexReader.Current);
				_lexReader.MoveNext();
			}

			if (_lexReader.Current == null) return Error("Expected relational expression", _lexems.IndexOf(_lexReader.Current));
			if (!IsArithmeticExpression()) return false;
			if (_lexReader.Current.Type == LexType.Rel)
			{
				_lexReader.MoveNext();

				while (_lexReader.Current?.Type == LexType.OpenPar)
				{
					parCount++;
					par.Push(_lexReader.Current);
					_lexReader.MoveNext();
				}

				if (!IsArithmeticExpression()) return false;
			}

			while (_lexReader.Current?.Type == LexType.ClosePar)
			{
				parCount--;
				if (par.Count == 0)
				{
					return Error("Incorrect count of parenthesis", _lexems.IndexOf(_lexReader.Current));
				}
				par.Pop();
				_lexReader.MoveNext();
			}

			return true;
		}

		private bool IsOperand()
		{
			if (_lexReader.Current == null ||
				(_lexReader.Current.Category != Category.Identifier 
				&& _lexReader.Current.Category != Category.Constant))
			{
				return Error("Expected identifier or constant", _lexems.IndexOf(_lexReader.Current));
			}

			_lexReader.MoveNext();
			return true;
		}

		private bool IsStatement()
		{
			if (_lexReader.Current != null && _lexReader.Current.Type == LexType.Then) return false;

			if (_lexReader.Current != null && _lexReader.Current.Type == LexType.Else)
			{
				isElse = true;
				return false;
			}

			if (_lexReader.Current != null && _lexReader.Current.Type == LexType.End) return false;

			if (_lexReader.Current == null || _lexReader.Current.Category != Category.Identifier)
			{
				if (_lexReader.Current != null 
					&&(_lexReader.Current.Type == LexType.Output || _lexReader.Current.Type == LexType.Input))
				{
					_lexReader.MoveNext();
					if (!IsOperand()) return false;
					return IsDelimiter();
				}
				else if (!_lexReader.MoveNext())
				{
					return Error("Expected keyword \"End\"", _lexems.Count);
				}

                return Error("Expected identifier", _lexems.IndexOf(_lexReader.Current));
			}


			_lexReader.MoveNext();

			if (_lexReader.Current == null || _lexReader.Current.Type != LexType.As)
			{
				return Error("Expected symbol \"=\"", _lexems.IndexOf(_lexReader.Current));
			}
			_lexReader.MoveNext();

			if (!IsArithmeticExpression()) return false;

			return IsDelimiter();
		}

		private bool IsDelimiter()
		{
			if (_lexReader.Current == null || _lexReader.Current.Type != LexType.Delimiter)
			{
				return Error("Expected symbol \";\"", _lexems.IndexOf(_lexReader.Current));
			}
			_lexReader.MoveNext();
			return true;
		}

		private bool IsArithmeticExpression()
		{
			while (_lexReader.Current?.Type == LexType.OpenPar)
            {
				parCount++;
				par.Push(_lexReader.Current);
				_lexReader.MoveNext();
			}

			if (!IsOperand()) return false;
			while (_lexReader.Current?.Type == LexType.Ao)
			{
				_lexReader.MoveNext();

				while (_lexReader.Current?.Type == LexType.OpenPar)
				{
					parCount++;
					par.Push(_lexReader.Current);
					_lexReader.MoveNext();
				}

				if (!IsOperand()) return false;
			}

			while (_lexReader.Current?.Type == LexType.ClosePar)
			{
				parCount--;
				if (par.Count == 0)
				{
					return Error("Incorrect count of parenthesis", _lexems.IndexOf(_lexReader.Current));
				}
				par.Pop();
				_lexReader.MoveNext();
			}

			return true;
		}

		private bool Error(string msg, int pos)
		{
			_errorMsg += $"Position: {pos}; Error! {msg}";
			return false;
		}

		private bool Error(string msg , string pos)
		{
			_errorMsg += $"Position: {pos}Error! {msg}";
			return false;
		}
	}

}
