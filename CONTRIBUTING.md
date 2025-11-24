# Contributing to Orionde.MoreLookup

Thank you for considering contributing to Orionde.MoreLookup! We welcome contributions from the community.

## How Can I Contribute?

### Reporting Bugs

Before creating bug reports, please check existing issues to avoid duplicates. When creating a bug report, include:

- **Clear title and description**
- **Steps to reproduce** the issue
- **Expected behavior** vs actual behavior
- **Code samples** demonstrating the issue
- **Environment details** (.NET version, OS, etc.)

### Suggesting Enhancements

Enhancement suggestions are tracked as GitHub issues. When creating an enhancement suggestion, include:

- **Clear title and description**
- **Use case** explaining why this enhancement would be useful
- **Examples** of how the feature would be used
- **Possible implementation** if you have ideas

### Pull Requests

1. **Fork the repository** and create your branch from `main`
2. **Follow the existing code style** (see .editorconfig)
3. **Add tests** for any new functionality
4. **Update documentation** (README.md, XML comments, CHANGELOG.md)
5. **Ensure all tests pass** locally before submitting
6. **Write clear commit messages** following conventional commits format

#### Development Setup

```bash
# Clone your fork
git clone https://github.com/YOUR-USERNAME/Orionde.MoreLookup.git
cd Orionde.MoreLookup

# Restore dependencies
dotnet restore

# Build the solution
dotnet build

# Run tests
dotnet test
```

#### Testing Requirements

- All new features must include unit tests
- Maintain or improve code coverage
- Tests should cover multiple target frameworks
- Use descriptive test names following the pattern: `MethodName_Scenario_ExpectedBehavior`

#### Code Style

- Follow the .editorconfig settings
- Use meaningful variable and method names
- Add XML documentation comments for public APIs
- Keep methods focused and concise
- Prefer LINQ and functional patterns where appropriate

#### Commit Message Format

We follow [Conventional Commits](https://www.conventionalcommits.org/):

```
<type>(<scope>): <subject>

<body>

<footer>
```

**Types:**
- `feat`: New feature
- `fix`: Bug fix
- `docs`: Documentation changes
- `test`: Adding or updating tests
- `refactor`: Code refactoring
- `perf`: Performance improvements
- `chore`: Build process or auxiliary tool changes

**Examples:**
```
feat(lookup): add GroupBy extension method

Adds GroupBy method to transform lookups by grouping values.

Closes #42
```

```
fix(union): handle null comparer correctly

Union operation was throwing NullReferenceException when
no comparer was provided. Now defaults to EqualityComparer<T>.Default.

Fixes #38
```

### Documentation

When adding new features:

1. **XML Comments**: Add complete XML documentation for all public APIs
2. **README.md**: Update with examples if the feature is user-facing
3. **CHANGELOG.md**: Add entry under "Unreleased" section
4. **Code Comments**: Add inline comments for complex logic

### Release Process

Releases are managed by maintainers:

1. Update version in `.csproj`
2. Update `CHANGELOG.md` with release date
3. Create release tag (`v1.0.0`)
4. GitHub Actions automatically publishes to NuGet

## Code of Conduct

This project adheres to the [Contributor Covenant Code of Conduct](CODE_OF_CONDUCT.md). 
By participating, you are expected to uphold this code.

## Questions?

Feel free to open an issue with the `question` label or start a discussion in GitHub Discussions.

## License

By contributing, you agree that your contributions will be licensed under the MIT License.
