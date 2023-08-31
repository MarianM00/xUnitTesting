using WiredBrainCoffee.DataProcessor.Data;
using WiredBrainCoffee.DataProcessor.Model;
using WiredBrainCoffee.DataProcessor.Processing;

namespace WiredBrainCoffee.DataProcess.Processing;

public class MachineDataProcessorTests : IDisposable
{
    private readonly FakeCoffeeCountStore _coffeeCountStore;
    private readonly MachineDataProcessor _matchineDataProcessor;

    public MachineDataProcessorTests()
    {
        _coffeeCountStore = new FakeCoffeeCountStore();
        _matchineDataProcessor = new MachineDataProcessor(_coffeeCountStore);
    }

    [Fact]
    public void ShouldSaveCountPerCoffeeType()
    {
        //Arrange
       
        var items = new[]
        {
            new MachineDataItem("Cappuccino", new DateTime(2022,10,27,8,0,0)),
            new MachineDataItem("Cappuccino", new DateTime(2022,10,27,9,0,0)),
            new MachineDataItem("Espresso", new DateTime(2022,10,27,10,0,0))

        };

        //Act

        _matchineDataProcessor.ProcessItems(items);

        //Assert

        Assert.Equal(2, _coffeeCountStore.SavedItems?.Count);

        var item = _coffeeCountStore.SavedItems?[0];
        Assert.Equal("Cappuccino", item?.CoffeeType);
    }

    [Fact]
    public void ShouldCleanPreviousCoffeeCount()
    {
        //Arrange
       
        var items = new[]
        {
            new MachineDataItem("Cappuccino", new DateTime(2022,10,27,8,0,0))

        };

        //Act

        _matchineDataProcessor.ProcessItems(items);

        _matchineDataProcessor.ProcessItems(items);
        //Assert
        Assert.Equal(2,_coffeeCountStore.SavedItems?.Count);

        foreach (var item in _coffeeCountStore.SavedItems)
        {
            Assert.Equal("Cappuccino", item.CoffeeType);
            Assert.Equal(1, item.Count);
        }
    }
    [Fact]
    public void ShouldIgnoreItemsThatAreNotNewer()
    {
        //Arrange

        var items = new[]
        {
            new MachineDataItem("Cappuccino", new DateTime(2022,10,27,8,0,0)),
            new MachineDataItem("Cappuccino", new DateTime(2022,10,27,7,10,0)),
            new MachineDataItem("Cappuccino", new DateTime(2022,10,27,7,0,0)),
            new MachineDataItem("Cappuccino", new DateTime(2022,10,27,9,0,0)),
            new MachineDataItem("Espresso", new DateTime(2022,10,27,10,0,0)),
            new MachineDataItem("Espresso", new DateTime(2022,10,27,10,0,0))

        };

        //Act

        _matchineDataProcessor.ProcessItems(items);

        //Assert

        Assert.Equal(2, _coffeeCountStore.SavedItems?.Count);

        var item = _coffeeCountStore.SavedItems?[0];
        Assert.Equal("Cappuccino", item?.CoffeeType);
        Assert.Equal(2, item?.Count);


        item = _coffeeCountStore.SavedItems?[1];
        Assert.Equal("Espresso", item?.CoffeeType);
        Assert.Equal(1, item?.Count);

    }



    public void Dispose()
    {
        //This runs after every test

    }
}

public class FakeCoffeeCountStore : ICoffeeCountStore
{

    public List<CoffeeCountItem>? SavedItems { get; } = new();

    public void Save(CoffeeCountItem item)
    {
        SavedItems?.Add(item);
    }
}