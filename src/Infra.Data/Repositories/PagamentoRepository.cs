using Domain.Entities;
using Domain.Repositories;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repositories
{
    public class PagamentoRepository : IPagamentoRepository
    {
        private readonly TechChallengeContext _context;
        public PagamentoRepository(TechChallengeContext context)
        {
            _context = context;
        }
        public async Task<Pagamento> Inserir(Pagamento pagamento)
        {
            if (pagamento is null)
            {
                throw new ArgumentNullException(nameof(pagamento));
            }

            _context.Pagamento.Add(pagamento);

            await _context.SaveChangesAsync();

            return pagamento;
        }
        public virtual async Task<Pagamento> Atualizar(Pagamento pagamento)
        {
            var entry = _context.Entry(pagamento);

            _context.Pagamento.Update(entry.Entity);

            await _context.SaveChangesAsync();

            return pagamento;
        }
        public async Task<Pagamento> ObterPorPedidoId(long id) => await _context.Pagamento.Include(x => x.Pedido).FirstOrDefaultAsync(x => x.PedidoId == id);
        public async Task<Pagamento> ObterPorGUID(Guid numeroPagamento) => await _context.Pagamento.Include(x => x.Pedido).FirstOrDefaultAsync(x => x.NumeroPagamento == numeroPagamento);
    }
}

