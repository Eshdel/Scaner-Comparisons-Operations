using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FORTRAN_COMMENTS.Parsers
{
    public class CommentParser
    {
        private int row = default;
        private int column = default;

        private List<string> chains;

        private List<Error> errors = new List<Error>();

        private string targetChain;

        public CommentParser(string data)
        {
            chains = new List<string>(data.Split('\n'));
        }

        private char? GetNextChar()
        {
            try
            {
                var data = targetChain[column + 1];
                column++;
                return data;
            }
            catch
            {
                return null;
            }

        }

        private char? nextChar => GetNextChar();

        public void Parse()
        {
            if (chains.Count == 0) return;

            foreach (var chain in chains)
            {
                if (chain == null) continue;
                targetChain = chain;
                column = default;

                State0(targetChain[column]);

                row++;
            }
        }

        private void State0(char? _)
        {

            if (_ == ' ')
            {
                State0(nextChar);
            }
            else if (_ == '!')
            {
                State1(nextChar);
            }
            else
            {
                int startIndexError = column;
                int lenght = default;
                var errorChain = targetChain.Substring(startIndexError).TrimEnd();

                foreach (var c in errorChain)
                {
                    if (c != '!')
                        lenght++;
                    else
                    {
                        errors.Add(new Error(new Position(column, row), targetChain.Substring(startIndexError, lenght).TrimEnd()));
                        return;
                    }

                }

                errors.Add(new Error(new Position(column, row), targetChain.Substring(startIndexError, lenght)));
            }

        }

        private void State1(char? _)
        {

            if (IsLetter(_))
                State2(nextChar);

            else if (IsDigit(_))
                State3(nextChar);

            else if (IsSymbol(_))
                State4(nextChar);

            else State5();
        }

        private void State2(char? _)
        {
            State1(_);
        }

        private void State3(char? _)
        {
            State1(_);
        }

        private void State4(char? _)
        {
            State1(_);
        }

        private void State5()
        {
            return;
        }

        public bool IsDigit(char? _)
        {
            if (_ == null) return false;

            return _ != null ? char.IsDigit((char)_) : false;
        }

        public bool IsSymbol(char? _)
        {
            return _ != null ? char.IsSymbol((char)_) || char.IsSeparator((char)_) || char.IsPunctuation((char)_) : false;
        }

        public bool IsLetter(char? _)
        {
            return _ != null ? char.IsLetter((char)_) : false;
        }

        public IEnumerable<Error> GetErrors()
        {
            return errors;
        }
    }

    public class Error
    {
        public string Data { get; set; }
        public Position Position { get; set; }

        public Error(Position position, string data)
        {
            Data = data;
            Position = position;
        }

        public override string ToString()
        {
            return $"Неожидаемый ввод {Data} на строке {Position.Row} в столбеце {Position.Column}";
        }
    }

    public class Position
    {
        public int Column { get; set; }
        public int Row { get; set; }

        public Position(int column, int row)
        {
            Column = column;
            Row = row;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
