using System;
using System.Diagnostics;
using System.Threading.Tasks;
using static Isotope80.Isotope;
using static Isotope80.Assertions;
using LanguageExt;
using static LanguageExt.Prelude;
using Xunit;
using Microsoft.Playwright;

namespace Isotope80.Samples.UnitTests;

public class MonadTests
{
    [Fact]
    public void Sequence_stops_on_first_error()
    {
        var items = Seq(pure(1), fail<int>("boom"), pure(3));
        var computation = items.Sequence();

        var (state, value) = computation.Run();
        Assert.True(state.IsFaulted, "Expected sequence to be faulted");
        Assert.Contains("boom", state.Error.Head.ToString());
    }

    [Fact]
    public void Collect_continues_past_errors()
    {
        var items = Seq(pure(1), fail<int>("boom"), pure(3));
        var computation = items.Collect();

        var (state, value) = computation.Run();
        Assert.True(state.IsFaulted, "Expected collect to be faulted (has errors)");
        // Collect continues past errors, so values at index 0 and 2 should be captured
        Assert.Equal(1, value[0]);
        Assert.Equal(3, value[2]);
        Assert.Contains("boom", state.Error.Head.ToString());
    }

    [Fact]
    public async Task Stopwatch_measures_duration()
    {
        var computation = stopwatch(pause(TimeSpan.FromMilliseconds(200)));

        var (state, result) = await computation.RunAndThrowOnError();
        Assert.True(result.Time >= TimeSpan.FromMilliseconds(150),
            $"Expected stopwatch >= 150ms, got {result.Time.TotalMilliseconds}ms");
    }

    [Fact]
    public async Task Pause_delays_execution()
    {
        var sw = Stopwatch.StartNew();

        var computation = pause(TimeSpan.FromMilliseconds(200));
        await computation.RunAndThrowOnError();

        sw.Stop();
        Assert.True(sw.Elapsed >= TimeSpan.FromMilliseconds(150),
            $"Expected elapsed >= 150ms, got {sw.Elapsed.TotalMilliseconds}ms");
    }

    [Fact]
    public void Use_disposes_resource()
    {
        var resource = new DisposableFlag();

        var computation = use(resource, r => pure(42));

        var (state, value) = computation.RunAndThrowOnError();
        Assert.Equal(42, value);
        Assert.True(resource.Disposed, "Expected resource to be disposed after use");
    }

    [Fact]
    public void DoWhile_repeats_until_condition_false()
    {
        var counter = 0;

        var increment = voida(() => counter++).Map(_ => counter);

        var computation = doWhile(increment, c => c < 3);

        var (state, value) = computation.RunAndThrowOnError();
        Assert.True(counter >= 3, $"Expected counter >= 3, got {counter}");
        Assert.Equal(3, value);
    }

    [Fact]
    public void DoWhileOrFail_fails_on_max_attempts()
    {
        // Condition is always true, so it should exhaust maxAttempts and fail
        var computation = doWhileOrFail(pure(42), _ => true, maxAttempts: 3);

        var (state, _) = computation.Run();
        Assert.True(state.IsFaulted, "Expected doWhileOrFail to be faulted after max attempts");
        Assert.Contains("max-attempts", state.Error.Head.ToString());
    }

    [Fact]
    public async Task DoWhile_async_variant()
    {
        var counter = 0;

        var increment = isoAsync<int>(async () =>
        {
            counter++;
            await Task.CompletedTask;
            return counter;
        });

        var computation = doWhile(increment, c => c < 3);

        var (state, value) = await computation.RunAndThrowOnError();
        Assert.True(counter >= 3, $"Expected counter >= 3, got {counter}");
        Assert.Equal(3, value);
    }

    [Fact]
    public void VoidA_wraps_action()
    {
        var sideEffectRan = false;

        var computation = voida(() => { sideEffectRan = true; });

        var (state, _) = computation.RunAndThrowOnError();
        Assert.False(state.IsFaulted, "Expected voida to succeed");
        Assert.True(sideEffectRan, "Expected side effect to have run");
    }

    [Fact]
    public void Iso_with_state_accessor()
    {
        // iso<A>(Func<IsotopeState, A>) provides access to the current state.
        // Configuration is internal, so we test with a publicly accessible property (IsFaulted).
        var computation = iso<string>(state => state.IsFaulted ? "faulted" : "default");

        var (state, value) = computation.RunAndThrowOnError();
        Assert.Equal("default", value);
    }

    private class DisposableFlag : IDisposable
    {
        public bool Disposed { get; private set; }
        public void Dispose() => Disposed = true;
    }
}
