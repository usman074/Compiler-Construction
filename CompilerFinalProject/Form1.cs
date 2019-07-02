using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompilerFinalProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Output.Clear();
            String input= tfinput.Text;
            symbolTable(input);
            
           
            
        }

        public void symbolTable(String o)
        {
            tftoken.Clear();
            symboltable.Clear();
            int row = 1;
            String[] a;
            ArrayList finalArray = new ArrayList();
            String input = o;
            List<String> keywordList = new List<String>();
            String[,] SymbolTable = new String[20, 6];
            int noOfVar = 0;
            keywordList.Add("int");
            keywordList.Add("float");
            keywordList.Add("boolean");
            keywordList.Add("String");
            keywordList.Add("if");
            keywordList.Add("else");
            keywordList.Add("begin");
            keywordList.Add("end");
            keywordList.Add("print");

            //Regular Expression for Variables
            Regex variable_Reg = new Regex(@"^[A-Za-z|_][A-Za-z|0-9]*$");
            //Regular Expression for Constants
            Regex constants_Reg = new Regex(@"^[0-9]+([.][0-9]+)?([e]([+|-])?[0-9]+)?$");
            //Regular Expression for Operators
            Regex operators_Reg = new Regex(@"^[-*+/><&&||=]$");
            //Regular Expression for Special_Characters
            Regex Special_Reg = new Regex(@"^[.,'\[\]{}();:?]$");
            String[] inp = Regex.Split(input, "\n");
            for (int i = 0; i < inp.Length; i++)
            {
                String line = "";
                //a contains each line of code separately splitted by space
                a = Regex.Split(inp[i], " ");
                //this loop checks each value at every index of and matches with above regexes
                for (int j = 0; j < a.Length; j++)
                {
                    //As we have splitted lines by space, so if a line contains every element valid 
                    //then we join these elements in below variable line to make it again a proper line
                    line = line + a[j] + " ";
                    Match Match_Variable = variable_Reg.Match(a[j] + "");
                    Match Match_Constant = constants_Reg.Match(a[j] + "");
                    Match Match_Operator = operators_Reg.Match(a[j] + "");
                    Match Match_Special = Special_Reg.Match(a[j] + "");
                    if (Match_Operator.Success)
                    {
                        // if a current lexeme is an operator then make a token e.g. < op, = >
                        tftoken.AppendText("< op, " + a[j].ToString() + "> ");
                    }
                    else if (Match_Constant.Success)
                    {
                        // if a current lexeme is a digit then make a token e.g. < digit, 12.33 >
                        tftoken.AppendText("< digit, " + a[j].ToString() + "> ");
                    }
                    else if (Match_Special.Success)
                    {
                        // if a current lexeme is a punctuation then make a token e.g. < punc, ; >
                        tftoken.AppendText("< punc, " + a[j].ToString() + "> ");
                    }
                    else if (Match_Variable.Success)
                    {
                        // if a current lexeme is a variable and not a keyword 
                        if (!keywordList.Contains(a[j].ToString())) // if it is not a keyword
                        {
                            noOfVar++;
                            tftoken.AppendText("<var" + noOfVar + ", " + (i + 1) + "> ");
                        }
                        // if a current lexeme is not a variable but a keyword then make a token such as: <keyword, int>
                        else
                        {
                            tftoken.AppendText("<keyword, " + a[j].ToString() + "> ");
                        }

                    }

                }
                finalArray.Add(line);
                tftoken.AppendText("\n");
            }
            for (int i = 0; i < finalArray.Count; i++)
            {
                Regex reg1 = new Regex(@"^(int|float)\s([A-Za-z|_][A-Za-z|0-9]{0,10})\s(=)\s([0-9]+([.][0-9]+)?([e][+|-]?[0-9]+)?)\s(;)\s$");
                Regex reg2 = new Regex(@"^(String)\s([A-Za-z|_][A-Za-z|0-9]{0,10})\s(=)\s[']\s([A-Za-z|_][A-Za-z|0-9]{0,30})\s[']\s(;)\s$");
                Regex reg3 = new Regex(@"^(boolean)\s([A-Za-z|_][A-Za-z|0-9]{0,10})\s(=)\s(true|false)\s(;)\s$");
                Match c1 = reg1.Match(finalArray[i].ToString());
                Match c2 = reg2.Match(finalArray[i].ToString());
                Match c3 = reg3.Match(finalArray[i].ToString());
                
                    if (c1.Success)
                    {
                        int temp_row = row;
                        //finalArray contains all the lines at each index
                        String[] q = Regex.Split(finalArray[i].ToString(), " ");
                        //this loop checks if the variable in a given line is already in symbol table or not
                        for (int l = 1; l <= SymbolTable.GetLength(0); l++)
                        {
                            if (SymbolTable[l, 2] != null)
                            {
                                //if its in symbol table then we just get its row value and update the new values in given line at that row
                                if (SymbolTable[l, 2].Equals(q[1]))
                                {
                                    row = l;
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            else 
                            {
                                break;
                            }
                           
                        }
                        

                        SymbolTable[row, 1] = row.ToString(); //index

                        SymbolTable[row, 2] = q[1]; //variable name

                        SymbolTable[row, 3] = q[0]; //type

                        SymbolTable[row, 4] = q[3]; //value

                        SymbolTable[row, 5] = (i + 1).ToString(); // line number

                        symboltable.AppendText(SymbolTable[row, 1].ToString() + " \t ");
                        symboltable.AppendText(SymbolTable[row, 2].ToString() + " \t ");
                        symboltable.AppendText(SymbolTable[row, 3].ToString() + " \t ");
                        symboltable.AppendText(SymbolTable[row, 4].ToString() + " \t ");
                        symboltable.AppendText(SymbolTable[row, 5].ToString() + " \n ");
                        //if the variable in a given line is not already in symbol table then we just increment the row value
                        if (temp_row == row)
                        {
                            row++;
                        }
                        //if the variable in a given line is already in symbol table then we restore the value of row
                        else 
                        {
                            row = temp_row;
                        }
                    }
                    else if (c2.Success)
                    {
                        int temp_row = row;
                        String[] q = Regex.Split(finalArray[i].ToString(), " ");
                        for (int l = 1; l <= SymbolTable.GetLength(0); l++)
                        {
                            if (SymbolTable[l, 2] != null)
                            {
                                if (SymbolTable[l, 2].Equals(q[1]))
                                {
                                    row = l;
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                break;
                            }

                        }

                        SymbolTable[row, 1] = row.ToString(); //index

                        SymbolTable[row, 2] = q[1]; //variable name

                        SymbolTable[row, 3] = q[0]; //type

                        SymbolTable[row, 4] = q[4]; //value

                        SymbolTable[row, 5] = (i + 1).ToString(); // line number

                        symboltable.AppendText(SymbolTable[row, 1].ToString() + " \t ");
                        symboltable.AppendText(SymbolTable[row, 2].ToString() + " \t ");
                        symboltable.AppendText(SymbolTable[row, 3].ToString() + " \t ");
                        symboltable.AppendText(SymbolTable[row, 4].ToString() + " \t ");
                        symboltable.AppendText(SymbolTable[row, 5].ToString() + " \n ");
                        if (temp_row == row)
                        {
                            row++;
                        }
                        else
                        {
                            row = temp_row;
                        }
                    }
                    else if (c3.Success)
                    {
                        int temp_row = row;
                        String[] q = Regex.Split(finalArray[i].ToString(), " ");
                        for (int l = 1; l <= SymbolTable.GetLength(0); l++)
                        {
                            if (SymbolTable[l, 2] != null)
                            {
                                if (SymbolTable[l, 2].Equals(q[1]))
                                {
                                    row = l;
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                break;
                            }

                        }

                        SymbolTable[row, 1] = row.ToString(); //index

                        SymbolTable[row, 2] = q[1]; //variable name

                        SymbolTable[row, 3] = q[0]; //type

                        SymbolTable[row, 4] = q[3]; //value

                        SymbolTable[row, 5] = (i + 1).ToString(); // line number

                        symboltable.AppendText(SymbolTable[row, 1].ToString() + " \t ");
                        symboltable.AppendText(SymbolTable[row, 2].ToString() + " \t ");
                        symboltable.AppendText(SymbolTable[row, 3].ToString() + " \t ");
                        symboltable.AppendText(SymbolTable[row, 4].ToString() + " \t ");
                        symboltable.AppendText(SymbolTable[row, 5].ToString() + " \n ");
                        if (temp_row == row)
                        {
                            row++;
                        }
                        else
                        {
                            row = temp_row;
                        }
                    }
                tftoken.AppendText("\n");
                
            }
            Parsing(o);
            SemanticAnalysis(o, SymbolTable);
        }

        

        public void Parsing(String o)
        {

            ArrayList States = new ArrayList();
            Stack<String> Stack = new Stack<String>();
            String Parser = "";

            States.Add("Programm_Program");//0
            States.Add("Program_begin ( ) { Code } end"); //1
            States.Add("Code_DecS");//2
            States.Add("Code_SDecS");//3
            States.Add("Code_AssS");//4
            States.Add("Code_IffS");//5
            States.Add("DecS_type Var = Const ;");//6
            States.Add("type_int");//7
            States.Add("type_float");//8
            States.Add("type_boolean");//9
            States.Add("SDecS_String Var = ' Const ' ;");//10
            States.Add("AssS_Var = Var op Var ;");//11
            States.Add("op_+");//12
            States.Add("op_-");//13
            States.Add("op_*");//14
            States.Add("op_/");//15
            States.Add("IffS_if ( Var Lop Var ) { PriS } else { PriS }");//16
            States.Add("Lop_<");//17
            States.Add("Lop_>");//18
            States.Add("Lop_==");//19
            States.Add("PriS_print Var ;");//20
            Stack.Push("0");
            int pointer = 0;

            Dictionary<String, Dictionary<Regex, String>> dict = new Dictionary<string, Dictionary<Regex, string>>();
            try
            {

                dict.Add("0", new Dictionary<Regex, string>()
            {
                {
                    new Regex("^begin$"),"S1"
                },
                {
                    new Regex("^Program$"),"S45"
                },
                {
                    new Regex("^Programm$"),"S46"
                },
                {
                    new Regex(@"^[!~@#$%^&<>?()/*+-=;]"),""
                },
                {
                   new Regex("^int|float|boolean|String|type|if$"),""
                },
                {
                    new Regex("^print|end|Var|Const$"),""
                },
                {
                    new Regex(@"^{"),""
                },
                {
                    new Regex(@"^}"),""
                },
                {
                    new Regex(@"^error"),""
                }
            });
                dict.Add("1", new Dictionary<Regex, string>()
            {
                 {
                    new Regex(@"^[(]$"),"S2"
                },
                {
                    new Regex("^begin$"),""
                },
                {
                    new Regex("^[!~@#$%^&<>?)/*+-=;]$"),""
                },
                {
                   new Regex("^int|float|boolean|String|type|if$"),""
                },
                {
                    new Regex("^print|end|Var|Const$"),""
                },
                {
                    new Regex(@"^{"),""
                },
                {
                    new Regex(@"^}"),""
                },
                {
                    new Regex(@"^error"),""
                }
            });
                dict.Add("2", new Dictionary<Regex, string>()
            {
                {
                    new Regex(@"^[)]$"),"S3"
                },
                {
                    new Regex("^begin$"),""
                },
                {
                    new Regex("^[!~@#$%^&<>?(/*+-=;]$"),""
                },
                {
                   new Regex("^int|float|boolean|String|type|if$"),""
                },
                {
                    new Regex("^print|end|Var|Const$"),""
                },
                {
                    new Regex(@"^{"),""
                },
                {
                    new Regex(@"^}"),""
                },
                {
                    new Regex(@"^error"),""
                }
            });
                dict.Add("3", new Dictionary<Regex, string>()
            {
                {
                    new Regex("^{$"),"S4"
                },
                {
                    new Regex("^begin$"),""
                },
                {
                    new Regex("^[!~@#$%^&<>?()/*+-=;]$"),""
                },
                {
                   new Regex("^int|float|boolean|String|type|if$"),""
                },
                {
                    new Regex("^print|end|Var|Const$"),""
                },
                {
                    new Regex(@"^}"),""
                },
                {
                    new Regex(@"^error"),""
                }
            });
                dict.Add("4", new Dictionary<Regex, string>()
            {
                {
                   new Regex("^[$]$"),"R1"
                },
                {
                   new Regex("^int$"),"S5"
                },
                {
                    new Regex("^float$"),"S6"
                },
                {
                    new Regex("^boolean$"),"S7"
                },
                {
                    new Regex("^DecS$"),"S13"
                },
                {
                    new Regex("^SDecS$"),"S22"
                },
                {
                    new Regex("^IffS$"),"S44"
                },
                {
                    new Regex("^Code$"),"S4"
                },
                {
                    new Regex("^AssS$"),"S30"
                },
                {
                    new Regex("^}$"),"S14"
                },
                {
                   new Regex("^String$"),"S15"
                },
                {
                    new Regex("^if$"),"S31"
                },
                {
                    new Regex("^Var$"),"S23"
                },
                {
                    new Regex("^type$"),"S8"
                },
                {
                    new Regex("^begin$"),""
                },
                {
                    new Regex("^[!~@#$%^&<>?()/*+-=;]$"),""
                },
                {
                    new Regex("^print|end|Const$"),""
                },
                {
                    new Regex(@"^{"),""
                },
                {
                    new Regex(@"^error"),""
                }
                
                
            });
                dict.Add("5", new Dictionary<Regex, string>()
            {
                {
                   new Regex("^Var$"),"R7"
                },
                {
                    new Regex("^begin$"),""
                },
                {
                    new Regex("^[!~@#$%^&<>?()/*+-=;]$"),""
                },
                {
                   new Regex("^int|float|boolean|type|if$"),""
                },
                {
                    new Regex("^print|end|Const$"),""
                },
                {
                   new Regex("^String$"),""
                },
                {
                    new Regex(@"^{"),""
                },
                {
                    new Regex(@"^}"),""
                },
                {
                    new Regex(@"^error"),""
                }
                
            });
                dict.Add("6", new Dictionary<Regex, string>()
            {
                {
                   new Regex("^Var$"),"R8"
                },
                {
                    new Regex("^[begin]$"),""
                },
                {
                    new Regex("^[!~@#$%^&<>?()/*+-=;]$"),""
                },
                {
                   new Regex("^int|float|boolean|type|if$"),""
                },
                {
                    new Regex("^print|end|Const$"),""
                },
                {
                   new Regex("^String$"),""
                },
                {
                    new Regex(@"^{"),""
                },
                {
                    new Regex(@"^}"),""
                },
                {
                    new Regex(@"^error"),""
                }
                
            });
                dict.Add("7", new Dictionary<Regex, string>()
            {
                {
                   new Regex("^Var$"),"R9"
                },
                {
                    new Regex("^begin$"),""
                },
                {
                    new Regex("^[!~@#$%^&<>?()/*+-=;]$"),""
                },
                {
                   new Regex("^int|float|boolean|type|if$"),""
                },
                {
                    new Regex("^print|end|Const$"),""
                },
                {
                   new Regex("^String$"),""
                },
                {
                    new Regex(@"^{"),""
                },
                {
                    new Regex(@"^}"),""
                },
                {
                    new Regex(@"^error"),""
                }
                
            });
                dict.Add("8", new Dictionary<Regex, string>()
            {
                {
                    new Regex("^Var"),"S9"
                },
                {
                    new Regex("^begin$"),""
                },
                {
                    new Regex("^[!~@#$%^&<>?()/*+-=;]$"),""
                },
                {
                   new Regex("^int|float|boolean|type|if$"),""
                },
                {
                    new Regex("^print|end|Const$"),""
                },
                {
                   new Regex("^String$"),""
                },
                {
                    new Regex(@"^{"),""
                },
                {
                    new Regex(@"^}"),""
                },
                {
                    new Regex(@"^error"),""
                }
                
            });
                dict.Add("9", new Dictionary<Regex, string>()
            {
                {
                    new Regex("^="),"S10"
                },
                {
                    new Regex("^begin$"),""
                },
                {
                    new Regex("^[!~@#$%^&<>?()/*+-;]$"),""
                },
                {
                   new Regex("^int|float|boolean|type|if$"),""
                },
                {
                    new Regex("^print|end|Var|Const$"),""
                },
                {
                   new Regex("^String$"),""
                },
                {
                    new Regex(@"^{"),""
                },
                {
                    new Regex(@"^}"),""
                },
                {
                    new Regex(@"^error"),""
                }
                
            });
                dict.Add("10", new Dictionary<Regex, string>()
            {
                {
                    new Regex("^Const"),"S11"
                },
                {
                    new Regex(@"^[']$"),""
                },
                {
                    new Regex("^begin$"),""
                },
                {
                    new Regex("^[!~@#$%^&<>?()/*+-=;]$"),""
                },
                {
                   new Regex("^int|float|boolean|type|if$"),""
                },
                {
                    new Regex("^print|end|Var$"),""
                },
                {
                   new Regex("^String$"),""
                },
                {
                    new Regex(@"^{"),""
                },
                {
                    new Regex(@"^}"),""
                },
                {
                    new Regex(@"^error"),""
                }
                
            });
                dict.Add("11", new Dictionary<Regex, string>()
            {
                {
                    new Regex("^;"),"S12"
                },
                {
                    new Regex("^begin$"),""
                },
                {
                    new Regex("^[!~@#$%^&<>?()/*+-=]$"),""
                },
                {
                   new Regex("^int|float|Var|boolean|type|if$"),""
                },
                {
                    new Regex("^print|end|Const$"),""
                },
                {
                   new Regex("^String$"),""
                },
                {
                    new Regex(@"^{"),""
                },
                {
                    new Regex(@"^}"),""
                },
                {
                    new Regex(@"^error"),""
                }
                
            });
                dict.Add("12", new Dictionary<Regex, string>()
            {
                {
                   new Regex("^String$"),"R6"
                },
                {
                    new Regex("^begin$"),""
                },
                {
                    new Regex("^[!~@#$%^&<>?()/*+-=;]$"),""
                },
                {
                   new Regex("^int|float|Var|boolean|if$"),"R6"
                },
                {
                    new Regex("^print|end|Const|type$"),""
                },
                {
                    new Regex(@"^{"),""
                },
                {
                    new Regex(@"^}"),"R6"
                },
                {
                    new Regex(@"^error"),""
                }
                
            });
                dict.Add("13", new Dictionary<Regex, string>()
            {
                {
                   new Regex("^String$"),"R2"
                },
                {
                    new Regex("^begin$"),""
                },
                {
                    new Regex("^[!~@#$%^&<>?()/*+-=;]$"),""
                },
                {
                   new Regex("^int|float|Var|boolean|if$"),"R2"
                },
                {
                    new Regex("^print|end|Const|type$"),""
                },
                {
                    new Regex(@"^{"),""
                },
                {
                    new Regex(@"^}"),"R2"
                },
                {
                    new Regex(@"^error"),""
                }
                
            });



                dict.Add("14", new Dictionary<Regex, string>()
            {
                {
                    new Regex("^end$"),"S4"///////
                },
                {
                   new Regex("^int$"),""
                },
                {
                    new Regex("^float$"),""
                },
                {
                    new Regex("^boolean$"),""
                },
                {
                    new Regex("^begin$"),""
                },
                {
                    new Regex("^[!~@#$%^&<>?()/*+-=;]$"),""
                },
                {
                    new Regex("^print|Const|Code$"),""
                },
                {
                   new Regex("^String$"),""
                },
                {
                    new Regex("^type$"),""
                },
                {
                    new Regex("^if$"),""
                },
                {
                    new Regex("^Var$"),""
                },
                {
                    new Regex("^DecS$"),""
                },
                {
                    new Regex(@"^{"),""
                },
                {
                    new Regex(@"^}"),""
                },
                {
                    new Regex(@"^error"),""
                }
                
                
            });
                dict.Add("15", new Dictionary<Regex, string>()
            {
                {
                   new Regex("^Var$"),"S16"
                },
                {
                    new Regex(@"^'$"),""
                },
                {
                    new Regex("^begin$"),""
                },
                {
                    new Regex("^[!~@#$%^&<>?()/*+-=;]$"),""
                },
                {
                   new Regex("^int|float|boolean|type|if$"),""
                },
                {
                    new Regex("^print|end|Const$"),""
                },
                {
                   new Regex("^String$"),""
                },
                {
                    new Regex(@"^{"),""
                },
                {
                    new Regex(@"^}"),""
                },
                {
                    new Regex(@"^error"),""
                }
                
            });
                dict.Add("16", new Dictionary<Regex, string>()
            {
                {
                   new Regex("^=$"),"S17"
                },
                {
                    new Regex("^begin$"),""
                },
                {
                    new Regex("^[!~@#$%^&<>?()/*+-;]$"),""
                },
                {
                   new Regex("^int|float|boolean|Var|type|if$"),""
                },
                {
                    new Regex("^print|end|Const$"),""
                },
                {
                   new Regex("^String$"),""
                },
                {
                    new Regex(@"^{"),""
                },
                {
                    new Regex(@"^}"),""
                }
                ,
                {
                    new Regex(@"^'"),""
                },
                {
                    new Regex(@"^error"),""
                }
                
            });
                dict.Add("17", new Dictionary<Regex, string>()
            {
                {
                   new Regex(@"^'$"),"S18"
                },
                {
                    new Regex("^begin$"),""
                },
                {
                    new Regex("^[!~@#$%^=&<>?()/*+-;]$"),""
                },
                {
                   new Regex("^int|float|boolean|Var|type|if$"),""
                },
                {
                    new Regex("^print|end|Const$"),""
                },
                {
                   new Regex("^String$"),""
                },
                {
                    new Regex(@"^{"),""
                },
                {
                    new Regex(@"^}"),""
                },
                {
                    new Regex(@"^error"),""
                }
                
            });
                dict.Add("18", new Dictionary<Regex, string>()
            {
                {
                   new Regex("^Const$"),"S19"
                },
                {
                    new Regex("^begin$"),""
                },
                {
                    new Regex("^[!~@#$%^=&<>?()/*+-;]$"),""
                },
                {
                   new Regex("^int|float|boolean|Var|type|if$"),""
                },
                {
                    new Regex("^print|end$"),""
                },
                {
                   new Regex("^String$"),""
                },
                {
                    new Regex(@"^{"),""
                },
                {
                    new Regex(@"^}"),""
                },
                {
                    new Regex(@"^'"),""
                },
                {
                    new Regex(@"^error"),""
                }
                
            });
                dict.Add("19", new Dictionary<Regex, string>()
            {
                {
                   new Regex(@"^'$"),"S20"
                },
                {
                    new Regex("^begin$"),""
                },
                {
                    new Regex("^[!~@#$%^=&<>?()/*+-;]$"),""
                },
                {
                   new Regex("^int|float|boolean|Var|type|if$"),""
                },
                {
                    new Regex("^print|end|Const$"),""
                },
                {
                   new Regex("^String$"),""
                },
                {
                    new Regex(@"^{"),""
                },
                {
                    new Regex(@"^}"),""
                },
                {
                    new Regex(@"^error"),""
                }
                
            });
                dict.Add("20", new Dictionary<Regex, string>()
            {
                {
                   new Regex(@"^;$"),"S21"
                },
                {
                    new Regex("^begin$"),""
                },
                {
                    new Regex("^[!~@#$%^=&<>?()/*+-]$"),""
                },
                {
                   new Regex("^int|float|boolean|Var|type|if$"),""
                },
                {
                    new Regex("^print|end|Const$"),""
                },
                {
                   new Regex("^String$"),""
                },
                {
                    new Regex(@"^{"),""
                },
                {
                    new Regex(@"^}"),""
                },
                {
                    new Regex(@"^'"),""
                },
                {
                    new Regex(@"^error"),""
                }
                
            });
                dict.Add("21", new Dictionary<Regex, string>()
            {
                {
                   new Regex("^String$"),"R10"
                },
                {
                    new Regex("^begin$"),""
                },
                {
                    new Regex("^[!~@#$%^&<>?()/*+-=;]$"),""
                },
                {
                   new Regex("^int|float|Var|boolean|if$"),"R10"
                },
                {
                    new Regex("^print|end|Const|type$"),""
                },
                {
                    new Regex(@"^{"),""
                },
                {
                    new Regex(@"^}"),"R10"
                },
                {
                    new Regex(@"^error"),""
                }
                
            });
                dict.Add("22", new Dictionary<Regex, string>()
            {
                {
                   new Regex("^String$"),"R3"
                },
                {
                    new Regex("^begin$"),""
                },
                {
                    new Regex("^[!~@#$%^&<>?()/*+-=;]$"),""
                },
                {
                   new Regex("^int|float|Var|boolean|if$"),"R3"
                },
                {
                    new Regex("^print|end|Const|type$"),""
                },
                {
                    new Regex(@"^{"),""
                },
                {
                    new Regex(@"^}"),"R3"
                },
                {
                    new Regex(@"^error"),""
                }
                
            });
                dict.Add("23", new Dictionary<Regex, string>()
            {
                {
                   new Regex("^=$"),"S24"
                },
                {
                    new Regex("^begin$"),""
                },
                {
                    new Regex("^[!~@#$%^&<>?()/*+-;]$"),""
                },
                {
                   new Regex("^int|float|boolean|type|if$"),""
                },
                {
                    new Regex("^print|end|Const|Var$"),""
                },
                {
                   new Regex("^String$"),""
                },
                {
                    new Regex(@"^{"),""
                },
                {
                    new Regex(@"^}"),""
                },
                {
                    new Regex(@"^error"),""
                }
                
            });
                dict.Add("24", new Dictionary<Regex, string>()
            {
                {
                   new Regex("^Var$"),"S25"
                },
                {
                    new Regex("^begin$"),""
                },
                {
                    new Regex("^[!~@#$%^&<>?()/*+-=;]$"),""
                },
                {
                   new Regex("^int|float|boolean|type|if$"),""
                },
                {
                    new Regex("^print|end|Const$"),""
                },
                {
                   new Regex("^String$"),""
                },
                {
                    new Regex(@"^{"),""
                },
                {
                    new Regex(@"^}"),""
                },
                {
                    new Regex(@"^error"),""
                }
                
            });
                dict.Add("25", new Dictionary<Regex, string>()
            {
                {
                   new Regex(@"^[+|*|/]$"),"S26"
                },
                {
                    new Regex("^-$"),"S26"
                },
                {
                    new Regex("^op$"),"S27"
                },
                {
                    new Regex("^begin$"),""
                },
                {
                    new Regex("^[!~@#$%^&<>?()=;]$"),""
                },
                {
                   new Regex("^int|float|boolean|type|if$"),""
                },
                {
                    new Regex("^print|end|Const|Var$"),""
                },
                {
                   new Regex("^String$"),""
                },
                {
                    new Regex(@"^{"),""
                },
                {
                    new Regex(@"^}"),""
                },
                {
                    new Regex(@"^error"),""
                }
                
            });
                dict.Add("26", new Dictionary<Regex, string>()
            {
                {
                   new Regex("^Var$"),"R12"
                },
                {
                    new Regex("^begin$"),""
                },
                {
                    new Regex("^[!~@#$%^&<>?()/*+-=;]$"),""
                },
                {
                   new Regex("^int|float|boolean|type|if$"),""
                },
                {
                    new Regex("^print|end|Const$"),""
                },
                {
                   new Regex("^String$"),""
                },
                {
                    new Regex(@"^{"),""
                },
                {
                    new Regex(@"^}"),""
                },
                {
                    new Regex(@"^error"),""
                }
                
            });
                dict.Add("27", new Dictionary<Regex, string>()
            {
                {
                   new Regex("^Var$"),"S28"
                },
                {
                    new Regex("^begin$"),""
                },
                {
                    new Regex("^[!~@#$%^&<>?()/*+-=;]$"),""
                },
                {
                   new Regex("^int|float|boolean|type|if$"),""
                },
                {
                    new Regex("^print|end|Const$"),""
                },
                {
                   new Regex("^String$"),""
                },
                {
                    new Regex(@"^{"),""
                },
                {
                    new Regex(@"^}"),""
                },
                {
                    new Regex(@"^error"),""
                }
                
            });
                dict.Add("28", new Dictionary<Regex, string>()
            {
                {
                   new Regex("^;$"),"S29"
                },
                {
                    new Regex("^begin$"),""
                },
                {
                    new Regex("^[!~@#$%^&<>?()/*+-=]$"),""
                },
                {
                   new Regex("^int|float|boolean|type|if$"),""
                },
                {
                    new Regex("^print|end|Const|Var$"),""
                },
                {
                   new Regex("^String$"),""
                },
                {
                    new Regex(@"^{"),""
                },
                {
                    new Regex(@"^}"),""
                },
                {
                    new Regex(@"^error"),""
                }
                
            });
                dict.Add("29", new Dictionary<Regex, string>()
            {
                {
                   new Regex("^String$"),"R11"
                },
                {
                    new Regex("^begin$"),""
                },
                {
                    new Regex("^[!~@#$%^&<>?()/*+-=;]$"),""
                },
                {
                   new Regex("^int|float|Var|boolean|if$"),"R11"
                },
                {
                    new Regex("^print|end|Const|type$"),""
                },
                {
                    new Regex(@"^{"),""
                },
                {
                    new Regex(@"^}"),"R11"
                },
                {
                    new Regex(@"^error"),""
                }
                
            });
                dict.Add("30", new Dictionary<Regex, string>()
            {
                {
                   new Regex("^String$"),"R4"
                },
                {
                    new Regex("^begin$"),""
                },
                {
                    new Regex("^[!~@#$%^&<>?()/*+-=;]$"),""
                },
                {
                   new Regex("^int|float|Var|boolean|if$"),"R4"
                },
                {
                    new Regex("^print|end|Const|type$"),""
                },
                {
                    new Regex(@"^{"),""
                },
                {
                    new Regex(@"^}"),"R4"
                },
                {
                    new Regex(@"^error"),""
                }
                
            });
                dict.Add("31", new Dictionary<Regex, string>()
            {
                {
                   new Regex(@"^[(]$"),"S32"
                },
                {
                    new Regex("^begin$"),""
                },
                {
                    new Regex("^[!~@#$%^&<>?)/*+-=;]$"),""
                },
                {
                   new Regex("^int|float|boolean|Var|type|if$"),""
                },
                {
                    new Regex("^print|end|Const$"),""
                },
                {
                   new Regex("^String$"),""
                },
                {
                    new Regex(@"^{"),""
                },
                {
                    new Regex(@"^}"),""
                },
                {
                    new Regex(@"^error"),""
                }
                
            });
                dict.Add("32", new Dictionary<Regex, string>()
            {
                {
                   new Regex("^Var$"),"S33"
                },
                {
                    new Regex("^begin$"),""
                },
                {
                    new Regex("^[!~@#$%^&<>?()/*+-=;]$"),""
                },
                {
                   new Regex("^int|float|boolean|type|if$"),""
                },
                {
                    new Regex("^print|end|Const$"),""
                },
                {
                   new Regex("^String$"),""
                },
                {
                    new Regex(@"^{"),""
                },
                {
                    new Regex(@"^}"),""
                },
                {
                    new Regex(@"^error"),""
                }
                
            });
                dict.Add("33", new Dictionary<Regex, string>()
            {
                {
                   new Regex(@"^[<|>]$"),"S34"
                },
                {
                   new Regex(@"^==$"),"S34"
                },
                {
                    new Regex("^Lop$"),"S35"
                },
                {
                    new Regex("^Var$"),""
                },
                {
                    new Regex("^begin$"),""
                },
                {
                    new Regex("^[!~@#$%^&<>?()/*+-=;]$"),""
                },
                {
                   new Regex("^int|float|boolean|type|if$"),""
                },
                {
                    new Regex("^print|end|Const$"),""
                },
                {
                   new Regex("^String$"),""
                },
                {
                    new Regex(@"^{"),""
                },
                {
                    new Regex(@"^}"),""
                },
                {
                    new Regex(@"^error"),""
                }
                
            });
                dict.Add("34", new Dictionary<Regex, string>()
            {
                {
                   new Regex("^Var$"),"R17"
                },
                {
                    new Regex("^begin$"),""
                },
                {
                    new Regex("^[!~@#$%^&<>?()/*+-=;]$"),""
                },
                {
                   new Regex("^int|float|boolean|type|if$"),""
                },
                {
                    new Regex("^print|end|Const$"),""
                },
                {
                   new Regex("^String$"),""
                },
                {
                    new Regex(@"^{"),""
                },
                {
                    new Regex(@"^}"),""
                },
                {
                    new Regex(@"^error"),""
                }
                
            });
                dict.Add("35", new Dictionary<Regex, string>()
            {
                {
                   new Regex("^Var$"),"S36"
                },
                {
                    new Regex("^begin$"),""
                },
                {
                    new Regex("^[!~@#$%^&<>?()/*+-=;]$"),""
                },
                {
                   new Regex("^int|float|boolean|type|if$"),""
                },
                {
                    new Regex("^print|end|Const$"),""
                },
                {
                   new Regex("^String$"),""
                },
                {
                    new Regex(@"^{"),""
                },
                {
                    new Regex(@"^}"),""
                },
                {
                    new Regex(@"^error"),""
                }
                
            });
                dict.Add("36", new Dictionary<Regex, string>()
            {
                {
                   new Regex(@"^[)]$"),"S37"
                },
                {
                    new Regex("^begin$"),""
                },
                {
                    new Regex("^[!~@#$%^&<>?(/*+-=;]$"),""
                },
                {
                   new Regex("^int|float|Var|boolean|type|if$"),""
                },
                {
                    new Regex("^print|end|Const$"),""
                },
                {
                   new Regex("^String$"),""
                },
                {
                    new Regex(@"^{"),""
                },
                {
                    new Regex(@"^}"),""
                },
                {
                    new Regex(@"^error"),""
                }
                
            });
                dict.Add("37", new Dictionary<Regex, string>()
            {
                {
                   new Regex(@"^{$"),"S38"
                },
                {
                    new Regex(@"^}"),""
                },
                {
                    new Regex("^begin$"),""
                },
                {
                    new Regex("^[!~@#$%^&<>?()/*+-=;]$"),""
                },
                {
                   new Regex("^int|float|Var|boolean|type|if$"),""
                },
                {
                    new Regex("^print|end|Const$"),""
                },
                {
                   new Regex("^String$"),""
                },
                {
                    new Regex(@"^error"),""
                }
                
            });
                dict.Add("38", new Dictionary<Regex, string>()
            {
                {
                   new Regex("^PriS$"),"S42"
                },
                {
                   new Regex("^print$"),"S39"
                },
                {
                    new Regex("^begin$"),""
                },
                {
                    new Regex("^[!~@#$%^&<>?()/*+-=;]$"),""
                },
                {
                   new Regex("^int|float|Var|boolean|type|if$"),""
                },
                {
                    new Regex("^end|Const$"),""
                },
                {
                   new Regex("^String$"),""
                },
                {
                    new Regex(@"^}"),""
                },
                 {
                    new Regex(@"^{"),""
                },
                {
                    new Regex(@"^error"),""
                }
                
            });
                dict.Add("39", new Dictionary<Regex, string>()
            {
                {
                   new Regex("^Var$"),"S40"
                },
                {
                    new Regex("^begin$"),""
                },
                {
                    new Regex("^[!~@#$%^&<>?()/*+-=;]$"),""
                },
                {
                   new Regex("^int|float|boolean|type|if$"),""
                },
                {
                    new Regex("^print|end|Const$"),""
                },
                {
                   new Regex("^String$"),""
                },
                {
                    new Regex(@"^}"),""
                },
                 {
                    new Regex(@"^{"),""
                },
                {
                    new Regex(@"^error"),""
                }
                
            });
                dict.Add("40", new Dictionary<Regex, string>()
            {
                {
                   new Regex("^;$"),"S41"
                },
                {
                    new Regex("^begin$"),""
                },
                {
                    new Regex("^[!~@#$%^&<>?()/*+-=]$"),""
                },
                {
                   new Regex("^int|float|Var|boolean|type|if$"),""
                },
                {
                    new Regex("^print|end|Const$"),""
                },
                {
                   new Regex("^String$"),""
                },
                {
                    new Regex(@"^}"),""
                },
                 {
                    new Regex(@"^{"),""
                },
                {
                    new Regex(@"^error"),""
                }
                
            });
                dict.Add("41", new Dictionary<Regex, string>()
            {
                {
                    new Regex(@"^}"),"R20"
                },
                {
                    new Regex("^begin$"),""
                },
                {
                    new Regex("^[!~@#$%^&<>?()/*+-=;]$"),""
                },
                {
                   new Regex("^int|float|Var|boolean|type|if$"),""
                },
                {
                    new Regex("^print|end|Const$"),""
                },
                {
                   new Regex("^String$"),""
                },
                {
                    new Regex(@"^{"),""
                },
                {
                    new Regex(@"^error"),""
                }
                
            });
                dict.Add("42", new Dictionary<Regex, string>()
            {
                {
                    new Regex("^else$"),""
                },
                {
                    new Regex(@"^}"),"S43"
                },
                {
                    new Regex("^begin$"),""
                },
                {
                    new Regex("^[!~@#$%^&<>?()/*+-=;]$"),""
                },
                {
                   new Regex("^int|float|Var|boolean|type|if$"),""
                },
                {
                    new Regex("^print|end|Const$"),""
                },
                {
                   new Regex("^String$"),""
                },
                {
                    new Regex(@"^{"),""
                },
                {
                    new Regex(@"^error"),""
                }
                
            });
                dict.Add("43", new Dictionary<Regex, string>()
            {
                {
                    new Regex("^else$"),"S37"
                },
                {
                    new Regex(@"^}"),"R16"
                },
                {
                    new Regex("^begin$"),""
                },
                {
                    new Regex("^[!~@#$%^&<>?()/*+-=;]$"),""
                },
                {
                   new Regex("^int|float|Var|boolean|type|if$"),"R16"
                },
                {
                    new Regex("^print|end|Const$"),""
                },
                {
                   new Regex("^String$"),"R16"
                },
                {
                    new Regex(@"^{"),""
                },
                {
                    new Regex(@"^error"),""
                }
                
            });
                dict.Add("44", new Dictionary<Regex, string>()
            {
                {
                   new Regex("^String$"),"R5"
                },
                {
                    new Regex("^begin$"),""
                },
                {
                    new Regex("^[!~@#$%^&<>?()/*+-=;]$"),""
                },
                {
                   new Regex("^int|float|Var|boolean|if$"),"R5"
                },
                {
                    new Regex("^print|end|Const|type$"),""
                },
                {
                    new Regex(@"^{"),""
                },
                {
                    new Regex(@"^}"),"R5"
                },
                {
                    new Regex(@"^error"),""
                }
                
            });
                dict.Add("45", new Dictionary<Regex, string>()
            {
                {
                   new Regex("^[$]$"),"R0"
                },
                {
                    new Regex("^begin$"),""
                },
                {
                    new Regex("^[!~@#$%^&<>?()/*+-=;]$"),""
                },
                {
                   new Regex("^int|float|boolean|type|if$"),""
                },
                {
                    new Regex("^print|end|Const$"),""
                },
                {
                   new Regex("^String$"),""
                },
                {
                    new Regex(@"^{"),""
                },
                {
                    new Regex(@"^}"),""
                },
                {
                    new Regex(@"^error"),""
                }
                
            });
                dict.Add("46", new Dictionary<Regex, string>()
            {
                {
                    new Regex("^[$]$"),"Accept"
                },
                {
                    new Regex("^begin$"),""
                },
                {
                    new Regex("^Program$"),""
                },
                {
                    new Regex("^Programm$"),""
                },
                {
                    new Regex(@"^[!~@#%^&<>?()/*+-=;]"),""
                },
                {
                   new Regex("^int|float|boolean|String|type|if$"),""
                },
                {
                    new Regex("^print|end|Var|Const$"),""
                },
                {
                    new Regex(@"^{"),""
                },
                {
                    new Regex(@"^}"),""
                },
                {
                    new Regex(@"^error"),""
                }
            });

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }


            String input = o;
            //line by line splitting of input
            String[] m = Regex.Split(input, "\n");
            String[] Col = new String[0];
            String[] temp;
            //storing each line at a single index of Col
            for (int l = 0; l < m.Length; l++)
            {
                temp = Regex.Split(m[l], " ");

                Col = Col.Concat(temp).ToArray();
            }

            //check == line 1
            var check = Col[pointer];

            while (true)
            {
                if (!Col.Contains("$"))
                {
                    Output.AppendText("Unable to Parse Unknown Input");
                    break;
                }

                int s = Stack.Count;
                string[] value = Stack.ToArray();
                if (check.Equals("int") || check.Equals("float") || check.Equals("boolean") || check.Equals("String") || check.Equals("if") || check.Equals("else") || check.Equals("begin") || check.Equals("end") || check.Equals("{") || check.Equals("}") || check.Equals("(") || check.Equals(")") || check.Equals("=") || check.Equals("print") || check.Equals("+") || check.Equals("-") || check.Equals("*") || check.Equals("/") || check.Equals(";") || check.Equals("'") || check.Equals(">") || check.Equals("<") || check.Equals("==") || check.Equals("Var") || check.Equals("Const") || check.Equals("$"))
                {
                    check = check;
                }
                //if its not a keyword it must be a variable or constant
                else
                {
                    if (s >= 2)
                    {
                        //if 1 step back theres a variable type then it must be a var name
                        if (value[1].Equals("int") || value[1].Equals("float") || value[1].Equals("boolean") || value[1].Equals("String"))
                        {
                            String w = CheckVar(check);
                            if (w.Equals("ok"))
                            {
                                check = "Var";
                            }
                            else
                            {
                                check = "error";
                            }

                        }
                        else if (value[3].Equals("if") || value[7].Equals("if") || value[1].Equals("print"))
                        {
                            String w = CheckVar(check);
                            if (w.Equals("ok"))
                            {
                                check = "Var";
                            }
                            else
                            {
                                check = "error";
                            }
                        }
                        //its for assigning value, this if is for first var
                        else if (value[1].Equals("{") || value[1].Equals(";"))
                        {
                            String w = CheckVar(check);
                            if (w.Equals("ok"))
                            {
                                check = "Var";
                            }
                            else
                            {
                                check = "error";
                            }
                        }
                        //its for assigning value, this if is for second var
                        else if (value[5].Equals("{") || value[5].Equals("Code"))
                        {
                            //MessageBox.Show(value[5]);
                            if (value[3].Equals("Var"))
                            {
                                String w = CheckVar(check);
                                if (w.Equals("ok"))
                                {
                                    check = "Var";
                                }
                                else
                                {
                                    check = "error";
                                }
                            }

                        }
                        //its for assigning value, this if is for third var
                        else if (value[9].Equals("{") || value[9].Equals("Code"))
                        {
                            if (value[7].Equals("Var"))
                            {
                                if (value[3].Equals("Var"))
                                {
                                    String w = CheckVar(check);
                                    if (w.Equals("ok"))
                                    {
                                        check = "Var";
                                    }
                                    else
                                    {
                                        check = "error";
                                    }
                                }
                            }

                        }
                    }
                    if (s >= 6)
                    {
                        //if 5 steps back there's a type then it must be a value of var
                        if (value[5].Equals("type"))
                        {
                            //type is int float and boolean so we check in CheckVar, if its ok with CheckVar 
                            //it means its a variable name so we must give an error but for boolean value we allow true and false
                            String w = CheckVar(check);
                            if (w.Equals("ok"))
                            {
                                if (check == "true" || check == "false")
                                {
                                    check = "Const";
                                }
                                else
                                {
                                    check = "error";
                                }

                            }
                            else
                            {
                                check = "Const";
                            }
                        }
                    }
                    if (s >= 8)
                    {
                        //if 7 steps back there's a String then it must be a value of var String
                        if (check != "'")
                        {
                            if (value[7].Equals("String") || value[5].Equals("String"))
                            {
                                check = "Const";
                            }
                        }

                    }

                }
                if (check == "'")
                {
                    //if 1 step back there's a String then it must be a name of var String
                    if (value[1].Equals("String"))
                    {
                        String w = CheckVar(check);
                        if (w.Equals("ok"))
                        {
                            check = "Var";
                        }
                        else
                        {
                            check = "error";
                        }
                    }
                }
                if (check.Equals("int") || check.Equals("float") || check.Equals("boolean") || check.Equals("String") || check.Equals("if") || check.Equals("else") || check.Equals("begin") || check.Equals("end") || check.Equals("{") || check.Equals("}") || check.Equals("(") || check.Equals(")") || check.Equals("=") || check.Equals("print") || check.Equals("+") || check.Equals("-") || check.Equals("*") || check.Equals("/") || check.Equals(";") || check.Equals("'") || check.Equals(">") || check.Equals("<") || check.Equals("==") || check.Equals("Var") || check.Equals("true") || check.Equals("false") || check.Equals("Const") || check.Equals("$"))
                {
                    check = check;
                }
                else
                {
                    check = "error";
                }
                //extract all regex and match input header i.e check with key
                //extract value for matched key
                foreach (var x in dict[Stack.Peek()])
                {
                    if (x.Key.Match(check).Success)
                    {
                        Parser = x.Value;
                        break;
                    }
                }
                if (check.Equals("end"))
                {
                    String[] stemp = new String[2];
                    for (int i = 0; i < 2; i++)
                    {
                        stemp[i] = Stack.Pop();
                    }
                    int q = Stack.Count;
                    string[] val = Stack.ToArray();
                   
                    for (int j = 1; j < (q - 9); j = j + 2)
                    {
                        if (val[0].Equals("4") && val[1].Equals("Code") && val[2].Equals("4") && val[3].Equals("Code"))
                        {
                            Stack.Pop();
                            Stack.Pop();
                        }
                        else
                        {
                            continue;
                        }
                        val = Stack.ToArray();
                    }
                    Stack.Push(stemp[1]);
                    Stack.Push(stemp[0]);

                }
                //it just push check and then value on stack and increments pointer for check
                if (Parser.Contains("S"))
                {
                    Stack.Push(check + "");
                    Parser = Parser.TrimStart('S');
                    Stack.Push(Parser);
                    pointer++;
                    check = Col[pointer];
                    Print_Stack(Stack);
                }
                if (Parser.Contains("R"))
                {
                    Parser = Parser.TrimStart('R');
                    //get grammar rule
                    String get = States[Convert.ToInt32(Parser)] + "";
                    String[] Splitted = get.Split('_');
                    String[] Final_ = Splitted[1].Split(' ');
                    int test = Final_.Length;
                    //pop value and check from stack wrt rule
                    for (int i = 0; i < test * 2; i++)
                    {
                        Stack.Pop();
                    }
                    //after pop last value
                    String row = Stack.Peek() + "";
                    //push rule left hand side
                    Stack.Push(Splitted[0]);
                    //find rule for L.H.S of grammar in state at top of stack i.e row
                    //extract key aginst matced key and push on top of stack
                    //input header is not moved so we will now again find key for that input in new state that we just pushed on stack top
                    foreach (var x in dict[row])
                    {
                        if (x.Key.Match(Splitted[0]).Success)
                        {
                            Parser = x.Value;
                            Parser = Parser.TrimStart('S');
                            Stack.Push(Parser);
                            break;
                        }
                    }
                    Print_Stack(Stack);
                }
                if (Parser.Contains("Accept"))
                {
                    Output.AppendText("Parsed");
                    break;
                }
                if (Parser.Equals(""))
                {
                    Output.AppendText("Unable to Parse");
                    break;
                }
            }
            slrTable(dict, States);
        }
        public string CheckVar(String a)
        {
            Dictionary<String, Dictionary<Regex, String>> machineStates = new Dictionary<string, Dictionary<Regex, string>>();
            machineStates.Add("S0", new Dictionary<Regex, string>()
            {
                {
                    new Regex("^[\\d]$"),"Se"
                },
                {
                    new Regex("^[!~@#$%^&*()/*=+]$"),"Se"
                },
                {
                    new Regex("^\\s$"),"se"
                },
                {
                    new Regex("^[a-zA-Z]+$"),"S1"
                },
                {
                    new Regex("^[_]$"),"S1"
                },
                {
                    new Regex(@"^'$"),"Se"
                }
            });
            machineStates.Add("S1", new Dictionary<Regex, string>()
            {
                {
                    new Regex("^[\\d]$"),"S1"
                },
                {
                    new Regex("^[!~@#$%^&*()/*=+]$"),"Se"
                },
                {
                    new Regex("^\\s$"),"se"
                },
                {
                    new Regex("^[a-zA-Z]$"),"S1"
                },
                {
                    new Regex("^[_]$"),"S1"
                },
                {
                    new Regex(@"^'$"),"Se"
                }
            });

            var chars = a.ToCharArray();
            var state = "S0";
            var i = 0;
            var check = chars[i];


            while (check != '$' && state != "Se")
            {

                foreach (var x in machineStates[state])
                {
                    if (x.Key.Match(check + "").Success)
                    {
                        state = x.Value;
                    }
                }
                i++;
                if (i < chars.Length)
                {
                    check = chars[i];
                }
                else
                {
                    check = '$';
                }

            }

            if (state == "Se")
            {
                return "error";
            }
            else
            {
                return "ok";
            }

        }
        private void Print_Stack(Stack<string> st)
        {
            Output.AppendText(st.Peek() + "\n");
        }

        public void slrTable(Dictionary<String, Dictionary<Regex, String>> d, ArrayList s)
        {
            DG.ColumnCount = 37;
            DG.Columns[0].Name = "States";
            DG.Columns[1].Name = "begin";
            DG.Columns[2].Name = "(";
            DG.Columns[3].Name = ")";
            DG.Columns[4].Name = "{";
            DG.Columns[5].Name = "}";
            DG.Columns[6].Name = "end";
            DG.Columns[7].Name = "int";
            DG.Columns[8].Name = "float";
            DG.Columns[9].Name = "boolean";
            DG.Columns[10].Name = "Var";
            DG.Columns[11].Name = "=";
            DG.Columns[12].Name = "Const";
            DG.Columns[13].Name = ";";
            DG.Columns[14].Name = "String";
            DG.Columns[15].Name = "'";
            DG.Columns[16].Name = "+";
            DG.Columns[17].Name = "-";
            DG.Columns[18].Name = "*";
            DG.Columns[19].Name = "/";
            DG.Columns[20].Name = "if";
            DG.Columns[21].Name = "<";
            DG.Columns[22].Name = ">";
            DG.Columns[23].Name = "==";
            DG.Columns[24].Name = "print";
            DG.Columns[25].Name = "else";
            DG.Columns[26].Name = "$";
            DG.Columns[27].Name = "program";
            DG.Columns[28].Name = "Code";
            DG.Columns[29].Name = "DecS";
            DG.Columns[30].Name = "SDecS";
            DG.Columns[31].Name = "type";
            DG.Columns[32].Name = "AssS";
            DG.Columns[33].Name = "op";
            DG.Columns[34].Name = "Lop";
            DG.Columns[35].Name = "IffS";
            DG.Columns[36].Name = "PriS";
            //String Parser = "";
            //int i = 0;
            String[] parser = new String[37];
            parser[0] = "01";

            for (int k = 0; k < 47; k++)
            {

                for (int j = 1; j < 37; j++)
                {
                    foreach (var x in d[k.ToString()])
                    {
                        if (x.Key.Match(DG.Columns[j].Name).Success)
                        {
                            if (x.Value.Contains("R"))
                            {
                                String temp = x.Value.TrimStart('R');
                                String get = s[Convert.ToInt32(temp)] + "";
                                parser[j] = x.Value+" "+"( "+get+" )";
                            }
                            else 
                            {
                                parser[j] = x.Value;
                            }
                            
                            break;
                        }
                        else
                        {
                            parser[j] = "";
                        }
                    }
                }
                parser[0] = k.ToString();
                DG.Rows.Add(parser);
            }
                
        }

        public void SemanticAnalysis(String o, String[,] SymbolTable)
        {
            SAOutput.Clear();
            Boolean find = true;
            String input = o;
            //line by line splitting of input
            String[] m = Regex.Split(input, "\n");
            String[] temp;
            for (int l = 0; l < m.Length; l++)
            {
                find = true;
                //split line on basis of space and stores in temp
                temp = Regex.Split(m[l], " ");

                //if first keyword is int go ahead
                if (temp[0].Equals("int") && temp.Length == 5)
                {
                    //iterate whole symbol table
                    for(int i=1; i <= SymbolTable.GetLength(0); i++)
                    {
                        //symbol table has 20 rows declared this cond checks not to exceed than used indexes
                        if (SymbolTable[i, 2] != null)
                        {
                            //check if symbol table has variable name
                            if (SymbolTable[i, 2].Equals(temp[1]))
                            {
                                try
                                {
                                    //try to convert string into integer if succed the type matched
                                    int p = Convert.ToInt32(temp[3]);
                                    SAOutput.AppendText("Line " + l + " is clear\n");
                                    find = false;
                                    break;
                                }
                                catch (Exception e)
                                {
                                    SAOutput.AppendText("Line " + l + " does not have integer value\n");
                                    find = false;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            break;
                        }

                        
                        
                    }
                    //if variable finds in symbole table then find = false so this cond only works when variable did n't finds in symbol table
                    if(find == true)
                        {
                            try
                            {
                                int p = Convert.ToInt32(temp[3]);
                                SymbolTable[(SymbolTable.GetLength(0)+1), 1] = (SymbolTable.GetLength(0)+1).ToString(); //index

                                SymbolTable[(SymbolTable.GetLength(0)+1), 2] = temp[1]; //variable name

                                SymbolTable[(SymbolTable.GetLength(0)+1), 3] = temp[0]; //type

                                SymbolTable[(SymbolTable.GetLength(0)+1), 4] = temp[3]; //value

                                SymbolTable[(SymbolTable.GetLength(0)+1), 5] = (l + 1).ToString(); // line number
                                SAOutput.AppendText("Line " + l + " is clear\n");
                                
                            }
                            catch (Exception e)
                            {
                                SAOutput.AppendText("Line " + l + " does not have integer value\n");
                            }
                        }
                }

                //if first keyword is float go ahead
                else if(temp[0].Equals("float") && temp.Length == 5)
                {
                    //iterate whole symbol table
                    for (int i = 1; i <= SymbolTable.GetLength(0); i++)
                    {
                        //symbol table has 20 rows declared this cond checks not to exceed than used indexes
                        if (SymbolTable[i, 2] != null)
                        {
                            //check if symbol table has variable name
                            if (SymbolTable[i, 2].Equals(temp[1]))
                            {
                                try
                                {
                                    //try to convert value in float. if succeded then type matched
                                    double p = Convert.ToDouble(temp[3]);
                                    SAOutput.AppendText("Line " + l + " is clear\n");
                                    find = false;
                                    break;
                                }
                                catch (Exception e)
                                {
                                    SAOutput.AppendText("Line " + l + " does not have float value\n");
                                    find = false;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
                        
                        
                    }
                    //if variable finds in symbole table then find = false so this cond only works when variable did n't finds in symbol table
                    if(find==true)
                        {
                            try
                            {
                                double p = Convert.ToDouble(temp[3]);
                                SymbolTable[(SymbolTable.GetLength(0) + 1), 1] = (SymbolTable.GetLength(0) + 1).ToString(); //index

                                SymbolTable[(SymbolTable.GetLength(0) + 1), 2] = temp[1]; //variable name

                                SymbolTable[(SymbolTable.GetLength(0) + 1), 3] = temp[0]; //type

                                SymbolTable[(SymbolTable.GetLength(0) + 1), 4] = temp[3]; //value

                                SymbolTable[(SymbolTable.GetLength(0) + 1), 5] = (l + 1).ToString(); // line number
                                SAOutput.AppendText("Line " + l + " is clear\n");

                            }
                            catch (Exception e)
                            {
                                SAOutput.AppendText("Line " + l + " does not have float value\n");
                            }
                        }
                }

                 //if first keyword is boolean go ahead
                else if(temp[0].Equals("boolean") && temp.Length == 5)
                {
                    //iterate whole symbol table
                    for (int i = 1; i <= SymbolTable.GetLength(0); i++)
                    {
                        //symbol table has 20 rows declared this cond checks not to exceed than used indexes
                        if (SymbolTable[i, 2] != null)
                        {
                            //match variable name in symbol table
                            if (SymbolTable[i, 2].Equals(temp[1]))
                            {
                                //check value of variable, if true / false then value matches the type
                                if (temp[3].Equals("true") || temp[3].Equals("false"))
                                {
                                    SAOutput.AppendText("Line " + l + " is clear\n");
                                    find = false;
                                    break;
                                }
                                else
                                {
                                    SAOutput.AppendText("Line " + l + " does not have boolean value\n");
                                    find = false;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
                        
                        
                    }
                    //if variable finds in symbole table then find = false so this cond only works when variable did n't finds in symbol table
                    if(find==true)
                        {
                           if(temp[3].Equals("true") || temp[3].Equals("false"))
                            {
                                SymbolTable[(SymbolTable.GetLength(0) + 1), 1] = (SymbolTable.GetLength(0) + 1).ToString(); //index

                                SymbolTable[(SymbolTable.GetLength(0) + 1), 2] = temp[1]; //variable name

                                SymbolTable[(SymbolTable.GetLength(0) + 1), 3] = temp[0]; //type

                                SymbolTable[(SymbolTable.GetLength(0) + 1), 4] = temp[3]; //value

                                SymbolTable[(SymbolTable.GetLength(0) + 1), 5] = (l + 1).ToString(); // line number
                                SAOutput.AppendText("Line " + l + " is clear\n");
                            }
                            else
                            {
                                SAOutput.AppendText("Line " + l + " does not have boolean value\n");
                            }
                        }
                }

                //if first keyword is String go ahead
                else if(temp[0].Equals("String") && temp.Length == 7)
                {
                    //iterate whole symbol table
                    for (int i = 1; i <= SymbolTable.GetLength(0); i++)
                    {
                        //symbol table has 20 rows declared this cond checks not to exceed than used indexes
                        if (SymbolTable[i, 2] != null)
                        {
                            //match variable name in symbol table
                            if (SymbolTable[i, 2].Equals(temp[1]))
                            {
                                //check if ' is present on given indexes then value is in ' ', which means value has matched type
                                if (temp[3].Equals("'") && temp[5].Equals("'"))
                                {
                                    SAOutput.AppendText("Line " + l + " is clear\n");
                                    find = false;
                                    break;
                                }
                                else
                                {
                                    SAOutput.AppendText("Line " + l + " does not have String value\n");
                                    find = false;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
                        
                        
                    }
                    //if variable finds in symbole table then find = false so this cond only works when variable did n't finds in symbol table
                    if(find==true)
                        {
                            if (temp[3].Equals("'") && temp[5].Equals("'"))
                            {
                                SymbolTable[(SymbolTable.GetLength(0) + 1), 1] = (SymbolTable.GetLength(0) + 1).ToString(); //index

                                SymbolTable[(SymbolTable.GetLength(0) + 1), 2] = temp[1]; //variable name

                                SymbolTable[(SymbolTable.GetLength(0) + 1), 3] = temp[0]; //type

                                SymbolTable[(SymbolTable.GetLength(0) + 1), 4] = temp[4]; //value

                                SymbolTable[(SymbolTable.GetLength(0) + 1), 5] = (l + 1).ToString(); // line number
                                SAOutput.AppendText("Line " + l + " is clear\n");
                            }
                            else
                            {
                                SAOutput.AppendText("Line " + l + " does not have String value\n");
                            }
                        }
                }

                //if first keyword is if go ahead
                else if(temp[0].Equals("if"))
                {
                    //iterate whole symbol table
                    for (int i = 1; i <= SymbolTable.GetLength(0); i++)
                    {
                        //symbol table has 20 rows declared this cond checks not to exceed than used indexes
                        if (SymbolTable[i, 2] != null)
                        {
                            //matches name of first variable in symbol table
                            if (SymbolTable[i, 2].Equals(temp[2]))
                            {
                                //iterate whole symbol table
                                for (int j = 1; j <= SymbolTable.GetLength(0); j++)
                                {
                                    //symbol table has 20 rows declared this cond checks not to exceed than used indexes
                                    if (SymbolTable[j, 2] != null)
                                    {
                                        //matches 2nd variable name in symbol table
                                        if (SymbolTable[j, 2].Equals(temp[4]))
                                        {
                                            //check if both variables have same typeor not
                                            if (SymbolTable[i, 3].Equals(SymbolTable[j, 3]))
                                            {
                                                SAOutput.AppendText("Line " + l + " is clear\n");
                                                find = false;
                                                break;
                                            }
                                            else
                                            {
                                                SAOutput.AppendText("Line " + l + " Type Mismatch\n");
                                                find = false;
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }
                                   
                                }
                                //if variable finds in symbole table then find = false so this cond only works when variable did n't finds in symbol table
                                if (find == true)
                                {
                                    find = false;
                                    SAOutput.AppendText("Line " + l + " has " + temp[4] + " not decalred in a program\n");
                                }
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                        
                    }
                    //if variable finds in symbole table then find = false so this cond only works when variable did n't finds in symbol table
                    if (find == true)
                    {
                        SAOutput.AppendText("Line " + l + " has " + temp[2] + " not decalred in a program\n");
                        
                    }
                }

                //if second keyword is = go ahead
                else if(temp.Length == 6 && temp[1].Equals("="))
                {
                    //iterate whole symbol table
                    for (int i = 1; i <= SymbolTable.GetLength(0); i++)
                    {
                        //symbol table has 20 rows declared this cond checks not to exceed than used indexes
                        if (SymbolTable[i, 2] != null)
                        {
                            //match variable name in symbol table
                            if (SymbolTable[i, 2].Equals(temp[0]))
                            {
                                //iterate whole symbol table
                                for (int j = 1; j <= SymbolTable.GetLength(0); j++)
                                {
                                    //symbol table has 20 rows declared this cond checks not to exceed than used indexes
                                    if (SymbolTable[j, 2] != null)
                                    {
                                        //match variable name in symbol table
                                        if (SymbolTable[j, 2].Equals(temp[2]))
                                        {
                                            //iterate whole symbol table
                                            for (int k = 1; k <= SymbolTable.GetLength(0); k++)
                                            {
                                                //symbol table has 20 rows declared this cond checks not to exceed than used indexes
                                                if (SymbolTable[k, 2] != null)
                                                {
                                                    //match variable name in symbol table
                                                    if (SymbolTable[k, 2].Equals(temp[4]))
                                                    {
                                                        //check type of LHS variable and 1st variable of right hand side
                                                        if (SymbolTable[i, 3].Equals(SymbolTable[j, 3]))
                                                        {
                                                            //check type of LHS variable and 12nd variable of right hand side
                                                            if (SymbolTable[i, 3].Equals(SymbolTable[k, 3]))
                                                            {
                                                                SAOutput.AppendText("Line " + l + " is clear\n");
                                                                find = false;
                                                                break;
                                                            }
                                                            else
                                                            {
                                                                SAOutput.AppendText("Line " + l + " Type Mismatch\n");
                                                                find = false;
                                                                break;
                                                            }

                                                        }
                                                        else
                                                        {
                                                            SAOutput.AppendText("Line " + l + " Type Mismatch\n");
                                                            find = false;
                                                            break;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                             
                                            }
                                            //if variable finds in symbole table then find = false so this cond only works when variable did n't finds in symbol table
                                            if (find == true)
                                            {
                                                find = false;
                                                SAOutput.AppendText("Line " + l + " has " + temp[4] + " not decalred in a program\n");
                                                //break;
                                            }
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }
                                    


                                }
                                //if variable finds in symbole table then find = false so this cond only works when variable did n't finds in symbol table
                                if (find == true)
                                {
                                    find = false;
                                    SAOutput.AppendText("Line " + l + " has " + temp[2] + " not decalred in a program\n");
                                }
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                        
                    }
                    //if variable finds in symbole table then find = false so this cond only works when variable did n't finds in symbol table
                    if (find == true)
                    {
                        SAOutput.AppendText("Line " + l + " has " + temp[0] + " not decalred in a program\n");
                    }

                }
            }

        }
    }
}
            