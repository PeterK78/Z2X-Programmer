# Z2X-Programmer Naming Convention
## Overview
| Identifier  | Case   | Example |
|---|---|---|
| Fields   | lowerCamelCase <br>(with leading underscore)  |  `_dirtyRect` |
| Read-only static fields  | PascalCase  | `RedValue`  |
| Class | Pascal  |  `AppDomain` |
| Enumeration type  |  Pascal | `ErrorLevel`	  |
| Enumeration values | Pascal  |  `FatalError` |
| Event |  	 Pascal  |  `ValueChanged` |
| Exception class | Pascal | `WebException`|
| Interface | Pascal | `IDisposable` |
| Method | Pascal | `ToString` |
| Namespace | Pascal |`System.Drawing` |
| Parameter | Camel | `typeName` |
|Property | Pascal | `BackColor` |


## Regions
We use regions to group parts of source code with a specific function. The following region names are allowed:

| Identifier  | Case   | 
|-----|-----|
| REGION: PUBLIC PROPERTIES | Used for a group of public properties.|
| REGION: PRIVATE FUNCTIONS | Used for a group of private functions.|
| REGION: PUBLIC FUNCTIONS | Used for a group of public functionss.|
| REGION: CONSTRUCTOR | Contains the constructor.|
| REGION: COMMANDS | Used for a group of commands.
| REGION: DECODER FEATURES | Used for a group of decoder features.|
| REGION: PRIVATE FIELDS | Used for a group of private fields.|
| REGION: LIMITS FOR ENTRY VALIDATION | Used for a set of functions which is used for the entry validation |
| REGION: PUBLIC DELEGATES | Used for a set of public delegates |

## Comments
Comments have a period at the end of the sentence.

`
/// Registers the .NET MAUI Maps handlers that are needed to render the map control.
`

## Design Decisions
* Use the Microsoft recommendation from https://learn.microsoft.com/en-us/previous-versions/dotnet/netframework-4.0/ms229043(v=vs.100)?redirectedfrom=MSDN
* No CV values are displayed in a frame header. These are located in the labels next to the controls.
