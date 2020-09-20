using System;
using System.Collections.Generic;
using System.Text;

namespace mini_java_compiler
{
    class Parser
    {
        List<ElToken> ListaTokens;
        StringBuilder builder = new StringBuilder();
        ElToken Token;
        int contador = 0;
        bool estaVaciaListaTokens = false;

        public StringBuilder Builder { get => builder; set => builder = value; }

        public void Empezar(List<ElToken> Listatoken)
        {
            ListaTokens = Listatoken;
            Token = ListaTokens[contador];
            Program();

        }

        //AQUI COMIENZA LA GRAMATICA
        private void Program()
        {
            Decl();
        }

        private void Decl()
        {
            if (Token.Tipo == "T_int" || Token.Tipo == "T_double" || Token.Tipo == "T_bool" || Token.Tipo == "T_string" || Token.Tipo == "T_ID")  // vaer los tterminales de viariableDecl
            {
                VariableDecl();
                DeclPrima();
            }
            else if (Token.Tipo == "T_int" || Token.Tipo == "T_double" || Token.Tipo == "T_bool" || Token.Tipo == "T_string" || Token.Tipo == "T_ID" || Token.Tipo == "T_void")
            {
                FunctionDecl();
                DeclPrima();
            }
            else
            {
                return;
            }
        }

        private void DeclPrima()
        {
            if (Token.Tipo == "T_int" || Token.Tipo == "T_double" || Token.Tipo == "T_bool" || Token.Tipo == "T_string" || Token.Tipo == "T_ID" || Token.Tipo == "T_void")
            {
                Decl();
                DeclPrima();
            }
            else
            {
                return;
            }
        }

        private void VariableDecl()
        {
            Variable();
            Consumir(new ElToken(";", "T_OPERADOR"));
        }

        private void Variable()
        {
            Type();
            Consumir("T_ID");
        }

        private void Type()
        {
            if (Token.Tipo.Equals("T_int"))
            {
                Consumir("T_int");
                Z();
            }
            else if (Token.Tipo.Equals("T_bool"))
            {
                Consumir("T_bool");
                Z();
            }
            else if (Token.Tipo.Equals("T_string"))
            {
                Consumir("T_string");
                Z();
            }
            else if (Token.Tipo.Equals("T_double"))
            {
                Consumir("T_double");
                Z();
            }
            else if (Token.Tipo.Equals("T_ID"))
            {
                Consumir("T_ID");
                Z();
            }
            else
            {
                ErrorSintaxis("T_int, T_bool, T_string, T_double, T_ID");
            }
        }

        private void Z()
        {
            if (Token.Elemento.Equals("[]") && Token.Tipo.Equals("T_Operador"))
            {
                Consumir("T_OPERADOR");
                Z();
            }
            else
            {
                return;
            }
        }

        private void FunctionDecl()
        {
            if (Token.Tipo.Equals("T_void"))
            {
                Consumir("T_void");
                K();
            }
            else if (Token.Tipo.Equals("T_int") || Token.Tipo.Equals("T_double") || Token.Tipo.Equals("T_bool") || Token.Tipo.Equals("T_string") || Token.Tipo.Equals("T_ID"))
            {
                Type();
                K();
            }
            else
            {
                ErrorSintaxis("T_int, T_bool, T_string, T_double, T_ID o T_void");
            }
        }

        private void K()
        {
            if (Token.Tipo.Equals("T_ID"))
            {
                Consumir("T_ID");
                if (Token.Elemento.Equals("()"))
                {
                    Consumir(new ElToken("()", "T_OPERADOR"));
                }
                else
                {
                    Consumir(new ElToken("(", "T_OPERADOR"));
                    Formals();
                    Consumir(new ElToken(")", "T_OPERADOR"));
                }

                StmtIntermedia();
            }
            else
            {
                ErrorSintaxis("T_ID, (), (, )");
            }

        }

        private void StmtIntermedia()
        {
            if (!estaVaciaListaTokens)
            {
                Stmt();
                StmtIntermedia();
            }
            else
            {
                return;
            }

        }

        private void Formals()
        {
            if ((Token.Tipo.Equals("T_int") || Token.Tipo.Equals("T_double") || Token.Tipo.Equals("T_bool") || Token.Tipo.Equals("T_string") || Token.Tipo.Equals("T_ID")))
            {
                Variable();
                VariableIntermedia();
                Consumir(new ElToken(",", "T_OPERADOR"));
            }
            else
            {
                return;
            }
        }



        private void VariableIntermedia()
        {
            if ((Token.Tipo.Equals("T_int") || Token.Tipo.Equals("T_double") || Token.Tipo.Equals("T_bool") || Token.Tipo.Equals("T_string") || Token.Tipo.Equals("T_ID")))
            {
                Variable();
                VariableIntermedia();
            }
            else
            {
                return;
            }
        }

        private void Stmt()
        {
            if (Token.Tipo.Equals("T_if"))
            {
                IfStmt();
            }
            else if (Token.Tipo.Equals("T_print"))
            {
                PrintStmt();
            }
            else if (Token.Elemento == "-" || (Token.Elemento == "(") || Token.Elemento == "New" || Token.Elemento == "this" || Token.Tipo == "T_ENTERO" || Token.Tipo == "T_DOUBLE" || Token.Tipo == "T_BOOLEAN" || Token.Tipo == "T_CADENA" || Token.Tipo == "T_null" || Token.Tipo == ("T_ID"))
            {
                Expr();
            }
            else
            {
                ErrorSintaxis("T_if, T_print o expresión");
            }
        }

        private void IfStmt()
        {
            if (Token.Tipo.Equals("T_if"))
            {
                Consumir("T_if");
                Consumir(new ElToken("(", "T_OPERADOR"));
                Expr();
                Consumir("T_OPERADOR");
                Stmt();
                ElseViene();
            }
            else
            {
                ErrorSintaxis("T_if");
            }

        }
        private void ElseViene()
        {
            if (Token.Tipo.Equals("T_else"))
            {
                Consumir("T_else");
                Stmt();
            }
            else
            {
                return;
            }
        }

        private void PrintStmt()
        {
            if (Token.Tipo.Equals("T_print"))
            {
                Consumir(new ElToken("Print", "T_print"));
                Consumir(new ElToken("(", "T_OPERADOR"));
                Expr();
                ExprIntermedia();
                Consumir(new ElToken(",", "T_OPERADOR"));
                Consumir("T_OPERADOR");
                Consumir(new ElToken(";", "T_OPERADOR"));
            }
            else
            {
                ErrorSintaxis("T_print");
            }
        }

        private void ExprIntermedia()
        {
            if (Token.Elemento == "-" || (Token.Elemento == "(") || Token.Elemento == "New" || Token.Elemento == "this" || Token.Tipo == "T_ENTERO" || Token.Tipo == "T_DOUBLE" || Token.Tipo == "T_BOOLEAN" || Token.Tipo == "T_CADENA" || Token.Tipo == "T_null")
            {
                Expr();
                ExprIntermedia();
            }
            else
            {
                return;
            }
        }
        private void Expr()
        {
            if (Token.Elemento == "-" || (Token.Elemento == "(") || Token.Elemento == "New" || Token.Elemento == "this" || Token.Tipo == "T_ENTERO" || Token.Tipo == "T_DOUBLE" || Token.Tipo == "T_BOOLEAN" || Token.Tipo == "T_CADENA" || Token.Tipo == "T_null")
            {
                X();
                ExprPrima();
            }
            else
            {
                ErrorSintaxis("Expresiones");
            }

        }
        private void ExprPrima()
        {
            if (Token.Elemento.Equals("&&") && Token.Tipo.Equals("T_Operador"))
            {
                X();
                Expr();
            }
            else if (Token.Elemento.Equals("||") && Token.Tipo.Equals("T_Operador"))
            {
                X();
                Expr();
            }
            else
            {
                return;
            }
        }

        private void X()
        {
            if (Token.Elemento == "-" || (Token.Elemento == "(") || Token.Elemento == "New" || Token.Elemento == "this" || Token.Tipo == "T_ENTERO" || Token.Tipo == "T_DOUBLE" || Token.Tipo == "T_BOOLEAN" || Token.Tipo == "T_CADENA" || Token.Tipo == "T_null")
            {
                A();
                XPrima();
            }
            else
            {
                ErrorSintaxis("Se esperaba una expresion");
            }

        }
        private void XPrima()
        {
            if (true)
                if (Token.Elemento.Equals("=="))
                {

                    Consumir(new ElToken("==", "T_OPERADOR"));
                    A();
                    XPrima();
                }
                else if (Token.Elemento.Equals("!="))
                {
                    Consumir(new ElToken("!=", "T_OPERADOR"));
                    A();
                    XPrima();
                }
                else
                {
                    return;
                }
        }

        private void A()
        {
            if (Token.Elemento == "-" || (Token.Elemento == "(") || Token.Elemento == "New" || Token.Elemento == "this" || Token.Tipo == "T_ENTERO" || Token.Tipo == "T_DOUBLE" || Token.Tipo == "T_BOOLEAN" || Token.Tipo == "T_CADENA" || Token.Tipo == "T_null")
            {
                B();
                APrima();
            }
            else
            {
                ErrorSintaxis("Se esperaba una expresion");
            }

        }
        private void APrima()
        {
            if (Token.Elemento.Equals("<"))
            {
                Consumir(new ElToken("<", "T_OPERADOR"));
                B();
                APrima();
            }
            else if (Token.Elemento.Equals(">"))
            {
                Consumir(new ElToken(">", "T_OPERADOR"));
                B();
                APrima();
            }
            else if (Token.Elemento.Equals(">="))
            {
                Consumir(new ElToken(">=", "T_OPERADOR"));
                B();
                APrima();
            }
            else if (Token.Elemento.Equals("<="))
            {
                Consumir(new ElToken("<=", "T_OPERADOR"));
                B();
                APrima();
            }
            else
            {
                return;
            }
        }

        private void B()
        {
            if (Token.Elemento == "-" || (Token.Elemento == "(") || Token.Elemento == "New" || Token.Elemento == "this" || Token.Tipo == "T_ENTERO" || Token.Tipo == "T_DOUBLE" || Token.Tipo == "T_BOOLEAN" || Token.Tipo == "T_CADENA" || Token.Tipo == "T_null")
            {
                C();
                BPrima();
            }
            else
            {
                ErrorSintaxis("Se esperaba una expresion");
            }

        }

        private void BPrima()
        {
            if (Token.Elemento.Equals("+"))
            {
                Consumir(new ElToken("+", "T_OPERADOR"));
                C();
                BPrima();
            }
            else if (Token.Elemento.Equals("-"))
            {
                Consumir(new ElToken("-", "T_OPERADOR"));
                C();
                BPrima();
            }
            else
            {
                return;
            }
        }
        private void C()
        {
            if (Token.Elemento == "-" || (Token.Elemento == "(") || Token.Elemento == "New" || Token.Elemento == "this" || Token.Tipo == "T_ENTERO" || Token.Tipo == "T_DOUBLE" || Token.Tipo == "T_BOOLEAN" || Token.Tipo == "T_CADENA" || Token.Tipo == "T_null")
            {
                D();
                CPrima();
            }
            else
            {
                ErrorSintaxis("Se esperaba una expresion");
            }

        }

        private void CPrima()
        {
            if (Token.Elemento.Equals("*"))
            {
                Consumir(new ElToken("*", "T_OPERADOR"));
                D();
                CPrima();
            }
            else if (Token.Elemento.Equals("/"))
            {
                Consumir(new ElToken("/", "T_OPERADOR"));
                D();
                CPrima();
            }
            else if (Token.Elemento.Equals("%"))
            {
                Consumir(new ElToken("%", "T_OPERADOR"));
                D();
                CPrima();
            }
            else
            {
                return;
            }
        }
        private void D()
        {
            if (Token.Elemento == "-")
            {
                Consumir(new ElToken("-", "T_OPERADOR"));
                Expr();
            }
            else if (Token.Elemento == "(")
            {
                Consumir(new ElToken("(", "T_OPERADOR"));
                Expr();
            }
            else if (Token.Elemento == "New")
            {
                Consumir("T_New");
                Expr();
            }
            else if (Token.Elemento == "this")
            {
                Consumir("T_this");
            }
            else if (Token.Tipo == "T_ENTERO" || Token.Tipo == "T_DOUBLE" || Token.Tipo == "T_BOOLEAN" || Token.Tipo == "T_CADENA" || Token.Tipo == "T_null")
            {
                Constant();
            }
            else if (Token.Elemento == "-" || (Token.Elemento == "(") || Token.Elemento == "New" || Token.Elemento == "this" || Token.Tipo == "T_ENTERO" || Token.Tipo == "T_DOUBLE" || Token.Tipo == "T_BOOLEAN" || Token.Tipo == "T_CADENA" || Token.Tipo == "T_null")
            {
                ExprViene();
            }
            else if (Token.Elemento == "-" || (Token.Elemento == "(") || Token.Elemento == "New" || Token.Elemento == "this" || Token.Tipo == "T_ENTERO" || Token.Tipo == "T_DOUBLE" || Token.Tipo == "T_BOOLEAN" || Token.Tipo == "T_CADENA" || Token.Tipo == "T_null" || Token.Tipo == "T_ID" || (Token.Tipo == "T_OPERADOR" && Token.Elemento == "[") || Token.Tipo == "T_OPERADOR" && Token.Elemento == "." || (Token.Tipo == "T_OPERADOR" && Token.Elemento == "="))
            {
                LValue();
                P();
            }
            else
            {
                return;
            }
        }

        public void P()
        {
            if (Token.Tipo == "T_OPERADOR" && Token.Elemento == "=")
            {
                Consumir(new ElToken("=", "T_OPERADOR"));
                Expr();
            }
            else
            {
                return;
            }
        }

        private void ExprViene()
        {
            if (Token.Elemento == "-" || (Token.Elemento == "(") || Token.Elemento == "New" || Token.Elemento == "this" || Token.Tipo == "T_ENTERO" || Token.Tipo == "T_DOUBLE" || Token.Tipo == "T_BOOLEAN" || Token.Tipo == "T_CADENA" || Token.Tipo == "T_null")
            {
                Expr();
            }
            else
            {
                return;
            }

        }

        private void LValue()
        {
            if (Token.Tipo.Equals("T_ID"))
            {
                Consumir("T_ID");
            }
            else if (Token.Elemento == "-" || (Token.Elemento == "(") || Token.Elemento == "New" || Token.Elemento == "this" || Token.Tipo == "T_ENTERO" || Token.Tipo == "T_DOUBLE" || Token.Tipo == "T_BOOLEAN" || Token.Tipo == "T_CADENA" || Token.Tipo == "T_null") //Ver cuales terminales son para Expr
            {
                Expr();
                Q();
            }
            else
            {
                return;
            }
        }

        private void Q()
        {
            if (Token.Tipo == "T_OPERADOR" && Token.Elemento == ".")
            {
                Consumir(new ElToken(".", "T_OPERADOR"));
                Consumir("T_ID");
            }
            else if (Token.Tipo == "T_OPERADOR" && Token.Elemento == "[]")
            {
                Consumir(new ElToken("[]", "T_OPERADOR"));
            }
            else if (Token.Tipo == "T_OPERADOR" && Token.Elemento == "[")
            {

                Consumir(new ElToken("[", "T_OPERADOR"));
                Expr();
                Consumir(new ElToken("]", "T_OPERADOR"));
            }
            else
            {
                return;
            }
        }

        private void Constant()
        {
            if (Token.Tipo == "T_ENTERO")
            {
                Consumir("T_ENTERO");
            }
            else if (Token.Tipo == "T_DOUBLE")
            {
                Consumir("T_DOUBLE");
            }
            else if (Token.Tipo == "T_BOOLEAN")
            {
                Consumir("T_BOOLEAN");
            }
            else if (Token.Tipo == "T_CADENA")
            {
                Consumir("T_CADENA");
            }
            else if (Token.Tipo == "T_null")
            {
                Consumir("null");
            }
            else
            {
                ErrorSintaxis("T_ENTERO, T_DOUBLE, T_BOOLEAN, T_CADENA, T_null");
            }
        }

        private void Consumir(ElToken esperado)
        {
            if (Token.Elemento == esperado.Elemento && Token.Tipo == esperado.Tipo)
            {
                SiguienteToken();
            }
            else
            {
                ErrorSintaxis(esperado);
            }
        }

        private void Consumir(string esperado)
        {
            if (Token.Tipo == esperado)
            {
                SiguienteToken();
            }
            else
            {
                ErrorSintaxis(esperado);
            }
        }

        private void SiguienteToken()
        {
            contador++;
            if (contador < ListaTokens.Count)
            {
                Token = ListaTokens[contador];
            }
            else
            {
                estaVaciaListaTokens = true;
            }
            return;
        }

        private string errorONE = String.Empty;
        private string errorTWO = String.Empty;
        private string errorG = String.Empty;
        public void ErrorSintaxis(ElToken esperado)
        {
            errorONE = Convert.ToString(Builder.Append("\n ERROR, SE ESPERABA: ").Append(esperado.Elemento));
            return;
        }

        public void ErrorSintaxis(string esperado)
        {
            errorTWO = Convert.ToString(Builder.Append("\n ERROR, SE ESPERABA: ").Append(esperado));
            return;
        }

        public string Error { get => errorONE; set => errorONE = value; }
        public string Error2 { get => errorTWO; set => errorTWO = value; }
    }
}
