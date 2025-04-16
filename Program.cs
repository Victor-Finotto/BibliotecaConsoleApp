using BibliotecaConsoleApp.Entidades;
using BibliotecaConsoleApp.Repositorios;
using BibliotecaConsoleApp.Servicos;
using BibliotecaConsoleApp.Apresentacao.Menus.Submenus;
using BibliotecaConsoleApp.Sessao;

var livroRepositorio = new LivroRepositorio();
var livroServico = new LivroServico(livroRepositorio);
var livroMenu = new LivroMenu(livroServico);
var sessaoContexto = new SessaoContexto(); 


var usuarioRepositorio = new UsuarioRepositorio();
var usuarioServico = new UsuarioServico(usuarioRepositorio);
var usuarioMenu = new UsuarioMenu(usuarioServico, sessaoContexto);


MenuPrincipal menuPrincipal = new(sessaoContexto, livroMenu, usuarioMenu);
menuPrincipal.ExibirMenu();
