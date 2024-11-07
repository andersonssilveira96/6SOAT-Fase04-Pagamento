using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IPagamentoGatewayService
    {
        Task<Pagamento> EnviarPagamento(long pedidoId);
    }
}
