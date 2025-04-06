using BibliotecaConsoleApp.Entidades;
using BibliotecaConsoleApp.Interfaces;

namespace BibliotecaConsoleApp.Repositorios
{
    class UsuarioRepositorio : IRepositorio<Usuario>
    {
        public List<Usuario> Usuarios { get; set; } = [];
        public void Adicionar(Usuario entidade)
        {
            if (Usuarios.Contains(entidade))
            {
                throw new Exception("USUÁRIO JÁ EXISTENTE.");
            }

            Usuarios.Add(entidade);
        }

        public void Atualizar(Usuario entidade)
        {
            // TODO: Implementar lógica de atualização de dados do Usuário
            // Essa lógica será feita no serviço e chamada aqui
            throw new NotImplementedException();
        }

        public Usuario? BuscarPorId(int id)
        {
            Usuario? usuarioProcurado = Usuarios.Find(u => u.Id == id);

            return usuarioProcurado ?? throw new Exception("USUARIO NÃO ENCONTRADO.");
        }

        public List<Usuario> ListarTodos()
        {
            return Usuarios;
        }

        public void Remover(int id)
        {
            Usuario? usuarioParaExclusao = BuscarPorId(id);

            Usuarios.Remove(usuarioParaExclusao!);
        }
    }
}
