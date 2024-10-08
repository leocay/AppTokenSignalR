using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace AppTokenSignalR;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<AccountInforModel> _listAccountInforModels;

    public MainWindowViewModel()
    {
        _listAccountInforModels = new ObservableCollection<AccountInforModel>();    
    }
}
