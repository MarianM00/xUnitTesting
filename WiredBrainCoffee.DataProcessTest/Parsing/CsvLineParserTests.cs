using WiredBrainCoffee.DataProcessor.Parsing;

namespace WiredBrainCoffee.DataProcess.Parsing
{

    public class CsvLineParserTests
    {
        [Fact]
        public void ShouldParseValidLine()
        {
            string[] csvLines = new[] { "Cappuccino;10/27/2022 8:15:43 AM" };

            var machineDataItems = CsvLineParser.Parse(csvLines);

            Assert.NotNull(machineDataItems);
            Assert.Single(machineDataItems);

            Assert.Equal("Cappuccino", machineDataItems[0].CoffeeType);
            Assert.Equal(new DateTime(2022, 10, 27, 8, 15, 43), machineDataItems[0].CreatedAt);
        }

        //[Fact]
        //public void ShouldSkipEmptyLines()
        //{
        //    string[] csvLines = new[] { "", " " };

        //    var machineDataItems = CsvLineParser.Parse(csvLines);

        //    Assert.NotNull(machineDataItems);
        //    Assert.Empty(machineDataItems);
        //}

        [Fact]
        public void ShouldThrowExceptionForInvalidLine()
        {

            var csvLine = "Cappuccino";
            //Arrange 
            var csvLines = new[] { csvLine };


            //Act and Assert
            var exception = Assert.Throws<Exception>(() => CsvLineParser.Parse(csvLines));

            Assert.Equal($"Invalid csv line: {csvLine}", exception.Message);
        }

        [Fact]
        public void ShouldThrowExceptionForInvalidLine2()
        {

            var csvLine = "Cappuccino;InvalidDateTime";
            //Arrange 
            var csvLines = new[] { csvLine };


            //Act and Assert
            var exception = Assert.Throws<Exception>(() => CsvLineParser.Parse(csvLines));

            Assert.Equal($"Invalid datetime in csv line: {csvLine}", exception.Message);
        }


        [InlineData("Cappuccino", "Invalid csv line")]
        [InlineData("Cappuccino;InvalidDateTime", "Invalid datetime in csv line")]
        [Theory]
        public void ShouldThrowExceptionForInvalidLine3 (string csvLine, string expectedMessagePrefix)
        {
            var csvLines = new[] { csvLine };

            //Act and Assert
            var exception = Assert.Throws<Exception>(() => CsvLineParser.Parse(csvLines));

            Assert.Equal($"{expectedMessagePrefix}: {csvLine}", exception.Message);
        }


    }
}
