using System.Runtime.InteropServices;

namespace DemoCore.Contract
{
    [ComVisible(true)]
    [Guid(ContractGuids.ServerInterface)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IServer
    {
        System.Windows.Window StartCore();

        int Sum(int a, int b);

        void StartWPF();
    }
}