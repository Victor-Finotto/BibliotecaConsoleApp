namespace BibliotecaConsoleApp.Interfaces
{
    interface IRepositorio<T> where T : IEntidade
    {
        void Adicionar(T entidade);
        void Remover(int id);
        T? BuscarPorId(int id);
        List<T> ListarTodos();
        void Atualizar(T entidade);

    }
}
