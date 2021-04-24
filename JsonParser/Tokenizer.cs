using System;
using System.Collections.Generic;

namespace JsonParser
{
    public class Token
    {
        public int Position { set; get; }
        public int LineNumber { set; get; }
        public string Type { set; get; }
        public string Value { set; get; }
        public Token(int position, int lineNumber, string type, string value)
        {
            this.Position = position;
            this.LineNumber = lineNumber;
            this.Type = type;
            this.Value = value;
        }
    }
    public abstract class Tokenizable
    {
        public abstract bool tokenizable(Tokenizer tokenizer);
        public abstract Token tokenize(Tokenizer tokenizer);

        public bool isOptional;
        public Tokenizable(bool isOptional = false)
        {
            this.isOptional = isOptional;
        }
    }
    public class Tokenizer
    {
        public List<Token> tokens;
        public bool enableHistory;
        public Input input;
        public Tokenizable[] handlers;
        public Tokenizer(string source, Tokenizable[] handlers)
        {
            this.input = new Input(source);
            this.handlers = handlers;
            //this.tokens = new List<Token>();
        }
        public Tokenizer(Input source, Tokenizable[] handlers)
        {
            this.input = source;
            this.handlers = handlers;
        }
        //public Token tokenize()
        //{
        //    foreach (var handler in this.handlers)
        //        if (handler.tokenizable(this)) return handler.tokenize(this);
        //    return null;
        //}
        public Token tokenize()
        {
            for (int i = 0; i < this.handlers.Length; i++)
            {
                if (this.handlers[i].tokenizable(this))
                {
                    if (this.handlers[i].isOptional)
                    {
                        this.handlers[i].tokenize(this);
                        i = -1;
                    }
                    else
                    {

                        return this.handlers[i].tokenize(this);

                    }

                }
            }

            return null;
        }

        public List<Token> all() { return this.tokens; }
    }

    public class WhiteSpaceTokenizer : Tokenizable
    {
        public WhiteSpaceTokenizer(bool isOptional = false)
        {
            this.isOptional = isOptional;
        }
        public override bool tokenizable(Tokenizer t)
        {
            return Char.IsWhiteSpace(t.input.peek());
        }
        static bool isWhiteSpace(Input input)
        {
            return Char.IsWhiteSpace(input.peek());
        }
        public override Token tokenize(Tokenizer t)
        {
            return new Token(t.input.Position, t.input.LineNumber,
                "whitespace", t.input.loop(isWhiteSpace));
        }
    }
    public class NewLineTokenizer : Tokenizable
    {
        public NewLineTokenizer(bool isOptional = false)
        {
            this.isOptional = isOptional;
        }
        public override bool tokenizable(Tokenizer t)
        {
            return String.Concat(t.input.peek(), t.input.peek(2)) == Environment.NewLine || t.input.peek().ToString() == Environment.NewLine;
        }

        public override Token tokenize(Tokenizer t)
        {
            t.input.step(Environment.NewLine.Length);
            return new Token(t.input.Position, t.input.LineNumber,
                "NewLine", Environment.NewLine);
        }
    }

    public class JSymbolsTokenizer : Tokenizable
    {

        private List<char> validSymobls;
        private char symbol;
        private string type;


        public JSymbolsTokenizer(char symbol, string type)
        {
            this.symbol = symbol;
            this.type = type;
        }
        public override bool tokenizable(Tokenizer t)
        {
            return this.symbol == t.input.peek();
        }

        public override Token tokenize(Tokenizer t)
        {
            char currentChar = t.input.peek();
            t.input.step();
            return new Token(t.input.Position, t.input.LineNumber,
                this.type, currentChar.ToString());
        }
    }


    public class KeywordsTokenizer : Tokenizable
    {
        private List<string> keywords;
        public KeywordsTokenizer(List<string> keywords)
        {
            this.keywords = keywords;
        }
        public override bool tokenizable(Tokenizer t)
        {
            return isLetter(t.input);
        }
        static bool isLetter(Input input)
        {
            char currentCharacter = input.peek();
            return Char.IsLetter(currentCharacter);
        }
        public override Token tokenize(Tokenizer t)
        {
            string value = t.input.loop(isLetter);

            string type;
            if (value == "null")
                type = "null";
            else
                type = "boolean";

            if (!this.keywords.Contains(value))
                throw new Exception("Unexpected token");

            return new Token(t.input.Position, t.input.LineNumber,
                type, value);
        }
    }

    public class StringTokenizer : Tokenizable
    {
        public override bool tokenizable(Tokenizer t)
        {
            return t.input.peek() == '"';
        }

        public override Token tokenize(Tokenizer t)
        {
            t.input.step();

            string value = t.input.loop(input => input.peek() != '"');

            if (t.input.peek() != '"')
                throw new Exception("Error");

            t.input.step();

            return new Token(t.input.Position, t.input.LineNumber,
                    "string", value);
        }
    }

    public class NumberTokenizer : Tokenizable
    {
        private bool isNegative;
        private bool isZero;
        public override bool tokenizable(Tokenizer t)
        {
            if ((t.input.peek(2) == '0' && t.input.peek() == '-') || t.input.peek() == '0')
            {
                isZero = true;
            }
            else
            {
                isZero = false;
            }

            if (t.input.peek() == '-' && Char.IsDigit(t.input.peek(2)))
            {
                isNegative = true;
                return true;
            }
            else
            {
                isNegative = false;
                return Char.IsDigit(t.input.peek());
            }

        }
        public override Token tokenize(Tokenizer t)
        {
            string value = "";
            if (isZero)
            {
                if (isNegative)
                {
                    if (Char.IsDigit(t.input.step().step().peek()))
                        throw new Exception("Error");

                    value = "-";

                }
                else
                {
                    if (Char.IsDigit(t.input.step().peek()))
                        throw new Exception("Error");
                }

                value += "0";
            }
            else
            {
                if (isNegative)
                {
                    value += t.input.step().Character;
                }

                value += t.input.loop(input => Char.IsDigit(t.input.peek()));

            }

            if (t.input.peek() == '.' && Char.IsDigit(t.input.peek(2)))
            {
                value += t.input.step().Character;
                value += t.input.loop(input => Char.IsDigit(t.input.peek()));
            }

            if ((t.input.peek() == 'E' || t.input.peek() == 'e') && (Char.IsDigit(t.input.peek(2)) || t.input.peek(2) == '+' || t.input.peek(2) == '-'))
            {
                value += t.input.step().Character;
                value += t.input.step().Character;

                value += t.input.loop(input => Char.IsDigit(t.input.peek()));
            }

            return new Token(t.input.Position, t.input.LineNumber, "number", value);
        }

    }
}
