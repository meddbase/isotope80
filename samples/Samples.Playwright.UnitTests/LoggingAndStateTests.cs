using System;
using System.Threading.Tasks;
using static Isotope80.Isotope;
using static Isotope80.Assertions;
using LanguageExt;
using static LanguageExt.Prelude;
using Xunit;
using Microsoft.Playwright;

namespace Isotope80.Samples.UnitTests;

public class LoggingAndStateTests
{
    [Fact]
    public void Info_warn_error_appear_in_log()
    {
        var computation =
            from _1 in info("hello")
            from _2 in warn("caution")
            from _3 in error("bad")
            select unit;

        var (state, _) = computation.Run();
        var logText = state.Log.ToString();

        Assert.Contains("hello", logText);
        Assert.Contains("caution", logText);
        Assert.Contains("bad", logText);
    }

    [Fact]
    public void Context_nests_log_entries()
    {
        var computation =
            context("outer", context("inner", info("msg")));

        var (state, _) = computation.Run();
        var logText = state.Log.ToString();

        Assert.Contains("outer", logText);
        Assert.Contains("inner", logText);
        Assert.Contains("msg", logText);
    }

    [Fact]
    public void Mute_suppresses_log_stream_output()
    {
        var streamMessages = new System.Collections.Generic.List<string>();
        var settings = IsotopeSettings.Create(
            new System.Reactive.Subjects.Subject<LanguageExt.Common.Error>(),
            new System.Reactive.Subjects.Subject<LogOutput>());
        settings.LogStream.Subscribe(lo => streamMessages.Add(lo.Message));

        var computation =
            from _1 in mute(
                from _ in info("should not appear")
                select unit)
            from _2 in info("should appear")
            select unit;

        var (state, _) = computation.Run(settings);

        Assert.Contains(streamMessages, m => m.Contains("should appear"));
        Assert.DoesNotContain(streamMessages, m => m.Contains("should not appear"));
    }

    [Fact]
    public void InitConfig_and_config_roundtrip()
    {
        var computation =
            from _1 in initConfig(("key1", "val1"), ("key2", "val2"))
            from v  in config("key1")
            select v;

        var (state, value) = computation.RunAndThrowOnError();
        Assert.Equal("val1", value);

        // Verify missing key fails
        var missingComputation =
            from _1 in initConfig(("key1", "val1"))
            from v  in config("missing")
            select v;

        var (missingState, _) = missingComputation.Run();
        Assert.True(missingState.IsFaulted, "Expected config('missing') to fail");
    }

    [Fact]
    public void Fail_and_or_operator_recovery()
    {
        // fail | pure should recover
        var recovered = fail<int>("oops") | pure(42);
        var (state1, value1) = recovered.RunAndThrowOnError();
        Assert.Equal(42, value1);

        // pure | pure should return left
        var leftWins = pure(1) | pure(2);
        var (state2, value2) = leftWins.RunAndThrowOnError();
        Assert.Equal(1, value2);
    }
}
