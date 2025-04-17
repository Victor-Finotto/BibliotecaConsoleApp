using BibliotecaConsoleApp.Entidades;
using BibliotecaConsoleApp.Repositorios;
using BibliotecaConsoleApp.Servicos;
using BibliotecaConsoleApp.Apresentacao.Menus.Submenus;
using BibliotecaConsoleApp.Sessao;

var sessaoContexto = new SessaoContexto();


var livroRepositorio = new LivroRepositorio();
var livroServico = new LivroServico(livroRepositorio);
var livroMenu = new LivroMenu(livroServico);

var usuarioRepositorio = new UsuarioRepositorio();
var usuarioServico = new UsuarioServico(usuarioRepositorio);
var usuarioMenu = new UsuarioMenu(usuarioServico, sessaoContexto);

var emprestimoRepositorio = new EmprestimoRepositorio();
var emprestimoServico = new EmprestimoServico(emprestimoRepositorio);
var emprestimoMenu = new EmprestimoMenu(emprestimoServico, livroServico, sessaoContexto);


MenuPrincipal menuPrincipal = new(sessaoContexto, livroMenu, usuarioMenu, emprestimoMenu);
menuPrincipal.ExibirMenu();
