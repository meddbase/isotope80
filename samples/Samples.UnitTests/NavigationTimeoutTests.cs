using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using LanguageExt;
using Xunit;
using static LanguageExt.Prelude;
using static Isotope80.Isotope;
using static Isotope80.Assertions;

namespace Samples.UnitTests
{
    public class NavigationTimeoutTests
    {
        /// <summary>
        /// Local HTTP server that waits for the given delay before answering every request,
        /// so a navigation takes a known amount of time regardless of network conditions.
        /// </summary>
        sealed class SlowServer : IDisposable
        {
            readonly HttpListener listener = new HttpListener();
            readonly TimeSpan responseDelay;

            public string Url { get; }

            public SlowServer(TimeSpan responseDelay)
            {
                this.responseDelay = responseDelay;
                Url = $"http://localhost:{FreePort()}/slow/";
                listener.Prefixes.Add(Url);
                listener.Start();
                Task.Run(ServeAsync);
            }

            static int FreePort()
            {
                var socket = new TcpListener(IPAddress.Loopback, 0);
                socket.Start();
                var port = ((IPEndPoint)socket.LocalEndpoint).Port;
                socket.Stop();
                return port;
            }

            async Task ServeAsync()
            {
                while (listener.IsListening)
                {
                    HttpListenerContext ctx;
                    try
                    {
                        ctx = await listener.GetContextAsync();
                    }
                    catch (Exception) when (!listener.IsListening)
                    {
                        return;
                    }

                    _ = Task.Run(() => RespondAsync(ctx));
                }
            }

            async Task RespondAsync(HttpListenerContext ctx)
            {
                await Task.Delay(responseDelay);
                var body = Encoding.UTF8.GetBytes("<html><body><h1 id='slow'>slow page</h1></body></html>");
                try
                {
                    ctx.Response.ContentType = "text/html";
                    ctx.Response.ContentLength64 = body.Length;
                    await ctx.Response.OutputStream.WriteAsync(body, 0, body.Length);
                    ctx.Response.Close();
                }
                catch (Exception)
                {
                    // The browser gave up waiting; nothing left to send to.
                }
            }

            public void Dispose()
            {
                listener.Stop();
                listener.Close();
            }
        }

        [Fact]
        public void Nav_with_timeout_navigates_to_page()
        {
            var iso =
                from _1 in nav("data:text/html,<html><body><h1>timeout nav</h1></body></html>", TimeSpan.FromSeconds(10))
                from src in pageSource
                from _2 in assert(src.Contains("timeout nav"), $"Expected page source to contain 'timeout nav', got '{src}'")
                select unit;

            withChromeDriver(iso).RunAndThrowOnError();
        }

        [Fact]
        public void Nav_with_timeout_fails_when_page_loads_slower_than_timeout()
        {
            using var server = new SlowServer(TimeSpan.FromSeconds(5));

            var iso =
                from _1 in nav(server.Url, TimeSpan.FromMilliseconds(500))
                select unit;

            var (state, _) = withChromeDriver(iso).Run();

            Assert.True(state.IsFaulted, "Expected navigation to fail when the page loads slower than the timeout");
            Assert.Contains("within 500ms", state.Error.Head.Message);
        }

        [Fact]
        public void Nav_with_timeout_allows_slow_page_to_load()
        {
            using var server = new SlowServer(TimeSpan.FromSeconds(2));

            var iso =
                from _1 in nav(server.Url, TimeSpan.FromSeconds(10))
                from heading in text(css("#slow"))
                from _2 in assert(heading == "slow page", $"Expected heading 'slow page', got '{heading}'")
                select unit;

            withChromeDriver(iso).RunAndThrowOnError();
        }

        [Fact]
        public void Nav_with_timeout_restores_driver_page_load_timeout()
        {
            using var server = new SlowServer(TimeSpan.FromSeconds(2));

            var iso =
                from d in webDriver
                let before = d.Manage().Timeouts().PageLoad
                from _1 in nav(server.Url, TimeSpan.FromMilliseconds(500)) | pure(unit)
                let after = d.Manage().Timeouts().PageLoad
                from _2 in assert(after == before, $"Expected page-load timeout restored to {before}, got {after}")
                from _3 in nav(server.Url)
                from heading in text(css("#slow"))
                from _4 in assert(heading == "slow page", $"Expected heading 'slow page', got '{heading}'")
                select unit;

            withChromeDriver(iso).RunAndThrowOnError();
        }
    }
}
