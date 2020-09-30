using OpenQA.Selenium.Chrome;
using System;
using LanguageExt;
using LanguageExt.Common;
using Xunit;
using static LanguageExt.Prelude;
using static System.Console;
using static Isotope80.Isotope;

namespace Isotope80.Samples.UnitTests
{
    public class LoggingTests
    {
        [Fact]
        public void TestNestedContextualLogs()
        {
            Seq<string> expected = Seq(
                "Info log",
                "\tTest 1",
                "\tInfo for test 1",
                "\tMore info for test 1",
                "\t\tTest 1.SubTest 1",
                "\t\tInfo for test Test 1.SubTest 1",
                "\t\tMore info for test Test 1.SubTest 1",
                "\t\tTest 1.SubTest 2",
                "\t\tInfo for test Test 1.SubTest 2",
                "\t\tMore info for test Test 1.SubTest 2");
            
            Seq<string> logs = default;
            
            var stgs = IsotopeSettings.Create();
            stgs.LogStream.Subscribe(x => logs = logs.Add(x.ToString()));

            var iso2 = from _ in info("Info log")
                       from r in context("Test 1",
                                         from a in info("Info for test 1")
                                         from b in info("More info for test 1")
                                         from c in context("Test 1.SubTest 1",
                                                           from d in info("Info for test Test 1.SubTest 1")
                                                           from e in info("More info for test Test 1.SubTest 1")
                                                           select unit)
                                         from f in context("Test 1.SubTest 2",
                                                           from g in warn("Info for test Test 1.SubTest 2")
                                                           from h in warn("More info for test Test 1.SubTest 2")
                                                           select unit)
                                         select unit)
                       select r;
                                              
                                
            (var state2, var value2) = iso2.Run(stgs);

            Assert.True(logs == expected);
        }
        
        [Fact]
        public void TestNestedContextualErrors()
        {
            var stgs = IsotopeSettings.Create();

            var iso = context("Chrome",
                           context("Start Page",
                                context("Patient tile",
                                     fail<Unit>("element not found"))));
                                
            (var state, var value) = iso.Run(stgs);

            Assert.True(state.Error.Head.Message == "element not found (Chrome → Start Page → Patient tile)");
        }
    }
}