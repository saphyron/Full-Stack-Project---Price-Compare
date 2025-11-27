using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;
using System.Runtime.CompilerServices;
public class PricesPredict
{
    private const int _maxPreditions = 10;
    private static MLContext _mlContext = new MLContext();
    private static bool _test;
    public PricesPredict(bool test = false)
    {
        _test = test;
    }

    public (List<float> forcastedprices,List<float> lowboundprices,List<float> upperboundprices) 
        GetPrediction(string product,int predictions)
    {
        if(predictions > _maxPreditions)
            throw new Exception("Give max 10 forudsigelser");

        var loadedModel = _mlContext.Model.Load(GetProductPath(product), out _);

        // Opret forecasting engine (samme input/output typer)
        var loadedEngine = loadedModel.CreateTimeSeriesEngine<PriceData, PriceForecast>(_mlContext);

        // Brug engine til at forudsige:
        var forecast = loadedEngine.Predict();

        return 
        (forecast?.ForecastedPrice?.Take(predictions).ToList(),
         forecast?.LowerBoundPrice?.Take(predictions).ToList(),
         forecast?.UpperBoundPrice?.Take(predictions).ToList());

        //return new PriceForecast()
        //{
            //ForecastedPrice = forecast?.ForecastedPrice ? .Take(predictions).ToArray(),
            //LowerBoundPrice = forecast?.LowerBoundPrice ? .Take(predictions).ToArray(),
            //UpperBoundPrice = forecast?.UpperBoundPrice ? .Take(predictions).ToArray()
        //};
    }

    public void RetrainModel(string product = "",List<float>? prics = null)
    {
        List<PriceData>? data;
        if(prics == null || prics?.Count == 0)
        {
            var allDataView = _mlContext.Data.LoadFromTextFile<PriceData>(
                    path: GetProductPath(filetype:"csv", create:true),
                    hasHeader: true,
                    separatorChar: ',');


            data = [.. _mlContext.Data.CreateEnumerable<PriceData>(allDataView, reuseRowObject: false)];
        }
        else
        {
            data = CreatePriceDataList(prics);
        }

        var testDataAmount = 10;

        var trainData = data.Take(data.Count - testDataAmount).ToList();

        var testData = data.Skip(data.Count - testDataAmount).Select(x => x.Price).ToArray();

        var dataView = _mlContext.Data.LoadFromEnumerable(trainData);

        // Byg pipeline til forecasting
        var pipeline = _mlContext.Forecasting.ForecastBySsa(
            outputColumnName: nameof(PriceForecast.ForecastedPrice),
            inputColumnName: nameof(PriceData.Price),
            windowSize: 15,       // antal tidligere punkter at kigge på
            seriesLength: 30,    // længde af serien
            trainSize: trainData.Count,
            horizon: _maxPreditions, // hvor mange fremtidige punkter du vil forudsige
            confidenceLevel: 0.95f,
            confidenceLowerBoundColumn: nameof(PriceForecast.LowerBoundPrice),
            confidenceUpperBoundColumn: nameof(PriceForecast.UpperBoundPrice)
            );         

        var model = pipeline.Fit(dataView);

        if(_test)
            PrintAccuacy(testData,model);
        
        _mlContext.Model.Save(model,dataView.Schema,GetProductPath(product,true));
    }

    private static List<PriceData> CreatePriceDataList(List<float>? prices) 
        => prices == null ? new List<PriceData>() : 
        [.. prices.Select(x => new PriceData{Price = x})];

    private static string GetProductPath(string product = "", bool create = false, string filetype = "zip", [CallerFilePath] string currentFile = "")
    {
        product = product == "" ? 
            "price_data" : 
            product;

        string projectRoot = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(currentFile), @"..\"));
        var fullpath = Path.Combine(projectRoot, "Models", $"{product}.{filetype}");

        if (!File.Exists(fullpath) && !create)
            throw new Exception("Der er ikke nogen model for produktet");
        
        return fullpath;
    }

    private static void PrintAccuacy(float[]? testData, SsaForecastingTransformer model)
    {
        // Lav forudsigelse
        var forecastEngine = model.CreateTimeSeriesEngine<PriceData, PriceForecast>(_mlContext);
        var forecast = forecastEngine.Predict();

        Console.WriteLine("Forudsagte priser:");
        for (int i = 0; i < testData.Length; i++)
        {
            float actual = testData[i];
            float predicted = forecast.ForecastedPrice[i];
            float lower = forecast.LowerBoundPrice[i];
            float upper = forecast.UpperBoundPrice[i];
            float procent = lower / upper;
            Console.WriteLine($"t+{i + 1}: actual={actual:0.00}, predicted={predicted:0.00} (CI: {lower:0.00} - {upper:0.00}) procent: {procent:0.00}");
        }
    }
    
}