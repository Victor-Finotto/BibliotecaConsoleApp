using BibliotecaConsoleApp.Interfaces;

namespace BibliotecaConsoleApp.Entidades
{
    public class Emprestimo : IEntidade
    {
        public required int Id { get; init; }
        public required int IdUsuario { get; set; }
        public required int IdLivro { get; set; }
        public required DateTime DataEmprestimo { get; set; }
        public DateTime? DataDevolucao { get; set; }
        public DateTime DataPrevistaDevolucao { get; set; }
        public bool Devolvido { get; set; }
    }
}
