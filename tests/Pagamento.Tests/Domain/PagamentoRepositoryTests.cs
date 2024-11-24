using Infra.Data.Repositories;
using MongoDB.Driver;
using Moq;

namespace Pagamento.Tests
{
    public class PagamentoRepositoryTests
    {
        private readonly Mock<IMongoCollection<Domain.Entities.Pagamento>> _mockPagamentoCollection;
        private readonly Mock<IMongoClient> _mockMongoClient;
        private readonly PagamentoRepository _repository;

        public PagamentoRepositoryTests()
        {
            _mockPagamentoCollection = new Mock<IMongoCollection<Domain.Entities.Pagamento>>();
            _mockMongoClient = new Mock<IMongoClient>();
            var mockDatabase = new Mock<IMongoDatabase>();

            mockDatabase
                .Setup(db => db.GetCollection<Domain.Entities.Pagamento>(It.IsAny<string>(), null))
                .Returns(_mockPagamentoCollection.Object);

            _mockMongoClient
                .Setup(client => client.GetDatabase(It.IsAny<string>(), null))
                .Returns(mockDatabase.Object);

            _repository = new PagamentoRepository(_mockMongoClient.Object);
        }

        [Fact]
        public async Task Inserir_DeveAdicionarPagamentoNaColecao()
        {
            // Arrange
            var pagamento = new Domain.Entities.Pagamento(Guid.NewGuid(), string.Empty, 1, 100.00M);

            _mockPagamentoCollection
                .Setup(c => c.InsertOneAsync(It.IsAny<Domain.Entities.Pagamento>(), null, default))
                .Returns(Task.CompletedTask);

            // Act
            var resultado = await _repository.Inserir(pagamento);

            // Assert
            Assert.Equal(1, resultado.Id);
            _mockPagamentoCollection.Verify(c => c.InsertOneAsync(It.IsAny<Domain.Entities.Pagamento>(), null, default), Times.Once);
        }

        [Fact]
        public async Task Inserir_DeveLancarExcecaoSePagamentoForNulo()
        {
            // Arrange
            Domain.Entities.Pagamento pagamento = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _repository.Inserir(pagamento));
        }

        [Fact]
        public async Task Atualizar_DeveAtualizarPagamentoNaColecao()
        {
            // Arrange
            var pagamento = new Domain.Entities.Pagamento(Guid.NewGuid(), string.Empty, 1, 200.00M);
            pagamento.AtualizarStatus(true);

            _mockPagamentoCollection
                .Setup(c => c.UpdateOneAsync(
                    It.IsAny<FilterDefinition<Domain.Entities.Pagamento>>(),
                    It.IsAny<UpdateDefinition<Domain.Entities.Pagamento>>(),
                    null,
                    default))
                .ReturnsAsync(new UpdateResult.Acknowledged(1, 1, null));

            // Act
            var resultado = await _repository.Atualizar(pagamento);

            // Assert
            Assert.Equal("Pago", resultado.Status.ToString());
            _mockPagamentoCollection.Verify(c => c.UpdateOneAsync(
                It.IsAny<FilterDefinition<Domain.Entities.Pagamento>>(),
                It.IsAny<UpdateDefinition<Domain.Entities.Pagamento>>(),
                null,
                default), Times.Once);
        }

        [Fact]
        public async Task ObterPorPedidoId_DeveRetornarPagamentoCorreto()
        {
            // Arrange
            var id = 12345;
            var pagamentoEsperado = new Domain.Entities.Pagamento(Guid.NewGuid(), string.Empty, 1, 100.00M);


            var mockCursor = new Mock<IAsyncCursor<Domain.Entities.Pagamento>>();
            mockCursor
                .SetupSequence(cursor => cursor.MoveNextAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(true)
                .ReturnsAsync(false);

            mockCursor
                .Setup(cursor => cursor.Current)
                .Returns(new List<Domain.Entities.Pagamento> { pagamentoEsperado });

            _mockPagamentoCollection
                .Setup(c => c.FindAsync(
                    It.IsAny<FilterDefinition<Domain.Entities.Pagamento>>(),
                    It.IsAny<FindOptions<Domain.Entities.Pagamento>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockCursor.Object);

            // Act
            var resultado = await _repository.ObterPorPedidoId(id);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(pagamentoEsperado.PedidoId, resultado.PedidoId);
        }


        [Fact]
        public async Task ObterPorPedidoId_DeveRetornarNullSeNaoEncontrarPagamento()
        {
            // Arrange
            var mockCursor = new Mock<IAsyncCursor<Domain.Entities.Pagamento>>();
            mockCursor
                .Setup(c => c.MoveNext(It.IsAny<System.Threading.CancellationToken>()))
                .Returns(false);

            _mockPagamentoCollection
                    .Setup(c => c.FindAsync(
                        It.IsAny<FilterDefinition<Domain.Entities.Pagamento>>(),
                        It.IsAny<FindOptions<Domain.Entities.Pagamento>>(),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(mockCursor.Object);

            // Act
            var resultado = await _repository.ObterPorPedidoId(99999);

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        public async Task ObterPorGUID_DeveRetornarPagamentoCorreto()
        {
            // Arrange
            var numeroPagamento = Guid.NewGuid();
            var pagamentoEsperado = new Domain.Entities.Pagamento(Guid.NewGuid(), string.Empty, 1, 100.00M);

            var mockCursor = new Mock<IAsyncCursor<Domain.Entities.Pagamento>>();
            mockCursor
                .SetupSequence(cursor => cursor.MoveNextAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(true)
                .ReturnsAsync(false);

            mockCursor
                .Setup(cursor => cursor.Current)
                .Returns(new List<Domain.Entities.Pagamento> { pagamentoEsperado });

            _mockPagamentoCollection
                .Setup(c => c.FindAsync(
                    It.IsAny<FilterDefinition<Domain.Entities.Pagamento>>(),
                    It.IsAny<FindOptions<Domain.Entities.Pagamento>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockCursor.Object);

            // Act
            var resultado = await _repository.ObterPorGUID(numeroPagamento);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(pagamentoEsperado.NumeroPagamento, resultado.NumeroPagamento);
        }
    }

}
