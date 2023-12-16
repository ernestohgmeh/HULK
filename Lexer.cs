using System;
using System.Collections.Generic;

namespace HULK
{        

    public class Token
    {
        public enum tokenType
        {
            keyWord,
            Number,
            Suma,
            Resta,
            Multipliacion,
            Division,
            pIzq,
            pDer,
            String,
            Boolean,
            implica,
            END
        }

        public tokenType Tipo { get; }
        public string Value { get; }

        public Token(tokenType tipo, string value = null)
        {
            Tipo = tipo;
            Value = value;
        }
    }

    public class Lexer
    {
        private string input;
        private int position;
        private char CurrentChar => position < input.Length ? input[position] : '\0';
        public List<Token> tokens = new List<Token>();
        public static List<string> Reservados = new List<string> {"print","let","in","if","else","function"};


        public Lexer(string input)
        {
            this.input = input;
            position = 0;
        }

        private string getNumeroCompleto()
        {
            string salida = "";

            while (CurrentChar != '\0' && (char.IsDigit(CurrentChar) || CurrentChar == '.') )
            {
                salida += CurrentChar;
                position++;
            }

            return salida;
        }

    
        private void SkipWhitespace()
        {
            while (CurrentChar != '\0' && char.IsWhiteSpace(CurrentChar))
            {
                position++;
            }
        }


        public void Tokenize()
        {
            while (CurrentChar != '\0')
            {
                #region basic math
                if (char.IsDigit(CurrentChar))
                {
                    tokens.Add(new Token(Token.tokenType.Number, getNumeroCompleto()));
                    continue;
                }
                else if (CurrentChar == '+')
                {
                    position++;
                    tokens.Add(new Token(Token.tokenType.Suma, "+"));
                    continue;
                }
                else if (CurrentChar == '-')
                {
                    position++;
                    tokens.Add(new Token(Token.tokenType.Resta, "-"));
                    continue;
                }
                else if (CurrentChar == '*')
                {
                    position++;
                    tokens.Add(new Token(Token.tokenType.Multipliacion, "*"));
                    continue;
                }
                else if (CurrentChar == '/')
                {
                    position++;
                    tokens.Add(new Token(Token.tokenType.Division, "/"));
                    continue;
                }
                else if (CurrentChar == '(')
                {
                    position++;
                    tokens.Add(new Token(Token.tokenType.pIzq, "("));
                    continue;
                }
                else if (CurrentChar == ')')
                {
                    position++;
                    tokens.Add(new Token(Token.tokenType.pDer, ")"));
                    continue;
                }
                else if (CurrentChar == '=' && input[position+1] == '>')
                {
                    tokens.Add(new Token(Token.tokenType.implica));
                    position++;
                    continue;
                }
                #endregion
                else if (char.IsLetter(CurrentChar))
                {
                    string output = "";
                    while (char.IsLetterOrDigit(CurrentChar))
                    {
                        output += CurrentChar;
                        position++;
                    }
                    if (Reservados.Contains(output))
                    {
                        tokens.Add(new Token(Token.tokenType.keyWord, output));
                        continue;
                    }
                    else if (output.ToLower() == "true" || output.ToLower() == "false")
                    {
                        tokens.Add(new Token(Token.tokenType.Boolean, output));
                        continue;
                    }
                    else
                    {
                        tokens.Add(new Token(Token.tokenType.String, output));
                        continue;
                    }
                }
                else if (char.IsWhiteSpace(CurrentChar))
                {
                    SkipWhitespace();
                    continue;
                }
                else
                {
                    throw new Exception("NO se admite este char: " + CurrentChar);
                }
            }
            tokens.Add(new Token(Token.tokenType.END));
        }
        /////////////////////////////////////////////////////
        private void DeclareFunction()
        {
            var functionName = "";
            while (CurrentChar != '\0' && char.IsLetterOrDigit(CurrentChar))
            {
                functionName += CurrentChar;
                position++;
            }
            tokens.Add(new Token(Token.tokenType.String, functionName));
            if (CurrentChar != '(')
            {
                throw new Exception("Se esperaba '(' después del nombre de la función");
            }
            position++;
            while (CurrentChar != '\0' && CurrentChar != ')')
            {
                SkipWhitespace();
                var parameter = "";
                while (CurrentChar != '\0' && char.IsLetterOrDigit(CurrentChar))
                {
                    parameter += CurrentChar;
                    position++;
                }
                tokens.Add(new Token(Token.tokenType.String, parameter));
                SkipWhitespace();
                if (CurrentChar == ',')
                {
                    position++;
                }
            }
            if (CurrentChar != ')')
            {
                throw new Exception($"Expected ')' after function parameters, but found '{CurrentChar}' at line {Line} and column {Column}");
            }
            position++;
            SkipWhitespace();
            if (CurrentChar != '=' && input[position+1
            ] != '>')
            {
                throw new Exception($"Expected '=>' after function declaration, but found '{CurrentChar}' at line {Line} and column {Column}");
            }
            SkipWhitespace();

            Tokenize();
        }

    }
}