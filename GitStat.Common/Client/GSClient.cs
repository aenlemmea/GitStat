using Octokit;

namespace GitStat.Common.Client;

public sealed class GSClient {
    public GitHubClient Client { get; private set; }
    public ProductHeaderValue ProductHeaderValue { get; private set; }
    public bool IsUnauthenticated = false;

    private GSClient() {
        ProductHeaderValue = new ProductHeaderValue("GitStat-PrivateConstructor");
        Client = new GitHubClient(ProductHeaderValue);
        IsUnauthenticated = true;
    }

    public static GSClient CreateUnauthenticated(string productName, string productVersion = "0.0.1") {
        var productHeader = new ProductHeaderValue(productName, productVersion);
        return new GSClient {
            ProductHeaderValue = productHeader,
            Client = new GitHubClient(productHeader),
            IsUnauthenticated = true
        };

    }
}
