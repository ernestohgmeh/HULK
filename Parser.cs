using System;
namespace HULK
{
    public class Parser
    {
        private List<Token> tokens;
        private int currentToken;

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
            currentToken = 0;
        }

        public double evaluateElMio()
        {
            return ParseAdditionSubtraction();
        }

        private double ParseAdditionSubtraction()
        {
            double left = ParseMultiplicationDivision();
            while (currentToken < tokens.Count)
            {
                Token op = tokens[currentToken];
                if (op.Tipo == Token.tokenType.Suma || op.Tipo == Token.tokenType.Resta)
                {
                    currentToken++;
                    double right = ParseMultiplicationDivision();
                    if (op.Tipo == Token.tokenType.Suma)
                    {
                        left += right;
                    }
                    else 
                    {
                        left -= right;
                    }
                }
                else
                {
                    break;
                }
            }
            return left;
        }

        private double ParseMultiplicationDivision()
        {
            double left = ParsePrimary();
            while (currentToken < tokens.Count)
            {
                Token op = tokens[currentToken];
                if (op.Tipo == Token.tokenType.Multipliacion || op.Tipo == Token.tokenType.Division)
                {
                    currentToken++;
                    double right = ParsePrimary();
                    if (op.Tipo == Token.tokenType.Multipliacion)
                    {
                         left *= right;
                    }
                    else
                    {
                        left /= right;
                    }
                }
                else
                {
                    break;
                }
            }
            return left;
        }

        private double ParsePrimary()
        {
            Token token = tokens[currentToken];
            if (token.Tipo == Token.tokenType.Number)
            {
                currentToken++;
                return double.Parse(token.Value);
            }
            else if (token.Tipo == Token.tokenType.pIzq)
            {
                currentToken++;
                double result = ParseAdditionSubtraction();
                if (tokens[currentToken].Tipo != Token.tokenType.pDer)
                    throw new Exception("Paréntesis desbalanceados");
                currentToken++;
                return result;
            }
            else
            {
                throw new Exception("Expresión aritmética inválida");
            }
        }
    }
}