namespace BibliotecaConsoleApp.Utils
{
    public static class MenuRenderer
    {
        public static void ExibirMenu(string titulo, string[] opcoes)
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
            Console.WriteLine($"|{linhaHorizontal}|");
        }
    }
}