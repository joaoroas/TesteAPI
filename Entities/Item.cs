
namespace tech_test_payment_api.Entities
{
    public class Item
    {
        public int Id { get; set; }
        public int VendaId { get; set; }
        public string Nome { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
    }
}