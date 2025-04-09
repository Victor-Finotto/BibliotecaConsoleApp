using System;
using System.IO;

namespace BibliotecaConsoleApp.Utils
{
    public static class Caminhos
    {
        private static readonly string Base = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\"));

        public static readonly string Livros = Path.Combine(Base, "Data", "livros.json");
        public static readonly string Usuarios = Path.Combine(Base, "Data", "usuarios.json");
        public static readonly string Emprestimos = Path.Combine(Base, "Data", "emprestimos.json");
    }
}