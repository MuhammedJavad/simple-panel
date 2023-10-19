using Domain.Common.Extensions;
using Domain.Infrastructure;

namespace Domain.Common.Types;

public interface IPaging
{
    public bool NeedToShow { get; }
    public bool IsEmpty { get; }
    public bool HasNext { get; }
    public bool HasPrev { get; }
    public int CurrentPage { get; }
    void SetPage(int index);
    void SetSize(int size);
    ValueTask<bool> MoveNextAsync();
}

public abstract class Paging<T> : IPaging, IAsyncEnumerable<T[]>, IAsyncEnumerator<T[]> where T : class
{
    private const int StartingPage = 0;
    private readonly int _totalCount;
    private int _size = 50;
    private int LastPage => _totalCount / _size;
    protected readonly ISqlConnectionFactory ConnectionFactory;
    protected Paging(int totalCount, ISqlConnectionFactory connectionFactory)
    {
        _totalCount = totalCount;
        ConnectionFactory = connectionFactory;
    }
    public T[] Current { get; private set; }
    public bool NeedToShow => true;
    public bool IsEmpty => Current.Empty();
    public bool HasNext => CurrentPage < LastPage;
    public bool HasPrev => (CurrentPage - 1) > StartingPage;
    public int CurrentPage { get; private set; } = StartingPage;
    public void SetPage(int index) => CurrentPage = index;
    public void SetSize(int size) => _size = size;

    public ValueTask DisposeAsync()
    {
        return Dispose();
    }

    public async ValueTask<bool> MoveNextAsync()
    {
        var items = await Fetch(CurrentPage * _size, _size);
        Current = items.ToArray();
        if (Current.Empty()) return false;
        if (HasNext) CurrentPage++;
        return true;
    }

    public async IAsyncEnumerator<T[]> GetAsyncEnumerator(CancellationToken cancellationToken = new())
    {
        while (await MoveNextAsync())
        {
            yield return Current;
        }
    }

    protected abstract Task<IEnumerable<T>> Fetch(int skip, int take);
    protected abstract ValueTask Dispose();
}