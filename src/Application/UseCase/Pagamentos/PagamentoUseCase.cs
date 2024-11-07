using Application.DTOs.Pagamentos;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Repositories;

namespace Application.UseCase.Pagamentos
{
    public class PagamentoUseCase : IPagamentoUseCase
    {
        private readonly IPagamentoGatewayService _gatewayService;
        private readonly IPagamentoRepository _pagamentoRepository;
        public PagamentoUseCase(IPagamentoGatewayService gatewayService, IPagamentoRepository pagamentoRepository)
        {
            _gatewayService = gatewayService;
            _pagamentoRepository = pagamentoRepository;
        }

        public async Task<Pagamento> AtualizarPagamento(AtualizarPagamentoDto atualizarPagamentoDto)
        {
            var pagamento = await _pagamentoRepository.ObterPorPedidoId(atualizarPagamentoDto.NumeroPedido);

            if (pagamento is null) throw new Exception("Número de pagamento inválido");

            pagamento.AtualizarStatus(atualizarPagamentoDto.Aprovado);

            await _pagamentoRepository.Atualizar(pagamento);
          
            //var pedido = pagamento.Pedido;
            //pedido.AtualizarStatus(pagamento.PagamentoAprovado() ? StatusEnum.EmPreparacao : StatusEnum.Cancelado);

            //await _pedidoRepository.Atualizar(pedido);

            return pagamento;
        }

        public async Task<Pagamento> GerarPagamento(long pedidoId)
        {
            var pagamento = await _gatewayService.EnviarPagamento(pedidoId);

            await _pagamentoRepository.Inserir(pagamento);

            return pagamento;
        }
    }
}
