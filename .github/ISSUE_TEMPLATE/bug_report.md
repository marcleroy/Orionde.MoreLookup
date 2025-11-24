---
name: Bug Report
about: Create a report to help us improve
title: '[BUG] '
labels: bug
assignees: ''
---

## Describe the Bug
A clear and concise description of what the bug is.

## To Reproduce
Steps to reproduce the behavior:
1. Create a lookup with '...'
2. Call method '....'
3. Pass parameters '....'
4. See error

## Code Sample
```csharp
// Provide a minimal code sample that reproduces the issue
var lookup1 = Lookup.Builder
    .WithKey("key1", new[] { "value1" })
    .Build();

// The problematic code
var result = lookup1.SomeMethod();
```

## Expected Behavior
A clear and concise description of what you expected to happen.

## Actual Behavior
What actually happened.

## Environment
- **Package Version**: [e.g., 0.9.0-beta]
- **.NET Version**: [e.g., .NET 8.0, .NET Framework 4.8]
- **OS**: [e.g., Windows 11, Ubuntu 22.04, macOS 14]
- **IDE**: [e.g., Visual Studio 2022, Rider, VS Code]

## Stack Trace (if applicable)
```
Paste the full stack trace here
```

## Additional Context
Add any other context about the problem here.

## Possible Solution (optional)
If you have ideas on how to fix this, please share.
