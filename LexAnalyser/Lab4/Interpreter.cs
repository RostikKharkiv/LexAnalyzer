using LexAnalyzer.Lab3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LexAnalyzer.Lab3.Enums;

namespace LexAnalyzer.Lab4
{
    public class Interpreter
    {
		private Stack<PostfixEntry> _stack;
		private readonly List<PostfixEntry> _entries;

		public Interpreter(List<PostfixEntry> entryList)
		{
			_entries = entryList;
		}

		public List<string> Logs { get; set; } = new List<string>();


		public void Interpret()
		{
			_stack = new Stack<PostfixEntry>();

			int pos = 0, temp;
			Log(pos);
			while (pos < _entries.Count)
			{
				if (_entries[pos].EntryType == EntryType.Cmd)
				{
					var cmd = _entries[pos].Cmd;
					switch (cmd)
					{
						case Cmd.JZ:
							temp = PopVal();
							if (PopVal() != 0) pos++; else pos = temp;
							break;
						case Cmd.SET:
							SetVarAndPop(PopVal());
							pos++;
							break;
						case Cmd.ADD:
							PushVal(PopVal() + PopVal());
							pos++;
							break;
						case Cmd.SUB:
							PushVal(-PopVal() + PopVal());
							pos++;
							break;
						case Cmd.MUL:
							PushVal(PopVal() * PopVal());
							pos++;
							break;
						case Cmd.DIV:
							PushVal((int)(1.0 / PopVal() * PopVal()));
							pos++;
							break;
						case Cmd.AND:
							PushVal((PopVal() != 0 && PopVal() != 0) ? 1 : 0);
							pos++;
							break;
						case Cmd.OR:
							PushVal((PopVal() != 0 || PopVal() != 0) ? 1 : 0);
							pos++;
							break;
						case Cmd.CMPE:
							PushVal((PopVal() == PopVal()) ? 1 : 0);
							pos++;
							break;
						case Cmd.CMPNE:
							PushVal((PopVal() != PopVal()) ? 1 : 0);
							pos++;
							break;
						case Cmd.CMPL:
							PushVal((PopVal() > PopVal()) ? 1 : 0);
							pos++;
							break;
						case Cmd.CMPLE:
							PushVal((PopVal() >= PopVal()) ? 1 : 0);
							pos++;
							break;
						case Cmd.CMPG:
							PushVal((PopVal() < PopVal()) ? 1 : 0);
							pos++;
							break;
						case Cmd.OUT:
							Console.WriteLine(PopVal());
							pos++;
							break;
						default:
							break;
					}
				}
				else PushElm(_entries[pos++]);

				if (pos < _entries.Count)
					Log(pos);
			}
		}

		private int PopVal()
		{
			if (_stack.Count != 0)
			{
				var obj = _stack.Pop();

				switch (obj.EntryType)
				{
					case EntryType.Var:
						return obj.CurrentValue.Value;
					case EntryType.Const:
						return int.Parse(obj.Value);
					case EntryType.CmdPtr:
						return obj.CmdPtr.Value;
					default:
						throw new ArgumentException("obj.EntryType");
				}
			}
			else
			{
				return 0;
			}
		}

		private void PushVal(int val)
		{
			var entry = new PostfixEntry
			{
				EntryType = EntryType.Const,
				Value = val.ToString()
			};
			_stack.Push(entry);
		}

		private void PushElm(PostfixEntry entry)
		{
			if (entry.EntryType == EntryType.Cmd)
			{
				throw new ArgumentException("EntryType");
			}
			_stack.Push(entry);
		}

		private void SetVarAndPop(int val)
		{
			var variable = _stack.Pop();
			if (variable.EntryType != EntryType.Var)
			{
				throw new ArgumentException("EntryType");
			}
			SetValuesToVariables(variable.Value, val);
		}
		//Pos[0]: Value = do (Category: Keyword, Type: Do
		private void Log(int pos)
		{
			Logs.Add($"Pos[{pos}]: Value = {GetEntryString(_entries[pos])}; Stack state: {GetStackState()}; Variable state: {GetVarValues()}");
		}

		private string GetEntryString(PostfixEntry entry)
		{
			if (entry.EntryType == EntryType.Var) return entry.Value;
			else if (entry.EntryType == EntryType.Const) return entry.Value;
			else if (entry.EntryType == EntryType.Cmd) return entry.Cmd.ToString();
			else if (entry.EntryType == EntryType.CmdPtr) return entry.CmdPtr.ToString();
			throw new ArgumentException("PostfixEntry");
		}

		private string GetStackState()
		{
			IEnumerable<PostfixEntry> entries = _stack;
			var sb = new StringBuilder();
			entries?.ToList().ForEach(e => sb.Append($"{GetEntryString(e)} "));
			return sb.ToString();
		}

		private string GetVarValues()
		{
			var sb = new StringBuilder();
			_entries
				.Where(e => e.EntryType == EntryType.Var)
				.Select(e => new { e.Value, e.CurrentValue })
				.Distinct()
				.ToList()
				.ForEach(e => sb.Append($"{e.Value} = {e.CurrentValue}; "));
			return sb.ToString();
		}

		public IEnumerable<PostfixEntry> GetVariables()
			=> _entries.Where(e => e.EntryType == EntryType.Var);

		public void SetValuesToVariables(string name, int value)
		{
			GetVariables().Where(v => v.Value == name).ToList().ForEach(v => v.CurrentValue = value);
		}
	}
}
