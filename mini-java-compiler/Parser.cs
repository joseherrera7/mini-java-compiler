using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mini_java_compiler
{
    class Parser
    {
        ElToken Token;
        StringBuilder builder = new StringBuilder();

        public StringBuilder Builder { get => builder; set => builder = value; }

        public void Empezar(ElToken token)
        {
            Token = token;
            Program();
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

        }

        private void Z()
        {

        }

        private void FunctionDecl()
        {

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
