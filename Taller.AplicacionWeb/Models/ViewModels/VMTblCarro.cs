namespace Taller.AplicacionWeb.Models.ViewModels
{
    public class VMTblCarro
    {
        public string Placa { get; set; } = null!;
        public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public int? Ano { get; set; }
        public int IdCarro { get; set; }

    }
}
