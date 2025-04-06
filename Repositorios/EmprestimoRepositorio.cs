using BibliotecaConsoleApp.Entidades;
using BibliotecaConsoleApp.Interfaces;

namespace BibliotecaConsoleApp.Repositorios
{
    class EmprestimoRepositorio : IRepositorio<Emprestimo>
    {
        public List<Emprestimo> Emprestimos { get; set; } = [];
        public void Adicionar(Emprestimo entidade)
        {
            Emprestimos.Add(entidade);
        }

        public void Atualizar(Emprestimo entidade)
        {
            // TODO: Implementar lógica de atualização de dados do Emprestimo
            // Essa lógica será feita no serviço e chamada aqui
            throw new NotImplementedException();
        }

        public Emprestimo? BuscarPorId(int id)
        {
            Emprestimo? registroProcurado = Emprestimos.Find(e => e.Id == id);

            return registroProcurado ?? throw new Exception("REGISTRO NÃO ENCONTRADO.");

        }

        public List<Emprestimo> ListarTodos()
        {
            return Emprestimos;
        }

        public void Remover(int id)
        {
            Emprestimo? registroParaExclusao = BuscarPorId(id);

            Emprestimos.Remove(registroParaExclusao!);
        }
    }
}
