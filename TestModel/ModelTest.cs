namespace TestModel
{
    public class ModelTest
    {
        [Fact]
        public void ExceptionTest()
        {
           // Arrange
           var prices = new PricesPredict();

           // Act & Assert
           Assert.Throws<Exception>(() => prices.GetPrediction("price_data", 11));
        }

        [Fact]
        public void PredictionsTest()
        {
            // Arrange
            var prices = new PricesPredict();

            // Act
            var predictions = prices.GetPrediction("price_data", 5);

            // Assert
            Assert.Equal(5,predictions.forcastedprices.Count);
        }

        [Fact]
        public void ModelNotExistsTest()
        {
            // Arrange
            var prices = new PricesPredict();

            // Act & Assert
            Assert.Throws<Exception>(() => prices.GetPrediction("Fil_eksister_ikke", 5));
        }
    }
}
