using BibliotecaConsoleApp.Interfaces;

namespace BibliotecaConsoleApp.Entidades
{
    class Livro : IEntidade
    {
        public required int Id { get; init; }
        public required string Titulo { get; set; }
        public required string Autor { get; set; }
        public required int AnoPublicacao { get; set; }
        public required string Genero { get; set; }
    }
}
