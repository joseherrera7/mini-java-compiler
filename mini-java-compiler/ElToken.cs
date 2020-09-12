using System;

namespace mini_java_compiler
{
    class ElToken
    {
        public ElToken(string elemento, String tipo)
        {
            Elemento = elemento;
            Tipo = tipo;
        }

        public ElToken()
        {
        }

        public string Elemento { get; set; }
        public string Tipo { get; set; }
    }
}
