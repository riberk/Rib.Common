namespace Rib.Common.Application.Wrappers
{
    public interface IWrappersManager : IWrappersInitializer, IWrappersDisposer
    {
        void ClearAll();
    }
}