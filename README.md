# dotnet-core

![](https://github.com/appshapes-org/dotnet-core/workflows/Integration/badge.svg)

## What does this project do?

dotnet-core is a collection of projects containing common patterns for .NET Core. Currently these include: core, database, logging, service, and testing.

## Why is this project useful?

dotnet-core allows downstream projects to focus on their bounded context.

## How do I get started?

Install package references to any or all of dotnet-core packages..

### Installation

```powershell
Install-Package AppShapes.Core -ProjectName <Project Name>
```

### Testing

dotnet-core pull requests are checked for code coverage. You can run the tests using .NET CLI:

```bash
dotnet test
```

dotnet-core integrates with [Coverlet](https://github.com/tonerdo/coverlet) so if you have it installed you can generate coverage reports. For example:

```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=OpenCover /p:Exclude=\"[xunit.*]*\"  /p:ExcludeByAttribute=\"Obsolete,GeneratedCodeAttribute,CompilerGeneratedAttribute\"
```

## Where can I get more help, if I need it?

### Issues

* [dotnet-core issues](https://github.com/appshapes-org/dotnet-core/issues)

## Contributing

Have an idea for an improvement? See an issue you want to fix? Start by commenting on an existing [issue][issue_list] or creating a new one. Once you have an issue to reference, create a branch named after the issue. When your improvement or fix is ready submit a PR for review. We closely monitor the issue tracker and pull requests.

For more details check out our [contributing guide](CONTRIBUTING.md). When contributing please keep in mind our [Code of Conduct](CODE_OF_CONDUCT.md).

## Authors

* [Rjae Easton](https://github.com/Rjae)

## License

This project is licensed under the [Apache License, Version 2.0](http://apache.org/licenses/LICENSE-2.0.html).
