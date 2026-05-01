using GitStat.Common.Client;
using Octokit;

namespace GitStat.Common.Fetch;

public sealed class PRStat(GSClient gsClient) {
    public Task<IReadOnlyList<PullRequest>> GetAllOpenAsync(string owner, string repo) => GetPR(owner, repo);
    public Task<IReadOnlyList<PullRequest>> GetAllClosedAsync(string owner, string repo) => GetPR(owner, repo, ItemStateFilter.Closed);
    public Task<IReadOnlyList<PullRequest>> GetAllAsync(string owner, string repo) => GetPR(owner, repo, ItemStateFilter.All);

    private async Task<IReadOnlyList<PullRequest>> GetPR(string owner, string repo, ItemStateFilter state = ItemStateFilter.Open) {
        var options = new ApiOptions {
            PageSize = 100,
        };
        return await gsClient.Client.PullRequest.GetAllForRepository(owner, repo, new PullRequestRequest { State = state }, options);
    }
}

