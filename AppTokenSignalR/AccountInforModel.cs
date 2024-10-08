using CommunityToolkit.Mvvm.ComponentModel;

namespace AppTokenSignalR;

public partial class AccountInforModel : ObservableObject
{
    [ObservableProperty]
    private string _name = string.Empty;
    [ObservableProperty]
    private string _map = string.Empty;
    [ObservableProperty]
    private int _sever;
    [ObservableProperty]
    private bool _isChecked;
    [ObservableProperty]
    private bool _isVisible;
    [ObservableProperty]
    private int _noId;

    public int Id;
    public double ProcessId;
    public double ThreadId;
    public IntPtr WindowHandle;

    private static int count = 0;
    public AccountInforModel()
    {

    }

    public AccountInforModel(string name, string map, int sever, double processId, double threadID, IntPtr windownHandle, bool isChecked, bool isVisible)
    {
        count++;
        _name = name;
        _map = map;
        _sever = sever;
        _isChecked = isChecked;
        _isVisible = isVisible;
        Id = count;
        ProcessId = processId;
        ThreadId = threadID;
        WindowHandle = windownHandle;
    }
}
