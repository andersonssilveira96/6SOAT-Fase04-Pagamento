using Domain.Entities;
using Domain.Interfaces;

namespace Infra.Gateway
{
    public class PagamentoGatewayService : IPagamentoGatewayService
    {      
        public async Task<Pagamento> EnviarPagamento(Pedido pedido)
        {
            return await Task.FromResult(new Pagamento(Guid.NewGuid(), "QRCode", pedido));
        }
    }
}
