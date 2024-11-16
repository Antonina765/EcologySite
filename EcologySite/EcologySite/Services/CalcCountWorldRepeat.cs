namespace EcologySite.Services;

public class CalcCountWorldRepeat
{
    public static int IsEclogyTextHas(string text)
    {
        var words = text.Split(new[] { ' ', '.', ',', ';', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
        var wordCounts = new Dictionary<string, int>();

        foreach (var word in words)
        {
            var lowerWord = word.ToLowerInvariant();
            if (wordCounts.ContainsKey(lowerWord))
            {
                wordCounts[lowerWord]++;
            }
            else
            {
                wordCounts[lowerWord] = 1;
            }
        }

        return wordCounts.Max(kv => kv.Value);
    }
}