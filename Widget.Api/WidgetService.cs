namespace Widget.Api;

public class WidgetService

{

    public int Add(int a, int b) => a + b;

    public string Describe(int id) => id <= 0

        ? throw new ArgumentOutOfRangeException(nameof(id))

        : $"widget-{id}";

}
