using BibliotecaConsoleApp.Apresentacao.Formularios;
using BibliotecaConsoleApp.Entidades;
using BibliotecaConsoleApp.Servicos;
using BibliotecaConsoleApp.Sessao;
using BibliotecaConsoleApp.Utils;

namespace BibliotecaConsoleApp.Apresentacao.Menus.Submenus
{
    public class UsuarioMenu
    {
        private readonly UsuarioServico _usuarioServico;
        private readonly SessaoContexto _sessao;

        public UsuarioMenu(UsuarioServico usuarioServico, SessaoContexto sessao)
        {
            _usuarioServico = usuarioServico;
            _sessao = sessao;
        }

        public void ExibirMenu()
        {
            while (true)
            {
                Console.Clear();
                MenuRenderer.ExibirMenu("GERENCIAR USUÁRIOS", new string[]
                {
                    "[1] ADICIONAR USUÁRIO",
                    "[2] EXCLUIR USUÁRIO",
                    "[3] EDITAR USUÁRIO",
                    "[4] LISTAR TODOS OS USUÁRIO",
                    "[5] CONSULTAR USUÁRIO POR ID",
                    "[6] SELECIONAR USUÁRIO",
                    "[0] SAIR"
                }, _sessao.UsuarioSelecionado);

                int opcao = LeitorConsole.LerNumero("\nDIGITE A OPÇÃO: ");

                switch (opcao)
                {
                    case 1: AdicionarUsuario(); break;
                    case 2: ExcluirUsuario(); break;
                    case 3: EditarUsuario(); break;
                    case 4: ExibirUsuariosCadastrados(); break;
                    case 5: ExibirUsuarioPorId(); break;
                    case 6: SelecionarUsuario(); break;
                    case 0: return;
                    default: break;
                }
            }
        }

        public void AdicionarUsuario()
        {
            Console.Clear();
            Console.WriteLine("PREENCHA OS DADOS DO USUÁRIO QUE DESEJA CADASTRAR:");
            Console.WriteLine("[DIGITE 0 A QUALQUER MOMENTO PARA CANCELAR]\n");

            var usuario = ObterDadosDoUsuario();

            if (usuario == null)
            {
                FormatadorMensagem.ExibirMensagemInfo("OPERAÇÃO DE CADASTRO CANCELADA PELO USUÁRIO.");
                return;
            }

            try
            {
                _usuarioServico.Adicionar(usuario);
                FormatadorMensagem.ExibirMensagemSucesso("USUÁRIO CADASTRADO!");
            }
            catch (Exception ex)
            {
                FormatadorMensagem.ExibirMensagemErro(ex);
            }
        }

        private Usuario? ObterDadosDoUsuario()
        {
            var form = new UsuarioFormState(_usuarioServico);

            while (!form.Completo)
            {
                Console.Clear();
                Console.WriteLine("PREENCHA OS DADOS DO USUÁRIO:\n");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[DIGITE 0 A QUALQUER MOMENTO PARA CANCELAR]\n");
                Console.ResetColor();

                form.ExibirFormulario();

                try
                {
                    bool continuar = form.PreencherProximoCampo();
                    if (!continuar) return null;
                }
                catch (Exception ex)
                {
                    FormatadorMensagem.ExibirMensagemErro(ex);
                }
            }

            Console.Clear();
            Console.WriteLine("PREENCHA OS DADOS DO USUÁRIO:\n");
            form.ExibirFormulario();

            return form.ConverterParaUsuario();
        }

        private void ExcluirUsuario()
        {
            Console.Clear();

            var usuarioParaExcluir = BuscarUsuarioPorId("INFORME O ID DO USUÁRIO QUE DESEJA REMOVER | [0] PARA CANCELAR: ", "OPERAÇÃO DE EXCLUSÃO CANCELADA PELO USUÁRIO.");
            if (usuarioParaExcluir == null) return;

            try
            {
                Console.ForegroundColor = ConsoleColor.Red;
                FormatadorMensagem.ExibirUsuarioFormatado(usuarioParaExcluir);
                Console.ResetColor();

                FormatadorMensagem.ConfirmarOperacao();
                _usuarioServico.Remover(usuarioParaExcluir.Id);

                FormatadorMensagem.ExibirMensagemSucesso("USUÁRIO REMOVIDO!");
            }
            catch (Exception ex)
            {
                FormatadorMensagem.ExibirMensagemErro(ex);
            }
        }

        private void EditarUsuario()
        {
            Console.Clear();

            var usuarioParaEditar = BuscarUsuarioPorId("INFORME O ID DO USUÁRIO QUE DESEJA EDITAR | [0] PARA CANCELAR: ", "OPERAÇÃO DE EDIÇÃO CANCELADA PELO USUÁRIO.");
            if (usuarioParaEditar == null) return;

            var usuarioFormState = new UsuarioFormState(_usuarioServico);
            usuarioFormState.PreencherCamposComDadosExistentes(usuarioParaEditar);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("DADOS ATUAIS DO USUÁRIO:");

                Console.ForegroundColor = ConsoleColor.Yellow;
                FormatadorMensagem.ExibirUsuarioFormatado(usuarioParaEditar);

                Console.WriteLine("\nESCOLHA O CAMPO QUE DESEJA ALTERAR:");

                Console.WriteLine("[1] NOME");
                Console.WriteLine("[2] E-MAIL");
                Console.WriteLine("[0] SALVAR E SAIR");

                int opcao = LeitorConsole.LerNumero("\nOPÇÃO: ");

                if (opcao == 0)
                {
                    break;
                }

                Console.Clear();

                Console.WriteLine("DADOS ATUAIS DO LIVRO:");

                Console.ForegroundColor = ConsoleColor.Yellow;
                FormatadorMensagem.ExibirUsuarioFormatado(usuarioParaEditar);

                switch (opcao)
                {
                    case 1:
                        usuarioParaEditar.Nome = LeitorConsole.LerTexto("\nDIGITE O NOVO NOME: ");
                        break;
                    case 2:
                        usuarioParaEditar.Email = LeitorConsole.LerTexto("\nDIGITE O NOVO E-MAIL: ");
                        break;
                }
            }

            try
            {
                _usuarioServico.Atualizar(usuarioParaEditar);
                FormatadorMensagem.ExibirMensagemSucesso("USUÁRIO ATUALIZADO COM SUCESSO!");
            }
            catch (Exception ex)
            {
                FormatadorMensagem.ExibirMensagemErro(ex);
            }
        }

        private void ExibirUsuarioPorId()
        {
            Console.Clear();

            while (true)
            {
                Console.Clear();
                int idUsuarioProcurado = LeitorConsole.LerNumero("DIGITE O ID DO USUÁRIO QUE DESEJA BUSCAR | [0] PARA CANCELAR: ");

                if (idUsuarioProcurado == 0)
                {
                    FormatadorMensagem.ExibirMensagemInfo("OPERAÇÃO DE EXCLUSÃO CANCELADA PELO USUÁRIO.");
                    return;
                }

                try
                {
                    var usuarioEncontrado = _usuarioServico.BuscarPorId(idUsuarioProcurado);
                    FormatadorMensagem.ExibirUsuarioFormatado(usuarioEncontrado);
                    break;
                }
                catch (Exception ex)
                {
                    FormatadorMensagem.ExibirMensagemErro(ex);
                }
            }
        }

        private void ExibirUsuariosCadastrados()
        {
            Console.Clear();
            Console.WriteLine("USUÁRIOS CADASTRADOS:");

            var usuariosCadastrados = _usuarioServico.ListarTodos();

            if (!usuariosCadastrados.Any())
            {
                FormatadorMensagem.ExibirMensagemInfo("NENHUM USUÁRIO CADASTRADO.");
                return;
            }

            FormatadorMensagem.ExibirUsuariosFormatados(usuariosCadastrados);
        }

        private void SelecionarUsuario()
        {
            Console.Clear();

            var usuarioSelecionado = BuscarUsuarioPorId("DIGITE O ID DO USUÁRIO QUE DESEJA SELECIONAR | [0] PARA CANCELAR: ", "OPERAÇÃO DE SELEÇÃO CANCELADA PELO USUÁRIO.");
            if (usuarioSelecionado == null) return;

            _sessao.UsuarioSelecionado = usuarioSelecionado;
        }

        // Método Auxiliar para Buscar Usuário por ID
        private Usuario? BuscarUsuarioPorId(string mensagem, string mensagemCancelamento)
        {
            while (true)
            {
                Console.Clear();
                int idUsuario = LeitorConsole.LerNumero(mensagem);

                if (idUsuario == 0)
                {
                    FormatadorMensagem.ExibirMensagemInfo(mensagemCancelamento);
                    return null;
                }

                try
                {
                    var usuario = _usuarioServico.BuscarPorId(idUsuario);
                    return usuario;
                }
                catch (Exception ex)
                {
                    FormatadorMensagem.ExibirMensagemErro(ex);
                }
            }
        }
    }
}
