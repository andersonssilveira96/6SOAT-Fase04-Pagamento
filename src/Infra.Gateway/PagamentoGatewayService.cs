using Application.DTOs.Pagamentos;
using Domain.Entities;
using Domain.Interfaces;

namespace Infra.Gateway
{
    public class PagamentoGatewayService : IPagamentoGatewayService
    {      
        public async Task<Pagamento> EnviarPagamento(long pedidoId, decimal valorTotal)
        {
            return await Task.FromResult(new Pagamento(Guid.NewGuid(), "QRCode", pedidoId, valorTotal));
        }
    }
}
