namespace Banks.Domain.Extensions
{
    public static class LinqExtension
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(action);

            foreach (T item in source)
            {
                action(item);
            }
        }
    }
}