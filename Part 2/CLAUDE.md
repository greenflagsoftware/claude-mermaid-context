# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a documentation and chart generation project focused on creating Mermaid diagrams with Claude Code. The project serves as a resource for generating various types of charts and pseudocode from natural language descriptions.

## Directory Structure

Follow the established directory conventions:

- **pseudocode/** - Generated pseudocode in markdown format, named using the source document name
- **src/dotnet/** - All .NET code should be placed in this directory
- **mermaid/** - Generated Mermaid .mmd files organized by chart type:
  - **mermaid/flowcharts/** - Flowchart diagrams
  - **mermaid/erd/** - Entity relationship diagrams
  - **mermaid/class/** - Class diagrams
- **docs/** - Documentation and guidance files
- **tasks/** - Task definitions and requirements

## File Naming Conventions

- Mermaid files: `[source-filename]_[chart-type].mmd` (e.g., `userregistration_flowchart.mmd`)
- Pseudocode files: Use the source markdown document name

## .NET Development Stack

When working with .NET code:

- **Language**: C#
- **Framework**: .NET Framework 10
- **Testing**: xUnit
- **Database**: Entity Framework
- **Dotnet Path**: `/mnt/c/Program Files/dotnet/dotnet.exe`

### .NET Project Structure

Before creating solution files, ask for the project name to use for naming:

- **[ProjectName].sln** - Solution file (add all projects to this)
- **[ProjectName].Shared.csproj** - Shared class library (all projects reference this)
- **[ProjectName].Integration.Tests** - Integration tests
- **[ProjectName].Unit.Tests** - Unit tests  
- **[ProjectName].ConsoleRunner.csproj** - CLI application

### Database Commands

For Entity Framework scaffolding:
```bash
dotnet ef dbcontext scaffold "[connection-string](TrustServerCertificate=true)"
```

Ask for connection parameters: host, port, database, username, password to build the connection string.

## Pseudocode Standards

Follow the structured pseudocode format defined in docs/pseudocode.txt:

- Use problem domain vocabulary, not implementation details
- Decompose logic to single loop or decision level
- Use standard constructs: SEQUENCE, WHILE, IF-THEN-ELSE, REPEAT-UNTIL, FOR, CASE
- Use keywords: READ/OBTAIN/GET (input), PRINT/DISPLAY/SHOW (output), COMPUTE/CALCULATE/DETERMINE (processing)
- Proper indentation for nested constructs
- CALL keyword for subprocedure invocation

## Mermaid Chart Generation

Reference docs/mermaid-js.txt for comprehensive Mermaid syntax examples and best practices. Key points:

- Initialize with security settings: `securityLevel: 'antiscript'`
- Support multiple diagram types: flowcharts, sequence, class, C4, gitgraph, etc.
- Use proper accessibility features with `accTitle` and `accDescr`
- Follow established styling and configuration patterns

## Workflow

1. Read task requirements from tasks/ directory
2. Generate pseudocode following the established format
3. Create appropriate Mermaid diagrams based on requirements
4. Place files in correct directories with proper naming
5. For .NET projects, ask for project name before creating solution structure

## Session Initialization

- Always read @docs/initial.md when starting a session to familiarize yourself with our context.