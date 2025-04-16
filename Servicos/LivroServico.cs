using BibliotecaConsoleApp.Entidades;
using BibliotecaConsoleApp.Interfaces;
using BibliotecaConsoleApp.Repositorios;

namespace BibliotecaConsoleApp.Servicos
{
    public class LivroServico(LivroRepositorio livroRepositorio) : IRepositorio<Livro>
    {
        private readonly LivroRepositorio _livroRepositorio = livroRepositorio;

        public void Adicionar(Livro livro)
        {
            ValidarLivroParaCadastro(livro);
            PadronizarInformacoesDoLivro(livro);
            _livroRepositorio.Adicionar(livro);
        }

        private void ValidarLivroParaCadastro(Livro livro)
        {
            ValidarIdLivro(livro.Id);
            ValidarCampoObrigatorio(livro.Titulo, "TÍTULO");
            ValidarCampoObrigatorio(livro.Autor, "AUTOR");
            ValidarCampoObrigatorio(livro.Genero, "GÊNERO");
            ValidarCampoObrigatorio(livro.ISBN, "ISBN");
            ValidarISBN(livro.ISBN);
            ValidarAnoPublicacao(livro.AnoPublicacao);
        }

        public void Remover(int id)
        {
            ValidarIdLivro(id, deveSerUnico: false);

            _livroRepositorio.Remover(id);
        }

        public void Atualizar(Livro livroAtualizado)
        {
            var livroExistente = _livroRepositorio.BuscarPorId(livroAtualizado.Id);
            
            if (livroExistente == null)
                throw new Exception("LIVRO NÃO ENCONTRADO.");

            if (_livroRepositorio.ListarTodos().Any(l => l.ISBN == livroAtualizado.ISBN && l.Id != livroAtualizado.Id))
                throw new Exception("JÁ EXISTE UM LIVRO COM ESSE ISBN.");

            _livroRepositorio.Atualizar(livroAtualizado);
        }

        public Livro? BuscarPorId(int id)
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

        public void ValidarIdLivro(int id, bool deveSerUnico = true)
        {
            if (id <= 0)
                throw new Exception("ID INVÁLIDO. O ID DEVE SER MAIOR QUE ZERO.");

            if (deveSerUnico && _livroRepositorio.BuscarPorId(id) != null)
                throw new Exception("JÁ EXISTE UM LIVRO COM ESSE ID.");
        }

        public void ValidarISBN(string isbn)
        {
            if (_livroRepositorio.ListarTodos().Any(l => l.ISBN == isbn))
                throw new Exception("JÁ EXISTE UM LIVRO COM ESSE ISBN.");
        }

        public void ValidarAnoPublicacao(int anoPublicacao)
        {
            if (anoPublicacao < 1000 || anoPublicacao > DateTime.Now.Year)
            {
                throw new Exception("ANO DE PUBLICAÇÃO INVÁLIDO");
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
