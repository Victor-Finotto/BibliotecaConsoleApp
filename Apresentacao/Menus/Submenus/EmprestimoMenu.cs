using BibliotecaConsoleApp.Entidades;
using BibliotecaConsoleApp.Servicos;
using BibliotecaConsoleApp.Sessao;
using BibliotecaConsoleApp.Utils;

namespace BibliotecaConsoleApp.Apresentacao.Menus.Submenus
{
    public class EmprestimoMenu
    {
        private readonly EmprestimoServico _emprestimoServico;
        private readonly LivroServico _livroServico;
        private readonly SessaoContexto _sessao;

        public EmprestimoMenu(EmprestimoServico emprestimoServico, LivroServico livroServico, SessaoContexto sessao)
        {
            _emprestimoServico = emprestimoServico;
            _livroServico = livroServico;
            _sessao = sessao;
        }

        public void ExibirMenu()
        {
            while (true)
            {
                Console.Clear();
                MenuRenderer.ExibirMenu("GERENCIAR EMPRÉSTIMO", new[]
                {
                    "[1] REGISTRAR EMPRÉSTIMO",
                    "[2] LISTAR EMPRÉSTIMOS",
                    "[3] ATUALIZAR EMPRÉSTIMO",
                    "[4] REMOVER EMPRÉSTIMO",
                    "[0] SAIR"
                }, _sessao.UsuarioSelecionado);

                int opcao = LeitorConsole.LerNumero("\nDIGITE A OPÇÃO: ");

                switch (opcao)
                {
                    case 1: RegistrarEmprestimo(); break;
                    case 2: ListarEmprestimos(); break;
                    case 3: AtualizarEmprestimo(); break;
                    case 4: RemoverEmprestimo(); break;
                    case 0: return;
                }
            }
        }

        private void RegistrarEmprestimo()
        {
            Console.Clear();

            try
            {
                if (_sessao.UsuarioSelecionado == null)
                    throw new Exception("NENHUM USUÁRIO SELECIONADO. SELECIONE UM USUÁRIO ANTES DE REGISTRAR UM EMPRÉSTIMO.");

                int idLivro = LeitorConsole.LerNumero("ID DO LIVRO A SER EMPRESTADO: ");

                var livro = _livroServico.BuscarPorId(idLivro);
                if (livro == null)
                {
                    throw new Exception("LIVRO NÃO ENCONTRADO. POR FAVOR, DIGITE UM ID VÁLIDO.");
                }

                int idEmprestimo = GerarIdEmprestimo();

                var emprestimo = new Emprestimo
                {
                    Id = idEmprestimo,
                    IdUsuario = _sessao.UsuarioSelecionado.Id,
                    IdLivro = idLivro,
                    DataEmprestimo = DateTime.Today,
                    DataPrevistaDevolucao = DateTime.Today.AddDays(7),
                    Devolvido = false
                };

                _emprestimoServico.Adicionar(emprestimo);

                FormatadorMensagem.ExibirLivroFormatado(livro);
                FormatadorMensagem.ExibirMensagemSucesso($"EMPRÉSTIMO REGISTRADO COM SUCESSO PARA O USUÁRIO!");
            }
            catch (Exception ex)
            {
                FormatadorMensagem.ExibirMensagemErro(ex);
            }
        }

        private void ListarEmprestimos()
        {
            Console.Clear();

            try
            {
                var emprestimos = _emprestimoServico.ListarTodos();

                if (emprestimos.Count == 0)
                {
                    FormatadorMensagem.ExibirMensagemInfo("NENHUM EMPRÉSTIMO CADASTRADO");
                    return;
                }

                foreach (var emprestimo in emprestimos)
                {
                    var livro = _livroServico.BuscarPorId(emprestimo.IdLivro);
                    FormatadorMensagem.ExibirEmprestimoFormatado(emprestimo);
                }
            }
            catch (Exception ex)
            {
                FormatadorMensagem.ExibirMensagemErro(ex);
            }
        }

        private void AtualizarEmprestimo()
        {
            Console.Clear();

            var emprestimoParaAtualizar = _emprestimoServico.BuscarPorId(
                LeitorConsole.LerNumero("INFORME O ID DO EMPRÉSTIMO QUE DESEJA ATUALIZAR | [0] PARA CANCELAR: ")
            );

            if (emprestimoParaAtualizar == null)
            {
                FormatadorMensagem.ExibirMensagemErro(new Exception("EMPRÉSTIMO NÃO ENCONTRADO OU CANCELADO PELO USUÁRIO."));
                return;
            }

            while (true)
            {
                Console.Clear();
                Console.WriteLine("DADOS ATUAIS DO EMPRÉSTIMO:");

                Console.ForegroundColor = ConsoleColor.Yellow;
                FormatadorMensagem.ExibirEmprestimoFormatado(emprestimoParaAtualizar);

                Console.WriteLine("\nESCOLHA O CAMPO QUE DESEJA ALTERAR:");

                Console.WriteLine("[1] LIVRO");
                Console.WriteLine("[2] DATA PREVISTA DE DEVOLUÇÃO");
                Console.WriteLine("[0] SALVAR E SAIR");

                int opcao = LeitorConsole.LerNumero("\nOPÇÃO: ");

                if (opcao == 0)
                {
                    break;
                }

                Console.Clear();
                Console.WriteLine("DADOS ATUAIS DO EMPRÉSTIMO:");

                Console.ForegroundColor = ConsoleColor.Yellow;
                FormatadorMensagem.ExibirEmprestimoFormatado(emprestimoParaAtualizar);

                switch (opcao)
                {
                    case 1:
                        try
                        {
                            int idLivro = LeitorConsole.LerNumero("DIGITE O NOVO ID DO LIVRO: ");
                            var livro = _livroServico.BuscarPorId(idLivro);
                            if (livro == null)
                            {
                                FormatadorMensagem.ExibirMensagemErro(new Exception("LIVRO NÃO ENCONTRADO. POR FAVOR, DIGITE UM ID VÁLIDO."));
                                continue;
                            }
                            emprestimoParaAtualizar.IdLivro = idLivro;
                        }
                        catch (Exception ex)
                        {
                            FormatadorMensagem.ExibirMensagemErro(ex);
                            continue;
                        }
                        break;

                    case 2:
                        try
                        {
                            DateTime novaDataPrevistaDevolucao = LeitorConsole.LerData("DIGITE A NOVA DATA PREVISTA DE DEVOLUÇÃO (dd/MM/yyyy): ");
                            if (novaDataPrevistaDevolucao < DateTime.Today)
                            {
                                FormatadorMensagem.ExibirMensagemErro(new Exception("A NOVA DATA NÃO PODE SER ANTERIOR À DATA ATUAL."));
                                continue;
                            }
                            emprestimoParaAtualizar.DataPrevistaDevolucao = novaDataPrevistaDevolucao;
                        }
                        catch (Exception ex)
                        {
                            FormatadorMensagem.ExibirMensagemErro(ex);
                            continue;
                        }
                        break;
                }
            }

            try
            {
                _emprestimoServico.Atualizar(emprestimoParaAtualizar);
                FormatadorMensagem.ExibirMensagemSucesso("EMPRÉSTIMO ATUALIZADO COM SUCESSO!");
            }
            catch (Exception ex)
            {
                FormatadorMensagem.ExibirMensagemErro(ex);
            }
        }

        private void RemoverEmprestimo()
        {
            Console.Clear();

            try
            {
                int idEmprestimo = LeitorConsole.LerNumero("ID DO EMPRÉSTIMO A SER REMOVIDO: ");

                var emprestimoExistente = _emprestimoServico.BuscarPorId(idEmprestimo);

                Console.ForegroundColor = ConsoleColor.Red;
                FormatadorMensagem.ExibirEmprestimoFormatado(emprestimoExistente);
                Console.ResetColor();

                if (emprestimoExistente == null)
                    throw new Exception("EMPRÉSTIMO NÃO ENCONTRADO.");

                FormatadorMensagem.ConfirmarOperacao();
                _emprestimoServico.Remover(idEmprestimo);

                FormatadorMensagem.ExibirMensagemSucesso("EMPRÉSTIMO REMOVIDO COM SUCESSO!");
            }
            catch (Exception ex)
            {
                FormatadorMensagem.ExibirMensagemErro(ex);
            }
        }

        private int GerarIdEmprestimo()
        {
            var todos = _emprestimoServico.ListarTodos();
            return (todos.Count == 0) ? 1 : todos.Max(e => e.Id) + 1;
        }
    }
}
