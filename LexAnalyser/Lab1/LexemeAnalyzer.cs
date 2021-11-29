using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexAnalyzer
{
    class LexemeAnalyzer
	{
		public List<Lexeme> Lexemes { get; private set; } = new List<Lexeme>();
		public bool Go(string text)
		{
			Lexemes = new List<Lexeme>();
			State state = State.Start, prevState;
			bool toAdd;
			text += " ";
			StringBuilder lexBufNext = new StringBuilder();
			StringBuilder lexBufCur = new StringBuilder();
			int textIndex = 0;
			int parCount = 0;
			while (state != State.Error && state != State.Final)
			{
				prevState = state;
				toAdd = true;
				if (textIndex == text.Length && state != State.Error)
				{
					state = State.Final;
					break;
				}
				if (textIndex == text.Length)
				{
					break;
				}
				char symbol = text[textIndex];
				switch (state)
				{
					case State.Start:
						if (char.IsWhiteSpace(symbol)) state = State.Start;
						else if (symbol == '(')
						{
							parCount++;
							state = State.OpenPar;
						}
						else if (symbol == ')')
						{
							parCount--;
							state = State.ClosePar;
						}
						else if (char.IsDigit(symbol)) state = State.Constant;
						else if (char.IsLetter(symbol)) state = State.Identifier;
						else if (symbol == '>') state = State.Comp;
						else if (symbol == '<') state = State.RevComp;
						else if (symbol == '+' || symbol == '-' || symbol == '/' || symbol == '*') state = State.Ao;
						else if (symbol == '=') state = State.As;
						else if (symbol == ';') state = State.Del;
						else state = State.Error;
						toAdd = false;
						if (!char.IsWhiteSpace(symbol))
							lexBufCur.Append(symbol);
						break;
					case State.Comp:
						if (char.IsWhiteSpace(symbol))
						{
							state = State.Start;
						}
						else if (symbol == '(')
						{
							parCount++;
							state = State.OpenPar;
							lexBufNext.Append(symbol);
						}
						else if (symbol == ')')
						{
							parCount--;
							state = State.ClosePar;
							lexBufNext.Append(symbol);
						}
						else if (symbol == '=')
						{
							state = State.Start;
							lexBufCur.Append(symbol);
						}
						else if (char.IsLetter(symbol))
						{
							state = State.Identifier;
							lexBufNext.Append(symbol);
						}
						else if (char.IsDigit(symbol))
						{
							state = State.Constant;
							lexBufNext.Append(symbol);
						}
						else
						{
							state = State.Error;
							toAdd = false;
						}
						break;
					case State.Del:
						if (char.IsWhiteSpace(symbol))
						{
							state = State.Start;
						}
						else if (symbol == '(')
						{
							parCount++;
							state = State.OpenPar;
							lexBufNext.Append(symbol);
						}
						else if (symbol == ')')
						{
							parCount--;
							state = State.ClosePar;
							lexBufNext.Append(symbol);
						}
						else if (char.IsLetter(symbol))
						{
							state = State.Identifier;
							lexBufNext.Append(symbol);
						}
						else
						{
							state = State.Error;
							toAdd = false;
						}
						break;
					case State.RevComp:
						if (char.IsWhiteSpace(symbol)) state = State.Start;
						else if (symbol == '(')
						{
							parCount++;
							state = State.OpenPar;
							lexBufNext.Append(symbol);
						}
						else if (symbol == ')')
						{
							parCount--;
							state = State.ClosePar;
							lexBufNext.Append(symbol);
						}
						else if (symbol == '>')
						{
							state = State.Start;
							lexBufCur.Append(symbol);
						}
						else if (symbol == '=')
						{
							state = State.Start;
							lexBufCur.Append(symbol);
						}
						else if (char.IsLetter(symbol))
						{
							state = State.Identifier;
							lexBufNext.Append(symbol);
						}
						else if (char.IsDigit(symbol))
						{
							state = State.Constant;
							lexBufNext.Append(symbol);
						}
						else
						{
							state = State.Error;
							toAdd = false;
						}
						break;
					case State.EqComp:
						if (char.IsWhiteSpace(symbol)) state = State.Start;
						else if (symbol == '(')
						{
							parCount++;
							state = State.OpenPar;
							lexBufNext.Append(symbol);
						}
						else if (symbol == ')')
						{
							parCount--;
							state = State.ClosePar;
							lexBufNext.Append(symbol);
						}
						else if (char.IsLetter(symbol))
						{
							state = State.Identifier;
							lexBufNext.Append(symbol);
						}
						else if (char.IsDigit(symbol))
						{
							state = State.Constant;
							lexBufNext.Append(symbol);
						}
						else
						{
							state = State.Error;
							toAdd = false;
						}
						break;
					case State.As:
						if (symbol == '=')
						{
							state = State.EqComp;
							lexBufCur.Append(symbol);
							toAdd = false;
						}
						else if (symbol == '(')
						{
							parCount++;
							state = State.OpenPar;
							lexBufNext.Append(symbol);
						}
						else if (symbol == ')')
						{
							parCount--;
							state = State.ClosePar;
							lexBufNext.Append(symbol);
						}
						else if (char.IsWhiteSpace(symbol))
						{
							state = State.Start;
						}
						else if (char.IsDigit(symbol))
						{
							state = State.Constant;
							lexBufNext.Append(symbol);
						}
						else if (char.IsLetter(symbol))
						{
							state = State.Identifier;
							lexBufNext.Append(symbol);
						}
						else
						{
							state = State.Error;
							toAdd = false;
						}
						break;
					case State.Constant:
						if (char.IsWhiteSpace(symbol)) state = State.Start;
						else if (symbol == '(')
						{
							parCount++;
							state = State.OpenPar;
							lexBufNext.Append(symbol);
						}
						else if (symbol == ')')
						{
							parCount--;
							state = State.ClosePar;
							lexBufNext.Append(symbol);
						}
						else if (char.IsDigit(symbol))
						{
							state = State.Constant;
							lexBufCur.Append(symbol);
							toAdd = false;
						}
						else if (symbol == '<')
						{
							state = State.RevComp;
							lexBufNext.Append(symbol);
						}
						else if (symbol == '>')
						{
							state = State.Comp;
							lexBufNext.Append(symbol);
						}
						else if (symbol == '=')
						{
							state = State.As;
							lexBufNext.Append(symbol);
						}
						else if (symbol == '+' || symbol == '-' || symbol == '/' || symbol == '*')
						{
							state = State.Ao;
							lexBufNext.Append(symbol);
						}
						else if (symbol == ';')
						{
							state = State.Del;
							lexBufNext.Append(symbol);
						}
						else
						{
							state = State.Error;
							toAdd = false;
						}
						break;
					case State.Identifier:
						if (char.IsWhiteSpace(symbol) && lexBufCur.ToString().ToLower().Equals("input")) state = State.Input;
						else if (char.IsWhiteSpace(symbol) && lexBufCur.ToString().ToLower().Equals("end")) state = State.Final;
						else if (char.IsWhiteSpace(symbol)) state = State.Start;
						else if (symbol == '(')
						{
							parCount++;
							state = State.OpenPar;
							lexBufNext.Append(symbol);
						}
						else if (symbol == ')')
						{
							parCount--;
							state = State.ClosePar;
							lexBufNext.Append(symbol);
						}
						else if (char.IsDigit(symbol) || char.IsLetter(symbol))
						{
							state = State.Identifier;
							lexBufCur.Append(symbol);
							toAdd = false;
						}
						else if (symbol == '<')
						{
							state = State.RevComp;
							lexBufNext.Append(symbol);
						}
						else if (symbol == '>')
						{
							state = State.Comp;
							lexBufNext.Append(symbol);
						}
						else if (symbol == '=')
						{
							state = State.As;
							lexBufNext.Append(symbol);
						}
						else if (symbol == '+' || symbol == '-' || symbol == '/' || symbol == '*')
						{
							state = State.Ao;
							lexBufNext.Append(symbol);
						}
						else if (symbol == '=')
						{
							state = State.As;
							lexBufNext.Append(symbol);
						}
						else if (symbol == ';')
						{
							state = State.Del;
							lexBufNext.Append(symbol);
						}
						else
						{
							state = State.Error;
							toAdd = false;
						}
						break;
					case State.OpenPar:
						if (char.IsWhiteSpace(symbol) && lexBufCur.ToString().ToLower().Equals("input")) state = State.Input;
						else if (char.IsWhiteSpace(symbol) && lexBufCur.ToString().ToLower().Equals("end")) state = State.Final;
						else if (char.IsWhiteSpace(symbol)) state = State.Start;
						else if (symbol == '(')
						{
							parCount++;
							state = State.OpenPar;
							lexBufNext.Append(symbol);
						}
						else if (symbol == ')')
						{
							parCount--;
							state = State.ClosePar;
							lexBufNext.Append(symbol);
						}
						else if (char.IsDigit(symbol) || char.IsLetter(symbol))
						{
							state = State.Identifier;
							lexBufNext.Append(symbol);
						}
						else if (symbol == '<')
						{
							state = State.RevComp;
							lexBufNext.Append(symbol);
						}
						else if (symbol == '>' || symbol == '=')
						{
							state = State.Comp;
							lexBufNext.Append(symbol);
						}
						else if (symbol == '+' || symbol == '-' || symbol == '/' || symbol == '*')
						{
							state = State.Ao;
							lexBufNext.Append(symbol);
						}
						else if (symbol == '=')
						{
							state = State.As;
							lexBufNext.Append(symbol);
						}
						else if (symbol == ';')
						{
							state = State.Del;
							lexBufNext.Append(symbol);
						}
						else
						{
							state = State.Error;
							toAdd = false;
						}
						break;
					case State.ClosePar:
						if (char.IsWhiteSpace(symbol) && lexBufCur.ToString().ToLower().Equals("input")) state = State.Input;
						else if (char.IsWhiteSpace(symbol) && lexBufCur.ToString().ToLower().Equals("end")) state = State.Final;
						else if (char.IsWhiteSpace(symbol)) state = State.Start;
						else if (symbol == '(')
						{
							parCount++;
							state = State.OpenPar;
							lexBufNext.Append(symbol);
						}
						else if (symbol == ')')
						{
							parCount--;
							state = State.ClosePar;
							lexBufNext.Append(symbol);
						}
						else if (char.IsDigit(symbol) || char.IsLetter(symbol))
						{
							state = State.Identifier;
							lexBufNext.Append(symbol);
						}
						else if (symbol == '<')
						{
							state = State.RevComp;
							lexBufNext.Append(symbol);
						}
						else if (symbol == '>' || symbol == '=')
						{
							state = State.Comp;
							lexBufNext.Append(symbol);
						}
						else if (symbol == '+' || symbol == '-' || symbol == '/' || symbol == '*')
						{
							state = State.Ao;
							lexBufNext.Append(symbol);
						}
						else if (symbol == '=')
						{
							state = State.As;
							lexBufNext.Append(symbol);
						}
						else if (symbol == ';')
						{
							state = State.Del;
							lexBufNext.Append(symbol);
						}
						else
						{
							state = State.Error;
							toAdd = false;
						}
						break;
					case State.Input:
						if (char.IsWhiteSpace(symbol)) state = State.Input;
						else if (char.IsLetter(symbol)) state = State.Identifier;
						else state = State.Error;
						toAdd = false;
						if (!char.IsWhiteSpace(symbol))
							lexBufCur.Append(symbol);
						break;
					case State.Ao:
						if (char.IsWhiteSpace(symbol))
						{
							state = State.Start;
						}
						else if (symbol == '(')
						{
							parCount++;
							state = State.OpenPar;
							lexBufNext.Append(symbol);
						}
						else if (symbol == ')')
						{
							parCount--;
							state = State.ClosePar;
							lexBufNext.Append(symbol);
						}
						else if (char.IsLetter(symbol))
						{
							state = State.Identifier;
							lexBufNext.Append(symbol);
						}
						else if (char.IsDigit(symbol))
						{
							state = State.Constant;
							lexBufNext.Append(symbol);
						}
						else
						{
							state = State.Error;
							toAdd = false;
						}
						break;
				}
				if (toAdd)
				{
					AddLexeme(prevState, lexBufCur.ToString());
					lexBufCur = new StringBuilder(lexBufNext.ToString());
					lexBufNext.Clear();
				}
				textIndex++;
			}

			return state == State.Final;

			//return state == State.Final && parCount == 0;

		}

		private void AddLexeme(State prevState, string value)
		{
			LexType lexType = LexType.None;
			Category lexCategory = Category.None;
			if (prevState == State.Ao)
			{
				lexType = LexType.Ao;
				lexCategory = Category.Symbol;
			}
			else if (prevState == State.OpenPar)
			{
				lexType = LexType.OpenPar;
				lexCategory = Category.Symbol;
			}
			else if (prevState == State.ClosePar)
			{
				lexType = LexType.ClosePar;
				lexCategory = Category.Symbol;
			}
			else if (prevState == State.As)
			{
				lexType = LexType.As;
				lexCategory = Category.Symbol;
			}
			else if (prevState == State.Constant)
			{
				lexType = LexType.None;
				lexCategory = Category.Constant;
			}
			else if (prevState == State.RevComp)
			{
				lexType = LexType.Rel;
				lexCategory = Category.Symbol;
			}
			else if (prevState == State.Comp)
			{
				lexType = LexType.Rel;
				lexCategory = Category.Symbol;
			}
			else if (prevState == State.EqComp)
			{
				lexType = LexType.Rel;
				lexCategory = Category.Symbol;
			}
			else if (prevState == State.Del)
			{
				lexType = LexType.Delimiter;
				lexCategory = Category.Symbol;
			}
			else if (prevState == State.Input)
			{
				lexType = LexType.Input;
				lexCategory = Category.Symbol;
			}
			else if (prevState == State.Identifier)
			{
				bool isKeyword = true;
				if (value.ToLower() == "if") lexType = LexType.If;
				else if (value.ToLower() == "then") lexType = LexType.Then;
				else if (value.ToLower() == "else") lexType = LexType.Else;
				else if (value.ToLower() == "end") lexType = LexType.End;
				else if (value.ToLower() == "and") lexType = LexType.And;
				else if (value.ToLower() == "or") lexType = LexType.Or;
				else if (value.ToLower() == "output") lexType = LexType.Output;
				else if (value.ToLower() == "input") lexType = LexType.Input;
				else
				{
					lexType = LexType.None;
					isKeyword = false;
				}
				if (isKeyword) lexCategory = Category.Keyword;
				else lexCategory = Category.Identifier;
			}
			var lexeme = new Lexeme
			{
				Category = lexCategory,
				Type = lexType,
				Value = value,
			};
			Lexemes.Add(lexeme);
		}
	}
}
