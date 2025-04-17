using BibliotecaConsoleApp.Entidades;
using BibliotecaConsoleApp.Interfaces;
using BibliotecaConsoleApp.Utils;

namespace BibliotecaConsoleApp.Repositorios
{
    public class EmprestimoRepositorio : IRepositorio<Emprestimo>
    {
        public List<Emprestimo> Emprestimos { get; set; } = [];

        public EmprestimoRepositorio()
        {
            Emprestimos = ManipuladorJson.CarregarLista<Emprestimo>(Caminhos.Emprestimos);
        }

        public void Adicionar(Emprestimo entidade)
        {
            Emprestimos.Add(entidade);
        }

        public void Atualizar(Emprestimo entidade)
        {
            int indiceEmprestimoParaEditar = Emprestimos.FindIndex(l => entidade.Id == l.Id);

            if (indiceEmprestimoParaEditar >= 0)
            {
                Emprestimos[indiceEmprestimoParaEditar] = entidade;
                Salvar();
            }
            else
            {
                throw new Exception("EMPRÉSTIMO NÃO ENCONTRADO.");
            }
        }

        public Emprestimo? BuscarPorId(int id)
        {
            return Emprestimos.Find(e => e.Id == id);
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

        private void Salvar()
        {
            ManipuladorJson.SalvarLista(Caminhos.Emprestimos, Emprestimos);
        }
    }
}

