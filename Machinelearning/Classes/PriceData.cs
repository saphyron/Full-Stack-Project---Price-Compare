
using Microsoft.ML.Data;
public class PriceData
{
    [LoadColumn(0)]
    public float Price { get; set; }
}