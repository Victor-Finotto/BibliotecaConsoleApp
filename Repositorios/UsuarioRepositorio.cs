using BibliotecaConsoleApp.Entidades;
using BibliotecaConsoleApp.Interfaces;
using BibliotecaConsoleApp.Utils;

namespace BibliotecaConsoleApp.Repositorios
{
    public class UsuarioRepositorio : IRepositorio<Usuario>
    {
        public List<Usuario> Usuarios { get; private set; }

        public UsuarioRepositorio()
        {
            Usuarios = ManipuladorJson.CarregarLista<Usuario>(Caminhos.Usuarios);
        }

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
            int indiceUsuarioParaEditar = Usuarios.FindIndex(l => entidade.Id == l.Id);

            if (indiceUsuarioParaEditar >= 0)
            {
                Usuarios[indiceUsuarioParaEditar] = entidade;
                Salvar();
            }
            else
            {
                throw new Exception("USUÁRIO NÃO ENCONTRADO.");
            }
        }

        public Usuario? BuscarPorId(int id)
        {
            return Usuarios.Find(u => u.Id == id);
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

        private void Salvar()
        {
            ManipuladorJson.SalvarLista(Caminhos.Usuarios, Usuarios);
        }
    }
}
