using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using AppShapes.Core.Service;
using AppShapes.Core.Testing.Logging;
using AppShapes.Core.Testing.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace AppShapes.Core.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class ApiControllerBaseTests : IClassFixture<WebApplicationBuilder<TestStartup>>
    {
        public ApiControllerBaseTests(WebApplicationBuilder<TestStartup> builder)
        {
            Client = GetHttpClient(builder.WithWebHostBuilder(Configure));
        }

        [Fact]
        public async void GetRouteUrlMustReturnRouteUrlWhenCalled()
        {
            HttpResponseMessage response = await Client.PostAsJsonAsync("api/words", new Word {Value = "42"});
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Word reversedWord = JsonSerializer.Deserialize<Word>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions {PropertyNameCaseInsensitive = true});
            Assert.Equal($"/api/words/{reversedWord.Value}", $"{response.Headers.Location}");
            Assert.Equal("24", reversedWord.Value);
        }

        protected virtual void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(x => x.Replace(new ServiceDescriptor(typeof(ILoggerFactory), new MockLoggerFactory(_ => NullLogger.Instance))));
        }

        protected virtual HttpClient GetHttpClient(WebApplicationFactory<TestStartup> builder)
        {
            HttpClient client = builder.CreateClient(new WebApplicationFactoryClientOptions {AllowAutoRedirect = false});
            client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue {NoCache = true};
            return client;
        }

        private HttpClient Client { get; }
    }

    public class TestStartup : TestStartupBase
    {
    }

    public class Word
    {
        public string Value { get; set; }
    }

    public class WordsController : ApiControllerBase
    {
        [HttpPost(Name = nameof(CreateReverseWord))]
        public virtual async Task<ActionResult<Word>> CreateReverseWord(Word word)
        {
            await Task.CompletedTask;
            Word reversedWord = GetReversedWord(word);
            return Created(GetRouteUrl(nameof(GetOriginalWord), new {reversedWord = reversedWord.Value}), reversedWord);
        }

        [HttpGet("{reversedWord}", Name = nameof(GetOriginalWord))]
        public async Task<ActionResult<Word>> GetOriginalWord(string reversedWord)
        {
            await Task.CompletedTask;
            return Ok(GetReversedWord(new Word {Value = reversedWord}));
        }

        protected virtual Word GetReversedWord(Word word)
        {
            return new Word {Value = new string(word.Value.ToCharArray().Reverse().ToArray())};
        }
    }
}