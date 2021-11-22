using System;
using System.IO;

namespace LexAnalyzer
{
    class Program
    {
		static void Main(string[] args)
		{
			var code = File.ReadAllText("input.txt")
				.Replace('\n', ' ')
				.Replace('\r', ' ');

			var lex = new LexemeAnalyzer();
			Console.WriteLine("Beginning of lexical analysis...");
			var lexicalResult = lex.Go(code);
			var lexemes = lex.Lexemes;

			Console.WriteLine($"Lexical analys completed. Result: {lexicalResult}");
			if (lexicalResult)
			{
				for (int i = 0; i < lexemes.Count; i++)
				{
					Console.WriteLine($"Pos[{i}]: Value = {lexemes[i].Value} (Category: {lexemes[i].Category}, Type: {lexemes[i].Type}");
				}
			}
		}

	}
}
