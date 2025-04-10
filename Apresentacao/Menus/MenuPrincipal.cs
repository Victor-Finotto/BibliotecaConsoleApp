using BibliotecaConsoleApp.Apresentacao.Menus.Submenus;
using BibliotecaConsoleApp.Repositorios;
using BibliotecaConsoleApp.Servicos;
using BibliotecaConsoleApp.Utils;

public class MenuPrincipal
{
    public void ExibirMenu()
    {
        var livroRepositorio = new LivroRepositorio();
        var livroServico = new LivroServico(livroRepositorio);
        var livroMenu = new LivroMenu(livroServico);

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
                case 1: livroMenu.ExibirMenu(); break;
                case 2: //UsuarioMenu.ExibirMenu(); break;
                case 3: //EmprestimoMenu.ExibirMenu(); break;
                case 0: return;
            }
        }
    }
}