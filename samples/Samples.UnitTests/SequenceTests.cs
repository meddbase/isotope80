using System;
using LanguageExt;
using LanguageExt.Common;
using OpenQA.Selenium.Chrome;
using Xunit;
using Isotope80;
using static System.Console;
using static LanguageExt.Prelude;
using static Isotope80.Isotope;
using Error = LanguageExt.Common.Error;

namespace Samples.UnitTests
{
    public class SequenceTests
    {
        [Fact]
        public void TestValidSequence()
        {
            var settings = IsotopeSettings.Create();
 
            var ma = pure(1);
            var mb = pure(2);
            var mc = pure(3);

            var ms = Seq(ma, mb, mc).Sequence();

            var (state, value) = ms.Run(settings);

            Assert.True(!state.IsFaulted);
            Assert.True(value == Seq(1, 2, 3));
        }

        [Fact]
        public void TestFailedSequence()
        {
            var settings = IsotopeSettings.Create();
 
            var ma = pure(1);
            var mb = fail("error in item 2");
            var mc = fail("error in item 3");

            var ms = Seq(ma, mb, mc).Sequence();

            var (state, value) = ms.Run(settings);

            Assert.True(state.IsFaulted);
            Assert.True(state.Error.Count == 1);
            Assert.True(state.Error.Head == Error.New("error in item 2"));
        }

        [Fact]
        public void TestFailedCollect()
        {
            var settings = IsotopeSettings.Create();
 
            var ma = pure(1);
            var mb = fail("error in item 2");
            var mc = fail("error in item 3");

            var ms = Seq(ma, mb, mc).Collect();

            var (state, value) = ms.Run(settings);

            Assert.True(state.IsFaulted);
            Assert.True(state.Error.Count == 2);
            Assert.True(state.Error[0] == Error.New("error in item 2"));
            Assert.True(state.Error[1] == Error.New("error in item 3"));
        }
    }
}