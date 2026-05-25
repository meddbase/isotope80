using System;
using LanguageExt;
using Xunit;
using Xunit.Abstractions;
using Isotope80;
using static LanguageExt.Prelude;
using static Isotope80.Isotope;
using static Isotope80.Assertions;

namespace Samples.UnitTests
{
    public class WindowAndTabTests
    {
        private readonly ITestOutputHelper output;

        public WindowAndTabTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Sequence_preserves_tab_switch()
        {
            var stgs = IsotopeSettings.Create();
            stgs.LogStream.Subscribe(x => output.WriteLine(x.ToString()));
            stgs.ErrorStream.Subscribe(x => output.WriteLine(x.ToString()));

            var iso =
                from _1 in nav("data:text/html,<html><body><h1>Tab 0</h1></body></html>")
                from _2 in newTab
                from _3 in nav("data:text/html,<html><body><h1>Tab 1</h1></body></html>")
                from _4 in switchTabs(0)
                from tabBefore in getCurrentTabNumber
                from _5 in assert(tabBefore == 0, $"Expected tab 0 before Sequence, got {tabBefore}")
                from _6 in Seq(
                    info("before switch"),
                    switchTabs(1),
                    info("after switch")
                ).Sequence().Map(_ => unit)
                from tabAfter in getCurrentTabNumber
                from _7 in assert(tabAfter == 1, $"Expected tab 1 after Sequence, got {tabAfter}")
                select unit;

            (var state, var value) = withChromeDriver(iso).RunAndThrowOnError(settings: stgs);
        }
        
        [Fact]
        public void OnFail_passes_failed_state_to_rhs()
        {
            var lhs = from _1 in initConfig(("marker", "from-lhs"))
                      from _2 in fail<string>("boom")
                      select "";

            var rhs = config("marker");

            var (state, value) = Isotope<string>.OnFail(lhs, rhs).Run();

            Assert.False(state.IsFaulted);
            Assert.Equal("from-lhs", value);
        }

        [Fact]
        public void Pipe_operator_does_not_pass_failed_state_to_rhs()
        {
            var lhs = from _1 in initConfig(("marker", "from-lhs"))
                      from _2 in fail<string>("boom")
                      select "";

            var rhs = config("marker");

            var (state, _) = (lhs | rhs).Run();

            Assert.True(state.IsFaulted, "Expected rhs to fail because | passes original state");
        }
    }
}
