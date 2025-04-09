using BibliotecaConsoleApp.Entidades;
using BibliotecaConsoleApp.Interfaces;
using BibliotecaConsoleApp.Utils;

namespace BibliotecaConsoleApp.Repositorios
{
    class LivroRepositorio : IRepositorio<Livro>
    {
        public List<Livro> Livros { get; private set; }

        public LivroRepositorio()
        {
            Livros = ManipuladorJson.CarregarLista<Livro>(Caminhos.Livros);
        }

        public void Adicionar(Livro entidade)
        {
            if (Livros.Any(l => l.Id == entidade.Id))
                throw new Exception("LIVRO JÁ EXISTENTE.");

            Livros.Add(entidade);
            Salvar();
        }

        public void Atualizar(Livro entidade)
        {
            int indiceLivroParaEditar = Livros.FindIndex(l => entidade.Id == l.Id);

            if (indiceLivroParaEditar >= 0)
            {
                Livros[indiceLivroParaEditar] = entidade;
                Salvar();
            }
            else
            {
                throw new Exception("LIVRO NÃO ENCONTRADO.");
            }
        }

        public Livro? BuscarPorId(int id)
        {
            return Livros.Find(l => l.Id == id) ?? throw new Exception("LIVRO NÃO ENCONTRADO.");
        }

        public List<Livro> ListarTodos()
        {
            return Livros;
        }

        public void Remover(int id)
        {
            Livro livroParaExclusao = BuscarPorId(id);
            Livros.Remove(livroParaExclusao);
            Salvar();
        }

        private void Salvar()
        {
            ManipuladorJson.SalvarLista(Caminhos.Livros, Livros);
        }
    }
}
