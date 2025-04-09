using BibliotecaConsoleApp.Entidades;
using BibliotecaConsoleApp.Servicos;
using System.Drawing;

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

            if (opcao == 0)
                break;

            switch (opcao)
            {
                case 1:
                    CadastrarLivro();
                    break;
                case 2:
                    RemoverLivro();
                    break;
                case 3:
                    AtualizarLivro();
                    break;
                case 4:
                    ExibirLivrosCadastrados();
                    break;
                case 5:
                    ExibirLivroPorId();
                    break;
            }
        }
    }

    private void CadastrarLivro()
    {
        Console.Clear();
        Console.WriteLine("PREENCHA OS DADOS DO LIVRO:");

        var livrosExistentes = _livroServico.ListarTodos();
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

    private void RemoverLivro()
    {
        Console.Clear();

        Livro livroParaRemover;

        while (true)
        {
            int idLivroProcurado = LerIdDoUsuario("DIGITE O ID DO LIVRO QUE DESEJA REMOVER");

            try
            {
                livroParaRemover = _livroServico.BuscarLivroPorId(idLivroProcurado);
                break;
            }
            catch (Exception ex)
            {
                ExibirMensagemErro(ex.Message);
                Console.WriteLine("\nPRESSIONE QUALQUER TECLA PARA TENTAR NOVAMENTE...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        try
        {
            Console.ForegroundColor = ConsoleColor.Red;
            PadronizarExibicaoLivro(livroParaRemover);
            Console.ResetColor();

            ConfirmarOperacao();
            _livroServico.RemoverLivro(livroParaRemover.Id);

            ExibirMensagemSucesso("LIVRO REMOVIDO!");
        }
        catch (Exception ex)
        {
            ExibirMensagemErro(ex.Message);
        }

        Console.WriteLine("\nPRESSIONE QUALQUER TECLA PARA CONTINUAR...");
        Console.ReadKey();
    }

    private void AtualizarLivro()
    {
        Console.Clear();

        while (true)
        {
            int idLivroProcurado = LerIdDoUsuario("DIGITE O ID DO LIVRO QUE DESEJA EDITAR");

            try
            {
                var livroOriginal = _livroServico.BuscarLivroPorId(idLivroProcurado);
                PadronizarExibicaoLivro(livroOriginal);

                if (!ConfirmarOperacao("DESEJA EDITAR ESTE LIVRO?", livroOriginal))
                    return;

                var livroAtualizado = EditarDadosLivroExistente(livroOriginal);
                _livroServico.AtualizarLivro(livroAtualizado);

                Console.Clear();
                ExibirMensagemSucesso("LIVRO ATUALIZADO COM SUCESSO!");
                break;
            }
            catch (Exception ex)
            {
                ExibirMensagemErro($"{ex.Message}");
                Console.WriteLine("\nPRESSIONE QUALQUER TECLA PARA CONTINUAR...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        Console.WriteLine("\nPRESSIONE QUALQUER TECLA PARA CONTINUAR...");
        Console.ReadKey();
    }

    private bool ConfirmarOperacao(string mensagem, Livro livro)
    {
        while (true)
        {
            Console.Clear();
            PadronizarExibicaoLivro(livro);

            Console.WriteLine();
            Console.Write($"{mensagem} (S/N): ");
            string? resposta = Console.ReadLine()?.Trim().ToUpper();

            if (resposta == "S") 
            { 
                return true;
            }
            else if (resposta == "N") 
            {
                return false;
            }

            ExibirMensagemErro("ENTRADA INVÁLIDA. DIGITE 'S' PARA SIM OU 'N' PARA NÃO.");
            Console.WriteLine("PRESSIONE QUALQUER TECLA PARA TENTAR NOVAMENTE...");
            Console.ReadKey(true);
        }
    }

    private Livro EditarDadosLivroExistente(Livro livroOriginal)
    {
        var estado = new LivroFormState
        {
            Id = livroOriginal.Id,
            Titulo = livroOriginal.Titulo,
            Autor = livroOriginal.Autor,
            Genero = livroOriginal.Genero,
            ISBN = livroOriginal.ISBN,
            AnoPublicacao = livroOriginal.AnoPublicacao
        };

        var livrosExistentes = _livroServico.ListarTodos()
            .Where(l => l.Id != livroOriginal.Id && l.ISBN != livroOriginal.ISBN)
            .ToList();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("EDIÇÃO DOS DADOS DO LIVRO:\n");

            MostrarCampo("ID", estado.Id.ToString());
            MostrarCampo("TÍTULO", estado.Titulo);
            MostrarCampo("AUTOR", estado.Autor);
            MostrarCampo("GÊNERO", estado.Genero);
            MostrarCampo("ISBN", estado.ISBN);
            MostrarCampo("ANO DE PUBLICAÇÃO", estado.AnoPublicacao.ToString());

            Console.WriteLine("\nESCOLHA O CAMPO QUE DESEJA ALTERAR:");
            Console.WriteLine("1 - TÍTULO");
            Console.WriteLine("2 - AUTOR");
            Console.WriteLine("3 - GÊNERO");
            Console.WriteLine("4 - ISBN");
            Console.WriteLine("5 - ANO DE PUBLICAÇÃO");
            Console.WriteLine("0 - SALVAR E SAIR");

            Console.Write("\nOPÇÃO: ");
            var entrada = Console.ReadLine();


            switch (entrada)
            {
                case "1":
                    estado.Titulo = LerCampoComPrefixo("TÍTULO", estado, () =>
                        TentarLerTextoObrigatorio("NOVO TÍTULO", estado));
                    break;
                case "2":
                    estado.Autor = LerCampoComPrefixo("AUTOR", estado, () =>
                        TentarLerTextoObrigatorio("NOVO AUTOR", estado));
                    break;
                case "3":
                    estado.Genero = LerCampoComPrefixo("GÊNERO", estado, () =>
                        TentarLerTextoObrigatorio("NOVO GÊNERO", estado));
                    break;
                case "4":
                    estado.ISBN = LerCampoComPrefixo("ISBN", estado, () =>
                    {
                        string? novoIsbn;
                        do
                        {
                            novoIsbn = TentarLerIsbnUnico(estado, livrosExistentes);
                        } while (novoIsbn == null);
                        return novoIsbn;
                    });
                    break;
                case "5":
                    estado.AnoPublicacao = LerCampoComPrefixo("ANO DE PUBLICAÇÃO", estado, () =>
                    {
                        int? novoAno;
                        do
                        {
                            novoAno = TentarLerAnoValido(estado);
                        } while (novoAno == null);
                        return novoAno;
                    });
                    break;
                case "0":
                    return new Livro
                    {
                        Id = estado.Id.Value,
                        Titulo = estado.Titulo!,
                        Autor = estado.Autor!,
                        Genero = estado.Genero!,
                        ISBN = estado.ISBN!,
                        AnoPublicacao = estado.AnoPublicacao.Value
                    };
                default:
                    ExibirMensagemErro("OPÇÃO INVÁLIDA. TENTE NOVAMENTE.");
                    Console.ReadKey();
                    break;
            }

        }
    }

    private T? LerCampoComPrefixo<T>(string campo, LivroFormState form, Func<T?> leitor)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("EDIÇÃO DOS DADOS DO LIVRO:\n");

            MostrarCampo("ID", form.Id?.ToString());
            MostrarCampo("TÍTULO", form.Titulo);
            MostrarCampo("AUTOR", form.Autor);
            MostrarCampo("GÊNERO", form.Genero);
            MostrarCampo("ISBN", form.ISBN);
            MostrarCampo("ANO DE PUBLICAÇÃO", form.AnoPublicacao?.ToString());

            Console.WriteLine();
            try
            {
                return leitor();
            }
            catch
            {
                ExibirMensagemErro("ENTRADA INVÁLIDA. TENTE NOVAMENTE.");
                Console.ReadKey();
            }
        }
    }


    private void ExibirLivrosCadastrados()
    {
        Console.Clear();
        Console.WriteLine("LIVROS CADASTRADOS: ");

        var livrosCadastrados = _livroServico.ListarTodos();

        foreach (var livro in livrosCadastrados)
        {
            PadronizarExibicaoLivro(livro);
        }

        Console.WriteLine("\nPRESSIONE QUALQUER TECLA PARA CONTINUAR...");
        Console.ReadKey();
    }

    private void ExibirLivroPorId()
    {
        Console.Clear();

        while (true)
        {
            int idLivroProcurado = LerIdDoUsuario("DIGITE O ID DO LIVRO QUE DESEJA BUSCAR");

            try
            {
                var livroEncontrado = _livroServico.BuscarLivroPorId(idLivroProcurado);
                PadronizarExibicaoLivro(livroEncontrado);
                break;
            }
            catch (Exception ex)
            {
                ExibirMensagemErro($"{ex.Message}");
                Console.WriteLine("\nPRESSIONE QUALQUER TECLA PARA CONTINUAR...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        Console.WriteLine("\nPRESSIONE QUALQUER TECLA PARA CONTINUAR...");
        Console.ReadKey();
    }


    public void PadronizarExibicaoLivro(Livro livro)
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

    private int LerIdDoUsuario(string mensagem)
    {
        while (true)
        {
            Console.Write($"{mensagem}: ");
            var entrada = Console.ReadLine();

            if (int.TryParse(entrada, out int id) && id > 0)
                return id;

            ExibirMensagemErro("ID INVÁLIDO - DIGITE UM VALOR VÁLIDO.");
            Console.WriteLine("\nPRESSIONE QUALQUER TECLA PARA CONTINUAR...");
            Console.ReadKey();
            Console.Clear();
        }
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

    private int ObterOpcaoMenu()
    {
        while (true)
        {
            string? entrada = Console.ReadLine();

            try
            {
                ValidarOEntradaMenu(entrada!);
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

    private void ConfirmarOperacao()
    {
        Console.Write("\nTEM CERTEZA QUE DESEJA EXECUTAR ESSA OPERAÇÃO? (S/N): ");

        var resposta = Console.ReadLine();
        if (resposta?.ToUpper() != "S")
        {
            throw new Exception("OPERAÇÃO CANCELADA");
            return;
        }
    }

    private void ValidarOEntradaMenu(string entrada)
    {
        if (string.IsNullOrWhiteSpace(entrada))
            throw new Exception("A OPÇÃO NÃO PODE SER VAZIA.");

        if (!int.TryParse(entrada, out int valor) || valor < 0 || valor > 5)
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
        Console.Write("ID: ");
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
            Console.Write($"\n{campo}: ");
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
        Console.Write("\nISBN: ");
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
        Console.Write("\nANO DE PUBLICAÇÃO: ");
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