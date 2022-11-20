namespace Common.Extensions;

public static class RandomExtensions
{
    public static T RandomItem<T>(List<T> originList)
    {
        var random = new Random(DateTime.Now.Millisecond);
        var rand = random.Next(0, originList.Count - 1);
        return originList[rand];
    }
}