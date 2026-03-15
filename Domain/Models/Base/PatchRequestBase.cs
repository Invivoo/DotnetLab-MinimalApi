using System.Runtime.CompilerServices;

namespace Domain.Models.Base
{
    public abstract class PatchRequestBase
    {
        public void ApplyTo<T>(T target)
        {
            var targetType = typeof(T);
            foreach (var property in PropertiesToUpdate)
            {
                var targetProperty = targetType.GetProperty(property.Key);
                if (targetProperty != null && targetProperty.CanWrite)
                {
                    targetProperty.SetValue(target, property.Value);
                }
            }
        }

        protected T? GetStructProperty<T>([CallerMemberName] string propertyName = null!)
            where T : struct
            => PropertiesToUpdate.TryGetValue(propertyName, out var value) ? value as T? : null;

        protected T? GetProperty<T>([CallerMemberName] string propertyName = null!) where T : class
            => PropertiesToUpdate.TryGetValue(propertyName, out var value) ? value as T : null;
        protected void SetProperty<T>(T value, [CallerMemberName] string propertyName = null!)
            => PropertiesToUpdate[propertyName] = value;

        private Dictionary<string, object?> PropertiesToUpdate { get; } = [];
    }
}