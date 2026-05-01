namespace GitStat.Common.Calculate;

public record MergeStats(
    double Average,
    double Median,
    double Shortest,
    double Longest,
    int TotalMerged) {
    public static readonly MergeStats Empty = new(0, 0, 0, 0, 0);

    public override string ToString() =>
        $"Merged: {TotalMerged} PRs | Avg: {Average:F1}d | Median: {Median:F1}d | Fastest: {Shortest:F1}d | Slowest: {Longest:F1}d";
}
