
using BibliotecaConsoleApp.Entidades;
using BibliotecaConsoleApp.Menus;
using BibliotecaConsoleApp.Repositorios;
using BibliotecaConsoleApp.Servicos;

LivroRepositorio livroRepositorio = new();
LivroServico livroServico = new(livroRepositorio);
MenuBiblioteca menuBiblioteca = new(livroServico);

menuBiblioteca.Executar();

