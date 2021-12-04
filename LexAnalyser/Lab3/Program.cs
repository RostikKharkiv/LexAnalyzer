//using LexAnalyzer.Lab2;
//using LexAnalyzer.Lab4;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace LexAnalyzer.Lab3
//{
//    public class Program
//    {
//        static void Main(string[] args)
//        {
//            var code = File.ReadAllText("input.txt")
//                .Replace('\n', ' ')
//                .Replace('\r', ' ');

//            var lex = new LexemeAnalyzer();
//            Console.WriteLine("Beginning of lexical analysis...");
//            var lexicalResult = lex.Go(code);
//            var lexemes = lex.Lexemes;

//            Console.WriteLine($"Lexical analys completed. Result: {lexicalResult}");
//            if (lexicalResult)
//            {
//                for (int i = 0; i < lexemes.Count; i++)
//                {
//                    Console.WriteLine($"Pos[{i}]: Value = {lexemes[i].Value} (Category: {lexemes[i].Category}, Type: {lexemes[i].Type}");
//                }
//            }
//            else
//            {
//                return;
//            }

//            var syn = new SyntaxAnalyzer(lexemes);
//            Console.WriteLine("Beginning of syntax analysis...");
//            var syntaxResult = syn.Go();
//            Console.WriteLine($"Syntax analys completed. Result: {syntaxResult.Success}");
//            if (!syntaxResult.Success) Console.WriteLine(syntaxResult.ErrorMessage);

//            if (syntaxResult.Success)
//            {
//                var postfix = new PostfixProgram(lexemes);
//                postfix.TakeThis();
//                postfix.Show();
//            }
//        }
//    }
//}
