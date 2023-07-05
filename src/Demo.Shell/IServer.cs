using System.Runtime.InteropServices;

namespace Demo.Shell
{
    [ComVisible(true)]
    [Guid(ContractGuids.ServerInferface)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IServer
    {
        void Start();
    }
}