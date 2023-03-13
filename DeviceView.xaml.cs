using DeviceManager.Models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace DeviceManager
{
    /// <summary>
    /// Interaction logic for DeviceView.xaml
    /// </summary>
    public partial class DeviceView : UserControl
    {       

        public Device Device
        {
            get { return (Device)GetValue(DeviceProperty); }
            set { SetValue(DeviceProperty, value); }
        }        
        public static readonly DependencyProperty DeviceProperty =
            DependencyProperty.Register("Device", typeof(Device), typeof(DeviceView), new PropertyMetadata(null));

         public Array Operations { get; set; }

        public DeviceView()
        {
            InitializeComponent();
            Operations = Enum.GetValues(typeof(Operations));
            Operation_CB.ItemsSource= Operations;
            this.DataContext = this.Device;

        }
    }   
}
