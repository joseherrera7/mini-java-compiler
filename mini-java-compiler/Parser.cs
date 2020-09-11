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
        private void Program()
        {
            Decl();
        }

        private void Decl()
        {
            /*if (Token == "")
            {

            }
            VariableDecl();*/
        }

        private void VariableDecl()
        {
           
        }

        private void Variable()
        {

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
            Type();
            if (true)
            {

            }
        }

        private void K()
        {

        }

        private void StmtIntermedia()
        {

        }

        private void Formals()
        {

        }

        private void VariableIntermedia()
        {

        }

        private void Stmt()
        {

        }

        private void IfStmt()
        {

        }
        private void ElseViene()
        {

        }

        private void PrintStmt()
        {

        }

        private void ExprIntermedia()
        {

        }
        private void Expr()
        {

        }
        private void ExprPrima()
        {

        }

        private void X()
        {

        }
        private void XPrima()
        {

        }

        private void A()
        {

        }
        private void APrima()
        {

        }

        private void B()
        {

        }

        private void BPrima()
        {

        }
        private void C()
        {

        }

        private void CPrima()
        {

        }
        private void D()
        {

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

        private void ErrorSintaxis(string esperado)
        {
            Builder.Append("ERROR, SE ESPERABA: ").Append(esperado);
        }
    }
}
