using System;

namespace LexicalAnalyzer
{
    class Token
    {
        public String Lexem { get; set; }
        public LexemType LexemType { get; set; } = LexemType.Undefined;

        public Token(String lexem)
        {
            Lexem = lexem;
        }

        public Token(String lexem, LexemType lexemType)
        {
            Lexem = lexem;
            LexemType = lexemType;
        }

        public override String ToString()
        {
            return $"[{Lexem}]:[{LexemType}]";
        }
    }
}
