﻿using Domain.Enums;

namespace Domain.Entities
{
    public class Pagamento
    {
        public Pagamento()
        {
            
        }
        public Pagamento(Guid numeroPagamento, string qrCode, long pedidoId)
        {                       
            PedidoId = pedidoId;   
            NumeroPagamento = numeroPagamento;
            QRCode = qrCode;
            Status = PagamentoStatusEnum.Pendente;
        }
        public long Id { get; private set; }
        public Guid NumeroPagamento { get; private set; }
        public string QRCode { get; private set; }
        public PagamentoStatusEnum Status { get; private set; }
        public long PedidoId { get; private set; }

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

    }
}
