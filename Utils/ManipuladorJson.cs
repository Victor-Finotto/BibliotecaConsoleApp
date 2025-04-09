using System.Text.Json;

namespace BibliotecaConsoleApp.Utils
{
    public static class ManipuladorJson
    {
        public static void SalvarLista<T>(string caminho, List<T> dados)
        {
            var diretorio = Path.GetDirectoryName(caminho);
            if (!Directory.Exists(diretorio))
            {
                Directory.CreateDirectory(diretorio!);
            }

            var json = JsonSerializer.Serialize(dados, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(caminho, json);
        }

        public static List<T> CarregarLista<T>(string caminho)
        {
            if (!File.Exists(caminho))
                return new List<T>();

            try
            {
                var json = File.ReadAllText(caminho);
                return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"[ERRO] Falha ao carregar arquivo JSON em '{caminho}': {ex.Message}");
                return new List<T>();
            }
        }
    }
}
