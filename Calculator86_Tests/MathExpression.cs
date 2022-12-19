using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// E::= E + T | E - T | T         Операции + и -
// T::= T * M | T / M | M         Операции * и /
// M ::= (E) | -M | +M | C        Cкобки, унарный минус, переменная, константа

namespace Calculator86_Tests
{
    public class MathExpression
    {
        private readonly string _inExpression;
        private int _pos = -1; 

        private char CurChar => _inExpression[_pos];
        public MathExpression(string expression)
        {
            
            
            _inExpression =  expression.Replace(" ", "") + " "; 
            NextChar();
        }

        public decimal Parse()
        {
            return ParsePlusAndMinus();
        }

        private void NextChar()
        {
            _pos++;
        }
        
        // M ::= (E) | -M | +M | C
        private decimal ParseParenthesesAndUnary()
        {
            decimal result;

            switch (CurChar)
            {
                case '(':
                {
                    NextChar();
                    result = ParsePlusAndMinus();//Е

                    if (CurChar != ')')
                        throw new InvalidExpressionException(") is missing."); // Выбрасываем ошибку форматирования

                    NextChar();
                    break;
                }
                case '-':
                    NextChar();
                    result = -ParseParenthesesAndUnary();//-М
                    break;
                case '+':
                    NextChar();
                    result = ParseParenthesesAndUnary(); //+М
                    break;
                default:
                    result = ParseConst();
                    break;
            }

            return result;

        }

        // E::= E + T | E - T | T
        private decimal ParsePlusAndMinus() 
        {
            var result = ParseMultAndDiv();

            while (CurChar is '+' or '-') 
            {
                var tempChar = CurChar; 
                NextChar();

                if (tempChar == '+')
                    result += ParseMultAndDiv();
                else
                    result -= ParseMultAndDiv();
            }

            return result;
        }

        // T::= T * M | T / M | M
        private decimal ParseMultAndDiv()
        {
            var result = ParseParenthesesAndUnary();

            while (CurChar is '*' or '/')
            {
                var tempChar = CurChar;
                NextChar();

                if (tempChar == '*')
                    result *= ParseParenthesesAndUnary();
                else
                    result /= ParseParenthesesAndUnary();
            }

            return result;
        }

        // C  (считываем константу) 
        private decimal ParseConst()
        {
            var buffer = "";
            
            while (char.IsDigit(CurChar) || CurChar == '.')
            {
                buffer += CurChar;
                NextChar();
            }
            
            var result = decimal.Parse(buffer, CultureInfo.InvariantCulture);

            return result;
        }


    }
}
