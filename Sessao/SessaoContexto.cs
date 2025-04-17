using BibliotecaConsoleApp.Entidades;

namespace BibliotecaConsoleApp.Sessao
{
    public class SessaoContexto
    {
        public Usuario? UsuarioSelecionado { get; set; }
        public string NomeUsuarioSelecionado { get; set; }

        public SessaoContexto()
        {
            if (UsuarioSelecionado != null)
            {
                NomeUsuarioSelecionado = UsuarioSelecionado?.Nome.ToUpper();
            }
        }
    }
}
