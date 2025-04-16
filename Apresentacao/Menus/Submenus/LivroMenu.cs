using BibliotecaConsoleApp.Entidades;
using BibliotecaConsoleApp.Servicos;
using BibliotecaConsoleApp.Utils;

namespace BibliotecaConsoleApp.Apresentacao.Menus.Submenus
{
    public class LivroMenu(LivroServico livroServico)
    {
        public readonly LivroServico _livroServico = livroServico;

        public void ExibirMenu()
        {
            while (true)
            {
                Console.Clear();
                MenuRenderer.ExibirMenu("GERENCIAR LIVROS", new string[]
                {
                "[1] ADICIONAR LIVRO",
                "[2] EXCLUIR LIVRO",
                "[3] EDITAR LIVRO",
                "[4] LISTAR TODOS OS LIVROS",
                "[5] CONSULTAR LIVRO POR ID",
                "[0] SAIR"
                });

                int opcao = LeitorConsole.LerNumero("\nDIGITE A OPÇÃO: ");

                switch (opcao)
                {
                    case 1: AdicionarLivro();  break;
                    case 2: ExcluirLivro();  break;
                    case 3: EditarLivro();  break;
                    case 4: ExibirLivrosCadastrados(); break;
                    case 5: ExibirLivroPorId();  break;
                    case 0: return;
                    default:break;
                }
            }
        }

        private void AdicionarLivro()
        {
            Console.Clear();
            Console.WriteLine("PREENCHA OS DADOS DO LIVRO QUE DESEJA CADASTRAR:");
            Console.WriteLine("[DIGITE 0 A QUALQUER MOMENTO PARA CANCELAR]\n");

            var livro = ObterDadosLivroDoUsuario();

            if (livro == null)
            {
                FormatadorMensagem.ExibirMensagemInfo("OPERAÇÃO DE CADASTRO CANCELADA PELO USUÁRIO.");
                return;
            }

            try
            {
                _livroServico.Adicionar(livro);
                FormatadorMensagem.ExibirMensagemSucesso("LIVRO CADASTRADO!");
            }
            catch (Exception ex)
            {
                FormatadorMensagem.ExibirMensagemErro(ex);
            }
        }

        private Livro? ObterDadosLivroDoUsuario()
        {
            var form = new LivroFormState(_livroServico);

            while (!form.Completo)
            {
                Console.Clear();
                Console.WriteLine("PREENCHA OS DADOS DO LIVRO:\n");

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
            Console.WriteLine("PREENCHA OS DADOS DO LIVRO:\n");
            form.ExibirFormulario();

            return form.ConverterParaLivro();
        }

        private void ExcluirLivro()
        {
            Console.Clear();

            Livro? livroParaExcluir = null;

            while (true)
            {
                Console.Clear();
                int idLivroParaExcluir = LeitorConsole.LerNumero("INFORME O ID DO LIVRO QUE DESEJA REMOVER | [0] PARA CANCELAR: ");

                if (idLivroParaExcluir == 0)
                {
                    FormatadorMensagem.ExibirMensagemInfo("OPERAÇÃO DE EXCLUSÃO CANCELADA PELO USUÁRIO.");
                    return;
                }

                try
                {
                    livroParaExcluir = _livroServico.BuscarPorId(idLivroParaExcluir);
                    break;
                }
                catch (Exception ex)
                {
                    FormatadorMensagem.ExibirMensagemErro(ex);
                }
            }

            try
            {
                Console.ForegroundColor = ConsoleColor.Red;
                FormatadorMensagem.ExibirLivroFormatado(livroParaExcluir);
                Console.ResetColor();

                FormatadorMensagem.ConfirmarOperacao();
                _livroServico.Remover(livroParaExcluir.Id);

                FormatadorMensagem.ExibirMensagemSucesso("LIVRO REMOVIDO!");
            }
            catch (Exception ex)
            {
                FormatadorMensagem.ExibirMensagemErro(ex);
            }
        }

        private void EditarLivro()
        {
            Console.Clear();
            Livro? livroParaEditar = null;

            while (true)
            {
                Console.Clear();
                int idLivroParaEditar = LeitorConsole.LerNumero("INFORME O ID DO LIVRO QUE DESEJA EDITAR | [0] PARA CANCELAR: ");

                if (idLivroParaEditar == 0)
                {
                    FormatadorMensagem.ExibirMensagemInfo("OPERAÇÃO DE EDIÇÃO CANCELADA PELO USUÁRIO.");
                    return;
                }

                try
                {
                    livroParaEditar = _livroServico.BuscarPorId(idLivroParaEditar);
                    break;
                }
                catch (Exception ex)
                {
                    FormatadorMensagem.ExibirMensagemErro(ex);
                }
            }

            var livroFormState = new LivroFormState(_livroServico);
            livroFormState.PreencherCamposComDadosExistentes(livroParaEditar);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("DADOS ATUAIS DO LIVRO:");

                Console.ForegroundColor = ConsoleColor.Yellow;
                FormatadorMensagem.ExibirLivroFormatado(livroParaEditar);

                Console.WriteLine("\nESCOLHA O CAMPO QUE DESEJA ALTERAR:");

                Console.WriteLine("[1] TÍTULO");
                Console.WriteLine("[2] AUTOR");
                Console.WriteLine("[3] GÊNERO");
                Console.WriteLine("[4] ISBN");
                Console.WriteLine("[5] ANO DE PUBLICAÇÃO");
                Console.WriteLine("[0] SALVAR E SAIR");

                int opcao = LeitorConsole.LerNumero("\nOPÇÃO: ");

                if (opcao == 0)
                {
                    break;
                }

                Console.Clear();

                Console.WriteLine("DADOS ATUAIS DO LIVRO:");

                Console.ForegroundColor = ConsoleColor.Yellow;
                FormatadorMensagem.ExibirLivroFormatado(livroParaEditar);

                switch (opcao)
                {
                    case 1:
                        livroParaEditar.Titulo = LeitorConsole.LerTexto("\nDIGITE O NOVO TÍTULO: ");
                        break;
                    case 2:
                        livroParaEditar.Autor = LeitorConsole.LerTexto("\nDIGITE O NOVO AUTOR: ");
                        break;
                    case 3:
                        livroParaEditar.Genero = LeitorConsole.LerTexto("\nDIGITE O NOVO GÊNERO: ");
                        break;
                    case 4:
                        while (true)
                        {
                            Console.Clear();
                            Console.WriteLine("DADOS ATUAIS DO LIVRO:");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            FormatadorMensagem.ExibirLivroFormatado(livroParaEditar);

                            string novoIsbn = LeitorConsole.LerTexto("\nDIGITE O NOVO ISBN: ");
                            try
                            {
                                _livroServico.ValidarISBN(novoIsbn);
                                livroParaEditar.ISBN = novoIsbn;
                                break;
                            }
                            catch (Exception ex)
                            {
                                FormatadorMensagem.ExibirMensagemErro(ex);
                            }
                        }
                        break;

                    case 5:
                        while (true)
                        {
                            Console.Clear();
                            Console.WriteLine("DADOS ATUAIS DO LIVRO:");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            FormatadorMensagem.ExibirLivroFormatado(livroParaEditar);

                            int ano = LeitorConsole.LerNumero("\nDIGITE O NOVO ANO DE PUBLICAÇÃO: ");
                            try
                            {
                                _livroServico.ValidarAnoPublicacao(ano);
                                livroParaEditar.AnoPublicacao = ano;
                                break;
                            }
                            catch (Exception ex)
                            {
                                FormatadorMensagem.ExibirMensagemErro(ex);
                            }
                        }
                        break;
                }
            }

            try
            {
                _livroServico.Atualizar(livroParaEditar);
                FormatadorMensagem.ExibirMensagemSucesso("LIVRO ATUALIZADO COM SUCESSO!");
            }
            catch (Exception ex)
            {
                FormatadorMensagem.ExibirMensagemErro(ex);
            }
        }

        private void ExibirLivroPorId()
        {
            Console.Clear();

            while (true)
            {
                Console.Clear();
                int idLivroProcurado = LeitorConsole.LerNumero("DIGITE O ID DO LIVRO QUE DESEJA BUSCAR | [0] PARA CANCELAR: ");

                if (idLivroProcurado == 0)
                {
                    FormatadorMensagem.ExibirMensagemInfo("OPERAÇÃO DE EXCLUSÃO CANCELADA PELO USUÁRIO.");
                    return;
                }

                try
                {
                    var livroEncontrado = _livroServico.BuscarPorId(idLivroProcurado);
                    FormatadorMensagem.ExibirLivroFormatado(livroEncontrado);
                    break;
                }
                catch (Exception ex)
                {
                    FormatadorMensagem.ExibirMensagemErro(ex);
                }
            }
        }

        private void ExibirLivrosCadastrados()
        {
            Console.Clear();
            Console.WriteLine("LIVROS CADASTRADOS:");

            var livrosCadastrados = _livroServico.ListarTodos();

            if (!livrosCadastrados.Any())
            {
                FormatadorMensagem.ExibirMensagemInfo("NENHUM LIVRO CADASTRADO.");
                return;
            }

            FormatadorMensagem.ExibirLivrosFormatados(livrosCadastrados);
        }

    }
}
