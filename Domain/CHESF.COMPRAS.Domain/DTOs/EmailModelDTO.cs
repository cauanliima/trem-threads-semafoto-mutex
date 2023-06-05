namespace CHESF.COMPRAS.Domain.DTOs
{
    public class EmailModelDTO <T>
    {
        public T Model { get; set; }
        public dynamic CustomData { get; set; }
        public string Environment { get; set; }
    }
}