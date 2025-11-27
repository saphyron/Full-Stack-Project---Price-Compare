namespace TestModel
{
    public class ModelTest
    {
        [Fact]
        public void ExceptionTest()
        {
            var prices = new PricesPredict();

           Assert.Throws<Exception>(() => prices.GetPrediction("price_data", 11));
        }

        [Fact]
        public void PredictionsTest()
        {
            var prices = new PricesPredict();

            var predictions = prices.GetPrediction("price_data", 5);

            Assert.Equal(5,predictions.forcastedprices.Count);
        }

        [Fact]
        public void ModelNotExistsTest()
        {
            var prices = new PricesPredict();

            Assert.Throws<Exception>(() => prices.GetPrediction("Fil_eksister_ikke", 5));
        }
    }
}
