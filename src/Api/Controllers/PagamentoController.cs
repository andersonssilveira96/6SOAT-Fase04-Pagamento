using Application.DTOs.Pagamentos;
using Application.UseCase.Pagamentos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class PagamentoController : ControllerBase
    {
        private readonly IPagamentoUseCase _pagamentoUseCase;
        public PagamentoController(IPagamentoUseCase pagamentoUseCase)
        {
            _pagamentoUseCase = pagamentoUseCase;
        }

        [HttpPost]
        [Route("webhook")]
        public async Task<IActionResult> Inserir(AtualizarPagamentoDto pagamentoDto)
        {
            try
            {
                return Ok(await _pagamentoUseCase.AtualizarPagamento(pagamentoDto));
            }
            catch (Exception ex)
            {
                return BadRequest(new { Mensagem = ex.Message });
            }
        }
    }
}
