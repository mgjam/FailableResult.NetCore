# FailableResult
Simple monad for handling failures as an alternative to the exception driven code

## Description
This monad can be used as an alternative to the exception driven code which is hard to navigate and maintain. With this package you get
* Visibility over all possible failures
* Fluid handling with extension methods

## Instalation
Install with NuGet package manager
`Install-Package FailableResult.NetCore`

## Usage
Following code demonstrates usages of the package
```csharp

void Main()
{
    var data = ...;

    SaveData(data)
        .OnSuccess(success => ...)
        .OnFailure(failure => switch(failure)
        {
            case FailureType.Timeout: ...
            default: ...
        });
}

IFailableResult<object, FailureType> SaveData(object data)
{
    if (IsDataInvalid(data))
        return FailureResult<object, FailureType>.Create(FailureType.InvalidData);
    
    ...

    return SuccessResult<object, FailureType>.Create(data);
}

enum FailureType
{
    InvalidData,
    Timeout,
    ServiceUnavailable,
    Other
}
```
For other use cases see unit tests