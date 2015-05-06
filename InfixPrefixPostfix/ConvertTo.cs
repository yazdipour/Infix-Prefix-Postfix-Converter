using InfixPrefixPostfixConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfixPrefixPostfixConverter
{
    class ConvertTo
    {
        Stack myStack;
        public string INstr = "", POSTstr = "", PREstr = "", eval = "";
        public ConvertTo(string myStr = "", string myStr2 = "", string myStr3 = "")
        {
            INstr = myStr;
            POSTstr = myStr2;
            PREstr = myStr3;
        }

        #region Checkers
        private bool isLetter(char ele)
        {
            return ((ele >= 'a' && ele <= 'z') || (ele >= 'A' && ele <= 'Z'));
        }
        private bool isOperator(char symbol)
        {
            switch (symbol)
            {
                case '+':
                case '-':
                case '*':
                case '/':
                case '^':
                case '(':
                case ')':
                case '%':
                    return true;
            }
            return false;
        }
        private bool isStringANum(string op1)
        {
            
            for (int i = 0; i < op1.Length; i++)
                if (Char.IsNumber(op1[i])) return true;
            return false;
        }
        private short priority(char symbol)
        {
            switch (symbol)
            {
                case '+':
                case '-':
                    return 2;
                case '*':
                case '/':
                case '%':
                    return 3;
                case '^':
                    return 4;
                default:
                    return 1;
            }
        }

        #endregion

        #region Callers
        public string InToPost()
        {
            //if (POSTstr != "") return POSTstr;
            if (INstr.Contains("Error")) return "Error";
            POSTstr = I2Po();
            return POSTstr;
        }
        public string InToPre()
        {
            PREstr = I2Pre();
            return PREstr;
        }
        public string Evaluate()
        {
            //if (eval != "") return eval;
            if (POSTstr.Contains("Error")) return "Error";
            else if (POSTstr == "")
            {
                if(INstr!="")POSTstr = InToPost();
                else if (PREstr != "") POSTstr = PreToPost();
                else return "Error";
            }
            eval = evaPost();
            return eval;
        }
        public string PostToIn()
        {
            INstr = Po2I();
            return INstr;
        }
        public string PreToIn()
        {
            INstr=Pre2I();
            return INstr;
        }
        public string PreToPost()
        {
            PreToIn();
            POSTstr = InToPost();
            return POSTstr;
        }
        public string PostToPre()
        {
            PostToIn();
            PREstr = InToPre();
            return PREstr;
        }

        #endregion

        #region CALCULATORS
        private string I2Po()
        {
            if (INstr.Contains("Error") || INstr == "") return "Error";

            myStack = new Stack(INstr.Length);
            string res = "";
            char ele;
            for (int pivot = 0; pivot != INstr.Length; pivot++)
            {
                ele = INstr[pivot];
                if (isLetter(ele))
                    res += ele;
                else if (Char.IsNumber(ele))
                {
                    string newStr = ele + "";
                    for (int i = pivot + 1; i < INstr.Length; i++, pivot++)
                    {
                        char newNum = INstr[i];
                        if (!Char.IsNumber(newNum)) break;
                        newStr += newNum;
                    }
                    if (newStr != "" + ele) newStr = "," + newStr + ",";
                    res += newStr;
                }
                else if (isOperator(ele))
                {
                    while (true)
                    {
                        if (myStack.isEmpty() || ele == '(')
                        {
                            myStack.push(ele.ToString());
                            break;
                        }
                        else if (ele == ')')
                        {
                            while (myStack.peek() != "(")
                                res += myStack.pop();
                            if (myStack.pop() != "(")
                                return "Error";
                            break;
                        }

                        else if (priority(ele) > priority(myStack.peek()[0]))
                        {
                            myStack.push(ele.ToString());
                            break;
                        }
                        else if (priority(ele) <= priority(myStack.peek()[0]))
                            res += myStack.pop();
                        else
                            return "Error";
                    }
                }
                else
                    return "Error";
            }
            while (!myStack.isEmpty())
                res += myStack.pop();
            return res;
        }
        private string evaPost()
        {
            myStack = new Stack(POSTstr.Length);
            for (int pivot = 0; pivot < POSTstr.Length; pivot++)
            {
                char ele = POSTstr[pivot];
                if (isLetter(ele) || Char.IsNumber(ele)) myStack.push(ele.ToString());
                else if (ele == ',')
                {
                    string newStr = "";
                    for (int i = pivot + 1; i < POSTstr.Length; i++)
                    {
                        char ch = POSTstr[i];
                        pivot++;
                        if (ch == ',') break;
                        newStr += ch;
                    }
                    myStack.push(newStr);
                }
                else if (isOperator(ele))
                {
                    ///if op2&op1 where number
                    string op2 = myStack.pop();
                    string op1 = myStack.pop();
                    if (isStringANum(op2) && isStringANum(op1))
                    {
                        long fst = Convert.ToInt64(op1);
                        long scnd = Convert.ToInt64(op2);
                        switch (ele)
                        {
                            case '+':
                                myStack.push((fst + scnd).ToString());
                                break;
                            case '-':
                                myStack.push((fst - scnd).ToString());
                                break;
                            case '*':
                                myStack.push((fst * scnd).ToString());
                                break;
                            case '/':
                                if (scnd == 0) return "Error-Division by zero";
                                myStack.push((fst / scnd).ToString());
                                break;
                            case '^':
                                myStack.push(Math.Pow(fst, scnd).ToString());
                                break;
                            case '%':
                                if (scnd == 0) return "Error-Division by zero";
                                myStack.push((fst % scnd).ToString());
                                break;
                            default:
                                return "Error- Unknown Char";
                        }
                    }
                    ///if op2&op1 where not NUM
                    else myStack.push('(' + op1 + ele + op2 + ')');
                }
            }
            return myStack.pop();
        }

        private string Po2I()
        {
            if (POSTstr.Contains("Error") || POSTstr == "") return "Error";

            String result = "", right = "", left = "";
            myStack = new Stack(POSTstr.Length);
            for (int pivot = 0; pivot < POSTstr.Length; pivot++)
            {
                if (isOperator(POSTstr[pivot]))
                {
                    right = myStack.pop();
                    left = myStack.pop();
                    myStack.push("(" + left + POSTstr[pivot] + right + ")");
                }
                else if (POSTstr[pivot] == ',')
                {
                    string newstr = "";
                    char ch = ' ';
                    while (true)
                    {
                        if (pivot < POSTstr.Length) pivot++;
                        ch = POSTstr[pivot];
                        if (ch == ',') break;
                        newstr += ch;
                    }
                    myStack.push(newstr);
                }
                else if (POSTstr[pivot] != ' ')
                {
                    myStack.push(POSTstr[pivot].ToString());
                }
            }
            result += myStack.pop();
            return result;
        }
        private string Pre2I()
        {
            if (PREstr.Contains("Error") || PREstr == "") return "Error";

            String result = "", right = "", left = "";
            myStack = new Stack(PREstr.Length);
            for (int pivot = PREstr.Length - 1; pivot >= 0; pivot--)
            {
                if (isOperator(PREstr[pivot]))
                {
                    left = myStack.pop();
                    right = myStack.pop();
                    myStack.push("(" + left + PREstr[pivot] + right + ")");
                }
                else if (PREstr[pivot] == ',')
                {
                    string newstr = "";
                    char ch = ' ';
                    while (true)
                    {
                        if (pivot >= 0) pivot--;
                        ch = PREstr[pivot];
                        if (ch == ',') break;
                        newstr = ch + newstr;
                    }
                    myStack.push(newstr);
                }
                else if (PREstr[pivot] != ' ')
                {
                    myStack.push(PREstr[pivot].ToString());
                }
            }
            result += myStack.pop();
            return result;
        }
        private string I2Pre()
        {
            if (INstr.Contains("Error") || INstr == "") return "Error";

            string result = "";
            myStack = new Stack(INstr.Length);
            string data = INstr;

            for (int pivot = data.Length - 1; pivot >= 0; pivot--)
            {
                if (isOperator(data[pivot]))
                {
                    if (data[pivot] == '(')
                    {
                        while (myStack.peek() != ")")
                        {
                            result = myStack.pop() + result;
                        }
                        myStack.pop();
                    }// (

                    else if (myStack.isEmpty() || priority(data[pivot]) >= priority(myStack.peek()[0]) || data[pivot] == ')')
                        myStack.push(data[pivot].ToString());
                    else
                    {
                        while (!myStack.isEmpty() && myStack.peek() != ")")
                        {
                            result = myStack.pop() + result;
                        }
                        myStack.push(data[pivot].ToString());
                    }
                }//if isOperator

                else if (Char.IsNumber(data[pivot]))
                {
                    string newStr = data[pivot] + "";
                    for (int i = pivot - 1; i >= 0; i--, pivot--)
                    {
                        char newNum = INstr[i];
                        if (!Char.IsNumber(newNum)) break;
                        newStr = newNum+newStr;
                    }
                    if (newStr != "" + data[pivot]) newStr = "," + newStr + ",";
                    result = newStr + result;
                }
                else if (data[pivot] != ' ')
                {
                    result = data[pivot] + result;
                }
            }

            while (!myStack.isEmpty())
            {
                result = myStack.pop() + result;
            }

            return result;
        }

        #endregion











        //private string I2Pre()
        //{
        //    if (PREstr.Contains("Error") || PREstr == "") return "Error";

        //    myStack = new Stack(INstr.Length);
        //    Stack myStackopt = new Stack(INstr.Length);
        //    for (int pivot = 0; pivot < INstr.Length; pivot++)
        //    {
        //        char ele = INstr[pivot];
        //        if (isOperator(ele))
        //        {
        //            while (true)
        //            {
        //                if (myStackopt.isEmpty() || ele == '(')
        //                {
        //                    myStackopt.push(ele.ToString());
        //                    break;
        //                }
        //                else if (ele == ')')
        //                {
        //                    while (myStackopt.peek() != "(")
        //                    {
        //                        string opt = myStackopt.pop();
        //                        string op2 = myStack.pop();
        //                        string op1 = myStack.pop();
        //                        myStack.push(opt + op1 + op2);
        //                    }
        //                    if (myStackopt.pop() != "(")
        //                        return "Error600";
        //                    break;
        //                }
        //                else if (priority(ele) > priority(myStackopt.peek()[0]))
        //                {
        //                    myStackopt.push(ele.ToString());
        //                    break;
        //                }
        //                else if (priority(ele) <= priority(myStackopt.peek()[0]))
        //                {
        //                    string opt = myStackopt.pop();
        //                    string op2 = myStack.pop();
        //                    string op1 = myStack.pop();
        //                    myStack.push(opt + op1 + op2);
        //                }
        //                else
        //                    return "Error";
        //            }
        //        }
        //        else if (isLetter(ele))
        //        {
        //            myStack.push(ele.ToString());
        //        }
        //        else if (Char.IsNumber(ele))
        //        {
        //            string newStr = ele + "";
        //            for (int i = pivot + 1; i < INstr.Length; i++, pivot++)
        //            {
        //                char newNum = INstr[i];
        //                if (!Char.IsNumber(newNum)) break;
        //                newStr += newNum;
        //            }
        //            if (newStr != "" + ele) newStr = "," + newStr + ",";
        //            myStack.push(newStr);
        //        }
        //        else
        //        {
        //            return "Error 2000";
        //        }
        //    }
        //    while (!myStackopt.isEmpty())
        //    {
        //        string opt = myStackopt.pop();
        //        string op2 = myStack.pop();
        //        string op1 = myStack.pop();
        //        myStack.push(opt + op1 + op2);
        //    }
        //    return myStack.pop();
        //}

    }
}