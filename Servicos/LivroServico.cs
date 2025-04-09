using BibliotecaConsoleApp.Entidades;
using BibliotecaConsoleApp.Interfaces;
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

        public void RemoverLivro(int id)
        {
            ValidarIdLivro(id);

            _livroRepositorio.Remover(id);
        }

        public void AtualizarLivro(Livro livroAtualizado)
        {
            var livroExistente = _livroRepositorio.BuscarPorId(livroAtualizado.Id);
            if (livroExistente == null)
                throw new Exception("LIVRO NÃO ENCONTRADO.");

            if (_livroRepositorio.ListarTodos().Any(l => l.ISBN == livroAtualizado.ISBN && l.Id != livroAtualizado.Id))
                throw new Exception("JÁ EXISTE UM LIVRO COM ESSE ISBN.");

            _livroRepositorio.Atualizar(livroAtualizado);
        }

        public Livro? BuscarLivroPorId(int id)
        {
            var livro = _livroRepositorio.BuscarPorId(id);

            if (livro == null)
                throw new Exception("LIVRO NÃO ENCONTRADO.");

            return livro;
        }

        public void ValidarCampoObrigatorio(string valor, string nomeCampo)
        {
            if (string.IsNullOrEmpty(valor))
            {
                throw new Exception($"O {nomeCampo} DO LIVRO NÃO PODE SER NULO OU VAZIO.");
            }
        }

        public void ValidarIdLivro(int id)
        {
            if (id <= 0)
            {
                throw new Exception("ID INVÁLIDO. O ID DEVE SER MAIOR QUE ZERO.");
            }

            var livro = _livroRepositorio.BuscarPorId(id);
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
