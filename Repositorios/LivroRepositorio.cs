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

        public void Atualizar(Livro entidade)
        {
            // TODO: Implementar lógica de atualização de dados do livro
            // Essa lógica será feita no serviço e chamada aqui
            throw new NotImplementedException();
        }

        public Livro? BuscarPorId(int id)
        {
            Livro? livroProcurado = Livros.Find(l => l.Id == id);

            if (!(livroProcurado is null))
            {
                return livroProcurado;
            }
            else
            {
                throw new Exception("LIVRO NÃO ENCONTRADO.");
            }
        }

        public List<Livro> ListarTodos()
        {
            return Livros;
        }

        public void Remover(int id)
        {
            Livro? livroParaExclusao = BuscarPorId(id);
            
            if (!(livroParaExclusao is null))
            {
                Livros.Remove(livroParaExclusao);
            }
            else
            {
                throw new Exception("LIVRO NÃO ENCONTRADO.");
            }
        }
    }
}
