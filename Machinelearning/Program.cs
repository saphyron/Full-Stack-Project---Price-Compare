var pricespredict = new PricesPredict();
pricespredict.TrainModel();
var nums = pricespredict.GetPredictions("price_data",5);

for (int i = 0; i < nums.forcastedprices.Count; i++)
{
    Console.WriteLine($"forcast {i}: " + nums.forcastedprices[i]);
    Console.WriteLine($"lowerbound {i}: " + nums.lowboundprices[i]);
    Console.WriteLine($"upperbound {i}: " + nums.upperboundprices[i]);
}