using BibliotecaConsoleApp.Entidades;

namespace BibliotecaConsoleApp.Utils
{
    public static class FormatadorMensagem
    {
        public static void ExibirLivrosFormatados(IEnumerable<Livro> livros)
        {
            foreach (var livro in livros)
            {
                void EscreverLinhaFormatada(string rotulo, string valor)
                {
                    int larguraTotal = 20;
                    Console.WriteLine($"{rotulo}{new string('.', larguraTotal - rotulo.Length)}: {valor}");
                }

                Console.WriteLine();
                EscreverLinhaFormatada("ID", livro.Id.ToString());
                EscreverLinhaFormatada("TÍTULO", livro.Titulo);
                EscreverLinhaFormatada("AUTOR", livro.Autor);
                EscreverLinhaFormatada("GÊNERO", livro.Genero);
                EscreverLinhaFormatada("ISBN", livro.ISBN);
                EscreverLinhaFormatada("ANO PUBLICAÇÃO", livro.AnoPublicacao.ToString());
            }

            AguardarUsuario();
        }

        public static void ExibirLivroFormatado(Livro livro)
        {
            ExibirLivrosFormatados(new[] { livro });
        }

        public static void ExibirMensagemErro(Exception ex, int linhaRetorno = -1, int colunaRetorno = 0)
        {
            int linhaErro = Console.CursorTop;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n[ERRO] {ex.Message}");
            Console.ResetColor();

            AguardarUsuario();

            LimparMensagem(linhaErro);

            if (linhaRetorno >= 0)
                Console.SetCursorPosition(colunaRetorno, linhaRetorno);
        }

        private static void LimparMensagem(int linhaInicial)
        {
            int linhaAtual = Console.CursorTop;

            for (int i = linhaAtual; i >= linhaInicial; i--)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(new string(' ', Console.WindowWidth));
            }
        }

        public static void ExibirMensagemInfo(string mensagem, int linhaRetorno = -1, int colunaRetorno = 0)
        {
            int linha = Console.CursorTop;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n[ALERTA] {mensagem}");
            Console.ResetColor();

            AguardarUsuario();

            LimparMensagem(linha);

            if (linhaRetorno >= 0)
                Console.SetCursorPosition(colunaRetorno, linhaRetorno);
        }

        public static void ExibirMensagemSucesso(string mensagem)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n[SUCESSO] {mensagem}");
            Console.ResetColor();

            AguardarUsuario();
        }

        public static void AguardarUsuario()
        {
            int linha = Console.CursorTop;

            Console.ResetColor();
            Console.WriteLine("\nPRESSIONE QUALQUER TECLA PARA CONTINUAR...");
            Console.ReadKey(true);


            LimparMensagem(linha);
        }

        public static void ConfirmarOperacao()
        {
            Console.Write("\nTEM CERTEZA QUE DESEJA EXECUTAR ESSA OPERAÇÃO? (S/N): ");

            var resposta = Console.ReadLine();
            if (resposta?.ToUpper() != "S")
            {
                throw new Exception("OPERAÇÃO CANCELADA");
                return;
            }
        }
    }
}
