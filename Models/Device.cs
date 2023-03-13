using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace DeviceManager.Models
{
    public class Device : INotifyPropertyChanged
    {
        private int id;
        private string name;
        private Operations operation;
        private int interval;
        private string log;
        private List<float[]> data;
        private bool stop;

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public Operations Operation { get => operation; set => operation = value; }
        public int Interval { get => interval; set => interval = value; }
        public string Log
        {
            get => log;
            set
            {
                if (value == log) return;
                log = value;
                OnPropertyChanged();
            }
        }
        public List<float[]> Data { get => data; set => data = value; }

        public event PropertyChangedEventHandler PropertyChanged;

        public Boolean Stop { get => stop; set => stop = value; }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task PerformOperationAsync()
        {
            while (!this.stop)
            {
                for (int i = 0; i < this.Data.Count; i++)
                {
                    float calcValue = 0;
                    switch (this.operation)
                    {
                        case Operations.Avg:
                            calcValue = Queryable.Average(this.Data[i].AsQueryable());
                            break;
                        case Operations.Min:
                            calcValue = Queryable.Min(this.Data[i].AsQueryable());
                            break;
                        case Operations.Max:
                            calcValue = Queryable.Max(this.Data[i].AsQueryable());
                            break;
                        case Operations.Sum:
                            calcValue = Queryable.Sum(this.Data[i].AsQueryable());
                            break;
                    }
                    this.Log = this.log + "\n" + DateTime.Now.ToString("HH:mm:ss") + $" {this.Name} " + calcValue;
                }
                this.Log = this.log + "\n==================================\n";
                await Task.Delay(TimeSpan.FromSeconds(this.Interval));
            }
        }
    }
}
