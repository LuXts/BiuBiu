using MagicOnion;

namespace BiuBiuShare.Tests
{
    public interface IMyTestService : IService<IMyTestService>
    {
        // The return type must be `UnaryResult<T>`.

        UnaryResult<int> SumAsync(int x, int y);

        UnaryResult<int> SumAsync1(int x, int y);

        UnaryResult<int> SumAsync2(int x, int y);
    }
}