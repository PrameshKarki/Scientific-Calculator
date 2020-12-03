/*Before editing any part of the code you must keep following ideas that i had implemented in your mind:
 1.I have assumed buttons as a element of matrix
 2.
*/

using Scientific_Calculator.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Scientific_Calculator
{
    public partial class MainForm : Form
    {
        //To Take the status of current command and current button index
        private string currentCommand = "";
        private string currentBtnIndex = "";

        //Status of which page is currently on Display
        bool isSecondPage = false;

        //Instantiating Calculator Class
        Calculator Calc = new Calculator();

        //Create default count of bracket button click is zero.
        int countOfBracket = 0;

        //status of calculator mode i.e Radian or Degree
        bool isDegree = false;

        //Evluation string which is used in the evluation
        string evaluationString = "";

        //Constructor
        public MainForm()
        {
            InitializeComponent();
        }

        //Form Load Event
        private void frmMain_Load(object sender, EventArgs e)
        {
            //To focus TextBox initially
            txtBoxExpression.Select();
            //Guna Shadow
            guna2ShadowForm1.SetShadowForm(this);
            //For the drag control feature
            guna2DragControl1.TargetControl = this;
        }

        //Method to Update expression on Text Field
        private void UpdateExpression(string ex)
        {
            //Update expression in txtBoxExpression as well as evaluationString
            evaluationString += ex;
            txtBoxExpression.Text += ex;
        }

        //Method to evaluate Expression
        private void Eval()
        {
            try
            {
                //This ensures at least one button which performs some function is clicked
                if (currentCommand != "")
                    Parse();
            }
            catch (Exception)
            {
                //If you want to display error message you can display from here
            }


            //Instantiating datatable
            DataTable dt = new DataTable();
            try
            {
                //Using Compute inbuilt method of datatable
                double result = Convert.ToDouble(dt.Compute(evaluationString, String.Empty));
                //Displaying result
                txtBoxResult.Text = Math.Round(result, 5).ToString();
            }
            catch (Exception)
            {
                //On Exception clear the result field
                txtBoxResult.Text = "";
            }

        }


        //Handling Click event on Equals button
        private void btnEquals_Click(object sender, EventArgs e)
        {
            //Swapping the values between two text fields
            SwapValues();
        }

        //Handling Click Events on Multiple Buttons
        private void HandleClick(object sender, EventArgs e)
        {
            //Type Casting
            Guna.UI2.WinForms.Guna2Button b = (Guna.UI2.WinForms.Guna2Button)sender;
            //Updating Expression When numeric button clicks
            UpdateExpression(b.Text);
        }

        //Handling Click events on Clear Button
        private void btnClear_Click(object sender, EventArgs e)
        {
            //Clear Both Input Fields
            txtBoxExpression.Text = "";
            txtBoxResult.Text = "";
            countOfBracket = 0;
            //Clear evaluationString as well
            evaluationString = "";
        }

        //Method to Swap Values between Two text fields
        private void SwapValues()
        {
            //Swap values in evaluation string as well
            evaluationString = txtBoxResult.Text;
            txtBoxExpression.Text = txtBoxResult.Text;
            txtBoxResult.Text = "";
            //Reset count of bracket and change sign button
            countOfBracket = 0;
            //Place Ibeam at the last of string
            txtBoxExpression.Select(txtBoxExpression.Text.Length, 0);


        }

        //Method to handle Click event on BackSpace Button
        private void btnBackSpace_Click(object sender, EventArgs e)
        {
            BackSpaceClick();
        }
        private void BackSpaceClick()
        {
            if (txtBoxExpression.Text.Length > 0)
            {
                //Temprorarily Hold Expression
                string tempExpression = txtBoxExpression.Text;

                //If removed char is ) then
                if (tempExpression[tempExpression.Length - 1] == '(')
                {
                    countOfBracket = 0;
                }
                //If removed char is ) then
                if (tempExpression[tempExpression.Length - 1] == ')')
                {
                    countOfBracket++;
                }
                txtBoxExpression.Text = tempExpression.Remove(tempExpression.Length - 1);
                //Set updated value in evaluationString as well
                evaluationString = txtBoxExpression.Text;
                //Clear ResultTextBox
                txtBoxResult.Text = "";
            }
            txtBoxExpression.Select(txtBoxExpression.Text.Length, 0);
            //Then evaluate again
            Eval();
        }


        //Handling TextChange Event on TextBoxExpression
        //Evaluate txtBoxExpression each time when text on it changes
        private void txtBoxExpression_TextChanged(object sender, EventArgs e)
        {
            Eval();

        }

        //Handling Click Events on brackets button
        private void btnBrackets_Click(object sender, EventArgs e)
        {
            //Regular Expression to check if string ends with any algebric opertaor
            Regex regex = new Regex(@"[+\-% */]$");
            //Regular expression to check if strikng ends with numeric digit
            Regex numericEnd = new Regex(@"\d$");

            //if Expression is empty
            if (countOfBracket == 0 && txtBoxExpression.Text.Length == 0)
            {
                UpdateExpression("(");
                countOfBracket++;
            }

            //If opening bracket is on last index
            else if (txtBoxExpression.Text.LastIndexOf('(') == txtBoxExpression.Text.Length - 1)
            {
                UpdateExpression("(");
                countOfBracket++;
            }
            //If string ends with algebric operator
            else if (regex.IsMatch(txtBoxExpression.Text))
            {
                UpdateExpression("(");
                countOfBracket++;
            }
            //
            else if (countOfBracket == 0 && numericEnd.IsMatch(txtBoxExpression.Text))
            {
                UpdateExpression("*(");
                countOfBracket++;

            }
            //Otherwise push closing bracket
            else
            {
                UpdateExpression(")");
                countOfBracket--;
            }



        }

        //Handling Click Events on ChangeSign Button
        private void btnChangeSign_Click(object sender, EventArgs e)
        {
            Regex regex = new Regex(@"[+\-%*/(]$");
            if (txtBoxExpression.Text.Length == 0)
            {
                UpdateExpression("(-");
                countOfBracket++;
            }
            else if (regex.IsMatch(txtBoxExpression.Text))

            {
                UpdateExpression("(-");
                countOfBracket++;


            }
            else if (txtBoxExpression.Text.EndsWith(")"))
            {
                UpdateExpression("*(-");
                countOfBracket++;
            }
        }


        //Method of parsing inputExpression on each time where text on inputExpression is Changed
        private void Parse()
        {
            //Declaration of variable
            int startPosition, endPosition, index;

            string parsedNumber = "";

            double number, result;

            //ScreenNumber is 1 if current screen is first screen otherwise 2
            int screenNumber = isSecondPage ? 2 : 1;

            //getting status
            int status = int.Parse(String.Concat(screenNumber.ToString(), currentBtnIndex));

            switch (status)
            {
                /*
                Explanation Here Case 102 Means:
                1->First Screen
                0->index of row i.e 0 means first row
                2->index of column i.e 2 means third row
                */
                case 102://Square Root
                    {
                        startPosition = evaluationString.IndexOf("sqrt(");
                        endPosition = evaluationString.IndexOf(')', startPosition);
                        //Where sqrt( length is 5 so
                        for (index = (startPosition + 5); index < endPosition; index++)
                        {
                            //Getting Number lies inside Bracket
                            parsedNumber += evaluationString[index];
                        }
                        //Parsing into float
                        number = double.Parse(parsedNumber);
                        //Round result
                        result = Math.Round(Calc.SquareRoot(number), 2);
                        //Than replaces in evaluationString
                        evaluationString = evaluationString.Remove(startPosition, (endPosition - startPosition + 1)).Insert(startPosition, result.ToString());
                        break;
                    }
                case 110://sin
                    {
                        startPosition = evaluationString.IndexOf("sin(");
                        endPosition = evaluationString.IndexOf(')', startPosition);
                        //Where sin( length is 4 so
                        for (index = (startPosition + 4); index < endPosition; index++)
                        {
                            //Getting Number lies inside Bracket
                            parsedNumber += evaluationString[index];
                        }
                        //Parsing into float
                        number = double.Parse(parsedNumber);
                        //Check calculator is in degree mode or not
                        if (isDegree) { number = Calc.DegreeToRadian(number); }
                        //Round result
                        result = Math.Round(Calc.Sin(number), 2);
                        //Than replaces in evaluationString
                        evaluationString = evaluationString.Remove(startPosition, (endPosition - startPosition + 1)).Insert(startPosition, result.ToString());
                        break;

                    }
                case 111://cos
                    {
                        startPosition = evaluationString.IndexOf("cos(");
                        endPosition = evaluationString.IndexOf(')', startPosition);
                        //Where cos( length is 4 so
                        for (index = (startPosition + 4); index < endPosition; index++)
                        {
                            //Getting Number lies inside Bracket
                            parsedNumber += evaluationString[index];
                        }
                        //Parsing into float
                        number = double.Parse(parsedNumber);
                        //Check calculator is in degree mode or not
                        if (isDegree) { number = Calc.DegreeToRadian(number); }

                        //Round result
                        result = Math.Round(Calc.Cos(number), 2);
                        //Than replaces in evaluationString
                        evaluationString = evaluationString.Remove(startPosition, (endPosition - startPosition + 1)).Insert(startPosition, result.ToString());
                        break;

                    }
                case 112://tan
                    {
                        startPosition = evaluationString.IndexOf("tan(");
                        endPosition = evaluationString.IndexOf(')', startPosition);
                        //Where tan( length is 4 so
                        for (index = (startPosition + 4); index < endPosition; index++)
                        {
                            //Getting Number lies inside Bracket
                            parsedNumber += evaluationString[index];
                        }
                        //Parsing into float
                        number = double.Parse(parsedNumber);
                        //Check calculator is in degree mode or not
                        if (isDegree) { number = Calc.DegreeToRadian(number); }

                        //Round result
                        result = Math.Round(Calc.Tan(number), 2);
                        //Than replaces in evaluationString
                        evaluationString = evaluationString.Remove(startPosition, (endPosition - startPosition + 1)).Insert(startPosition, result.ToString());
                        break;

                    }
                case 120://ln
                    {
                        startPosition = evaluationString.IndexOf("ln(");
                        endPosition = evaluationString.IndexOf(')', startPosition);
                        //Where ln( length is 3 so
                        for (index = (startPosition + 3); index < endPosition; index++)
                        {
                            //Getting Number lies inside Bracket
                            parsedNumber += evaluationString[index];
                        }
                        //Parsing into float
                        number = double.Parse(parsedNumber);
                        //Round result
                        result = Math.Round(Calc.Ln(number), 4);
                        //Than replaces in evaluationString
                        evaluationString = evaluationString.Remove(startPosition, (endPosition - startPosition + 1)).Insert(startPosition, result.ToString());
                        break;

                    }
                case 121://log
                    {
                        startPosition = evaluationString.IndexOf("log(");
                        endPosition = evaluationString.IndexOf(')', startPosition);
                        //Where log( length is 4 so
                        for (index = (startPosition + 4); index < endPosition; index++)
                        {
                            //Getting Number lies inside Bracket
                            parsedNumber += evaluationString[index];
                        }
                        //Parsing into float
                        number = double.Parse(parsedNumber);
                        //Round result
                        result = Math.Round(Calc.Log(number), 4);
                        //Than replaces in evaluationString
                        evaluationString = evaluationString.Remove(startPosition, (endPosition - startPosition + 1)).Insert(startPosition, result.ToString());
                        break;

                    }
                case 130://e^
                    {
                        startPosition = evaluationString.IndexOf("e^(");
                        endPosition = evaluationString.IndexOf(')', startPosition);
                        //Where e^( length is 3 so
                        for (index = (startPosition + 3); index < endPosition; index++)
                        {
                            //Getting Number lies inside Bracket
                            parsedNumber += evaluationString[index];
                        }
                        //Parsing into float
                        number = double.Parse(parsedNumber);
                        //Round result
                        result = Math.Round(Calc.Exponential(number), 5);
                        //Than replaces in evaluationString
                        evaluationString = evaluationString.Remove(startPosition, (endPosition - startPosition + 1)).Insert(startPosition, result.ToString());
                        break;

                    }
                case 140://absolute
                    {
                        startPosition = evaluationString.IndexOf("abs(");
                        endPosition = evaluationString.IndexOf(')', startPosition);
                        //Where abs( length is 4 so
                        for (index = (startPosition + 4); index < endPosition; index++)
                        {
                            //Getting Number lies inside Bracket
                            parsedNumber += evaluationString[index];
                        }
                        //Parsing into float
                        number = double.Parse(parsedNumber);
                        //Round result
                        result = Calc.Absolute(number);
                        //Than replaces in evaluationString
                        evaluationString = evaluationString.Remove(startPosition, (endPosition - startPosition + 1)).Insert(startPosition, result.ToString());
                        break;

                    }
                case 202://Cube root
                    {
                        startPosition = evaluationString.IndexOf("cbrt(");
                        endPosition = evaluationString.IndexOf(')', startPosition);
                        //Where cbrt( length is 5 so
                        for (index = (startPosition + 5); index < endPosition; index++)
                        {
                            //Getting Number lies inside Bracket
                            parsedNumber += evaluationString[index];
                        }
                        //Parsing into float
                        number = double.Parse(parsedNumber);
                        //Round result
                        result = Math.Round(Calc.CubeRoot(number), 2);
                        //Than replaces in evaluationString
                        evaluationString = evaluationString.Remove(startPosition, (endPosition - startPosition + 1)).Insert(startPosition, result.ToString());
                        break;

                    }
                case 210://SineInverse
                    {
                        startPosition = evaluationString.IndexOf("Asin(");
                        endPosition = evaluationString.IndexOf(')', startPosition);
                        //Where Asin( length is 5 so
                        for (index = (startPosition + 5); index < endPosition; index++)
                        {
                            //Getting Number lies inside Bracket
                            parsedNumber += evaluationString[index];
                        }
                        //Parsing into float
                        number = double.Parse(parsedNumber);
                        //Round result
                        result = Math.Round(Calc.SineInverse(number), 2);
                        //Check calculator is in degree mode or not
                        if (isDegree) { result = Math.Round(Calc.RadianToDegree(result), 3); }

                        //Than replaces in evaluationString
                        evaluationString = evaluationString.Remove(startPosition, (endPosition - startPosition + 1)).Insert(startPosition, result.ToString());
                        break;
                    }
                case 211://CosInverse
                    {
                        startPosition = evaluationString.IndexOf("Acos(");
                        endPosition = evaluationString.IndexOf(')', startPosition);
                        //Where Acos( length is 5 so
                        for (index = (startPosition + 5); index < endPosition; index++)
                        {
                            //Getting Number lies inside Bracket
                            parsedNumber += evaluationString[index];
                        }
                        //Parsing into float
                        number = double.Parse(parsedNumber);
                        //Round result
                        result = Math.Round(Calc.CosInverse(number), 2);
                        //Check calculator is in degree mode or not
                        if (isDegree) { result = Math.Round(Calc.RadianToDegree(result), 3); }
                        //Than replaces in evaluationString
                        evaluationString = evaluationString.Remove(startPosition, (endPosition - startPosition + 1)).Insert(startPosition, result.ToString());
                        break;

                    }
                case 212://TanInverse
                    {
                        startPosition = evaluationString.IndexOf("Atan(");
                        endPosition = evaluationString.IndexOf(')', startPosition);
                        //Where Atan( length is 5 so
                        for (index = (startPosition + 5); index < endPosition; index++)
                        {
                            //Getting Number lies inside Bracket
                            parsedNumber += evaluationString[index];
                        }
                        //Parsing into float
                        number = double.Parse(parsedNumber);
                        //Round result
                        result = Math.Round(Calc.TanInverse(number), 2);
                        //Check calculator is in degree mode or not
                        if (isDegree) { result = Math.Round(Calc.RadianToDegree(result), 3); }
                        //Than replaces in evaluationString
                        evaluationString = evaluationString.Remove(startPosition, (endPosition - startPosition + 1)).Insert(startPosition, result.ToString());
                        break;

                    }
                case 220://Sine Hyperbolic
                    {
                        startPosition = evaluationString.IndexOf("sinh(");
                        endPosition = evaluationString.IndexOf(')', startPosition);
                        //Where sinh( length is 5 so
                        for (index = (startPosition + 5); index < endPosition; index++)
                        {
                            //Getting Number lies inside Bracket
                            parsedNumber += evaluationString[index];
                        }
                        //Parsing into float
                        number = double.Parse(parsedNumber);
                        //Round result
                        result = Math.Round(Calc.SineHyperbolic(number), 2);
                        //Than replaces in evaluationString
                        evaluationString = evaluationString.Remove(startPosition, (endPosition - startPosition + 1)).Insert(startPosition, result.ToString());
                        break;

                    }
                case 221://Cos Hyperbolic
                    {
                        startPosition = evaluationString.IndexOf("cosh(");
                        endPosition = evaluationString.IndexOf(')', startPosition);
                        //Where cosh( length is 5 so
                        for (index = (startPosition + 5); index < endPosition; index++)
                        {
                            //Getting Number lies inside Bracket
                            parsedNumber += evaluationString[index];
                        }
                        //Parsing into float
                        number = double.Parse(parsedNumber);
                        //Round result
                        result = Math.Round(Calc.CosHyperbolic(number), 2);
                        //Than replaces in evaluationString
                        evaluationString = evaluationString.Remove(startPosition, (endPosition - startPosition + 1)).Insert(startPosition, result.ToString());
                        break;

                    }
                case 222://Tan Hyperbolic
                    {
                        startPosition = evaluationString.IndexOf("tanh(");
                        endPosition = evaluationString.IndexOf(')', startPosition);
                        //Where tanh( length is 5 so
                        for (index = (startPosition + 5); index < endPosition; index++)
                        {
                            //Getting Number lies inside Bracket
                            parsedNumber += evaluationString[index];
                        }
                        //Parsing into float
                        number = double.Parse(parsedNumber);
                        //Round result
                        result = Math.Round(Calc.TanHyperbolic(number), 2);
                        //Than replaces in evaluationString
                        evaluationString = evaluationString.Remove(startPosition, (endPosition - startPosition + 1)).Insert(startPosition, result.ToString());
                        break;

                    }
                case 230://Inverse Sine Hyperbolic
                    {
                        startPosition = evaluationString.IndexOf("Asinh(");
                        endPosition = evaluationString.IndexOf(')', startPosition);
                        //Where Asinh( length is 6 so
                        for (index = (startPosition + 6); index < endPosition; index++)
                        {
                            //Getting Number lies inside Bracket
                            parsedNumber += evaluationString[index];
                        }
                        //Parsing into float
                        number = double.Parse(parsedNumber);
                        //Round result
                        result = Math.Round(Calc.InverseSineHyperbolic(number), 2);
                        //Than replaces in evaluationString
                        evaluationString = evaluationString.Remove(startPosition, (endPosition - startPosition + 1)).Insert(startPosition, result.ToString());
                        break;

                    }
                case 231://Inverse Cos Hyperbolic
                    {
                        startPosition = evaluationString.IndexOf("Acosh(");
                        endPosition = evaluationString.IndexOf(')', startPosition);
                        //Where Acosh( length is 6 so
                        for (index = (startPosition + 6); index < endPosition; index++)
                        {
                            //Getting Number lies inside Bracket
                            parsedNumber += evaluationString[index];
                        }
                        //Parsing into float
                        number = double.Parse(parsedNumber);
                        //Round result
                        result = Math.Round(Calc.InverseCosHyperbolic(number), 2);
                        //Than replaces in evaluationString
                        evaluationString = evaluationString.Remove(startPosition, (endPosition - startPosition + 1)).Insert(startPosition, result.ToString());
                        break;

                    }
                case 232://Inverse Tan Hyperbolic
                    {
                        startPosition = evaluationString.IndexOf("Atanh(");
                        endPosition = evaluationString.IndexOf(')', startPosition);
                        //Where Atanh( length is 6 so
                        for (index = (startPosition + 6); index < endPosition; index++)
                        {
                            //Getting Number lies inside Bracket
                            parsedNumber += evaluationString[index];
                        }
                        //Parsing into float
                        number = double.Parse(parsedNumber);
                        //Round result
                        result = Math.Round(Calc.InverseTanHyperbolic(number), 2);
                        //Than replaces in evaluationString
                        evaluationString = evaluationString.Remove(startPosition, (endPosition - startPosition + 1)).Insert(startPosition, result.ToString());
                        break;

                    }
                case 240://2^
                    {
                        startPosition = evaluationString.IndexOf("2^(");
                        endPosition = evaluationString.IndexOf(')', startPosition);
                        //Where 2^( length is 3 so
                        for (index = (startPosition + 3); index < endPosition; index++)
                        {
                            //Getting Number lies inside Bracket
                            parsedNumber += evaluationString[index];
                        }
                        //Parsing into float
                        number = double.Parse(parsedNumber);
                        //Round result
                        result = Math.Round(Calc.PowerOf2(number), 2);
                        //Than replaces in evaluationString
                        evaluationString = evaluationString.Remove(startPosition, (endPosition - startPosition + 1)).Insert(startPosition, result.ToString());
                        break;

                    }
                //Same process on three cases so
                case 131:
                case 132:
                case 241:
                    {
                        string givenNumber = "";
                        startPosition = evaluationString.IndexOf("^(");
                        endPosition = evaluationString.IndexOf(")", startPosition);
                        //Where ^( length is 2 so
                        for (index = (startPosition + 2); index < endPosition; index++)
                        {
                            //Getting Number Lies inside Bracket
                            parsedNumber += evaluationString[index];
                        }
                        double power = double.Parse(parsedNumber);
                        //To get the actual number
                        index = startPosition - 1;

                        //Replace this with regular expression for short hand
                        while (index >= 0 && (evaluationString[index] != '+' && txtBoxExpression.Text[index] != '-' && txtBoxExpression.Text[index] != '*' && txtBoxExpression.Text[index] != '/' && txtBoxExpression.Text[index] != '%' && txtBoxExpression.Text[index] != '(' && txtBoxExpression.Text[index] != ')'))
                        {
                            givenNumber += evaluationString[index];
                            index--;
                        }
                        //Result
                        result = Math.Round(Calc.Power(double.Parse(givenNumber), power), 2);
                        //Replace result on txtBoxExpression
                        evaluationString = evaluationString.Remove(index + 1, endPosition - index).Insert(index + 1, result.ToString());

                        break;
                    }
                case 242://Factorial
                    {
                        startPosition = evaluationString.IndexOf("facto(");
                        endPosition = evaluationString.IndexOf(')', startPosition);
                        //Where ( length is 6 so
                        for (index = (startPosition + 6); index < endPosition; index++)
                        {
                            //Getting Number lies inside Bracket
                            parsedNumber += evaluationString[index];
                        }
                        //Parsing into float
                        number = Math.Abs(double.Parse(parsedNumber));
                        //Round result
                        result = Calc.Factorial(number);
                        //Than replaces in txBoxExpression
                        evaluationString = evaluationString.Remove(startPosition, (endPosition - startPosition + 1)).Insert(startPosition, result.ToString());
                        break;

                    }
                //On Default Case do nothing 

                default: { break; }

            }

        }


        //Handle Click on Buttons which are like function
        private void btnFunction_Clicked(object sender, EventArgs e)
        {
            //Type Casting
            Guna.UI2.WinForms.Guna2Button b = (Guna.UI2.WinForms.Guna2Button)sender;
            //To Parse details from button
            ParseFunctionText(b.Text, b.Name);

        }
        //Method to parse function text
        private void ParseFunctionText(string Text, string name)
        {
            Regex regex = new Regex(@"[+\-% */]$");


            //Declaration and Initialization of variable
            string templateString = "", command = Text;
            //Checking command Text to perform operation
            if (Text == "√") { command = "sqrt"; }
            if (Text == "3√x") { command = "cbrt"; }
            if (Text == "e^x") { command = "e^"; }
            if (Text == "|x|") { command = "abs"; }
            if (Text == "2^x") { command = "2^"; }
            if (Text == "x!") { command = "facto"; }

            //To save the status of currently which button is clicked
            currentCommand = command;
            templateString = name;

            //To get Last Two char from templateString
            currentBtnIndex = templateString.Substring(templateString.Length - 2);

            if (txtBoxExpression.Text.Length == 0 || regex.IsMatch(txtBoxExpression.Text))
            {
                //Updating Expression
                UpdateExpression(command + "(");
                //Increment countOfBracket
                countOfBracket++;
            }

            else
            {
                //Updating Expression
                UpdateExpression("*" + command + "(");
                //Increment countOfBracket
                countOfBracket++;
            }

        }
        //Handling Click Events in ChangeButton
        private void btnChangeButtons_Click(object sender, EventArgs e)
        {
            if (!isSecondPage)
            {
                btn00.Text = "1st";
                btn10.Text = "Asin";
                btn20.Text = "sinh";
                btn30.Text = "Asinh";
                btn40.Text = "2^x";
                btn11.Text = "Acos";
                btn21.Text = "cosh";
                btn31.Text = "Acosh";
                btn41.Text = "x^3";
                btn02.Text = "3√x";
                btn12.Text = "Atan";
                btn22.Text = "tanh";
                btn32.Text = "Atanh";
                btn42.Text = "x!";
                isSecondPage = true;
            }
            else
            {
                btn00.Text = "2nd";
                btn10.Text = "sin";
                btn20.Text = "ln";
                btn30.Text = "e^x";
                btn40.Text = "|x|";
                btn11.Text = "cos";
                btn21.Text = "log";
                btn31.Text = "x^2";
                btn41.Text = "PI";
                btn02.Text = "√";
                btn12.Text = "tan";
                btn22.Text = "1/x";
                btn32.Text = "x^y";
                btn42.Text = "e";
                isSecondPage = false;
            }

        }
        //When button of fifth row and 2nd column is clicked
        private void btn41_Click(object sender, EventArgs e)
        {
            Regex regex = new Regex(@"[+\-% */]$");

            if (!isSecondPage)
            {
                if (txtBoxExpression.Text.Length == 0 || regex.IsMatch(txtBoxExpression.Text))
                {
                    UpdateExpression(Calc.PI().ToString());
                }
                else
                {
                    UpdateExpression("*" + Calc.PI().ToString());

                }
            }
            //On second page
            else
            {
                Guna.UI2.WinForms.Guna2Button b = (Guna.UI2.WinForms.Guna2Button)sender;
                ParsePowerFunctionText(b.Text, b.Name);
            }

        }
        //When button of fifth row and 3rd column is clicked
        private void btn42_Click(object sender, EventArgs e)
        {
            Regex regex = new Regex(@"[+\-% */]$");

            if (!isSecondPage)
            {

                if (txtBoxExpression.Text.Length == 0 || regex.IsMatch(txtBoxExpression.Text))
                {
                    UpdateExpression(Calc.E().ToString());
                }
                //On second page
                else
                {
                    UpdateExpression("*" + Calc.E().ToString());

                }
            }
            //On second page
            else
            {
                //Type Casting
                Guna.UI2.WinForms.Guna2Button b = (Guna.UI2.WinForms.Guna2Button)sender;
                //To Parse details from button
                ParseFunctionText(b.Text, b.Name);
            }

        }

        //When button of third row and third column is clicked
        private void btn22_Click(object sender, EventArgs e)
        {
            Regex regex = new Regex(@"[+\-% */]$");

            if (!isSecondPage)//I.e When (1/x)button is clicked
            {
                if (txtBoxExpression.Text.Length == 0 || regex.IsMatch(txtBoxExpression.Text))
                {
                    UpdateExpression("1/");
                }
                else
                {
                    UpdateExpression("*1/");
                }


            }
            //On Second Page
            else
            {
                //Type Casting
                Guna.UI2.WinForms.Guna2Button b = (Guna.UI2.WinForms.Guna2Button)sender;
                //To Parse details from button
                ParseFunctionText(b.Text, b.Name);

            }
        }
        //Handling Click events on buttons which display power of something

        private void PowerFunction_Clicked(object sender, EventArgs e)
        {
            if (!isSecondPage)
            {
                Guna.UI2.WinForms.Guna2Button b = (Guna.UI2.WinForms.Guna2Button)sender;

                ParsePowerFunctionText(b.Text, b.Name);

            }
            //On Second Page
            else
            {
                //Type Casting
                Guna.UI2.WinForms.Guna2Button b = (Guna.UI2.WinForms.Guna2Button)sender;
                //To Parse details from button
                ParseFunctionText(b.Text, b.Name);

            }


        }
        //Method to parse power function text
        private void ParsePowerFunctionText(string command, string Name)
        {
            Regex regex = new Regex(@"[+\-% */()]$");

            string expression = txtBoxExpression.Text;
            //To Save the status of currently which button is clicked
            currentBtnIndex = Name.Substring(Name.Length - 2);
            currentCommand = command;

            //Check Whether txtBoxExpression is empty or not and it doesn't ends with /,*,-,+,%,(,)
            if (!(expression.Length == 0 || regex.IsMatch(txtBoxExpression.Text)))
            {
                if (command == "x^2")
                {
                    UpdateExpression("^(2)");
                }
                else if (command == "x^3")
                {

                    UpdateExpression("^(3)");
                }
                else
                {
                    UpdateExpression("^(");
                    countOfBracket++;
                }
            }

        }
        //Click event on Radian/Degree Change Button
        private void btn01_Click(object sender, EventArgs e)
        {
            if (!isDegree)
            {
                btn01.Text = "Rad";
                lblCalculatorMode.Text = "Deg";
                isDegree = true;
            }
            else
            {
                btn01.Text = "Deg";
                lblCalculatorMode.Text = "Rad";
                isDegree = false;
            }
        }
        //Close Button
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //Minimize button
        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        //Validate key press on txtBoxExpression
        private void txtBoxExpression_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                SwapValues();
            }
            else if (e.KeyChar == (char)Keys.Back)
            {
                BackSpaceClick();
                e.Handled = true;
            }
            else if (e.KeyChar == 37 || e.KeyChar == 40 || e.KeyChar == 41 || e.KeyChar == 42 || e.KeyChar == 43 || e.KeyChar == 45 || e.KeyChar == 46 || e.KeyChar == 47)
            {
                e.Handled = false;
                evaluationString += ((char)e.KeyChar).ToString();
            }
            else
            {
                bool status = !char.IsDigit(e.KeyChar);
                if (status) { e.Handled = true; }
                else
                {
                    evaluationString += ((char)e.KeyChar).ToString();
                    e.Handled = false;
                }

            }

        }

        private void btnShowSideBar_Click_1(object sender, EventArgs e)
        {
            if (sideBar.Visible == false)
            {
                transition.ShowSync(sideBar);
            }
            else
            {
                transition.HideSync(sideBar);
            }

        }
        //Button to close sidebar
        private void btnCloseSideBar_Click(object sender, EventArgs e)
        {
            transition.HideSync(sideBar);
        }
        //Button to close panel
        private void btnCLosePanel_Click(object sender, EventArgs e)
        {
            verticalTransition.HideSync(temperatureConversionBanner);
        }
        //Change label text
        private void radioButtonFirst_CheckedChanged(object sender, EventArgs e)
        {
            ResetField();
            if (radioButtonFirst.Checked)
            {
                labelFirst.Location = new Point(152, 189);
                labelSecond.Location = new Point(119, 235);
                labelFirst.Text = "Celcius:";
                labelSecond.Text = "Fahrenheit:";
            }
        }
        //Change label text
        private void radioButtonSecond_CheckedChanged(object sender, EventArgs e)
        {
            ResetField();
            if (radioButtonSecond.Checked)
            {
                labelFirst.Location = new Point(113, 184);
                labelSecond.Location = new Point(145, 235);
                labelFirst.Text = "Fahrenheit:";
                labelSecond.Text = "Celcius:";
            }
        }
        //Click on Temerature converter button
        private void btnTemperatureConverter_Click(object sender, EventArgs e)
        {
            transition.HideSync(sideBar);
            verticalTransition.ShowSync(temperatureConversionBanner);
        }
        //Reset fields
        private void ResetField()
        {
            txtBoxFirst.Text = "";
            txtBoxSecond.Text = "";
        }
        //Conversion of temperature
        private void txtBoxFirst_TextChanged(object sender, EventArgs e)
        {
            if (radioButtonFirst.Checked)
            {
                try
                {
                    string celciusInp = txtBoxFirst.Text;
                    if (celciusInp == "")
                    {
                        txtBoxSecond.Text = "";
                    }
                    else
                    {
                        double celciusInput = double.Parse(celciusInp);
                        txtBoxSecond.Text = Math.Round(((celciusInput * 9) / 5 + 32), 2).ToString();

                    }

                }
                catch (Exception)
                {

                }
            }
            else if (radioButtonSecond.Checked)
            {
                try
                {
                    string fahrenheitInp = txtBoxFirst.Text;
                    if (fahrenheitInp == "")
                    {
                        txtBoxSecond.Text = "";
                    }
                    else
                    {
                        double fahrenheitInput = double.Parse(fahrenheitInp);
                        txtBoxSecond.Text = Math.Round(((fahrenheitInput - 32) * (double)5 / 9), 2).ToString();

                    }

                }
                catch (Exception)
                {
                }
            }
        }

    }
}

//Pramesh Karki
