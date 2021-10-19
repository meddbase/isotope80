using OpenQA.Selenium.Chrome;
using System;
using System.Collections;
using System.Collections.Generic;
using LanguageExt;
using LanguageExt.Common;
using Xunit;
using Xunit.Abstractions;
using Isotope80;
using static LanguageExt.Prelude;
using static System.Console;
using static Isotope80.Isotope;

namespace Samples.UnitTests
{
    public class EvalTests
    {
        protected readonly ITestOutputHelper Output;

        public EvalTests(ITestOutputHelper output)
        {
            Output = output;
        }
        
        [Fact]
        public void TestEvalLong()
        {
            var stgs = IsotopeSettings.Create();
            stgs.LogStream.Subscribe(x => Output.WriteLine(x.ToVerboseString()));

            var iso = from _2 in setWindowSize(1280, 960)
                      from _3 in nav("https://www.meddbase.com")
                      from _ in info("Eval tests")
                      from r in eval<long>("return 123;")
                      select r;
            
            (var state, var value) = withChromeDriver(iso).RunAndThrowOnError(settings: stgs);
            
            Assert.True(value == 123);
        }
        
        [Fact]
        public void TestEvalString()
        {
            var stgs = IsotopeSettings.Create();
            stgs.LogStream.Subscribe(x => Output.WriteLine(x.ToVerboseString()));

            var iso = from _2 in setWindowSize(1280, 960)
                      from _3 in nav("https://www.meddbase.com")
                      from _ in info("Eval tests")
                      from r in eval<string>("return 'test';")
                      select r;
            
            (var state, var value) = withChromeDriver(iso).RunAndThrowOnError(settings: stgs);
            
            Assert.True(value == "test");
        }
        
        [Fact]
        public void TestEvalObject()
        {
            var stgs = IsotopeSettings.Create();
            stgs.LogStream.Subscribe(x => Output.WriteLine(x.ToVerboseString()));

            var iso = from _2 in setWindowSize(1280, 960)
                      from _3 in nav("https://www.meddbase.com")
                      from _ in info("Eval tests")
                      from r in eval<IDictionary<string, object>>("return {x: 5, y: 'test'};")
                      select r;
            
            (var state, var value) = withChromeDriver(iso).RunAndThrowOnError(settings: stgs);
            
            Assert.True((long)value["x"] == 5);
            Assert.True((string)value["y"] == "test");
        }
        
        [Fact]
        public void TestEvalVoid()
        {
            var stgs = IsotopeSettings.Create();
            stgs.LogStream.Subscribe(x => Output.WriteLine(x.ToVerboseString()));

            var iso = from _2 in setWindowSize(1280, 960)
                      from _3 in nav("https://www.meddbase.com")
                      from _ in info("Eval tests")
                      from r in eval<object>("return;")
                      select r;
            
            (var state, var value) = withChromeDriver(iso).RunAndThrowOnError(settings: stgs);
            
            Assert.True(value == null);
        }
    }
}