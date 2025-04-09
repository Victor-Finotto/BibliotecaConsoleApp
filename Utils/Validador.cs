using BibliotecaConsoleApp.Entidades;

namespace BibliotecaConsoleApp.Utils
{
    public static class Validador
    {
        public static int? ValidarIdUnico(LivroFormState form, List<Livro> livros)
        {
            Console.Clear();
            FormatadorMensagem.ExibirFormulario(form);
            Console.Write("ID: ");
            var entrada = Console.ReadLine();

            if (int.TryParse(entrada, out int idValido) && idValido > 0)
            {
                if (livros.Any(l => l.Id == idValido))
                {
                    FormatadorMensagem.ExibirMensagemErro("JÁ EXISTE UM LIVRO COM ESSE ID. DIGITE OUTRO.");
                    Console.ReadKey();
                    return null;
                }
                return idValido;
            }

            FormatadorMensagem.ExibirMensagemErro("O CAMPO ID DEVE SER UM NÚMERO INTEIRO POSITIVO.");
            Console.ReadKey();
            return null;
        }


        public static string ValidarTextoObrigatorio(string campo, LivroFormState form)
        {
            while (true)
            {
                Console.Clear();
                FormatadorMensagem.ExibirFormulario(form);
                Console.Write($"\n{campo}: ");
                var entrada = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(entrada))
                    return entrada;

                FormatadorMensagem.ExibirMensagemErro("ESTE CAMPO É OBRIGATÓRIO.");
                Console.ReadKey();
            }
        }

        public static string? ValidarIsbnUnico(LivroFormState form, List<Livro> livros)
        {
            Console.Clear();
            FormatadorMensagem.ExibirFormulario(form);
            Console.Write("\nISBN: ");
            var entrada = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(entrada))
            {
                FormatadorMensagem.ExibirMensagemErro("ESTE CAMPO É OBRIGATÓRIO.");
                Console.ReadKey();
                return null;
            }

            if (livros.Any(l => l.ISBN == entrada))
            {
                FormatadorMensagem.ExibirMensagemErro("JÁ EXISTE UM LIVRO COM ESSE ISBN. DIGITE OUTRO.");
                Console.ReadKey();
                return null;
            }

            return entrada;
        }

        public static int? ValidarLerAnoValido(LivroFormState form)
        {
            Console.Clear();
            FormatadorMensagem.ExibirFormulario(form);
            Console.Write("\nANO DE PUBLICAÇÃO: ");
            var entrada = Console.ReadLine();

            if (int.TryParse(entrada, out int anoValido) && anoValido > 1000 && anoValido <= DateTime.Now.Year)
                return anoValido;

            FormatadorMensagem.ExibirMensagemErro("ANO INVÁLIDO. TENTE NOVAMENTE.");
            Console.ReadKey();
            return null;
        }
    }
}
