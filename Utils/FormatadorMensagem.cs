using BibliotecaConsoleApp.Entidades;

namespace BibliotecaConsoleApp.Utils
{
    public static class FormatadorMensagem
    {
        public static void ExibirDadosLivroFormatado(Livro livro)
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

        public static void ExibirMensagemErro(string mensagem)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n[ERRO] {mensagem}");
            Console.ResetColor();
        }

        public static void ExibirMensagemSucesso(string mensagem)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n[SUCESSO] {mensagem}");
            Console.ResetColor();
        }

        public static void ExibirCabecalho(string mensagem)
        {
            Console.Clear();
            Console.WriteLine($"{mensagem}\n");
        }


        public static void ExibirMenuCamposEdicao()
        {
            Console.WriteLine("\nESCOLHA O CAMPO QUE DESEJA ALTERAR:");
            Console.WriteLine("1 - TÍTULO");
            Console.WriteLine("2 - AUTOR");
            Console.WriteLine("3 - GÊNERO");
            Console.WriteLine("4 - ISBN");
            Console.WriteLine("5 - ANO DE PUBLICAÇÃO");
            Console.WriteLine("0 - SALVAR E SAIR");
            Console.Write("\nOPÇÃO: ");
        }

        public static void MostrarCampo(string label, string? valor)
        {
            if (!string.IsNullOrWhiteSpace(valor))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(label.PadRight(20) + $": {valor}");
                Console.ResetColor();
            }
        }

        public static void ExibirMenuPrincipal()
        {
            int largura = 52;
            string titulo = "BIBLIOTECA";
            int margem = (largura - titulo.Length - 2) / 2;
            string linhaHorizontal = new string('-', largura);

            Console.WriteLine($"|{new string('-', margem)} {titulo} {new string('-', margem)}|");
            Console.WriteLine($"| {"".PadRight(largura - 2)} |");
            Console.WriteLine($"| {"ESCOLHA UMA OPÇÃO:".PadRight(largura - 2)} |");
            Console.WriteLine($"| {"".PadRight(largura - 2)} |");

            string[] opcoes = {
                "[1] ADICIONAR LIVRO",
                "[2] EXCLUIR LIVRO",
                "[3] ATUALIZAR LIVRO",
                "[4] LISTAR TODOS OS LIVROS",
                "[5] CONSULTAR LIVRO POR ID",
                "[0] SAIR"
            };

            foreach (var opcao in opcoes)
            {
                Console.WriteLine($"| {opcao.PadRight(largura - 2)} |");
            }

            Console.WriteLine($"| {"".PadRight(largura - 2)} |");
            Console.WriteLine($"|{linhaHorizontal}|");
            Console.Write("\n[OPÇÃO]: ");
        }

        public static void ExibirFormulario(LivroFormState form)
        {
            Console.WriteLine("PREENCHA OS DADOS DO LIVRO:\n");

            FormatadorMensagem.MostrarCampo("ID", form.Id?.ToString());
            FormatadorMensagem.MostrarCampo("TÍTULO", form.Titulo);
            FormatadorMensagem.MostrarCampo("AUTOR", form.Autor);
            FormatadorMensagem.MostrarCampo("GÊNERO", form.Genero);
            FormatadorMensagem.MostrarCampo("ISBN", form.ISBN);
            FormatadorMensagem.MostrarCampo("ANO DE PUBLICAÇÃO", form.AnoPublicacao?.ToString());
        }
    }
}
