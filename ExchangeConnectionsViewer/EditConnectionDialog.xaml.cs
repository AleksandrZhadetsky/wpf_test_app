using System;
using System.Windows;
using System.Windows.Controls;

namespace ExchangeConnectionsViewer
{
    public partial class EditConnectionDialog : Window
    {
        public ExchangeConnection EditedConnection { get; private set; }
        private ExchangeConnection OriginalConnection;

        public EditConnectionDialog(ExchangeConnection connection)
        {
            InitializeComponent();
            OriginalConnection = connection;
            LoadConnectionData();
        }

        private void LoadConnectionData()
        {

            foreach (ComboBoxItem item in TitleComboBox.Items)
            {
                if (item.Content.ToString() == OriginalConnection.Title)
                {
                    TitleComboBox.SelectedItem = item;
                    break;
                }
            }

            foreach (ComboBoxItem item in TypeComboBox.Items)
            {
                if (item.Content.ToString() == OriginalConnection.Type)
                {
                    TypeComboBox.SelectedItem = item;
                    break;
                }
            }
            
            foreach (ComboBoxItem item in StatusComboBox.Items)
            {
                if (item.Content.ToString() == OriginalConnection.Status)
                {
                    StatusComboBox.SelectedItem = item;
                    break;
                }
            }
            
            DatePicker.SelectedDate = OriginalConnection.LastConnect.Date;
            TimeTextBox.Text = OriginalConnection.LastConnect.ToString("HH:mm");
        }

        private void OkButtonClickHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedDate = DatePicker.SelectedDate ?? DateTime.Now;
                var timeText = TimeTextBox.Text;
                DateTime lastConnect;

                if (DateTime.TryParse(timeText, out var time))
                {
                    lastConnect = selectedDate.Date.Add(time.TimeOfDay);
                }
                else
                {
                    lastConnect = selectedDate;
                }

                EditedConnection = new ExchangeConnection
                {
                    Id = OriginalConnection.Id,
                    Title = (TitleComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "ByBit",
                    Type = (TypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Spot",
                    Status = (StatusComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Connected",
                    LastConnect = lastConnect
                };

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при редактировании подключения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButtonClickHandler(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
} 