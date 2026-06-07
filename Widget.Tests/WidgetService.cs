using Widget.Api;

using Xunit;

public class WidgetServiceTests

{

    private readonly WidgetService _svc = new();

    [Fact] public void Add_Works() => Assert.Equal(5, _svc.Add(2, 3));

    [Theory]

    [InlineData(1, "widget-1")]

    [InlineData(42, "widget-42")]

    public void Describe_Formats(int id, string expected)

        => Assert.Equal(expected, _svc.Describe(id));

    [Fact]

    public void Describe_Rejects_NonPositive()

        => Assert.Throws<ArgumentOutOfRangeException>(() => _svc.Describe(0));

}
