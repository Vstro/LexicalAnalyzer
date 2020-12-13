using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexicalAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Input name of file with text to analyze: ");
            String fileName = Console.ReadLine();
            LA lexicalAnalyzer = new LA(fileName);
            foreach (Token token in lexicalAnalyzer.GetTokens())
            {
                Console.WriteLine(token);
            }
            Console.ReadKey();
        }
    }
}