namespace DataAccess.Helpers;

internal static class ApplyPatchExtension
{
    public static void ApplyTo<T>(this IDictionary<string, object?> patchRequest, T target)
    {
        var targetType = typeof(T);
        foreach (var property in patchRequest)
        {
            var targetProperty = targetType.GetProperty(property.Key);
            if (targetProperty != null && targetProperty.CanWrite)
            {
                targetProperty.SetValue(target, property.Value);
            }
        }
    }
}
