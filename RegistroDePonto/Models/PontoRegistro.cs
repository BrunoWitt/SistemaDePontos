public class PontoRegistro
{
    public int Id { get; set; }
    public int ColaboradorId { get; set; }
    public DateTime DataRegistro { get; set; }
    public TimeSpan Entrada { get; set; }
    public TimeSpan? InicioIntervalo { get; set; }
    public TimeSpan? FimIntervalo { get; set; }
    public TimeSpan? Saida { get; set; }
}