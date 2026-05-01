using GitStat.Common.Calculate;
using GitStat.Common.Client;
using GitStat.Common.Fetch;

var client = GSClient.CreateUnauthenticated("GitStatPRCalculator");
var prStat = new PRStat(client);

const string Owner = "openai";
const string Repo = "parameter-golf";

// Check rate limit before making any expensive calls
var rateLimitInfo = (await client.Client.RateLimit.GetRateLimits()).Resources.Core;

Console.WriteLine($"Rate limit: {rateLimitInfo.Remaining}/{rateLimitInfo.Limit} requests remaining, resets at {rateLimitInfo.Reset.ToLocalTime():HH:mm:ss} @ LocalTime");

if (rateLimitInfo.Remaining < 10) {
    var wait = rateLimitInfo.Reset - DateTimeOffset.UtcNow;
    Console.WriteLine($"Rate limit nearly exhausted, waiting {wait.TotalSeconds:F0} seconds before proceeding...");
    await Task.Delay(wait);
}

Console.WriteLine($"Fetching all closed PRs for {Owner}/{Repo}...");
var closedPRs = await prStat.GetAllClosedAsync(Owner, Repo);

// Check rate limit after to understand the cost of the call
var postCallInfo = client.Client.GetLastApiInfo()?.RateLimit;
Console.WriteLine($"Call consumed {rateLimitInfo.Remaining - postCallInfo?.Remaining} requests, {postCallInfo?.Remaining} remaining");

Console.WriteLine($"Merge rate for {Owner}/{Repo}: {closedPRs.MergeRate():F1}%");

Console.WriteLine($"Calling MergeDuration...");
Console.WriteLine(closedPRs.AverageMergeDuration());