namespace BibliotecaConsoleApp.Utils
{
    public static class LeitorConsole
    {
        public static string LerTexto(string mensagem, bool obrigatorio = true)
        {
            Console.Write(mensagem);
            int linhaCampo = Console.CursorTop;
            int colunaCampo = Console.CursorLeft;
            Console.SetCursorPosition(colunaCampo, linhaCampo);

            while (true)
            {
                try
                {
                    LimparLinhaCampo(linhaCampo, colunaCampo);
                    var entrada = Console.ReadLine();

                    if (!obrigatorio || !string.IsNullOrWhiteSpace(entrada))
                        return entrada!.Trim();

                    throw new Exception("CAMPO OBRIGATÓRIO. TENTE NOVAMENTE");
                }
                catch (Exception ex)
                {
                    FormatadorMensagem.ExibirMensagemErro(ex, linhaCampo, colunaCampo);
                }
            }
        }

        public static int LerNumero(string mensagem)
        {
            Console.Write(mensagem);
            int linhaCampo = Console.CursorTop;
            int colunaCampo = Console.CursorLeft;
            Console.SetCursorPosition(colunaCampo, linhaCampo);


            while (true)
            {
                LimparLinhaCampo(linhaCampo, colunaCampo);
                var entrada = Console.ReadLine();

                try
                {
                    if (!int.TryParse(entrada, out int numero))
                        throw new Exception("ENTRADA INVÁLIDA. DIGITE UM NÚMERO.");

                    if (numero < 0)
                        throw new Exception("ENTRADA INVÁLIDA. DIGITE UM NÚMERO POSITIVO.");

                    return numero;
                }
                catch (Exception ex)
                {
                    FormatadorMensagem.ExibirMensagemErro(ex, linhaCampo, colunaCampo);
                }
            }
        }

        private static void LimparLinhaCampo(int linha, int coluna)
        {
            Console.SetCursorPosition(coluna, linha);
            Console.Write(new string(' ', Console.WindowWidth - coluna));
            Console.SetCursorPosition(coluna, linha);
        }
    }
}
