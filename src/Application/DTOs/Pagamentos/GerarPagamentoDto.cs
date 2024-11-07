namespace Application.DTOs.Pagamentos
{
    public class GerarPagamentoDto
    {
        public long PedidoId { get; set; }
        public decimal ValorTotal { get; set; }
    }
}
