using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tech_test_payment_api.Data;
using tech_test_payment_api.Entities;

namespace tech_test_payment_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendaController : ControllerBase
    {
        private Context _context { get; set; }

        public VendaController(Context context)
        {
            this._context = context;
        }

        [HttpPost("RegistrarVenda")]
        public IActionResult RegistrarVenda(Venda venda)
        {
            if(venda.StatusVenda != EnumStatusVenda.AguardandoPagamento)
                return BadRequest(new { Erro = "Só é possivél registrar venda com status de Aguardando pagamento" });

            try
            {
                _context.Vendas.Add(venda);
                _context.SaveChanges();
                return Created("Venda criada com sucesso", venda);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("BuscarVendaPorId/{id}")]
        public IActionResult BuscarVendaPorId(int id)
        {
            var result = _context.Vendas.Include(v => v.Vendedor)
                                        .Include(v => v.ItensVendidos)
                                        .SingleOrDefault(v => v.Id == id);

            if (result is null)
            {
                return BadRequest(new { Erro = "Esse registro não existe"});
            }
            return Ok(result);
        }

        [HttpPut("AtualizarStatusVenda/{id}")]
        public IActionResult AtualizarStatusVenda(int id, EnumStatusVenda status)
        {
             var pedidoBanco = _context.Vendas.Find(id);

            if (pedidoBanco == null)
                return NotFound();

            if (pedidoBanco.StatusVenda == EnumStatusVenda.Entregue ||
                    pedidoBanco.StatusVenda == EnumStatusVenda.Cancelada)
                        return BadRequest(new { Erro = "Não é possivel alterar o status do pedido a venda foi cancelada ou entregue" });  

            if (pedidoBanco.StatusVenda == EnumStatusVenda.EnviadoParaTransportadora && (status == EnumStatusVenda.Entregue))
            {
                pedidoBanco.StatusVenda = status;
                _context.Vendas.Update(pedidoBanco);
                _context.SaveChanges();
                return Ok(status);
            }
                 
            
            if (pedidoBanco.StatusVenda == EnumStatusVenda.PagamentoAprovado && 
                (status == EnumStatusVenda.EnviadoParaTransportadora || status == EnumStatusVenda.Cancelada))
            {
                pedidoBanco.StatusVenda = status;
                _context.Vendas.Update(pedidoBanco);
                _context.SaveChanges();
                return Ok(status);
            }                      
            
            if (pedidoBanco.StatusVenda == EnumStatusVenda.AguardandoPagamento && 
                (status == EnumStatusVenda.PagamentoAprovado || status == EnumStatusVenda.Cancelada))
            {
                pedidoBanco.StatusVenda = status;
                _context.Vendas.Update(pedidoBanco);
                _context.SaveChanges();
                return Ok(status);
            }
            return BadRequest(new { Erro = $"Só é possivél alterar o status da venda de:" + 
            "AguardandoPagamento para PagamentoAprovado ou Cancelada, " +
            "PagamentoAprovado para EnviadoParaTransportadora ou Cancelada, " +
            "EnviadoParaTransportadora para Entregue"});

        }
    }
}