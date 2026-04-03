namespace ServerContainerManager.Application.Extensions
{
    internal static class CollectionsExtensions
    {
        /// <summary>
        /// Compares two collections for equality
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static bool ContentEquals<T>(this IEnumerable<T>? a, IEnumerable<T>? b, IEqualityComparer<T>? comparer = null)
        {
            if (ReferenceEquals(a, b)) return true;
            if (a is null || b is null) return false;

            comparer ??= EqualityComparer<T>.Default;

            // Count occurrences in 'a'
            var counts = new Dictionary<T, int>(comparer);
            var countA = 0;
            foreach (var item in a)
            {
                counts.TryGetValue(item, out var c);
                counts[item] = c + 1;
                countA++;
            }

            // Decrease for items in 'b'
            var countB = 0;
            foreach (var item in b)
            {
                if (!counts.TryGetValue(item, out var c))
                    return false; // 'b' contains an item not in 'a'

                if (c == 1)
                    counts.Remove(item);
                else
                    counts[item] = c - 1;

                countB++;
            }

            // If counts different, sequences differ
            if (countA != countB) return false;

            // If dictionary empty all counts matched
            return counts.Count == 0;
        }
    }
}
