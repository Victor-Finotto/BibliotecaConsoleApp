using BibliotecaConsoleApp.Entidades;
using BibliotecaConsoleApp.Servicos;
using BibliotecaConsoleApp.Utils;

namespace BibliotecaConsoleApp.Apresentacao.Formularios
{
    public class UsuarioFormState
    {
        private readonly UsuarioServico _usuarioServico;

        public UsuarioFormState(UsuarioServico usuarioServico)
        {
            _usuarioServico = usuarioServico;
        }

        private int? id;
        private string? nome;
        private string? email;

        public bool Completo => id.HasValue && nome != null && email != null;

        public void ExibirFormulario()
        {
            Console.WriteLine($"ID                 : {id}");
            Console.WriteLine($"NOME               : {nome}");
            Console.WriteLine($"E-MAIL             : {email}");
        }

        public bool PreencherProximoCampo()
        {
            Console.WriteLine();

            if (!id.HasValue)
            {
                var valor = LeitorConsole.LerNumero("ID: ");
                if (valor == 0) return false;
                _usuarioServico.ValidarId(valor, deveSerUnico: true);
                id = valor;
            }
            else if (string.IsNullOrWhiteSpace(nome))
            {
                var valor = LeitorConsole.LerTexto("NOME: ");
                if (valor == "0") return false;
                _usuarioServico.ValidarCampoObrigatorio(valor, "NOME");
                nome = valor;
            }
            else if (string.IsNullOrWhiteSpace(email))
            {
                var valor = LeitorConsole.LerTexto("E-MAIL: ");
                if (valor == "0") return false;
                _usuarioServico.ValidarCampoObrigatorio(valor, "E-MAIL");
                email = valor;
            }

            return true;
        }

        public void PreencherCamposComDadosExistentes(Usuario usuario)
        {
            id = usuario.Id;
            nome = usuario.Nome;
            email = usuario.Email;

        }

        public Usuario ConverterParaUsuario()
        {
            return new Usuario
            {
                Id = id!.Value,
                Nome = nome!,
                Email = email!,
            };
        }
    }
}
