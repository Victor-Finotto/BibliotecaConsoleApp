using BibliotecaConsoleApp.Entidades;
using BibliotecaConsoleApp.Servicos;

class MenuBiblioteca
{
    private readonly LivroServico _livroServico;

    public MenuBiblioteca(LivroServico livroServico)
    {
        _livroServico = livroServico;
    }

    public void Executar()
    {
        while (true)
        {
            Console.Clear();
            ExibirMenu();

            int opcao = ObterOpcaoMenu();

            switch (opcao)
            {
                case 1:
                    CadastrarLivro();
                    break;
                case 2:
                    // Excluir livro
                    break;
                case 3:
                    // Atualizar livro
                    break;
                case 4:
                    // Listar livros
                    break;
                case 5:
                    // Listar por ID
                    break;
            }

            break; // <- temporário, só para testes. Remover para loop contínuo.
        }
    }

    private void CadastrarLivro()
{
    Console.Clear();
    Console.WriteLine("PREENCHA OS DADOS DO LIVRO:");

    var livrosExistentes = _livroServico.ListarTodos(); // <- Consulta única
    var livro = ObterDadosLivroDoUsuario(livrosExistentes);

    try
    {
        _livroServico.AdicionarLivro(livro);
        ExibirMensagemSucesso("LIVRO CADASTRADO COM SUCESSO!");
    }
    catch (Exception ex)
    {
        ExibirMensagemErro(ex.Message);
    }

    Console.WriteLine("\nPRESSIONE QUALQUER TECLA PARA CONTINUAR...");
    Console.ReadKey();
}


    private void ExibirMenu()
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
            "[5] CONSULTAR LIVRO POR ID"
        };

        foreach (var opcao in opcoes)
            Console.WriteLine($"| {opcao.PadRight(largura - 2)} |");

        Console.WriteLine($"| {"".PadRight(largura - 2)} |");
        Console.WriteLine($"|{linhaHorizontal}|");
        Console.Write("\n[OPÇÃO]: ");
    }

    private int ObterOpcaoMenu()
    {
        while (true)
        {
            string? entrada = Console.ReadLine();

            try
            {
                ValidarOpcao(entrada!);
                return int.Parse(entrada!);
            }
            catch (Exception ex)
            {
                ExibirMensagemErro(ex.Message);
                Console.Write("PRESSIONE QUALQUER TECLA PARA CONTINUAR...");
                Console.ReadKey();
                Console.Clear();
                ExibirMenu();
            }
        }
    }

    private void ValidarOpcao(string entrada)
    {
        if (string.IsNullOrWhiteSpace(entrada))
            throw new Exception("A OPÇÃO NÃO PODE SER VAZIA.");

        if (!int.TryParse(entrada, out int valor) || valor < 1 || valor > 5)
            throw new Exception("OPÇÃO INVÁLIDA. DIGITE UM NÚMERO DE 1 A 5.");
    }

    private Livro ObterDadosLivroDoUsuario(List<Livro> livrosExistentes)
    {
        var form = new LivroFormState();

        while (form.Id == null) form.Id = TentarLerIdUnico(form, livrosExistentes);
        while (form.Titulo == null) form.Titulo = TentarLerTextoObrigatorio("TÍTULO", form);
        while (form.Autor == null) form.Autor = TentarLerTextoObrigatorio("AUTOR", form);
        while (form.Genero == null) form.Genero = TentarLerTextoObrigatorio("GÊNERO", form);
        while (form.ISBN == null) form.ISBN = TentarLerIsbnUnico(form, livrosExistentes);
        while (form.AnoPublicacao == null) form.AnoPublicacao = TentarLerAnoValido(form);

        Console.Clear();
        ExibirFormulario(form);

        return new Livro
        {
            Id = form.Id.Value,
            Titulo = form.Titulo!,
            Autor = form.Autor!,
            Genero = form.Genero!,
            ISBN = form.ISBN!,
            AnoPublicacao = form.AnoPublicacao.Value
        };
    }

    private void MostrarCampo(string label, string? valor)
    {
        if (!string.IsNullOrWhiteSpace(valor))
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(label.PadRight(20) + $": {valor}");
            Console.ResetColor();
        }
    }

    private int? TentarLerIdUnico(LivroFormState form, List<Livro> livros)
    {
        Console.Clear();
        ExibirFormulario(form);
        Console.Write("ID".PadRight(20) + ": ");
        var entrada = Console.ReadLine();

        if (int.TryParse(entrada, out int idValido) && idValido > 0)
        {
            if (livros.Any(l => l.Id == idValido))
            {
                ExibirMensagemErro("JÁ EXISTE UM LIVRO COM ESSE ID. DIGITE OUTRO.");
                Console.ReadKey();
                return null;
            }
            return idValido;
        }

        ExibirMensagemErro("O CAMPO ID DEVE SER UM NÚMERO INTEIRO POSITIVO.");
        Console.ReadKey();
        return null;
    }


    private string TentarLerTextoObrigatorio(string campo, LivroFormState form)
    {
        while (true)
        {
            Console.Clear();
            ExibirFormulario(form);
            Console.Write(campo.PadRight(20) + ": ");
            var entrada = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(entrada))
                return entrada;

            ExibirMensagemErro("ESTE CAMPO É OBRIGATÓRIO.");
            Console.ReadKey();
        }
    }
    private string? TentarLerIsbnUnico(LivroFormState form, List<Livro> livros)
    {
        Console.Clear();
        ExibirFormulario(form);
        Console.Write("ISBN".PadRight(20) + ": ");
        var entrada = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(entrada))
        {
            ExibirMensagemErro("ESTE CAMPO É OBRIGATÓRIO.");
            Console.ReadKey();
            return null;
        }

        if (livros.Any(l => l.ISBN == entrada))
        {
            ExibirMensagemErro("JÁ EXISTE UM LIVRO COM ESSE ISBN. DIGITE OUTRO.");
            Console.ReadKey();
            return null;
        }

        return entrada;
    }

    private int? TentarLerAnoValido(LivroFormState form)
    {
        Console.Clear();
        ExibirFormulario(form);
        Console.Write("ANO DE PUBLICAÇÃO".PadRight(20) + ": ");
        var entrada = Console.ReadLine();

        if (int.TryParse(entrada, out int anoValido) && anoValido > 1000 && anoValido <= DateTime.Now.Year)
            return anoValido;

        ExibirMensagemErro("ANO INVÁLIDO. TENTE NOVAMENTE.");
        Console.ReadKey();
        return null;
    }

    private void ExibirFormulario(LivroFormState form)
    {
        Console.WriteLine("PREENCHA OS DADOS DO LIVRO:\n");

        MostrarCampo("ID", form.Id?.ToString());
        MostrarCampo("TÍTULO", form.Titulo);
        MostrarCampo("AUTOR", form.Autor);
        MostrarCampo("GÊNERO", form.Genero);
        MostrarCampo("ISBN", form.ISBN);
        MostrarCampo("ANO DE PUBLICAÇÃO", form.AnoPublicacao?.ToString());
    }

    private void ExibirMensagemErro(string mensagem)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n[ERRO] {mensagem}");
        Console.ResetColor();
    }

    private void ExibirMensagemSucesso(string mensagem)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n[SUCESSO] {mensagem}");
        Console.ResetColor();
    }
}