using Octokit;

namespace GitStat.Common.Calculate;

public static class PRStatCalculation {
    /// <summary>
    /// Extension method over specifc <see cref="IReadOnlyList{T}"/> to calculate Merge Rate
    /// </summary>
    /// <param name="pullRequests"><see cref="PRStat"/> for available lists (Closed, Open, All) to pass.</param>
    /// <returns>Merge Rate</returns>
    public static double MergeRate(this IReadOnlyList<PullRequest> pullRequests) {

        var merged = from pr in pullRequests
                     where pr.MergedAt.HasValue // .Merge == true check seems unreliable?
                     select pr;
        return (double)merged.Count() / pullRequests.Count * 100;
    }

    public static MergeStats AverageMergeDuration(this IReadOnlyList<PullRequest> pullRequests) {

        var mergeDurations = (from pr in pullRequests
                              where pr.MergedAt.HasValue
                              select (pr.MergedAt!.Value - pr.CreatedAt).TotalDays).ToList();

        if (mergeDurations.Count == 0) return MergeStats.Empty;
        
        mergeDurations.Sort();
        int count = mergeDurations.Count;

        var median = mergeDurations.Count % 2 == 0 ?
            (mergeDurations[count / 2 - 1] + mergeDurations[count / 2]) / 2
            : mergeDurations[count / 2];

        return new MergeStats(
            Average: mergeDurations.Average(),
            Median: median,
            Shortest: mergeDurations.Min(),
            Longest: mergeDurations.Max(),
            TotalMerged: count
            );
    }

}
