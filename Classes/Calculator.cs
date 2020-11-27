using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scientific_Calculator.Classes
{
    class Calculator
    {
        //Cube Root
        public double CubeRoot(double Num)
        {
            return Math.Pow(Num,(double) 1 / 3);
            
        }
        //Square Root
        public double SquareRoot(double Num)
        {
            return Math.Pow(Num,(double) 1 / 2);
            
        }
        //Tan Inverse
        public double TanInverse(double Num)
        {
            return Math.Atan(Num);
        }
        //Sine Inverse
        public double SineInverse(double Num)
        {
            return Math.Asin(Num);
        }
        //Cos Inverse
        public double CosInverse(double Num)
        {
            if (Math.Abs(Num) > 1)
                throw new ArgumentException("Num");
            else
                return Math.Acos(Num);
        }
        //Tan Hyperbolic
        public double TanHyperbolic(double Num)
        {
            return Math.Tanh(Num);
        }
        //Cos Hyperbolic
        public double CosHyperbolic(double Num)
        {
            return Math.Cosh(Num);
        }
        //Sine Hyperbolic
        public Double SineHyperbolic(double Num)
        {
            return Math.Sinh(Num);
        }
        //Inverse Tan Hyperbolic
        public double InverseTanHyperbolic(double Num)
        {
            if (Math.Abs(Num) > 1)
                throw new ArgumentException("Num");
            else
            return 0.5 * Math.Log((1 + Num) / (1 - Num));
        }
        //Inverse Cos Hyperbolic
        public double InverseCosHyperbolic(double Num)
        {
            return Math.Log(Num + Math.Sqrt(Num * Num - 1));

        }
        //Inverse Sine Hyperbolic
        public double InverseSineHyperbolic(double Num)
        {
            return Math.Log(Num + Math.Sqrt(Num * Num + 1));
        }
        //Tan
        public double Tan(double Num)
        {
            return Math.Tan(Num);
        }
        //Cos
        public double Cos(double Num)
        {
            return Math.Cos(Num);
        }
        //Sin
        public double Sin(double Num)
        {
            return Math.Sin(Num);
        }
        //Ln
        public double Ln(double Num)
        {
            return Math.Log(Num);
        }
        //Log
        public double Log(double Num)
        {
            return Math.Log10(Num);
        }
        //Exponential
        public double Exponential(double Num)
        {
            return Math.Pow(Math.E,Num);
        }
        //Absolute
        public double Absolute(double Num)
        {
            return Math.Abs(Num);
        }
        //PI
        public double PI()
        {
            return Math.PI;
        }
        //E
        public double E()
        {
            return Math.E;
        }
        //PowerOf2
        public Double PowerOf2(double Num)
        {
            return Math.Pow(2, Num);
        }
        //Power
        public Double Power(double Num,Double power)
        {
            return Math.Pow(Num, power);
        }
        //Radian to Degree conversion
        public double DegreeToRadian(double num)
        {
            return num * (Math.PI /180);
        }
        //Radian to degree
        public double RadianToDegree(double num)
        {
            return num * (180 /Math.PI);
        }
        //Factorial
        public int Factorial(double num)
        {
            int factorial = 1;
            for(int index = 1; index <= num; index++)
            {
                factorial *= index;
            }
            return factorial;
        }
    }

}
