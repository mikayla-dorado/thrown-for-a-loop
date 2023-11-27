public class Product
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public bool Sold { get; set; }
    public DateTime StockDate { get; set; }
    public int ManufactureYear { get; set; }
    public double Condition { get; set; }
    public DateTime? SoldOnDate { get; set; }
    public TimeSpan TimeInStock
    {
        get
        {
            DateTime lastDay = SoldOnDate != null ? (DateTime)SoldOnDate : DateTime.Now;
            return lastDay - StockDate;
        }
    }
}