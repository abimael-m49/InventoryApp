namespace InventoryApp.Domain
{
    public class Sale
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public string ClienteNombre { get; set; } = string.Empty; // ← NUEVA
        public DateTime Fecha { get; set; } = DateTime.Now;
        public decimal Total { get; set; }
        public List<SaleDetail> Detalles { get; set; } = new();
    }
}