using Application.DTOs.Pagamentos;
using Application.UseCase.Pagamentos;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Producer;
using Domain.Repositories;
using Moq;
using System.Drawing;

[Binding]
public class PagamentoUseCaseSteps
{
    private readonly Mock<IPagamentoGatewayService> _gatewayServiceMock = new();
    private readonly Mock<IPagamentoRepository> _pagamentoRepositoryMock = new();
    private readonly Mock<IMessageBrokerProducer> _messageBrokerProducerMock = new();

    private PagamentoUseCase _useCase;
    private Domain.Entities.Pagamento _resultado;
    private Exception _excecao;

    public PagamentoUseCaseSteps()
    {
        _useCase = new PagamentoUseCase(
            _gatewayServiceMock.Object,
            _pagamentoRepositoryMock.Object,
            _messageBrokerProducerMock.Object
        );
    }

    [Given(@"existe um pagamento com o número de pedido ""(.*)""")]
    public void DadoExisteUmPagamentoComONumeroDePedido(long numeroPedido)
    {
        var pagamento = new Domain.Entities.Pagamento(Guid.NewGuid(), string.Empty, numeroPedido, 10);
        _pagamentoRepositoryMock
            .Setup(repo => repo.ObterPorPedidoId(numeroPedido))
            .ReturnsAsync(pagamento);
    }

    [Given(@"não existe um pagamento com o número de pedido ""(.*)""")]
    public void DadoNaoExisteUmPagamentoComONumeroDePedido(long numeroPedido)
    {
        _pagamentoRepositoryMock
            .Setup(repo => repo.ObterPorPedidoId(numeroPedido))
            .ReturnsAsync((Domain.Entities.Pagamento)null);
    }

    [Given(@"o serviço de gateway retorna um pagamento para o pedido ""(.*)"" com valor ""(.*)""")]
    public void DadoOServicoDeGatewayRetornaUmPagamentoParaOPedidoComValor(long pedidoId, decimal valor)
    {
        var pagamento = new Domain.Entities.Pagamento(Guid.NewGuid(), string.Empty, pedidoId, valor);
        _gatewayServiceMock
            .Setup(service => service.EnviarPagamento(pedidoId, valor))
            .ReturnsAsync(pagamento);
    }

    [When(@"eu atualizo o pagamento para ""(.*)""")]
    public async Task QuandoEuAtualizoOPagamentoPara(string aprovado)
    {
        try
        {
            var dto = new AtualizarPagamentoDto
            {
                NumeroPedido = 12345,
                Aprovado = aprovado.ToLower() == "aprovado"
            };
            _resultado = await _useCase.AtualizarPagamento(dto);
        }
        catch (Exception ex)
        {
            _excecao = ex;
        }
    }

    [When(@"eu tento atualizar o pagamento")]
    public async Task QuandoEuTentoAtualizarOPagamento()
    {
        try
        {
            var dto = new AtualizarPagamentoDto
            {
                NumeroPedido = 99999,
                Aprovado = true
            };
            _resultado = await _useCase.AtualizarPagamento(dto);
        }
        catch (Exception ex)
        {
            _excecao = ex;
        }
    }

    [When(@"eu gero o pagamento")]
    public async Task QuandoEuGeroOPagamento()
    {
        var dto = new GerarPagamentoDto
        {
            PedidoId = 54321,
            ValorTotal = 100.00M
        };
        _resultado = await _useCase.GerarPagamento(dto);
    }

    [Then(@"o pagamento deve ser salvo com o status ""(.*)""")]
    public void EntaoOPagamentoDeveSerSalvoComOStatus(string status)
    {
        Assert.Equal(status, _resultado.Status.ToString());
        _pagamentoRepositoryMock.Verify(repo => repo.Atualizar(It.Is<Domain.Entities.Pagamento>(p => p.Status == Enum.Parse<PagamentoStatusEnum>(status))), Times.Once);
    }

    [Then(@"uma mensagem deve ser enviada para o tópico ""(.*)""")]
    public void EntaoUmaMensagemDeveSerEnviadaParaOTopico(string topico)
    {
        _messageBrokerProducerMock.Verify(producer => producer.SendMessageAsync(topico, It.IsAny<object>()), Times.Once);
    }

    [Then(@"deve ser lançada uma exceção com a mensagem ""(.*)""")]
    public void EntaoDeveSerLancadaUmaExcecaoComAMensagem(string mensagem)
    {
        Assert.NotNull(_excecao);
        Assert.Equal(mensagem, _excecao.Message);
    }

    [Then(@"o pagamento deve ser salvo no repositório")]
    public void EntaoOPagamentoDeveSerSalvoNoRepositorio()
    {
        _pagamentoRepositoryMock.Verify(repo => repo.Inserir(It.IsAny<Domain.Entities.Pagamento>()), Times.Once);
    }

    [Then(@"o pagamento retornado deve ter o pedido ""(.*)""")]
    public void EntaoOPagamentoRetornadoDeveTerOPedido(long pedidoId)
    {
        Assert.Equal(pedidoId, _resultado.PedidoId);
    }
}
