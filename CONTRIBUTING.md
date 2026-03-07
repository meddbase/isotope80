# Contributing to Isotope80

## Building

```bash
dotnet build Isotope80.sln
```

## Running the samples

```bash
dotnet run --project samples/Samples.Console
```

Requires Chrome and ChromeDriver to be available on your PATH.

## Regenerating API docs

The API documentation in `docs/` is generated from XML comments using [xmldoc2md](https://github.com/nicodoerre/xmldoc2md).

Install the tool (one-time):

```bash
dotnet tool install -g XMLDoc2Markdown
```

Build in Release mode and regenerate:

```bash
dotnet build src/Isotope80/Isotope80.csproj -c Release
xmldoc2md src/Isotope80/bin/Release/netstandard2.0/Isotope80.dll -o docs/ --back-button --member-accessibility-level public
```

## Conventions

- Use `Select` (not raw `By`) for all public selector parameters
- Use LINQ query syntax (`from ... in ... select`) for Isotope chains
- Use `trya` for wrapping void calls that may throw, `tryf` for calls that return a value
- Use `get` prefix for read-accessors (`getScreenshot`, `getWindowSize`)
- Use action verbs for methods with side effects (`saveScreenshot`, `click`, `sendKeys`)
- Properties that read simple values may omit the prefix (`url`, `title`, `pageSource`)
- Wrap error messages with `context` to provide meaningful scope
- Don't leak `IWebElement` into the public API — use `WebElement` or `Select`
- XML doc comments on all public members
