namespace CosmicDefenders.Helpers;

internal class ScoresHelper
{
    public static List<int> GetScores()
    {
        List<int> result = new List<int>();
        if (!File.Exists("scores.txt"))
            return result;

        string[] lines = File.ReadAllLines("scores.txt");
        return lines.Select(l => Convert.ToInt32(l)).ToList();
    }

    public static void WriteScore(int score)
    {
        List<int> scores = GetScores();
        scores.Add(score);
        scores = scores.OrderByDescending(x => x).Take(10).ToList();
        if (File.Exists("scores.txt"))
            File.Delete("scores.txt");
        using FileStream fileStream = File.Create("scores.txt");
        using StreamWriter streamWriter = new StreamWriter(fileStream);
        foreach (int s in scores)
        {
            streamWriter.WriteLine(s);
        }
    }
}