//using LexAnalyzer.Lab2;
//using LexAnalyzer.Lab3;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using static LexAnalyzer.Lab3.Enums;

//namespace LexAnalyzer.Lab4
//{
//    class Program
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
//            if (!lexicalResult)
//            {
//                return;
//            }
//            if (lexicalResult)
//            {
//                for (int i = 0; i < lexemes.Count; i++)
//                {
//                    Console.WriteLine($"Pos[{i}]: Value = {lexemes[i].Value} (Category: {lexemes[i].Category}, Type: {lexemes[i].Type}");
//                }
//            }


//            var syn = new SyntaxAnalyzer(lexemes);
//            Console.WriteLine("Beginning of syntax analysis...");
//            var syntaxResult = syn.Go();
//            Console.WriteLine($"Syntax analys completed. Result: {syntaxResult.Success}");
//            if (!syntaxResult.Success)
//            {
//                Console.WriteLine(syntaxResult.ErrorMessage);
//                return;
//            }

//            foreach (var entry in syntaxResult.EntryList)
//            {
//                Console.Write($"{EntryToString(entry)} ");
//            }
//            Console.WriteLine();
//            for (int i = 0; i < syntaxResult.EntryList.Count; i++)
//            {
//                Console.Write($"{i} ");
//            }
//            Console.WriteLine();

//            var postfix = new PostfixProgram(lexemes);

//            if (syntaxResult.Success)
//            {
//                postfix.TakeThis();
//                postfix.Show();
//            }
//            Console.WriteLine();
//            Console.WriteLine();
//            var interpreter = new Interpreter(postfix.GetEntryList);
//            SetDefaultVars(interpreter);

//            Console.WriteLine("Interpreting...");
//            interpreter.Interpret();
//            Console.WriteLine("End of program!");
//            Console.WriteLine("Execution history");

//            foreach (var item in interpreter.Logs)
//            {
//                Console.WriteLine(item);
//            }

//        }

//        private static string EntryToString(PostfixEntry entry)
//        {
//            if (entry.EntryType == EntryType.Var) return entry.Value;
//            else if (entry.EntryType == EntryType.Const) return entry.Value;
//            else if (entry.EntryType == EntryType.Cmd) return entry.Cmd.ToString();
//            else if (entry.EntryType == EntryType.CmdPtr) return entry.CmdPtr.ToString();
//            throw new ArgumentException(nameof(entry));
//        }

//        private static void SetDefaultVars(Interpreter interpreter)
//        {
//            var vars = interpreter.GetVariables()
//                .Select(x => x.Value)
//                .Distinct()
//                .ToList();

//            foreach (var item in vars)
//            {
//                interpreter.SetValuesToVariables(item, GetVal(item));
//            }
//        }

//        private static int GetVal(string name)
//        {
//            bool isParsed;
//            int value;

//            do
//            {
//                Console.Write($"Enter default value for variable \"{name}\": ");
//                isParsed = int.TryParse(Console.ReadLine(), out value);
//            } while (!isParsed);
//            return value;
//        }
//    }
//}
