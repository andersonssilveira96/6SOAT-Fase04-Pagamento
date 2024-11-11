using Domain.Entities;
using Domain.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Xml.Linq;

namespace Infra.Data.Repositories
{
    public class PagamentoRepository : IPagamentoRepository
    {
        private readonly IMongoCollection<Pagamento> _pagamentoCollection;
        public PagamentoRepository(IMongoClient client)
        {
            var database = client.GetDatabase("TechChallengeDb");
            var collection = database.GetCollection<Pagamento>(nameof(Pagamento));

            _pagamentoCollection = collection;
        }
        public async Task<Pagamento> Inserir(Pagamento pagamento)
        {
            if (pagamento is null)
            {
                throw new ArgumentNullException(nameof(pagamento));
            }

            pagamento.IncrementarId(_pagamentoCollection.CountDocuments(FilterDefinition<Pagamento>.Empty)! + 1);
           
            await _pagamentoCollection.InsertOneAsync(pagamento);          

            return pagamento;
        }
        public virtual async Task<Pagamento> Atualizar(Pagamento pagamento)
        {
            var filter = Builders<Pagamento>.Filter.Eq(c => c.Id, pagamento.Id);
            var update = Builders<Pagamento>.Update
                .Set(c => c.ValorTotal, pagamento.ValorTotal)
                .Set(c => c.Status, pagamento.Status)
                .Set(c => c.QRCode, pagamento.QRCode);

            var result = await _pagamentoCollection.UpdateOneAsync(filter, update);

            return pagamento;
        }
        public async Task<Pagamento> ObterPorPedidoId(long id)
        {
            var filter = Builders<Pagamento>.Filter.Eq(c => c.PedidoId, id);            
            return await _pagamentoCollection.Find(filter).FirstOrDefaultAsync();
        }
        public async Task<Pagamento> ObterPorGUID(Guid numeroPagamento)
        {
            var filter = Builders<Pagamento>.Filter.Eq(c => c.NumeroPagamento, numeroPagamento);
            return await _pagamentoCollection.Find(filter).FirstOrDefaultAsync();           
        }
    }
}

