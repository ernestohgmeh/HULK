using System;
using System.Security.Cryptography.X509Certificates;
namespace HULK
{
    public class Program
    {
        public static void Main(string[] args)
        {
             void Empieza()
            {
            string entrada = Console.ReadLine();
            Lexer lexer = new Lexer(entrada);
            lexer.Tokenize();
            Parser parser = new Parser(lexer.tokens);
            System.Console.WriteLine(parser.evaluateElMio());   
            Empieza(); 
            }
        }    
    }  
}
