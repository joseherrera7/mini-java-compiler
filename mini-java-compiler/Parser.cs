using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mini_java_compiler
{
    class Parser
    {
        List<ElToken> ListaTokens;
        StringBuilder builder = new StringBuilder();
        ElToken Token;

        public StringBuilder Builder { get => builder; set => builder = value; }

        public void Empezar(List<ElToken> Listatoken)
        {
            ListaTokens = Listatoken;
            
            foreach (var token in ListaTokens)
            {
                Token = token;
                Program();
            }
        }

        //AQUI COMIENZA LA GRAMATICA
        private void Program()
        {
            Decl();
        }

        private void Decl()
        {
            /*if (Token == "")  // vaer los tterminales de viariableDecl

            }
            else if (true)
            {

            }
            else if (true)
            {

            }*/
        }

        private void VariableDecl()
        {
            Variable();
            Consumir(new ElToken(";","T_OPERADOR"));
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
            else
            {
                Type();
                K();
            }
        }

        private void K()
        {
            Consumir("T_ID");
            Consumir(new ElToken("(","T_OPERADOR"));
            Formals();
            Consumir("T_OPERADOR");
            StmtIntermedia();
        }

        private void StmtIntermedia()
        {
            if (Token.Tipo)
            {
                
            }
            else
            {

            }
        }

        private void Formals()
        {

        }

        private void VariableIntermedia()
        {

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
            else
            {
                Expr();
            }
        }

        private void IfStmt()
        {
            Consumir("T_if");
            Consumir(new ElToken("(", "T_OPERADOR"));
            Expr();
            Consumir("T_OPERADOR");
            Stmt();
            ElseViene();
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
            Consumir(new ElToken("Print", "T_print"));
            Consumir(new ElToken("(", "T_OPERADOR"));
            Expr();
            ExprIntermedia();
            Consumir(new ElToken(",", "T_OPERADOR"));
            Consumir("T_OPERADOR");
            Consumir(new ElToken(";", "T_OPERADOR"));
        }

        private void ExprIntermedia()
        {
            if (Token.Tipo.Equals("T_Expr"))
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

        }
        private void ExprPrima()
        {

        }

        private void X()
        {
            A();
            XPrima();
        }
        private void XPrima()
        {
            if (true)
            {

            }
        }

        private void A()
        {
            B();
            APrima();
        }
        private void APrima()
        {

        }

        private void B()
        {
            C();
            BPrima();
        }

        private void BPrima()
        {

        }
        private void C()
        {
            D();
            CPrima();
        }

        private void CPrima()
        {

        }
        private void D()
        {
            if (true)
            {

            }
        }
        private void LValue()
        {
            if (Token.Tipo.Equals("T_ID"))
            {
                Consumir("T_ID");
            }
            else if (Token.Tipo.Equals("")) //Ver cuales terminales son para Expr
            {
                
            }
            else if (true)
            {

            }
            else
            {

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
            else if (Token .Tipo== "T_BOOLEAN")
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
            if (Token == esperado)
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
            return;
        }

        private void ErrorSintaxis(ElToken esperado)
        {
            Builder.Append("ERROR, SE ESPERABA: ").Append(esperado.Elemento);
        }

        private void ErrorSintaxis(string esperado)
        {
            Builder.Append("ERROR, SE ESPERABA: ").Append(esperado);
        }
    }
}
