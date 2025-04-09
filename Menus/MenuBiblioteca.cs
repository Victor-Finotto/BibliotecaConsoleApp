using BibliotecaConsoleApp.Entidades;
using BibliotecaConsoleApp.Servicos;
using BibliotecaConsoleApp.Utils;
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
            FormatadorMensagem.ExibirMenuPrincipal();

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
        FormatadorMensagem.ExibirCabecalho("PREENCHA OS DADOS DO LIVRO:");

        var livrosExistentes = _livroServico.ListarTodos();
        var livro = new Livro
        {
            Id = ValidarIdUnico(new LivroFormState(), livrosExistentes),
            Titulo = ValidarCampo("TÍTULO", new LivroFormState(), livrosExistentes, ValidarTextoObrigatorio),
            Autor = ValidarCampo("AUTOR", new LivroFormState(), livrosExistentes, ValidarTextoObrigatorio),
            Genero = ValidarCampo("GÊNERO", new LivroFormState(), livrosExistentes, ValidarTextoObrigatorio),
            ISBN = ValidarCampo("ISBN", new LivroFormState(), livrosExistentes, ValidarIsbnUnico),
            AnoPublicacao = ValidarCampo("ANO DE PUBLICAÇÃO", new LivroFormState(), livrosExistentes, ValidarAnoPublicacao)
        };

        try
        {
            _livroServico.AdicionarLivro(livro);
            FormatadorMensagem.ExibirMensagemSucesso("LIVRO CADASTRADO COM SUCESSO!");
        }
        catch (Exception ex)
        {
            FormatadorMensagem.ExibirMensagemErro(ex.Message);
        }

        AguardarUsuario();
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
                FormatadorMensagem.ExibirMensagemErro(ex.Message);
                
                AguardarUsuario();
                Console.Clear();
            }
        }

        try
        {
            Console.ForegroundColor = ConsoleColor.Red;
            FormatadorMensagem.ExibirDadosLivroFormatado(livroParaRemover);
            Console.ResetColor();

            ConfirmarOperacao();
            _livroServico.RemoverLivro(livroParaRemover.Id);

            FormatadorMensagem.ExibirMensagemSucesso("LIVRO REMOVIDO!");
        }
        catch (Exception ex)
        {
            FormatadorMensagem.ExibirMensagemErro(ex.Message);
        }

        AguardarUsuario();
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
                FormatadorMensagem.ExibirDadosLivroFormatado(livroOriginal);

                if (!ConfirmarOperacao("DESEJA EDITAR ESTE LIVRO?", livroOriginal))
                    return;

                var livroAtualizado = EditarDadosLivroExistente(livroOriginal);
                _livroServico.AtualizarLivro(livroAtualizado);

                Console.Clear();
                FormatadorMensagem.ExibirMensagemSucesso("LIVRO ATUALIZADO COM SUCESSO!");
                break;
            }
            catch (Exception ex)
            {
                FormatadorMensagem.ExibirMensagemErro($"{ex.Message}");
                
                AguardarUsuario();
                Console.Clear();
            }
        }

        AguardarUsuario();

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

    private bool ConfirmarOperacao(string mensagem, Livro livro)
    {
        while (true)
        {
            Console.Clear();
            FormatadorMensagem.ExibirDadosLivroFormatado(livro);

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

            FormatadorMensagem.ExibirMensagemErro("ENTRADA INVÁLIDA. DIGITE 'S' PARA SIM OU 'N' PARA NÃO.");
            AguardarUsuario();
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
            FormatadorMensagem.ExibirCabecalho("EDIÇÃO DOS DADOS DO LIVRO:");

            FormatadorMensagem.MostrarCampo("ID", estado.Id.ToString());
            FormatadorMensagem.MostrarCampo("TÍTULO", estado.Titulo);
            FormatadorMensagem.MostrarCampo("AUTOR", estado.Autor);
            FormatadorMensagem.MostrarCampo("GÊNERO", estado.Genero);
            FormatadorMensagem.MostrarCampo("ISBN", estado.ISBN);
            FormatadorMensagem.MostrarCampo("ANO DE PUBLICAÇÃO", estado.AnoPublicacao.ToString());

            FormatadorMensagem.ExibirMenuCamposEdicao();
            var entrada = Console.ReadLine();

            switch (entrada)
            {
                case "1":
                    estado.Titulo = ValidarCampo("TÍTULO", estado, livrosExistentes, ValidarTextoObrigatorio);
                    break;
                case "2":
                    estado.Autor = ValidarCampo("AUTOR", estado, livrosExistentes, ValidarTextoObrigatorio);
                    break;
                case "3":
                    estado.Genero = ValidarCampo("GÊNERO", estado, livrosExistentes, ValidarTextoObrigatorio);
                    break;
                case "4":
                    estado.ISBN = ValidarCampo("ISBN", estado, livrosExistentes, ValidarIsbnUnico);
                    break;
                case "5":
                    estado.AnoPublicacao = ValidarCampo("ANO DE PUBLICAÇÃO", estado, livrosExistentes, ValidarAnoPublicacao);
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
                    FormatadorMensagem.ExibirMensagemErro("OPÇÃO INVÁLIDA. TENTE NOVAMENTE.");
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

            FormatadorMensagem.MostrarCampo("ID", form.Id?.ToString());
            FormatadorMensagem.MostrarCampo("TÍTULO", form.Titulo);
            FormatadorMensagem.MostrarCampo("AUTOR", form.Autor);
            FormatadorMensagem.MostrarCampo("GÊNERO", form.Genero);
            FormatadorMensagem.MostrarCampo("ISBN", form.ISBN);
            FormatadorMensagem.MostrarCampo("ANO DE PUBLICAÇÃO", form.AnoPublicacao?.ToString());

            Console.WriteLine();
            try
            {
                return leitor();
            }
            catch
            {
                FormatadorMensagem.ExibirMensagemErro("ENTRADA INVÁLIDA. TENTE NOVAMENTE.");
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
            FormatadorMensagem.ExibirDadosLivroFormatado(livro);
        }

        AguardarUsuario();
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
                FormatadorMensagem.ExibirDadosLivroFormatado(livroEncontrado);
                break;
            }
            catch (Exception ex)
            {
                FormatadorMensagem.ExibirMensagemErro($"{ex.Message}");
                Console.WriteLine("\nPRESSIONE QUALQUER TECLA PARA CONTINUAR...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        AguardarUsuario();
    }

    private int LerIdDoUsuario(string mensagem)
    {
        while (true)
        {
            Console.Write($"{mensagem}: ");
            var entrada = Console.ReadLine();

            if (int.TryParse(entrada, out int id) && id > 0)
                return id;

            FormatadorMensagem.ExibirMensagemErro("ID INVÁLIDO - DIGITE UM VALOR VÁLIDO.");

            AguardarUsuario();
            Console.Clear();
        }
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
                FormatadorMensagem.ExibirMensagemErro(ex.Message);
                
                AguardarUsuario();
                Console.Clear();

                FormatadorMensagem.ExibirMenuPrincipal();
            }
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

        // Utilizando o método auxiliar para validar os campos de forma mais concisa
        form.Id = ValidarCampo("ID", form, livrosExistentes, ValidarIdUnico);
        form.Titulo = ValidarCampo("TÍTULO", form, livrosExistentes, ValidarTextoObrigatorio);
        form.Autor = ValidarCampo("AUTOR", form, livrosExistentes, ValidarTextoObrigatorio);
        form.Genero = ValidarCampo("GÊNERO", form, livrosExistentes, ValidarTextoObrigatorio);
        form.ISBN = ValidarCampo("ISBN", form, livrosExistentes, ValidarIsbnUnico);
        form.AnoPublicacao = ValidarCampo("Ano de Publicação", form, livrosExistentes, ValidarAnoPublicacao);

        // Exibe o formulário final
        Console.Clear();
        FormatadorMensagem.ExibirFormulario(form);

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

    // Método genérico para validar os campos de forma reutilizável
    private T ValidarCampo<T>(string campo, LivroFormState form, List<Livro> livros, Func<LivroFormState, List<Livro>, T> validacao)
    {
        T valor;
        do
        {
            Console.Clear();
            FormatadorMensagem.ExibirFormulario(form);
            Console.Write($"\n{campo}: ");
            valor = validacao(form, livros);

            if (valor == null)
            {
                FormatadorMensagem.ExibirMensagemErro($"O campo {campo} é obrigatório ou inválido.");
                Console.ReadKey();
            }
        } while (valor == null);

        return valor;
    }

    private int ValidarIdUnico(LivroFormState form, List<Livro> livros)
    {
        var entrada = Console.ReadLine();
        if (int.TryParse(entrada, out int id) && id > 0 && !livros.Any(l => l.Id == id))
            return id;

        FormatadorMensagem.ExibirMensagemErro("ID inválido ou já existente. Tente novamente.");
        return 0; // Retorna um valor padrão para indicar erro
    }

    private string ValidarTextoObrigatorio(LivroFormState form, List<Livro> livros)
    {
        var entrada = Console.ReadLine();
        return !string.IsNullOrWhiteSpace(entrada) ? entrada : null;
    }

    private string ValidarIsbnUnico(LivroFormState form, List<Livro> livros)
    {
        var isbn = Console.ReadLine();
        if (livros.Any(l => l.ISBN == isbn))
        {
            FormatadorMensagem.ExibirMensagemErro("Já existe um livro com esse ISBN. Digite outro.");
            return null;
        }
        return isbn;
    }

    private int ValidarAnoPublicacao(LivroFormState form, List<Livro> livros)
    {
        var entrada = Console.ReadLine();
        if (int.TryParse(entrada, out int ano) && ano > 1000 && ano <= DateTime.Now.Year)
            return ano;

        FormatadorMensagem.ExibirMensagemErro("Ano inválido. Tente novamente.");
        return 0; // Retorna um valor padrão para indicar erro
    }

    public static void AguardarUsuario()
    {
        Console.WriteLine("\nPRESSIONE QUALQUER TECLA PARA CONTINUAR...");
        Console.ReadKey();
    }
}