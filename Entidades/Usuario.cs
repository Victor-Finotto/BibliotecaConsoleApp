﻿using BibliotecaConsoleApp.Interfaces;

namespace BibliotecaConsoleApp.Entidades
{
    public class Usuario : IEntidade
    {
        public required int Id { get; init; }
        public required string Nome { get; set; }
        public required string Email { get; set; }
    }
}
