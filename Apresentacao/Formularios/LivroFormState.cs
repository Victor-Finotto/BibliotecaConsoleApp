using BibliotecaConsoleApp.Entidades;
using BibliotecaConsoleApp.Servicos;
using BibliotecaConsoleApp.Utils;
using System.Drawing;

public class LivroFormState
{
    private readonly LivroServico _livroServico;

    public LivroFormState(LivroServico livroServico)
    {
        _livroServico = livroServico;
    }

    private int? id;
    private string? titulo;
    private string? autor;
    private string? genero;
    private string? isbn;
    private int? anoPublicacao;

    public bool Completo => id.HasValue && titulo != null && autor != null && genero != null && isbn != null && anoPublicacao.HasValue;

    public void ExibirFormulario()
    {
        Console.WriteLine($"ID                 : {id}");
        Console.WriteLine($"TÍTULO             : {titulo}");
        Console.WriteLine($"AUTOR              : {autor}");
        Console.WriteLine($"GÊNERO             : {genero}");
        Console.WriteLine($"ISBN               : {isbn}");
        Console.WriteLine($"ANO DE PUBLICAÇÃO  : {anoPublicacao}");
    }

    public bool PreencherProximoCampo()
    {
        Console.WriteLine();

        if (!id.HasValue)
        {
            var valor = LeitorConsole.LerNumero("ID: ");
            if (valor == 0) return false;
            _livroServico.ValidarIdLivro(valor, deveSerUnico: true);
            id = valor;
        }
        else if (string.IsNullOrWhiteSpace(titulo))
        {
            var valor = LeitorConsole.LerTexto("TÍTULO: ");
            if (valor == "0") return false;
            _livroServico.ValidarCampoObrigatorio(valor, "TÍTULO");
            titulo = valor;
        }
        else if (string.IsNullOrWhiteSpace(autor))
        {
            var valor = LeitorConsole.LerTexto("AUTOR: ");
            if (valor == "0") return false;
            _livroServico.ValidarCampoObrigatorio(valor, "AUTOR");
            autor = valor;
        }
        else if (string.IsNullOrWhiteSpace(genero))
        {
            var valor = LeitorConsole.LerTexto("GÊNERO: ");
            if (valor == "0") return false;
            _livroServico.ValidarCampoObrigatorio(valor, "GÊNERO");
            genero = valor;
        }
        else if (string.IsNullOrWhiteSpace(isbn))
        {
            var valor = LeitorConsole.LerTexto("ISBN: ");
            if (valor == "0") return false;
            _livroServico.ValidarCampoObrigatorio(valor, "ISBN");
            _livroServico.ValidarISBN(valor);
            isbn = valor;
        }
        else if (!anoPublicacao.HasValue)
        {
            var valor = LeitorConsole.LerNumero("ANO DE PUBLICAÇÃO: ");
            if (valor == 0) return false;
            _livroServico.ValidarAnoPublicacao(valor);
            anoPublicacao = valor;
        }

        return true;
    }

    public void PreencherCamposComDadosExistentes(Livro livro)
    {
        id = livro.Id;
        titulo = livro.Titulo;
        autor = livro.Autor;
        genero = livro.Genero;
        isbn = livro.ISBN;
        anoPublicacao = livro.AnoPublicacao;
    }

    public Livro ConverterParaLivro()
    {
        return new Livro
        {
            Id = id!.Value,
            Titulo = titulo!,
            Autor = autor!,
            Genero = genero!,
            ISBN = isbn!,
            AnoPublicacao = anoPublicacao!.Value
        };
    }
}