using System.Net;
using System.Text;

namespace LiveKit.Tests.Helpers;

public sealed class MockHttpMessageHandler : HttpMessageHandler
{
    private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _responseFactory;

    public HttpRequestMessage? LastRequest { get; private set; }
    public string? LastRequestBody { get; private set; }
    public int RequestCount { get; private set; }

    public MockHttpMessageHandler(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> responseFactory)
    {
        _responseFactory = responseFactory;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        LastRequest = request;
        RequestCount++;

        if (request.Content != null)
        {
            LastRequestBody = await request.Content.ReadAsStringAsync(cancellationToken);
        }

        return await _responseFactory(request, cancellationToken);
    }

    public static MockHttpMessageHandler WithJsonResponse(string json, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        return new MockHttpMessageHandler((_, _) =>
        {
            var response = new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            return Task.FromResult(response);
        });
    }

    public static MockHttpMessageHandler WithEmptyResponse(HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        return WithJsonResponse("{}", statusCode);
    }
}
