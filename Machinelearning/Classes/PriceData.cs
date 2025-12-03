
using Microsoft.ML.Data;
internal class PriceData
{
    [LoadColumn(0)]
    internal float Price { get; set; }
}