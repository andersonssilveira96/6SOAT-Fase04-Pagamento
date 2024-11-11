using Application.DTOs.Pagamentos;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Producer;
using Domain.Repositories;

namespace Application.UseCase.Pagamentos
{
    public class PagamentoUseCase : IPagamentoUseCase
    {
        private readonly IPagamentoGatewayService _gatewayService;
        private readonly IPagamentoRepository _pagamentoRepository;
        private readonly IMessageBrokerProducer _messageBrokerProducer;
        public PagamentoUseCase(IPagamentoGatewayService gatewayService, IPagamentoRepository pagamentoRepository, IMessageBrokerProducer messageBrokerProducer)
        {
            _gatewayService = gatewayService;
            _pagamentoRepository = pagamentoRepository;
            _messageBrokerProducer = messageBrokerProducer;
        }

        public async Task<Pagamento> AtualizarPagamento(AtualizarPagamentoDto atualizarPagamentoDto)
        {
            var pagamento = await _pagamentoRepository.ObterPorPedidoId(atualizarPagamentoDto.NumeroPedido);

            if (pagamento is null) throw new Exception("Número de pagamento inválido");

            pagamento.AtualizarStatus(atualizarPagamentoDto.Aprovado);

            await _pagamentoRepository.Atualizar(pagamento);

            await _messageBrokerProducer.SendMessageAsync(atualizarPagamentoDto.Aprovado ? "pedidos-pagos" : "pedidos-atualizados", new { Id = pagamento.PedidoId });

            return pagamento;
        }

        public async Task<Pagamento> GerarPagamento(GerarPagamentoDto pagamentoDto)
        {
            var pagamento = await _gatewayService.EnviarPagamento(pagamentoDto.PedidoId, pagamentoDto.ValorTotal);

            await _pagamentoRepository.Inserir(pagamento);

            return pagamento;
        }
    }
}
