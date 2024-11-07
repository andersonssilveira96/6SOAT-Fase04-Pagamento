using Application.DTOs.Pagamentos;
using Domain.Entities;

namespace Application.UseCase.Pagamentos
{
    public interface IPagamentoUseCase
    {
        Task<Pagamento> GerarPagamento(GerarPagamentoDto pagamento);
        Task<Pagamento> AtualizarPagamento(AtualizarPagamentoDto pagamento);
    }
}
