using BibliotecaConsoleApp.Entidades;
using BibliotecaConsoleApp.Repositorios;

namespace BibliotecaConsoleApp.Servicos
{
    class LivroServico(LivroRepositorio livroRepositorio)
    {
        private readonly LivroRepositorio _livroRepositorio = livroRepositorio;

        public void AdicionarLivro(Livro livro)
        {
            if (_livroRepositorio.ListarTodos().Any(l => l.Id == livro.Id))
            {
                throw new Exception("JÁ EXISTE UM LIVRO COM ESSE ID.");
            }

            ValidarCampoObrigatorio(livro.Titulo, "TÍTULO");
            ValidarCampoObrigatorio(livro.Autor, "AUTOR");
            ValidarCampoObrigatorio(livro.Genero, "GÊNERO");
            ValidarCampoObrigatorio(livro.ISBN, "ISBN");

            if (livro.AnoPublicacao < 1000 || livro.AnoPublicacao > DateTime.Now.Year)
            {
                throw new Exception("ANO DE PUBLICAÇÃO INVÁLIDO");
            }

            if (_livroRepositorio.ListarTodos().Any(l => l.ISBN == livro.ISBN))
            {
                throw new Exception("JÁ EXISTE UM LIVRO CADASTRADO COM ESSE ISBN.");
            }

            PadronizarInformacoesDoLivro(livro);

            _livroRepositorio.Adicionar(livro);
        }

        public void ValidarCampoObrigatorio(string valor, string nomeCampo)
        {
            if (string.IsNullOrEmpty(valor))
            {
                throw new Exception($"O {nomeCampo} DO LIVRO NÃO PODE SER NULO OU VAZIO.");
            }
        }

        public void PadronizarInformacoesDoLivro(Livro livro)
        {
            livro.Titulo = livro.Titulo.ToUpper();
            livro.Autor = livro.Autor.ToUpper();
            livro.Genero = livro.Genero.ToUpper();
        }

        public List<Livro> ListarTodos()
        {
            return _livroRepositorio.ListarTodos();
        }
    }
}
