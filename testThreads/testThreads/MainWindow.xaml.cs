using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace testThreads
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ClassInfo _worker;
        public MainWindow()
        {
            InitializeComponent();
            button.Click += Button_Click;
            button1.Click += ButtonStop_Click;
            Loaded += MainWindow_Loaded;         
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromSeconds(40);
            dt.Tick += Dt_Tick;
            dt.Start();                        
        }

        private void Dt_Tick(object sender, EventArgs e)
        {
            funTime();            
            _worker = new ClassInfo();
            _worker.ProcessChanged += _worker_ProcessChanged;
            _worker.WorkCompleted += _worker_WorkCompleted;
            _worker.DataSave += _worker_DataSave;
            button.IsEnabled = false;
            Thread thread = new Thread(_worker.Work);
            thread.Start();
        }

        private void funTime()
        {
            Action action = () =>
            {
                while (true)
                {
                    Dispatcher.Invoke((Action)(() => label.Content = DateTime.Now.ToLongTimeString()));
                    Thread.Sleep(1000);
                }
            };
            Task task = Task.Factory.StartNew(action);
        }

        private void ButtonStop_Click(object sender, RoutedEventArgs e)
        {
            _worker.Cancel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _worker = new ClassInfo();
            _worker.ProcessChanged += _worker_ProcessChanged;
            _worker.WorkCompleted += _worker_WorkCompleted;
            _worker.DataSave += _worker_DataSave;
            button.IsEnabled = false;
            Thread thread = new Thread(_worker.Work);
            thread.Start();
        }

        private void _worker_DataSave(bool obj)
        {
            Action action = () =>
            {
                dataGrid.ItemsSource = _worker.list;
            };
            if (!Dispatcher.CheckAccess()) Dispatcher.Invoke(action);
            else action();
           
        }

        private void _worker_WorkCompleted(bool cancelled)
        {
            Action action = () =>
                {
                    string mes = cancelled ? "Процесс отменен" : "Процесс завершен";
                    MessageBox.Show(mes);
                    button.IsEnabled = true;
                };
            if (!Dispatcher.CheckAccess()) Dispatcher.Invoke(action);
            else action();
            
        }

        private void _worker_ProcessChanged(int progress)
        {
            Action action = () =>
            {
                progressBar.Value = progress;
            };
            if (!Dispatcher.CheckAccess()) Dispatcher.Invoke(action);
            else action();          
        }      
    }
}
