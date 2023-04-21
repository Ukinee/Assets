namespace Tmpo;

internal static class Program
{
    private static void Main()
    {
        
    }
}

public class WeightedElements<T> where T : class
{
    private readonly Random _random = new();
    private readonly List<Entry> _elements = new();

    public void Add(int weight, T element)
    {
        var elementIndex = _elements.FindIndex(entry => entry.Element == element);

        if (elementIndex == -1)
            _elements.Add(new Entry(weight, element));
        else
            _elements[elementIndex].Weight += weight;
    }

    public bool Remove(T element)
    {
        if (TryGetEntry(entry => entry.Element == element, out Entry? result) == false) 
            return false;
        
        _elements.Remove(result!);
        return true;
    }

    public bool Remove(int weight, T element)
    {
        if (TryGetEntry(entry => entry.Element == element, out Entry? result) == false) 
            return false;

        if (result!.Weight - weight <= 0)
        {
            _elements.Remove(result);
            return true;
        }

        result.Weight -= weight;
        return true;
    }

    public T? GetRandomElement()
    {
        var totalWeight = _elements.Sum(element => element.Weight);
        var randomWeight = _random.Next(totalWeight + 1);
        var tmpWeight = 0;

        foreach (Entry entry in _elements)
        {
            tmpWeight += entry.Weight;

            if (tmpWeight >= randomWeight)
                return entry.Element;
        }

        return null;
    }

    public void ApplyToRandomElement(Action<T> callback)
    {
        T? element = GetRandomElement();

        if (element is null)
            return;

        callback.Invoke(element);
    }

    public T? ApplyToRandomElement(Func<T, T> callback)
    {
        T? element = GetRandomElement();

        if (element is null)
            return null;

        return callback.Invoke(element);
    }

    public TResult? ApplyToRandomElement<TResult>(Func<T, TResult> callback) where TResult : class
    {
        T? element = GetRandomElement();

        if (element is null)
            return null;

        return callback.Invoke(element);
    }

    public override string ToString()
    {
        return _elements.Aggregate("", (current, entry) => current + $"{entry.Element} - {entry.Weight} / ");
    }

    private bool TryGetEntry(Predicate<Entry> predicate, out Entry? entry)
    {
        var entryIndex = _elements.FindIndex(predicate);

        if (entryIndex == -1)
        {
            entry = null;
            return false;
        }

        entry = _elements[entryIndex];
        return true;
    }

    private class Entry
    {
        public Entry(int weight, T element)
        {
            Weight = weight;
            Element = element;
        }

        public T Element { get; }
        public int Weight { get; set; }
    }
}