using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;

var mlContext = new MLContext();

// Indlæs data (fx liste af priser)
var data = new List<PriceData>
{
    new() { Price = 499 },
    new() { Price = 502 },
    new() { Price = 505 },
    new() { Price = 507 },
    new() { Price = 510 },
    new() { Price = 512 },
    new() { Price = 515 },
    new() { Price = 518 },
    new() { Price = 520 },
    new() { Price = 523 },
    new() { Price = 525 },
    new() { Price = 528 },
    new() { Price = 530 },
    new() { Price = 533 },
    new() { Price = 535 },
    new() { Price = 538 },
    new() { Price = 540 },
    new() { Price = 543 },
    new() { Price = 545 },
    new() { Price = 548 },
    new() { Price = 499 },
    new() { Price = 502 },
    new() { Price = 505 },
    new() { Price = 507 },
    new() { Price = 510 },
    new() { Price = 512 },
    new() { Price = 515 },
    new() { Price = 518 },
    new() { Price = 520 },
    new() { Price = 523 },
    new() { Price = 525 },
    new() { Price = 528 },
    new() { Price = 530 },
    new() { Price = 533 },
    new() { Price = 535 },
    new() { Price = 538 },
    new() { Price = 540 },
    new() { Price = 543 },
    new() { Price = 545 },
    new() { Price = 548 }
    // ... historiske priser
};

var dataView = mlContext.Data.LoadFromEnumerable(data);

// Byg pipeline til forecasting
var pipeline = mlContext.Forecasting.ForecastBySsa(
    outputColumnName: "ForecastedPrice",
    inputColumnName: "Price",
    windowSize: 10,       // antal tidligere punkter at kigge på
    seriesLength: 30,    // længde af serien
    trainSize: data.Count,
    horizon: 10 // hvor mange fremtidige punkter du vil forudsige

    );         

var model = pipeline.Fit(dataView);

// Lav forudsigelse
var forecastEngine = model.CreateTimeSeriesEngine<PriceData, PriceForecast>(mlContext);
var forecast = forecastEngine.Predict();

Console.WriteLine("Forudsagte priser:");
foreach (var p in forecast.ForecastedPrice)
    Console.WriteLine($"trin: {p:0.00}");


public class PriceData
{
    public float Price { get; set; }
}

public class PriceForecast
{
    public float[] ForecastedPrice { get; set; }
}
