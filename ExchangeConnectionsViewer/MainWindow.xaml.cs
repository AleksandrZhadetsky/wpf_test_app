using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExchangeConnectionsViewer;

public partial class MainWindow : Window
{
    public ObservableCollection<ExchangeConnection> Connections { get; set; }

    public MainWindow()
    {
        InitializeComponent();
        InitializeData();
        DataContext = this;
    }

    private void InitializeData()
    {
        Connections = new ObservableCollection<ExchangeConnection>
        {
            new ExchangeConnection 
            { 
                Status = "Connected", 
                Title = "Binance", 
                Type = "Spot", 
                LastConnect = DateTime.Now.AddHours(-2) 
            },
            new ExchangeConnection 
            { 
                Status = "Disconnected", 
                Title = "Coinbase", 
                Type = "Spot", 
                LastConnect = DateTime.Now.AddDays(-1) 
            },
            new ExchangeConnection 
            { 
                Status = "Connected", 
                Title = "Kraken", 
                Type = "Futures", 
                LastConnect = DateTime.Now.AddMinutes(-30) 
            }
        };
    }

    private void EditButtonClickHandler(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.DataContext is ExchangeConnection connection)
        {
            var dialog = new EditConnectionDialog(connection);
            dialog.Owner = this;
            
            if (dialog.ShowDialog() == true && dialog.EditedConnection != null)
            {
                int index = Connections.IndexOf(connection);
                if (index >= 0)
                {
                    Connections[index] = dialog.EditedConnection;
                }
            }
        }
    }

    private void DeleteButtonClickHandler(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.DataContext is ExchangeConnection connection)
        {
            var result = MessageBox.Show($"Вы уверены, что хотите удалить подключение '{connection.Title}'?", 
                "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            if (result == MessageBoxResult.Yes)
            {
                Connections.Remove(connection);
            }
        }
    }

    private void AddButtonClickHandler(object sender, RoutedEventArgs e)
    {
        var dialog = new EditConnectionDialog(new ExchangeConnection(DateTime.Now));
        dialog.Owner = this;
        
        if (dialog.ShowDialog() == true && dialog.EditedConnection != null)
        {
            Connections.Add(dialog.EditedConnection);
        }
    }
}