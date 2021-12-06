using LexAnalyzer.Lab2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LexAnalyzer.Lab3.Enums;

namespace LexAnalyzer.Lab3
{
    public class PostfixProgram
    {
        private List<Lexeme> _lexems;
        private IEnumerator<Lexeme> _lexReader;
        private List<PostfixEntry> _entryList = new List<PostfixEntry>();
        private Stack<PostfixEntry> _entryStack = new Stack<PostfixEntry>();

        public PostfixProgram(List<Lexeme> lexemeList)
        {
            _lexems = lexemeList;
		}

		public List<PostfixEntry> GetEntryList
        {
			get
            {
				return _entryList;
            }
        }

		public void TakeThis()
        {
			_lexReader = _lexems.GetEnumerator();
			_lexReader.MoveNext();

			var jzIndex = _entryList.Count;

            if (_lexReader.Current.Type == LexType.If)
            {
				WriteCmd(Cmd.JZ);
				_lexReader.MoveNext();
			}

			WhileIsNotKeyword();
			int cmdIndex = WriteCmdPtr(_lexems.IndexOf(_lexReader.Current));
			if (_lexReader.Current != null && _lexReader.Current.Type == LexType.Then) {
				_lexReader.MoveNext();
				if (_entryStack.Count != 0)
				{
					_entryList.Add(_entryStack.Peek());
					_entryStack.Pop();
				}

				WhileIsNotKeyword();
			}

			WhileIsNotKeyword();

			if (_lexReader.Current != null && _lexReader.Current.Type == LexType.Else)
			{
				_lexReader.MoveNext();
				jzIndex = _lexems.IndexOf(_lexReader.Current);
				SetCmdPtr(cmdIndex, jzIndex);
				WhileIsNotKeyword();
				if (_lexReader.Current != null && _lexReader.Current.Type == LexType.End)
				{
					_lexReader.MoveNext();
				}
			}

			else if (_lexReader.Current != null && _lexReader.Current.Type == LexType.End)
            {
				_lexReader.MoveNext();
				jzIndex = _lexems.IndexOf(_lexems.Last());
				SetCmdPtr(cmdIndex, jzIndex);
			}
		}

		private void WhileIsNotKeyword()
        {
			while (_lexReader.Current != null && 
				(_lexReader.Current.Type != LexType.Then &&
				_lexReader.Current.Type != LexType.Else &&
				_lexReader.Current.Type != LexType.End))
			{

				while ((_lexReader.Current.Type == LexType.OpenPar))
					_lexReader.MoveNext();

				while (_lexReader.Current.Type == LexType.ClosePar)
				{
					Parenthesis();
				}

				while (_lexReader.Current.Type == LexType.Delimiter)
                {
					Delemiter();
				}

				if (_lexReader.Current.Category == Category.Identifier)
				{
					WriteVar(_lexems.IndexOf(_lexReader.Current));
					_lexReader.MoveNext();
				}
				if (_lexReader.Current.Category == Category.Constant)
				{
					WriteConst(_lexems.IndexOf(_lexReader.Current));
					_lexReader.MoveNext();
				}

				if (_lexReader.Current.Type == LexType.Input) LowPriority(Cmd.INP);

				if (_lexReader.Current.Type == LexType.Output) LowPriority(Cmd.OUT);

				if (_lexReader.Current.Value == "-") LowPriority(Cmd.SUB);

				if (_lexReader.Current.Value == "+") LowPriority(Cmd.ADD);

				if (_lexReader.Current.Value == ">") LowPriority(Cmd.CMPG);

				if (_lexReader.Current.Value == "<") LowPriority(Cmd.CMPL);

				if (_lexReader.Current.Value == "<=") LowPriority(Cmd.CMPLE);

				if (_lexReader.Current.Value == ">=") LowPriority(Cmd.CMPGE);

				if (_lexReader.Current.Value == "==") LowPriority(Cmd.CMPE);

				if (_lexReader.Current.Value == "<>") LowPriority(Cmd.CMPNE);

				if (_lexReader.Current.Type == LexType.Or) LowPriority(Cmd.OR);

				if (_lexReader.Current.Type == LexType.As) LowPriority(Cmd.SET);

				if (_lexReader.Current.Type == LexType.And) HighPriority(Cmd.AND);

				if (_lexReader.Current.Value == "*") HighPriority(Cmd.MUL);

				if (_lexReader.Current.Value == "/") HighPriority(Cmd.DIV);
			}
		}

		private void HighPriority(Cmd cmd)
		{
			_lexReader.MoveNext();
			WriteCmd(cmd);

			if (_lexReader.Current.Category == Category.Identifier)
			{
				WriteVar(_lexems.IndexOf(_lexReader.Current));
				_entryList.Add(_entryStack.Peek());
				_entryStack.Pop();
				_lexReader.MoveNext();
			}
			else if (_lexReader.Current.Category == Category.Constant)
			{
				WriteConst(_lexems.IndexOf(_lexReader.Current));
				_entryList.Add(_entryStack.Peek());
				_entryStack.Pop();
				_lexReader.MoveNext();
			}

			if (_lexReader.Current.Type == LexType.ClosePar)
			{
				while (_entryStack?.Peek().Cmd != Cmd.SET && _entryStack?.Peek().Cmd != Cmd.JZ && _entryStack.Count != 0)
				{
					_entryList.Add(_entryStack.Peek());
					_entryStack.Pop();
				}
				_lexReader.MoveNext();
			}

			if (_lexReader.Current.Type == LexType.Delimiter)
			{
				while (!(_entryStack.Count == 0) && _entryStack.Peek().Cmd != Cmd.JZ)
				{
					_entryList.Add(_entryStack.Peek());
					_entryStack.Pop();
				}
				_lexReader.MoveNext();
			}
		}

		private void LowPriority(Cmd cmd)
        {
			WriteCmd(cmd);
			_lexReader.MoveNext();

			if (_lexReader.Current.Category == Category.Identifier)
			{
				//WriteVar(_lexems.IndexOf(_lexReader.Current));
			}
			else if (_lexReader.Current.Category == Category.Constant)
			{
				//WriteConst(_lexems.IndexOf(_lexReader.Current));
			}

			if (_lexReader.Current.Type == LexType.ClosePar)
			{
				while (_entryStack?.Peek().Cmd != Cmd.SET && _entryStack?.Peek().Cmd != Cmd.JZ && _entryStack.Count != 0)
				{
					_entryList.Add(_entryStack.Peek());
					_entryStack.Pop();
				}
				_lexReader.MoveNext();
			}

			if (_lexReader.Current.Type == LexType.Delimiter)
			{
				while (_entryStack.Count != 0 &&  _entryStack.Peek().Cmd != Cmd.JZ)
				{
					_entryList.Add(_entryStack.Peek());
					_entryStack.Pop();
				}
				_lexReader.MoveNext();
			}
		}

		private void Parenthesis()
		{
			while (_entryStack.Count != 0 && _entryStack.Peek().Cmd != Cmd.SET && _entryStack.Peek().Cmd != Cmd.JZ)
			{
				_entryList.Add(_entryStack.Peek());
				_entryStack.Pop();
			}
			_lexReader.MoveNext();
		}

		private void Delemiter()
		{
			while (_entryStack.Count != 0 && _entryStack.Peek().Cmd != Cmd.JZ)
			{
				_entryList.Add(_entryStack.Peek());
				_entryStack.Pop();
			}
			_lexReader.MoveNext();
		}

		private int WriteCmd(Cmd cmd)
		{
			var command = new PostfixEntry
			{
				EntryType = EntryType.Cmd,
				Cmd = cmd,
			};
			_entryStack.Push(command);
			return _entryStack.Count - 1;
		}

		private int WriteCmdToList(Cmd cmd)
		{
			var command = new PostfixEntry
			{
				EntryType = EntryType.Cmd,
				Cmd = cmd,
			};
			_entryList.Add(command);
			return _entryList.Count - 1;
		}

		private int WriteVar(int index)
		{
			var variable = new PostfixEntry
			{
				EntryType = EntryType.Var,
				Value = _lexems[index].Value
			};
			_entryList.Add(variable);
			return _entryList.Count - 1;
		}

		private int WriteConst(int index)
		{
			var variable = new PostfixEntry
			{
				EntryType = EntryType.Const,
				Value = _lexems[index].Value
			};
			_entryList.Add(variable);
			return _entryList.Count - 1;
		}

		private int WriteCmdPtr(int ptr)
		{
			var cmdPtr = new PostfixEntry
			{
				EntryType = EntryType.CmdPtr,
				CmdPtr = ptr,
			};
			_entryList.Add(cmdPtr);
			return _entryList.Count - 1;
		}

		private void SetCmdPtr(int index, int ptr)
		{
			_entryList[index].CmdPtr = ptr;
		}

		public void Show()
        {
            foreach (var item in _entryList)
            {
				if (item.EntryType == EntryType.Cmd)
					Console.Write($"{item.Cmd} ");
				if (item.EntryType == EntryType.Const)
					Console.Write($"{item.Value} ");
				if (item.EntryType == EntryType.Var)
					Console.Write($"{item.Value} ");
				if (item.EntryType == EntryType.CmdPtr)
					Console.Write($"{item.CmdPtr} ");
			}
        }
	}
}
