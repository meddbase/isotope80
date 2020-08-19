using LanguageExt;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using Xunit;
using Xunit.Abstractions;
using static Isotope80.Assertions;
using static Isotope80.Isotope;
using static LanguageExt.Prelude;

namespace Isotope80.Samples.UnitTests
{
    public class Environment
    {
        [Fact]
        public void Environment_CanBeAskedFor()
        {
            var computation =
                from env in Isotope.ask<ITestEnv>()
                let result = env.Add(2, 3)
                select result;

            var (state, value) = computation.Run(new TestEnv());

            assert<ITestEnv>(value == 5, $"Expected 5 but was {value}");
        }

        [Fact]
        public void EnvironmentMember_CanBeAskedFor()
        {
            var computation =
                from add in Isotope.asks<ITestEnv, Func<int, int, int>>(e => e.Add)
                let result = add(2, 3)
                select result;

            var (state, value) = computation.Run(new TestEnv());

            assert<ITestEnv>(value == 5, $"Expected 5 but was {value}");
        }
    }

    public interface ITestEnv
    {
        int Add(int a, int b);
    }

    public class TestEnv : ITestEnv
    {
        public int Add(int a, int b) => a + b;
    }
}
