using BibliotecaConsoleApp.Entidades;
using BibliotecaConsoleApp.Interfaces;
using BibliotecaConsoleApp.Repositorios;

namespace BibliotecaConsoleApp.Servicos
{
    public class UsuarioServico(UsuarioRepositorio usuarioRepositorio) : IRepositorio<Usuario>
    {
        private readonly UsuarioRepositorio _usuarioRepositorio = usuarioRepositorio;
        public void Adicionar(Usuario usuario)
        {
            ValidarUsuarioParaCadastro(usuario);
            PadronizarInformacoes(usuario);
            _usuarioRepositorio.Adicionar(usuario);
        }

        private void ValidarUsuarioParaCadastro(Usuario usuario)
        {
            ValidarId(usuario.Id);
            ValidarCampoObrigatorio(usuario.Nome, "NOME");
            ValidarCampoObrigatorio(usuario.Email, "E-MAIL");
        }

        public void ValidarCampoObrigatorio(string valor, string nomeCampo)
        {
            if (string.IsNullOrEmpty(valor))
            {
                throw new Exception($"O {nomeCampo} DO USUÁRIO NÃO PODE SER NULO OU VAZIO.");
            }
        }

        public void ValidarId(int id, bool deveSerUnico = true)
        {
            if (id <= 0)
                throw new Exception("ID INVÁLIDO. O ID DEVE SER MAIOR QUE ZERO.");

            if (deveSerUnico && _usuarioRepositorio.BuscarPorId(id) != null)
                throw new Exception("JÁ EXISTE UM USUÁRIO COM ESSE ID.");
        }

        public void PadronizarInformacoes(Usuario usuario)
        {
            usuario.Nome = usuario.Nome.ToUpper();
            usuario.Email = usuario.Email.ToUpper();
        }

        public void Remover(int id)
        {
            ValidarId(id, deveSerUnico: false);

            _usuarioRepositorio.Remover(id);
        }

        public void Atualizar(Usuario usuarioAtualizado)
        {
            var usuarioExistente = _usuarioRepositorio.BuscarPorId(usuarioAtualizado.Id);

            if (usuarioExistente == null)
                throw new Exception("USUÁRIO NÃO ENCONTRADO.");

            if (_usuarioRepositorio.ListarTodos().Any(usuario => usuario.Nome == usuarioAtualizado.Nome && usuario.Id != usuarioAtualizado.Id))
                throw new Exception("JÁ EXISTE UM USUÁRIO COM ESSE ID.");

            _usuarioRepositorio.Atualizar(usuarioAtualizado);
        }

        public Usuario? BuscarPorId(int id)
        {
            var usuario = _usuarioRepositorio.BuscarPorId(id);

            if (usuario == null)
                throw new Exception("USUÁRIO NÃO ENCONTRADO.");

            return usuario;
        }

        public List<Usuario> ListarTodos()
        {
            return _usuarioRepositorio.ListarTodos();
        }

    }
}
