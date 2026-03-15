using System.Runtime.CompilerServices;

namespace Host.Dto.Base;

public abstract class PatchRequestBase
{
    public IDictionary<string, object?>  GetPropertiesToUpdate() => PropertiesToUpdate;

    protected T? GetStructProperty<T>([CallerMemberName] string propertyName = null!)
        where T : struct
        => PropertiesToUpdate.TryGetValue(propertyName, out var value) ? value as T? : null;

    protected T? GetProperty<T>([CallerMemberName] string propertyName = null!) where T : class
        => PropertiesToUpdate.TryGetValue(propertyName, out var value) ? value as T : null;
    protected void SetProperty<T>(T value, [CallerMemberName] string propertyName = null!)
        => PropertiesToUpdate[propertyName] = value;

    private Dictionary<string, object?> PropertiesToUpdate { get; } = [];
}