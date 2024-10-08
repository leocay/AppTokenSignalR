using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppTokenSignalR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _token = string.Empty;
        private HubConnection? _connection;
        private MainWindowViewModel mainWindowViewModel;

        public MainWindow()
        {
            mainWindowViewModel = new MainWindowViewModel();    
            InitializeComponent();
            this.DataContext = mainWindowViewModel;
        }

        // Nút Login: Lấy JWT Token
        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // URL của API để lấy token
                var apiUrl = "http://192.168.1.49:5154/api/auth/login";

                // Thông tin đăng nhập (giả sử là "testuser"/"password")
                var loginData = new
                {
                    Username = "testuser",
                    Password = "password"
                };

                using (var client = new HttpClient())
                {
                    var jsonContent = JsonConvert.SerializeObject(loginData);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(apiUrl, content);
                    response.EnsureSuccessStatusCode();

                    var responseBody = await response.Content.ReadAsStringAsync();
                    var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseBody);

                    _token = tokenResponse?.Token;

                    if (!string.IsNullOrEmpty(_token))
                    {
                        LogBox.Text += "Login successful! Token acquired.\n";
                    }
                    else
                    {
                        LogBox.Text += "Failed to retrieve token.\n";
                    }
                }
            }
            catch (Exception ex)
            {
                LogBox.Text += $"Error during login: {ex.Message}\n";
            }
        }

        // Nút Connect: Kết nối tới SignalR Hub với Token
        private async void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_token))
            {
                LogBox.Text += "Please login first to get the token.\n";
                return;
            }

            try
            {
                _connection = new HubConnectionBuilder()
                    .WithUrl("http://192.168.1.49:5154/hub/chat", options =>
                    {
                        options.AccessTokenProvider = () => Task.FromResult(_token)!;
                    })
                    .WithAutomaticReconnect()
                    .Build();

                _connection.On<ObservableCollection<AccountInforModel>>("ReceiveNewMap", (list) =>
                {
                    mainWindowViewModel.ListAccountInforModels = list;
                });

                await _connection.StartAsync();
                LogBox.Text += "Connected to SignalR Hub.\n";
            }
            catch (Exception ex)
            {
                LogBox.Text += $"Error connecting to SignalR: {ex.Message}\n";
            }
        }
    }

    // Lớp để chứa token từ API trả về
    public class TokenResponse
    {
        public string Token { get; set; }
    }
}
