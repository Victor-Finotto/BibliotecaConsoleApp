using BibliotecaConsoleApp.Entidades;

namespace BibliotecaConsoleApp.Utils
{
    public static class MenuRenderer
    {
        public static void ExibirMenu(string titulo, string[] opcoes, Usuario usuarioSelecionado = null)
        {
            int largura = 52;
            int margem = (largura - titulo.Length - 2) / 2;
            string linhaHorizontal = new string('-', largura);

            Console.WriteLine($"|{new string('-', margem)} {titulo} {new string('-', margem)}|");
            Console.WriteLine($"| {"".PadRight(largura - 2)} |");
            Console.WriteLine($"| {"ESCOLHA UMA OPÇÃO:".PadRight(largura - 2)} |");
            Console.WriteLine($"| {"".PadRight(largura - 2)} |");

            foreach (var opcao in opcoes)
            {
                Console.WriteLine($"| {opcao.PadRight(largura - 2)} |");
            }

            Console.WriteLine($"| {"".PadRight(largura - 2)} |");

            if (usuarioSelecionado != null)
            {
                string prefixo = "USUÁRIO SELECIONADO: ";
                int limiteNome = largura - 2 - prefixo.Length;

                string nome = usuarioSelecionado.Nome.ToUpper();
                if (nome.Length > limiteNome)
                    nome = nome.Substring(0, limiteNome - 3) + "...";

                string linha = $"{prefixo}{nome}".PadRight(largura - 2);
                Console.WriteLine($"| {linha} |");
            }


            Console.WriteLine($"|{linhaHorizontal}|");
        }
    }
}