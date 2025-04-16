using BibliotecaConsoleApp.Apresentacao.Menus.Submenus;
using BibliotecaConsoleApp.Servicos;
using BibliotecaConsoleApp.Sessao;
using BibliotecaConsoleApp.Utils;

public class MenuPrincipal
{
    private readonly SessaoContexto _sessaoContexto;
    private readonly LivroMenu _livroMenu;
    private readonly UsuarioMenu _usuarioMenu;

    public MenuPrincipal(SessaoContexto sessaoContexto, LivroMenu livroMenu, UsuarioMenu usuarioMenu)
    {
        _sessaoContexto = sessaoContexto;
        _livroMenu = livroMenu;
        _usuarioMenu = usuarioMenu;
    }

    public void ExibirMenu()
    {
        while (true)
        {
            Console.Clear();
            MenuRenderer.ExibirMenu("BIBLIOTECA", new string[]
            {
                "[1] GERENCIAR LIVROS",
                "[2] GERENCIAR USUÁRIOS",
                "[3] GERENCIAR EMPRÉSTIMOS",
                "[0] SAIR"
            });

            int opcao = LeitorConsole.LerNumero("\nDIGITE A OPÇÃO: ");

            switch (opcao)
            {
                case 1: _livroMenu.ExibirMenu(); break;
                case 2: _usuarioMenu.ExibirMenu(); break;
                case 3: // EmprestimoMenu.ExibirMenu(); break;
                case 0: return;
            }
        }
    }
}
