using System;
using LanguageExt;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;
using Xunit.Abstractions;
using Isotope80;
using static LanguageExt.Prelude;
using static Isotope80.Isotope;
using static Isotope80.Assertions;

namespace Samples.UnitTests;

public class AlertHandling
{
    private readonly ITestOutputHelper output;

    public AlertHandling(ITestOutputHelper output)
    {
        this.output = output;
    }
    
    public static Isotope<Unit> GoToDesktopSite =>
        context("Go to Desktop Site",
                from _1 in info("Update Window Size")
                from _2 in setWindowSize(1280, 960)
                from _3 in nav("https://www.meddbase.com")
                select unit);

    const string alertText = "Alert with text 1";
    
    public static Isotope<Unit> TriggerAndAcceptPrompt =>
        context("Triger alert, assert text and accept it",
                from _1 in eval<object>($"$(prompt('{alertText}'))")
                from alertPresent1 in isAlertPresent
                from _2 in assert(alertPresent1, "Expected alert to be present")
                from _3 in info($"alert present: {alertPresent1}")
                from text in getAlertText
                from _4 in assert(text == alertText, $"Expected alert text to be {alertText} but it was {text}")
                from _5 in info($"alert text: {text}")
                from _6 in sendKeysToAlert("meddbase")
                from _7 in acceptAlert
                from alertPresent2 in isAlertPresent
                from _8 in assert(!alertPresent2, "Expected alert to be present")
                from _9 in info($"alert present: {alertPresent2}")
                select unit);
    
    [Fact]
    public void HandlingAlerts()
    {
        var iso = from _1 in GoToDesktopSite
                  from _2 in TriggerAndAcceptPrompt
                  select unit;
           
        var stgs = IsotopeSettings.Create();
        stgs.LogStream.Subscribe(x => output.WriteLine(x.ToString()));
        stgs.ErrorStream.Subscribe(x => output.WriteLine(x.ToString()));

        // chrome driver would automatically close any alerts/ prompts without this
        var chromeOptions = new ChromeOptions() {UnhandledPromptBehavior = UnhandledPromptBehavior.Ignore}; 
            
        (var state, var value) = withChromeDriver(iso, chromeOptions).RunAndThrowOnError(settings: stgs);
    }
}