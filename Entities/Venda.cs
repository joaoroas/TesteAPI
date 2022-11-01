
using System.ComponentModel.DataAnnotations.Schema;

namespace tech_test_payment_api.Entities
{
    public class Venda
    {
        public Venda()
        {
            this.StatusVenda = EnumStatusVenda.AguardandoPagamento;
        }
        public int Id { get; set; }
        public int CodigoPedido { get; set; }
        public DateTime DataVenda { get; set; }
        public Vendedor Vendedor { get; set; }
        public List<Item> ItensVendidos { get; set; }
        public EnumStatusVenda StatusVenda { get; set; }
        
    }
}