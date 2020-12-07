using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LexicalAnalyzer
{
    class LA
    {
        public String Text { get; set; }

        public LA(String fileName)
        {
            using (StreamReader file = new StreamReader(fileName, Encoding.UTF8))
            {
                Text = file.ReadToEnd();
            }
        }

        public List<Token> GetTokens()
        {
            List<Token> tokens = SplitIntoLexems().Select(l => new Token(l)).ToList();
            ClassifyTokens(tokens);
            return tokens;
        }

        private List<String> SplitIntoLexems()
        {
            List<String> lexems = new Regex(@"[\s+\t+\n+]").Split(Text).ToList();
            for (int i = 0; i < lexems.Count; i++)
            {
                for (int j = 0; j < lexems[i].Length; j++)
                {
                    if (lexems[i][j] == '(' || lexems[i][j] == ')' ||
                        lexems[i][j] == '{' || lexems[i][j] == '}' || lexems[i][j] == ';')
                    {
                        lexems.Insert(i + 1, lexems[i][j].ToString());
                        lexems.Insert(i + 2, lexems[i].Skip(j + 1).ToString());
                        lexems[i].Remove(j);
                        i++;
                        break;
                    }
                    if (lexems[i][j] == ':')
                    {
                        lexems.Insert(i + 1, lexems[i].Substring(j, 2));
                        lexems.Insert(i + 2, lexems[i].Skip(j + 2).ToString());
                        lexems[i].Remove(j);
                        i++;
                        break;
                    }
                }
            }
            return lexems;
        }

        private void ClassifyTokens(List<Token> tokens)
        {
            bool isComment = false;
            for (int i = 0; i < tokens.Count; i++)
            {
                if (isComment)
                {
                    tokens[i].LexemType = LexemType.Comment;
                }
                else if (IsKeyword(tokens[i].Lexem))
                {
                    tokens[i].LexemType = LexemType.Keyword;
                }
                else if (IsIdentifier(tokens[i].Lexem))
                {
                    tokens[i].LexemType = LexemType.Identifier;
                }
                else if (IsUint(tokens[i].Lexem))
                {
                    tokens[i].LexemType = LexemType.Uint;
                }
                else if (IsDelimiter(tokens[i].Lexem))
                {
                    tokens[i].LexemType = LexemType.Delimiter;
                    if (tokens[i].Lexem.Equals("{"))
                    {
                        isComment = true;
                    }
                    else if (tokens[i].Lexem.Equals("}"))
                    {
                        isComment = false;
                    }
                }
                else if (IsAssignment(tokens[i].Lexem))
                {
                    tokens[i].LexemType = LexemType.Assignment;
                }
            }
        }

        private bool IsKeyword(String lexem)
        {
            if (lexem.Equals("if") || lexem.Equals("then") || lexem.Equals("else")|| lexem.Equals("or")|| lexem.Equals("xor")|| lexem.Equals("and"))
            {
                return true;
            }
            return false;
        }

        private bool IsIdentifier(String lexem)
        {
            return true;
        }

        private bool IsUint(String lexem)
        {
            return true;
        }

        private bool IsDelimiter(String lexem)
        {
            return true;
        }

        private bool IsAssignment(String lexem)
        {
            return true;
        }     
    }
}
