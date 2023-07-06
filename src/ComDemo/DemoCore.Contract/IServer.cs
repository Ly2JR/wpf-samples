using System.Runtime.InteropServices;

namespace DemoCore.Contract
{
    [ComVisible(true)]
    [Guid(ContractGuids.ServerInferface)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IServer
    {
        void StartCore();
    }
}