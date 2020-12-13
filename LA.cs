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
            UniteComments(tokens);
            return tokens;
        }

        private List<String> SplitIntoLexems()
        {
            List<String> lexems = new Regex(@"[\s+\t+\n+]").Split(Text).Where(s => s.Length > 0).ToList();
            for (int i = 0; i < lexems.Count; i++)
            {
                for (int j = 0; j < lexems[i].Length; j++)
                {
                    if (lexems[i][j] == '(' || lexems[i][j] == ')' ||
                        lexems[i][j] == '{' || lexems[i][j] == '}' || lexems[i][j] == ';')
                    {
                        lexems.Insert(i + 1, lexems[i][j].ToString());
                        if (lexems[i].Skip(j + 1).Count() != 0)
                        {
                            lexems.Insert(i + 2, new String(lexems[i].Skip(j + 1).ToArray()));
                        }                       
                        lexems[i] = lexems[i].Remove(j);
                        if (lexems[i].Length == 0)
                        {
                            lexems.RemoveAt(i);
                        }
                        else
                        {
                            i++;
                        }
                        break;
                    }
                    if (lexems[i][j] == ':')
                    {
                        lexems.Insert(i + 1, lexems[i].Substring(j, 2));
                        if (lexems[i].Skip(j + 2).Count() != 0)
                        {
                            lexems.Insert(i + 2, new String(lexems[i].Skip(j + 2).ToArray()));
                        }
                        lexems[i] = lexems[i].Remove(j);
                        if (lexems[i].Length == 0)
                        {
                            lexems.RemoveAt(i);
                        }
                        else
                        {
                            i++;
                        }
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
                if (IsDelimiter(tokens[i].Lexem))
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
                else if (isComment)
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
                else if (IsAssignment(tokens[i].Lexem))
                {
                    tokens[i].LexemType = LexemType.Assignment;
                }
            }
        }

        private void UniteComments(List<Token> tokens)
        {
            for (int i = 1; i < tokens.Count - 2; i++)
            {
                if (tokens[i].LexemType == LexemType.Comment && tokens[i + 1].LexemType == LexemType.Comment)
                {
                    tokens[i].Lexem = tokens[i].Lexem + tokens[i + 1].Lexem;
                    tokens.RemoveAt(i + 1);
                    i--;
                }
            }
        }

        private bool IsKeyword(String lexem)
        {
            if (lexem.Equals("if") || lexem.Equals("then") || lexem.Equals("else") || lexem.Equals("or") || lexem.Equals("xor") || lexem.Equals("and"))
            {
                return true;
            }
            return false;
        }

        private bool IsIdentifier(String lexem)
        {
            if (lexem.Length == 0)
            {
                return false;
            }
            if (!IsLetter(lexem[0]))
            {
                return false;
            }
            foreach (char c in lexem.Skip(1))
            {
                if (!IsLetter(c) && !IsNumber(c))
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsUint(String lexem)
        {
            if (lexem.Length == 0)
            {
                return false;
            }
            if (lexem.Length > 1 && lexem[0] == '0')
            {
                return false;
            }
            foreach (char c in lexem)
            {
                if (!IsNumber(c))
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsDelimiter(String lexem)
        {
            if (lexem.Equals("(") || lexem.Equals(")") || lexem.Equals("{") ||
                lexem.Equals("}") || lexem.Equals(";"))
            {
                return true;
            }
            return false;
        }

        private bool IsAssignment(String lexem)
        {
            if (lexem.Equals(":="))
            {
                return true;
            }
            return false;
        }

        private bool IsLetter(char c)
        {
            if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || (c >= 'А' && c <= 'я') || c == 'ё' || c == 'Ё')
            {
                return true;
            }
            return false;
        }

        private bool IsNumber(char c)
        {
            if (c >= '0' && c <= '9')
            {
                return true;
            }
            return false;
        }
    }
}