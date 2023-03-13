using DeviceManager.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using System.IO;
using Microsoft.Win32;

namespace DeviceManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Device> devices;        
        public MainWindow()
        {
            InitializeComponent();           
        }
        private void datasource_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            DeviceGridContainer.Children.Clear();
            if (!String.IsNullOrEmpty(datasource_textbox.Text))
            {
                try
                {
                    JsonSerializerSettings config = new JsonSerializerSettings { DateParseHandling = DateParseHandling.None };
                    JSONDataSource dataSource = JsonConvert.DeserializeObject<JSONDataSource>(datasource_textbox.Text, config);
                    if (dataSource.Generators.Count != 0)
                    {
                        devices = new List<Device>();
                        for (int i = 0; i < dataSource.Generators.Count; i++)
                        {
                            Device device = dataSource.Generators[i];
                            device.Data = dataSource.DataSets;
                            devices.Add(device);
                            RowDefinition row = new RowDefinition();
                            row.Height = GridLength.Auto;                            
                            DeviceGridContainer.RowDefinitions.Add(row);
                            ColumnDefinition columnDefinition = new ColumnDefinition() { Width = GridLength.Auto};
                            DeviceGridContainer.ColumnDefinitions.Add(columnDefinition);
                            DeviceView deviceView = new DeviceView();
                            deviceView.Device = device;                           
                            deviceView.VerticalAlignment= VerticalAlignment.Stretch;
                            deviceView.HorizontalAlignment= HorizontalAlignment.Stretch;
                            Grid.SetRow(deviceView, i);
                            Grid.SetColumn(deviceView, 0);
                            DeviceGridContainer.Children.Add(deviceView);
                        }
                    }
                }
                catch (Exception ex)
                {
                    errorMessage.Text = "Invalid JSON. Please supply a valid JSON data";
                }
            }
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                foreach (Device device in devices)
                {
                    device.Stop = true;
                }
            }));
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if (devices != null && devices.Count != 0)
            {
                Dispatcher.Invoke((Action)(() =>
                {
                    foreach (Device device in devices)
                    {
                        device.Stop = false;
                        device.PerformOperationAsync();
                    }

                }));
            }
            else 
                errorMessage.Text = "No devices are configured to run. Please load valid json to start";
        }

        private void LoadFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;

            openFileDialog.Filter = "JSON files (*.json)|*.json";
            if (openFileDialog.ShowDialog() == true)
                datasource_textbox.Text = File.ReadAllText(openFileDialog.FileName);

        }
    }
}
