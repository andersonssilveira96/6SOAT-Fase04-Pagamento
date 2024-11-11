using Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Security.Cryptography;

namespace Domain.Entities
{
    public class Pagamento
    {
        public Pagamento()
        {
            
        }
        public Pagamento(Guid numeroPagamento, string qrCode, long pedidoId, decimal valorTotal)
        {                       
            PedidoId = pedidoId;   
            NumeroPagamento = numeroPagamento;
            QRCode = qrCode;
            Status = PagamentoStatusEnum.Pendente;
            ValorTotal = valorTotal;
        }
        public long Id { get; private set; }
        [BsonGuidRepresentation(GuidRepresentation.Standard)] public Guid NumeroPagamento { get; private set; }
        public string QRCode { get; private set; }
        public PagamentoStatusEnum Status { get; private set; }
        public long PedidoId { get; private set; }
        public decimal ValorTotal { get; set; }
        public bool PagamentoAprovado() => Status == PagamentoStatusEnum.Pago;
        public void AtualizarStatus(bool aprovado)
        {
            if (aprovado)
                Pagar();
            else
                Rejeitar();
        }
        private void Pagar() => Status = PagamentoStatusEnum.Pago;
        private void Rejeitar() => Status = PagamentoStatusEnum.Rejeitado;
        public void IncrementarId(long id) => Id = id;
    }
}
