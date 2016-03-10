using System;

namespace InfixPrefixPostfix {
    internal class ConvertTo {
        public string INstr = "", POSTstr = "", PREstr = "", eval = "";
        private Stack myStack;

        public ConvertTo(string myStr = "",string myStr2 = "",string myStr3 = "") {
            INstr = myStr;
            POSTstr = myStr2;
            PREstr = myStr3;
        }

        #region Checkers

        private bool isLetter(char ele) {
            return (ele >= 'a' && ele <= 'z') || (ele >= 'A' && ele <= 'Z');
        }

        private bool isOperator(char symbol) {
            switch(symbol) {
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

        private bool isStringANum(string op1) {
            for(var i = 0;i < op1.Length;i++)
                if(char.IsNumber(op1[i]))
                    return true;
            return false;
        }

        private short priority(char symbol) {
            switch(symbol) {
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

        #region Public Callers

        public string InToPost() {
            //if (POSTstr != "") return POSTstr;
            if(INstr.Contains("Error"))
                return "Error";
            POSTstr = I2Po();
            return POSTstr;
        }

        public string InToPre() {
            PREstr = I2Pre();
            return PREstr;
        }

        public string Evaluate() {
            //if (eval != "") return eval;
            if(POSTstr.Contains("Error"))
                return "Error";
            if(POSTstr == "") {
                if(INstr != "")
                    POSTstr = InToPost();
                else if(PREstr != "")
                    POSTstr = PreToPost();
                else
                    return "Error";
            }
            eval = evaPost();
            return eval;
        }

        public string PostToIn() {
            INstr = Po2I();
            return INstr;
        }

        public string PreToIn() {
            INstr = Pre2I();
            return INstr;
        }

        public string PreToPost() {
            PreToIn();
            POSTstr = InToPost();
            return POSTstr;
        }

        public string PostToPre() {
            PostToIn();
            PREstr = InToPre();
            return PREstr;
        }

        #endregion

        #region CALCULATORS

        private string I2Po() {
            if(INstr.Contains("Error") || INstr == "")
                return "Error";

            myStack = new Stack(INstr.Length);
            var res = "";
            char ele;
            for(var pivot = 0;pivot != INstr.Length;pivot++) {
                ele = INstr[pivot];
                if(isLetter(ele))
                    res += ele;
                else if(char.IsNumber(ele)) {
                    var newStr = ele.ToString();
                    for(var i = pivot + 1;i < INstr.Length;i++, pivot++) {
                        var newNum = INstr[i];
                        if(!char.IsNumber(newNum))
                            break;
                        newStr += newNum;
                    }
                    if(newStr != ele.ToString())
                        newStr = "," + newStr + ",";
                    res += newStr;
                }
                else if(isOperator(ele)) {
                    while(true) {
                        if(myStack.isEmpty() || ele == '(') {
                            myStack.push(ele.ToString());
                            break;
                        }
                        if(ele == ')') {
                            while(myStack.peek() != "(")
                                res += myStack.pop();
                            if(myStack.pop() != "(")
                                return "Error";
                            break;
                        }

                        if(priority(ele) > priority(myStack.peek()[0])) {
                            myStack.push(ele.ToString());
                            break;
                        }
                        if(priority(ele) <= priority(myStack.peek()[0]))
                            res += myStack.pop();
                        else
                            return "Error";
                    }
                }
                else
                    return "Error";
            }
            while(!myStack.isEmpty())
                res += myStack.pop();
            return res;
        }



        private string Po2I() {
            if(POSTstr.Contains("Error") || POSTstr == "")
                return "Error";

            string result = "", right = "", left = "";
            myStack = new Stack(POSTstr.Length);
            for(var pivot = 0;pivot < POSTstr.Length;pivot++) {
                if(isOperator(POSTstr[pivot])) {
                    right = myStack.pop();
                    left = myStack.pop();
                    myStack.push("(" + left + POSTstr[pivot] + right + ")");
                }
                else if(POSTstr[pivot] == ',') {
                    var newstr = "";
                    var ch = ' ';
                    while(true) {
                        if(pivot < POSTstr.Length)
                            pivot++;
                        ch = POSTstr[pivot];
                        if(ch == ',')
                            break;
                        newstr += ch;
                    }
                    myStack.push(newstr);
                }
                else if(POSTstr[pivot] != ' ') {
                    myStack.push(POSTstr[pivot].ToString());
                }
            }
            result += myStack.pop();
            return result;
        }

        private string Pre2I() {
            if(PREstr.Contains("Error") || PREstr == "")
                return "Error";

            string result = "", right = "", left = "";
            myStack = new Stack(PREstr.Length);
            for(var pivot = PREstr.Length - 1;pivot >= 0;pivot--) {
                if(isOperator(PREstr[pivot])) {
                    left = myStack.pop();
                    right = myStack.pop();
                    myStack.push("(" + left + PREstr[pivot] + right + ")");
                }
                else if(PREstr[pivot] == ',') {
                    var newstr = "";
                    var ch = ' ';
                    while(true) {
                        if(pivot >= 0)
                            pivot--;
                        ch = PREstr[pivot];
                        if(ch == ',')
                            break;
                        newstr = ch + newstr;
                    }
                    myStack.push(newstr);
                }
                else if(PREstr[pivot] != ' ') {
                    myStack.push(PREstr[pivot].ToString());
                }
            }
            result += myStack.pop();
            return result;
        }

        private string I2Pre() {
            if(INstr.Contains("Error") || INstr == "")
                return "Error";

            var result = "";
            myStack = new Stack(INstr.Length);
            var data = INstr;

            for(var pivot = data.Length - 1;pivot >= 0;pivot--) {
                if(isOperator(data[pivot])) {
                    if(data[pivot] == '(') {
                        while(myStack.peek() != ")") {
                            result = myStack.pop() + result;
                        }
                        myStack.pop();
                    } // (

                    else if(myStack.isEmpty() || priority(data[pivot]) >= priority(myStack.peek()[0]) ||
                             data[pivot] == ')')
                        myStack.push(data[pivot].ToString());
                    else {
                        while(!myStack.isEmpty() && myStack.peek() != ")") {
                            result = myStack.pop() + result;
                        }
                        myStack.push(data[pivot].ToString());
                    }
                } //if isOperator

                else if(char.IsNumber(data[pivot])) {
                    var newStr = data[pivot] + "";
                    for(var i = pivot - 1;i >= 0;i--, pivot--) {
                        var newNum = INstr[i];
                        if(!char.IsNumber(newNum))
                            break;
                        newStr = newNum + newStr;
                    }
                    if(newStr != "" + data[pivot])
                        newStr = "," + newStr + ",";
                    result = newStr + result;
                }
                else if(data[pivot] != ' ') {
                    result = data[pivot] + result;
                }
            }

            while(!myStack.isEmpty()) {
                result = myStack.pop() + result;
            }

            return result;
        }
        private string evaPost() {
            myStack = new Stack(POSTstr.Length);
            for(var pivot = 0;pivot < POSTstr.Length;pivot++) {
                var ele = POSTstr[pivot];
                if(isLetter(ele) || char.IsNumber(ele))
                    myStack.push(ele.ToString());
                else if(ele == ',') {
                    var newStr = "";
                    for(var i = pivot + 1;i < POSTstr.Length;i++) {
                        var ch = POSTstr[i];
                        pivot++;
                        if(ch == ',')
                            break;
                        newStr += ch;
                    }
                    myStack.push(newStr);
                }
                else if(isOperator(ele)) {
                    ///if op2&op1 where number
                    var op2 = myStack.pop();
                    var op1 = myStack.pop();
                    if(isStringANum(op2) && isStringANum(op1)) {
                        var fst = Convert.ToInt64(op1);
                        var scnd = Convert.ToInt64(op2);
                        switch(ele) {
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
                                if(scnd == 0)
                                    return "Error-Division by zero";
                                myStack.push((fst / scnd).ToString());
                                break;
                            case '^':
                                myStack.push(Math.Pow(fst,scnd).ToString());
                                break;
                            case '%':
                                if(scnd == 0)
                                    return "Error-Division by zero";
                                myStack.push((fst % scnd).ToString());
                                break;
                            default:
                                return "Error- Unknown Char";
                        }
                    }
                    ///if op2&op1 where not NUM
                    else
                        myStack.push('(' + op1 + ele + op2 + ')');
                }
            }
            return myStack.pop();
        }
        #endregion
    }
}