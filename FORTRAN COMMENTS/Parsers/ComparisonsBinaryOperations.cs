    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FORTRAN_COMMENTS.Parsers.Token;

namespace FORTRAN_COMMENTS.Parsers
{
    public class Token
    {
        public enum TokenType
        {
            IntKeyword,
            Identifier,
            Digit,
            AssignmentOperator,
            EndOfStatement,
            EndOfLine,
            ComparisonOperator,
            Comment,
            LeftParenthesis,
            RightParenthesis,
            Invalid
        }

        public TokenType Type { get; }
        public string Value { get; }

        public Token(TokenType type, string value)
        {
            Type = type;
            Value = value;
        }
    }

    public class Scanner
    {
        private readonly string sourceCode;
        private int currentPosition;

        public Scanner(string sourceCode)
        {
            this.sourceCode = sourceCode;
            currentPosition = 0;
        }

        public List<Token> Scan()
        {
            List<Token> tokens = new List<Token>();

            while (currentPosition < sourceCode.Length)
            {
                char currentChar = sourceCode[currentPosition];

                if (char.IsWhiteSpace(currentChar))
                {
                    // Пробельные символы - игнорируем
                    currentPosition++;
                }
                else if (char.IsLetter(currentChar))
                {
                    // Идентификатор
                    string identifier = ReadIdentifier();
                    TokenType type = TokenType.Identifier;

                    if (identifier == "int")
                    {
                        // Ключевое слово int
                        type = TokenType.IntKeyword;
                    }

                    tokens.Add(new Token(type, identifier));
                }
                else if (char.IsDigit(currentChar))
                {
                    // ЦБЗ
                    string digit = ReadDigit();
                    tokens.Add(new Token(TokenType.Digit, digit));
                }
                else if (currentChar == '=')
                {
                    // Операция присваивания или операция сравнения ==
                    if (currentPosition + 1 < sourceCode.Length && sourceCode[currentPosition + 1] == '=')
                    {
                        tokens.Add(new Token(TokenType.ComparisonOperator, "=="));
                        currentPosition += 2;
                    }
                    else
                    {
                        tokens.Add(new Token(TokenType.AssignmentOperator, "="));
                        currentPosition++;
                    }
                }
                else if (currentChar == ';')
                {
                    // Конец оператора
                    tokens.Add(new Token(TokenType.EndOfStatement, ";"));
                    currentPosition++;
                }
                else if (currentChar == '\n')
                {
                    // Конец строки
                    tokens.Add(new Token(TokenType.EndOfLine, "\\n"));
                    currentPosition++;
                }
                else if (currentChar == '<' || currentChar == '>' || currentChar == '!' || currentChar == '=')
                {
                    // Операции сравнения: <, <=, >, >=, ==, !=
                    if (currentPosition + 1 < sourceCode.Length && sourceCode[currentPosition + 1] == '=')
                    {
                        tokens.Add(new Token(TokenType.ComparisonOperator, currentChar + "="));
                        currentPosition += 2;
                    }
                    else if(currentChar != '!') 
                    {
                        tokens.Add(new Token(TokenType.ComparisonOperator, currentChar.ToString()));
                        currentPosition++;
                    }
                    else
                    {
                        tokens.Add(new Token(TokenType.Invalid, currentChar.ToString()));
                        currentPosition++;
                    }
                }
                else if (currentChar == '(')
                {
                    // Левая круглая скобка
                    tokens.Add(new Token(TokenType.LeftParenthesis, "("));
                    currentPosition++;
                }
                else if (currentChar == ')')
                {
                    // Правая круглая скобка
                    tokens.Add(new Token(TokenType.RightParenthesis, ")"));
                    currentPosition++;
                }
                else if (currentChar == '/')
                {
                    // Комментарий
                    if (currentPosition + 1 < sourceCode.Length && sourceCode[currentPosition + 1] == '*')
                    {
                        currentPosition += 2;
                        ReadComment();
                    }
                    else
                    {
                        tokens.Add(new Token(TokenType.Invalid, currentChar.ToString()));
                        currentPosition++;
                    }
                }
                else
                {
                    // Недопустимый символ
                    tokens.Add(new Token(TokenType.Invalid, currentChar.ToString()));
                    currentPosition++;
                }
            }

            return tokens;
        }

        private string ReadIdentifier()
        {
            int start = currentPosition;
            while (currentPosition < sourceCode.Length && (char.IsLetterOrDigit(sourceCode[currentPosition]) || sourceCode[currentPosition] == '_'))
            {
                currentPosition++;
            }
            return sourceCode.Substring(start, currentPosition - start);
        }

        private string ReadDigit()
        {
            int start = currentPosition;
            while (currentPosition < sourceCode.Length && char.IsDigit(sourceCode[currentPosition]))
            {
                currentPosition++;
            }
            return sourceCode.Substring(start, currentPosition - start);
        }

        private void ReadComment()
        {
            while (currentPosition < sourceCode.Length - 1 && !(sourceCode[currentPosition] == '*' && sourceCode[currentPosition + 1] == '/'))
            {
                currentPosition++;
            }
            currentPosition += 2;
        }
    }


    internal class ComparisonsBinaryOperationsParser
    {
        private string _code;
        public List<Token> tokens;

        public ComparisonsBinaryOperationsParser(string code) 
        { 
            _code = code;
        }

        public void Parse() 
        {
            Scanner scanner = new Scanner(_code);
             tokens = scanner.Scan();
        }
    }
}
