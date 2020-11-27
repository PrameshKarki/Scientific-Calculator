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

namespace Scientific_Calculator
{
    public partial class MainForm : Form
    {
        //To Take the status of current command and current status index
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

            guna2ShadowForm1.SetShadowForm(this);
            guna2DragControl1.TargetControl = this;
        }
        //Method to Update expression on Text Field
        private void UpdateExpression(string ex)
        {
            txtBoxExpression.Text += ex;
        }


        //Method to evaluate Expression
        private void Eval()
        {
            try
            {
                if (currentCommand != "")
                    Parse();
            }
            catch (Exception)
            {
                //If you want to display error message you can display from here
            }

            //Take expression from text field
            string expression = txtBoxExpression.Text;

            //Instantiating datatable
            DataTable dt = new DataTable();
            try
            {
                //Using Compute inbuilt method of datatable
                string result = dt.Compute(expression, String.Empty).ToString();
                //Displaying result
                txtBoxResult.Text = result;
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
        }

        //Method to Swap Values between Two text fields
        private void SwapValues()
        {
            txtBoxExpression.Text = txtBoxResult.Text;
            txtBoxResult.Text = "";
            //Reset count of bracket and change sign button
            countOfBracket = 0;


        }

        //Method to handle Click event on BackSpace Button
        private void btnBackSpace_Click(object sender, EventArgs e)
        {
            if (txtBoxExpression.Text.Length > 0)
            {
                //Temprorarily Hold Expression
                string tempExpression = txtBoxExpression.Text;
                txtBoxExpression.Text = tempExpression.Remove(tempExpression.Length - 1);
                //Clear ResultTextBox
                txtBoxResult.Text = "";
            }
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
            else if (countOfBracket == 0 && txtBoxExpression.Text.Length != 0)
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

            if (txtBoxExpression.Text.Length == 0)
            {
                UpdateExpression("(-");
                countOfBracket++;
            }
            //Replace it with regular expression
            else if (txtBoxExpression.Text.EndsWith("+") || txtBoxExpression.Text.EndsWith("-") || txtBoxExpression.Text.EndsWith("*") || txtBoxExpression.Text.EndsWith("/") || txtBoxExpression.Text.EndsWith("%") || txtBoxExpression.Text.EndsWith("+") || txtBoxExpression.Text.EndsWith("("))

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
                        startPosition = txtBoxExpression.Text.IndexOf("sqrt(");
                        endPosition = txtBoxExpression.Text.IndexOf(')', startPosition);
                        //Where sqrt( length is 5 so
                        for (index = (startPosition + 5); index < endPosition; index++)
                        {
                            //Getting Number lies inside Bracket
                            parsedNumber += txtBoxExpression.Text[index];
                        }
                        //Parsing into float
                        number = double.Parse(parsedNumber);
                        //Round result
                        result = Math.Round(Calc.SquareRoot(number), 2);
                        //Than replaces in txBoxExpression
                        txtBoxExpression.Text = txtBoxExpression.Text.Remove(startPosition, (endPosition - startPosition + 1)).Insert(startPosition, result.ToString());
                        break;
                    }
                case 110://sin
                    {
                        startPosition = txtBoxExpression.Text.IndexOf("sin(");
                        endPosition = txtBoxExpression.Text.IndexOf(')', startPosition);
                        //Where sin( length is 4 so
                        for (index = (startPosition + 4); index < endPosition; index++)
                        {
                            //Getting Number lies inside Bracket
                            parsedNumber += txtBoxExpression.Text[index];
                        }
                        //Parsing into float
                        number = double.Parse(parsedNumber);
                        //Check calculator is in degree mode or not
                        if (isDegree) { number = Calc.DegreeToRadian(number); }
                        //Round result
                        result = Math.Round(Calc.Sin(number), 2);
                        //Than replaces in txBoxExpression
                        txtBoxExpression.Text = txtBoxExpression.Text.Remove(startPosition, (endPosition - startPosition + 1)).Insert(startPosition, result.ToString());
                        break;

                    }
                case 111://cos
                    {
                        startPosition = txtBoxExpression.Text.IndexOf("cos(");
                        endPosition = txtBoxExpression.Text.IndexOf(')', startPosition);
                        //Where cos( length is 4 so
                        for (index = (startPosition + 4); index < endPosition; index++)
                        {
                            //Getting Number lies inside Bracket
                            parsedNumber += txtBoxExpression.Text[index];
                        }
                        //Parsing into float
                        number = double.Parse(parsedNumber);
                        //Check calculator is in degree mode or not
                        if (isDegree) { number = Calc.DegreeToRadian(number); }

                        //Round result
                        result = Math.Round(Calc.Cos(number), 2);
                        //Than replaces in txBoxExpression
                        txtBoxExpression.Text = txtBoxExpression.Text.Remove(startPosition, (endPosition - startPosition + 1)).Insert(startPosition, result.ToString());
                        break;

                    }
                case 112://tan
                    {
                        startPosition = txtBoxExpression.Text.IndexOf("tan(");
                        endPosition = txtBoxExpression.Text.IndexOf(')', startPosition);
                        //Where tan( length is 4 so
                        for (index = (startPosition + 4); index < endPosition; index++)
                        {
                            //Getting Number lies inside Bracket
                            parsedNumber += txtBoxExpression.Text[index];
                        }
                        //Parsing into float
                        number = double.Parse(parsedNumber);
                        //Check calculator is in degree mode or not
                        if (isDegree) { number = Calc.DegreeToRadian(number); }

                        //Round result
                        result = Math.Round(Calc.Tan(number), 2);
                        //Than replaces in txBoxExpression
                        txtBoxExpression.Text = txtBoxExpression.Text.Remove(startPosition, (endPosition - startPosition + 1)).Insert(startPosition, result.ToString());
                        break;

                    }
                case 120://ln
                    {
                        startPosition = txtBoxExpression.Text.IndexOf("ln(");
                        endPosition = txtBoxExpression.Text.IndexOf(')', startPosition);
                        //Where ln( length is 3 so
                        for (index = (startPosition + 3); index < endPosition; index++)
                        {
                            //Getting Number lies inside Bracket
                            parsedNumber += txtBoxExpression.Text[index];
                        }
                        //Parsing into float
                        number = double.Parse(parsedNumber);
                        //Round result
                        result = Math.Round(Calc.Ln(number), 4);
                        //Than replaces in txBoxExpression
                        txtBoxExpression.Text = txtBoxExpression.Text.Remove(startPosition, (endPosition - startPosition + 1)).Insert(startPosition, result.ToString());
                        break;

                    }
                case 121://log
                    {
                        startPosition = txtBoxExpression.Text.IndexOf("log(");
                        endPosition = txtBoxExpression.Text.IndexOf(')', startPosition);
                        //Where log( length is 4 so
                        for (index = (startPosition + 4); index < endPosition; index++)
                        {
                            //Getting Number lies inside Bracket
                            parsedNumber += txtBoxExpression.Text[index];
                        }
                        //Parsing into float
                        number = double.Parse(parsedNumber);
                        //Round result
                        result = Math.Round(Calc.Log(number), 4);
                        //Than replaces in txBoxExpression
                        txtBoxExpression.Text = txtBoxExpression.Text.Remove(startPosition, (endPosition - startPosition + 1)).Insert(startPosition, result.ToString());
                        break;

                    }
                case 130://e^
                    {
                        startPosition = txtBoxExpression.Text.IndexOf("e^(");
                        endPosition = txtBoxExpression.Text.IndexOf(')', startPosition);
                        //Where e^( length is 3 so
                        for (index = (startPosition + 3); index < endPosition; index++)
                        {
                            //Getting Number lies inside Bracket
                            parsedNumber += txtBoxExpression.Text[index];
                        }
                        //Parsing into float
                        number = double.Parse(parsedNumber);
                        //Round result
                        result = Math.Round(Calc.Exponential(number), 6);
                        //Than replaces in txBoxExpression
                        txtBoxExpression.Text = txtBoxExpression.Text.Remove(startPosition, (endPosition - startPosition + 1)).Insert(startPosition, result.ToString());
                        break;

                    }
                case 140://absolute
                    {
                        startPosition = txtBoxExpression.Text.IndexOf("abs(");
                        endPosition = txtBoxExpression.Text.IndexOf(')', startPosition);
                        //Where abs( length is 4 so
                        for (index = (startPosition + 4); index < endPosition; index++)
                        {
                            //Getting Number lies inside Bracket
                            parsedNumber += txtBoxExpression.Text[index];
                        }
                        //Parsing into float
                        number = double.Parse(parsedNumber);
                        //Round result
                        result = Math.Round(Calc.Absolute(number));
                        //Than replaces in txBoxExpression
                        txtBoxExpression.Text = txtBoxExpression.Text.Remove(startPosition, (endPosition - startPosition + 1)).Insert(startPosition, result.ToString());
                        break;

                    }
                case 202://Cube root
                    {
                        startPosition = txtBoxExpression.Text.IndexOf("cbrt(");
                        endPosition = txtBoxExpression.Text.IndexOf(')', startPosition);
                        //Where cbrt( length is 5 so
                        for (index = (startPosition + 5); index < endPosition; index++)
                        {
                            //Getting Number lies inside Bracket
                            parsedNumber += txtBoxExpression.Text[index];
                        }
                        //Parsing into float
                        number = double.Parse(parsedNumber);
                        //Round result
                        result = Math.Round(Calc.CubeRoot(number), 2);
                        //Than replaces in txBoxExpression
                        txtBoxExpression.Text = txtBoxExpression.Text.Remove(startPosition, (endPosition - startPosition + 1)).Insert(startPosition, result.ToString());
                        break;

                    }
                case 210://SineInverse
                    {
                        startPosition = txtBoxExpression.Text.IndexOf("Asin(");
                        endPosition = txtBoxExpression.Text.IndexOf(')', startPosition);
                        //Where Asin( length is 5 so
                        for (index = (startPosition + 5); index < endPosition; index++)
                        {
                            //Getting Number lies inside Bracket
                            parsedNumber += txtBoxExpression.Text[index];
                        }
                        //Parsing into float
                        number = double.Parse(parsedNumber);
                        //Round result
                        result = Math.Round(Calc.SineInverse(number), 2);
                        //Check calculator is in degree mode or not
                        if (isDegree) { result = Math.Round(Calc.RadianToDegree(result), 3); }

                        //Than replaces in txBoxExpression
                        txtBoxExpression.Text = txtBoxExpression.Text.Remove(startPosition, (endPosition - startPosition + 1)).Insert(startPosition, result.ToString());
                        break;
                    }
                case 211://CosInverse
                    {
                        startPosition = txtBoxExpression.Text.IndexOf("Acos(");
                        endPosition = txtBoxExpression.Text.IndexOf(')', startPosition);
                        //Where Acos( length is 5 so
                        for (index = (startPosition + 5); index < endPosition; index++)
                        {
                            //Getting Number lies inside Bracket
                            parsedNumber += txtBoxExpression.Text[index];
                        }
                        //Parsing into float
                        number = double.Parse(parsedNumber);
                        //Round result
                        result = Math.Round(Calc.CosInverse(number), 2);
                        //Check calculator is in degree mode or not
                        if (isDegree) { result = Math.Round(Calc.RadianToDegree(result), 3); }
                        //Than replaces in txBoxExpression
                        txtBoxExpression.Text = txtBoxExpression.Text.Remove(startPosition, (endPosition - startPosition + 1)).Insert(startPosition, result.ToString());
                        break;

                    }
                case 212://TanInverse
                    {
                        startPosition = txtBoxExpression.Text.IndexOf("Atan(");
                        endPosition = txtBoxExpression.Text.IndexOf(')', startPosition);
                        //Where Atan( length is 5 so
                        for (index = (startPosition + 5); index < endPosition; index++)
                        {
                            //Getting Number lies inside Bracket
                            parsedNumber += txtBoxExpression.Text[index];
                        }
                        //Parsing into float
                        number = double.Parse(parsedNumber);
                        //Round result
                        result = Math.Round(Calc.TanInverse(number), 2);
                        //Check calculator is in degree mode or not
                        if (isDegree) { result = Math.Round(Calc.RadianToDegree(result), 3); }
                        //Than replaces in txBoxExpression
                        txtBoxExpression.Text = txtBoxExpression.Text.Remove(startPosition, (endPosition - startPosition + 1)).Insert(startPosition, result.ToString());
                        break;

                    }
                case 220://Sine Hyperbolic
                    {
                        startPosition = txtBoxExpression.Text.IndexOf("sinh(");
                        endPosition = txtBoxExpression.Text.IndexOf(')', startPosition);
                        //Where sinh( length is 5 so
                        for (index = (startPosition + 5); index < endPosition; index++)
                        {
                            //Getting Number lies inside Bracket
                            parsedNumber += txtBoxExpression.Text[index];
                        }
                        //Parsing into float
                        number = double.Parse(parsedNumber);
                        //Round result
                        result = Math.Round(Calc.SineHyperbolic(number), 2);
                        //Than replaces in txBoxExpression
                        txtBoxExpression.Text = txtBoxExpression.Text.Remove(startPosition, (endPosition - startPosition + 1)).Insert(startPosition, result.ToString());
                        break;

                    }
                case 221://Cos Hyperbolic
                    {
                        startPosition = txtBoxExpression.Text.IndexOf("cosh(");
                        endPosition = txtBoxExpression.Text.IndexOf(')', startPosition);
                        //Where cosh( length is 5 so
                        for (index = (startPosition + 5); index < endPosition; index++)
                        {
                            //Getting Number lies inside Bracket
                            parsedNumber += txtBoxExpression.Text[index];
                        }
                        //Parsing into float
                        number = double.Parse(parsedNumber);
                        //Round result
                        result = Math.Round(Calc.CosHyperbolic(number), 2);
                        //Than replaces in txBoxExpression
                        txtBoxExpression.Text = txtBoxExpression.Text.Remove(startPosition, (endPosition - startPosition + 1)).Insert(startPosition, result.ToString());
                        break;

                    }
                case 222://Tan Hyperbolic
                    {
                        startPosition = txtBoxExpression.Text.IndexOf("tanh(");
                        endPosition = txtBoxExpression.Text.IndexOf(')', startPosition);
                        //Where tanh( length is 5 so
                        for (index = (startPosition + 5); index < endPosition; index++)
                        {
                            //Getting Number lies inside Bracket
                            parsedNumber += txtBoxExpression.Text[index];
                        }
                        //Parsing into float
                        number = double.Parse(parsedNumber);
                        //Round result
                        result = Math.Round(Calc.TanHyperbolic(number), 2);
                        //Than replaces in txBoxExpression
                        txtBoxExpression.Text = txtBoxExpression.Text.Remove(startPosition, (endPosition - startPosition + 1)).Insert(startPosition, result.ToString());
                        break;

                    }
                case 230://Inverse Sine Hyperbolic
                    {
                        startPosition = txtBoxExpression.Text.IndexOf("Asinh(");
                        endPosition = txtBoxExpression.Text.IndexOf(')', startPosition);
                        //Where Asinh( length is 6 so
                        for (index = (startPosition + 6); index < endPosition; index++)
                        {
                            //Getting Number lies inside Bracket
                            parsedNumber += txtBoxExpression.Text[index];
                        }
                        //Parsing into float
                        number = double.Parse(parsedNumber);
                        //Round result
                        result = Math.Round(Calc.InverseSineHyperbolic(number), 2);
                        //Than replaces in txBoxExpression
                        txtBoxExpression.Text = txtBoxExpression.Text.Remove(startPosition, (endPosition - startPosition + 1)).Insert(startPosition, result.ToString());
                        break;

                    }
                case 231://Inverse Cos Hyperbolic
                    {
                        startPosition = txtBoxExpression.Text.IndexOf("Acosh(");
                        endPosition = txtBoxExpression.Text.IndexOf(')', startPosition);
                        //Where Acosh( length is 6 so
                        for (index = (startPosition + 6); index < endPosition; index++)
                        {
                            //Getting Number lies inside Bracket
                            parsedNumber += txtBoxExpression.Text[index];
                        }
                        //Parsing into float
                        number = double.Parse(parsedNumber);
                        //Round result
                        result = Math.Round(Calc.InverseCosHyperbolic(number), 2);
                        //Than replaces in txBoxExpression
                        txtBoxExpression.Text = txtBoxExpression.Text.Remove(startPosition, (endPosition - startPosition + 1)).Insert(startPosition, result.ToString());
                        break;

                    }
                case 232://Inverse Tan Hyperbolic
                    {
                        startPosition = txtBoxExpression.Text.IndexOf("Atanh(");
                        endPosition = txtBoxExpression.Text.IndexOf(')', startPosition);
                        //Where Atanh( length is 6 so
                        for (index = (startPosition + 6); index < endPosition; index++)
                        {
                            //Getting Number lies inside Bracket
                            parsedNumber += txtBoxExpression.Text[index];
                        }
                        //Parsing into float
                        number = double.Parse(parsedNumber);
                        //Round result
                        result = Math.Round(Calc.InverseTanHyperbolic(number), 2);
                        //Than replaces in txBoxExpression
                        txtBoxExpression.Text = txtBoxExpression.Text.Remove(startPosition, (endPosition - startPosition + 1)).Insert(startPosition, result.ToString());
                        break;

                    }
                case 240://2^
                    {
                        startPosition = txtBoxExpression.Text.IndexOf("2^(");
                        endPosition = txtBoxExpression.Text.IndexOf(')', startPosition);
                        //Where 2^( length is 3 so
                        for (index = (startPosition + 3); index < endPosition; index++)
                        {
                            //Getting Number lies inside Bracket
                            parsedNumber += txtBoxExpression.Text[index];
                        }
                        //Parsing into float
                        number = double.Parse(parsedNumber);
                        //Round result
                        result = Math.Round(Calc.PowerOf2(number), 2);
                        //Than replaces in txBoxExpression
                        txtBoxExpression.Text = txtBoxExpression.Text.Remove(startPosition, (endPosition - startPosition + 1)).Insert(startPosition, result.ToString());
                        break;

                    }
                //Same process on three cases so
                case 131:
                case 132:
                case 241:
                    {
                        string givenNumber = "";
                        startPosition = txtBoxExpression.Text.IndexOf("^(");
                        endPosition = txtBoxExpression.Text.IndexOf(")", startPosition);
                        //Where ^( length is 2 so
                        for (index = (startPosition + 2); index < endPosition; index++)
                        {
                            //Getting Number Lies inside Bracket
                            parsedNumber += txtBoxExpression.Text[index];
                        }
                        double power = double.Parse(parsedNumber);
                        //To get the actual number
                        index = startPosition - 1;

                        //Replace this with regular expression for short hand
                        while (index >= 0 && (txtBoxExpression.Text[index] != '+' && txtBoxExpression.Text[index] != '-' && txtBoxExpression.Text[index] != '*' && txtBoxExpression.Text[index] != '/' && txtBoxExpression.Text[index] != '%' && txtBoxExpression.Text[index] != '(' && txtBoxExpression.Text[index] != ')'))
                        {
                            givenNumber += txtBoxExpression.Text[index];
                            index--;
                        }
                        //Result
                        result = Math.Round(Calc.Power(double.Parse(givenNumber), power), 2);
                        //Replace result on txtBoxExpression
                        txtBoxExpression.Text = txtBoxExpression.Text.Remove(index + 1, endPosition - index).Insert(index + 1, result.ToString());

                        break;
                    }
                case 242://Factorial
                    {
                        startPosition = txtBoxExpression.Text.IndexOf("facto(");
                        endPosition = txtBoxExpression.Text.IndexOf(')', startPosition);
                        //Where ( length is 6 so
                        for (index = (startPosition + 6); index < endPosition; index++)
                        {
                            //Getting Number lies inside Bracket
                            parsedNumber += txtBoxExpression.Text[index];
                        }
                        //Parsing into float
                        number = Math.Abs(double.Parse(parsedNumber));
                        //Round result
                        result = Calc.Factorial(number);
                        //Than replaces in txBoxExpression
                        txtBoxExpression.Text = txtBoxExpression.Text.Remove(startPosition, (endPosition - startPosition + 1)).Insert(startPosition, result.ToString());
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
        private void ParseFunctionText(string Text, string name)
        {
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

            //Replace it with regular expression
            if (txtBoxExpression.Text.Length == 0 || txtBoxExpression.Text.EndsWith("+") || txtBoxExpression.Text.EndsWith("-") || txtBoxExpression.Text.EndsWith("*") || txtBoxExpression.Text.EndsWith("/") || txtBoxExpression.Text.EndsWith("%"))
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
            if (!isSecondPage)
            {
                //Replace it with regular expression
                if (txtBoxExpression.Text.Length == 0 || txtBoxExpression.Text.EndsWith("+") || txtBoxExpression.Text.EndsWith("-") || txtBoxExpression.Text.EndsWith("*") || txtBoxExpression.Text.EndsWith("/") || txtBoxExpression.Text.EndsWith("%"))
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
            if (!isSecondPage)
            {
                //Replace it with regular expression
                if (txtBoxExpression.Text.Length == 0 || txtBoxExpression.Text.EndsWith("+") || txtBoxExpression.Text.EndsWith("-") || txtBoxExpression.Text.EndsWith("*") || txtBoxExpression.Text.EndsWith("/") || txtBoxExpression.Text.EndsWith("%"))
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
            if (!isSecondPage)//I.e When (1/x)button is clicked
            {
                if (txtBoxExpression.Text.Length == 0 || txtBoxExpression.Text.EndsWith("+") || txtBoxExpression.Text.EndsWith("-") || txtBoxExpression.Text.EndsWith("*") || txtBoxExpression.Text.EndsWith("/") || txtBoxExpression.Text.EndsWith("%"))
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
            //CheckBox Whether txtBoxExpression is empty or not and it doesn't ends with /,*,-,+,%,(,)
            string expression = txtBoxExpression.Text;
            //To Save the status of currently which button is clicked
            currentBtnIndex = Name.Substring(Name.Length - 2);
            currentCommand = command;

            //Replace it with regular expression
            if (!(expression.Length == 0 || expression.EndsWith("/") || expression.EndsWith("*") || expression.EndsWith("-") || expression.EndsWith("+") || expression.EndsWith("(") || expression.EndsWith(")")))
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

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void txtBoxExpression_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                SwapValues();
                //To set the Ibeam always right position
                txtBoxExpression.Select(txtBoxExpression.Text.Length, 0);
                return;
            }
            for (int h = 58; h <= 127; h++)
            {

                if (e.KeyChar == h)
                {
                    e.Handled = true;
                }
            }
            for (int k = 32; k <= 47; k++)
            {
                if (e.KeyChar == 37 || e.KeyChar == 40 || e.KeyChar == 41 || e.KeyChar == 42 || e.KeyChar == 43 || e.KeyChar == 45 || e.KeyChar == 47)
                {
                    continue;
                }

                else if (e.KeyChar == k)
                    e.Handled = true;
            }

        }


    }
}
