using BibliotecaConsoleApp.Entidades;
using BibliotecaConsoleApp.Interfaces;

namespace BibliotecaConsoleApp.Repositorios
{
    class LivroRepositorio : IRepositorio<Livro>
    {
        public List<Livro> Livros { get; set; } = [];
        public void Adicionar(Livro entidade)
        {
            if (Livros.Contains(entidade))
            {
                throw new Exception("LIVRO JÁ EXISTENTE.");
            }
        
            Livros.Add(entidade);
        }

        public void Atualizar(Livro entidade) //fazer commit
        {
            int indiceLivroParaEditar = Livros.FindIndex(l => entidade.Id == l.Id);

            if (indiceLivroParaEditar >= 0)
            {
                Livros[indiceLivroParaEditar] = entidade;
            }
            else
            {
                throw new Exception("LIVRO NÃO ENCONTRADO.");
            }
        }

        public Livro? BuscarPorId(int id)
        {
            Livro? livroProcurado = Livros.Find(l => l.Id == id);

            return livroProcurado ?? throw new Exception("LIVRO NÃO ENCONTRADO.");
        }

        public List<Livro> ListarTodos()
        {
            return Livros;
        }

        public void Remover(int id)
        {
            Livro? livroParaExclusao = BuscarPorId(id);

            Livros.Remove(livroParaExclusao!);
        }
    }
}
