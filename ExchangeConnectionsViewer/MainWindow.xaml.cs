using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
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
    private const string CONNECTIONS_FILE = @".\connections.json";
    
    public ObservableCollection<ExchangeConnection> Connections { get; set; }

    public MainWindow()
    {
        InitializeComponent();
        InitializeData();
        DataContext = this;
    }

    private void InitializeData()
    {
        Connections = new ObservableCollection<ExchangeConnection>();
        LoadConnectionsFromJson();
    }

    private void LoadConnectionsFromJson()
    {
        try
        {
            if (File.Exists(CONNECTIONS_FILE))
            {
                string jsonContent = File.ReadAllText(CONNECTIONS_FILE);
                var connections = JsonSerializer.Deserialize<ExchangeConnection[]>(jsonContent);
                if (connections != null)
                {
                    foreach (var connection in connections)
                    {
                        Connections.Add(connection);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при загрузке подключений: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private void SaveConnectionsToJson()
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string jsonContent = JsonSerializer.Serialize(Connections, options);
            File.WriteAllText(CONNECTIONS_FILE, jsonContent);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при сохранении подключений: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void EditButtonClickHandler(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.DataContext is ExchangeConnection connection)
        {
            var dialog = new EditConnectionDialog(connection);
            dialog.Owner = this;
            
            dialog.AddHandler(EditConnectionDialog.SaveButtonClickedEvent, new RoutedEventHandler(OnSaveButtonClicked));
            
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
                SaveConnectionsToJson();
            }
        }
    }

    private void AddButtonClickHandler(object sender, RoutedEventArgs e)
    {
        var dialog = new EditConnectionDialog(new ExchangeConnection(DateTime.Now));
        dialog.Owner = this;
        
        dialog.AddHandler(EditConnectionDialog.SaveButtonClickedEvent, new RoutedEventHandler(OnSaveButtonClicked));
        
        if (dialog.ShowDialog() == true && dialog.EditedConnection != null)
        {
            Connections.Add(dialog.EditedConnection);
        }
    }

    private void OnSaveButtonClicked(object sender, RoutedEventArgs e)
    {
        if (sender is EditConnectionDialog dialog && dialog.EditedConnection != null)
        {
            var connection = dialog.EditedConnection;
            
            SaveConnectionsToJson();
            
            MessageBox.Show($"Подключение '{connection.Title}' успешно сохранено в файл {CONNECTIONS_FILE}", 
                "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}