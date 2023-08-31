using WiredBrainCoffee.DataProcessor.Data;
using WiredBrainCoffee.DataProcessor.Model;

namespace WiredBrainCoffee.DataProcess.Data;

public class ConsoleCoffeeCountStoreTests
{
    [Fact]
    public void ShouldWriteOutputToConsole()
    {
        // Arrange 
        var coffeeCountItem = new CoffeeCountItem("Cappuccino", 5);
        var stringWritter = new StringWriter();
        var consoleCoffeeCountStore = new ConsoleCoffeeCountStore(stringWritter);

        //Act

        consoleCoffeeCountStore.Save(coffeeCountItem);

        //Assert

        var result = stringWritter.ToString();

        Assert.Equal($"{coffeeCountItem.CoffeeType}:{coffeeCountItem.Count}{Environment.NewLine}", result);

    }
}
