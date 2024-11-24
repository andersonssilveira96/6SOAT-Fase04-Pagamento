Feature: PagamentoUseCase
  Como um desenvolvedor
  Eu quero garantir que o comportamento de AtualizarPagamento e GerarPagamento esteja correto
  Para que o sistema processe pagamentos de forma esperada

  Scenario: Atualizar pagamento com número de pedido válido
    Given existe um pagamento com o número de pedido "12345"
    When eu atualizo o pagamento para "aprovado"
    Then o pagamento deve ser salvo com o status "Pago"
    And uma mensagem deve ser enviada para o tópico "pedidos-pagos"

  Scenario: Atualizar pagamento com número de pedido inválido
    Given não existe um pagamento com o número de pedido "99999"
    When eu tento atualizar o pagamento
    Then deve ser lançada uma exceção com a mensagem "Número de pagamento inválido"

  Scenario: Gerar um novo pagamento
    Given o serviço de gateway retorna um pagamento para o pedido "54321" com valor "100.00"
    When eu gero o pagamento
    Then o pagamento deve ser salvo no repositório
    And o pagamento retornado deve ter o pedido "54321"
