using CatRenta.Application;
using CatRenta.Application.Interfaces;
using CatRenta.EFData;
using CatRenta.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using Wpf.CatRent.Views;

namespace Wpf.CatRent
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<CatVM> _cats = new ObservableCollection<CatVM>();
        private EFDataContext _context = new EFDataContext();
        private ICatService _catService = new CatService();
        ManualResetEvent _mrse = new ManualResetEvent(false);
        bool abort = false;
        public MainWindow()
        {
            InitializeComponent();
            _catService.EventInsertItem += UpdateUIAsync;
        }

        public async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblInfoStatus.Text = "Підключаємося до БД-----";
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            await Task.Run(() =>
            {
                _context.Cats.Count(); //jніціалуємо підклчюення
            });

            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            //Debug.WriteLine("Сідер 1 закінчив свою роботу: " + elapsedTime);
            lblCursorPosition.Text = elapsedTime;
            lblInfoStatus.Text = "Підключення до БД успішно";

            await DataSeed.SeedDataAsync(_context);

            stopWatch = new Stopwatch();
            stopWatch.Start();
            var list = _context.Cats.AsQueryable()//.AsParallel()
                .Select(x => new CatVM()
                {
                    Name = x.Name,
                    Birthday = x.Birthday,
                    Details = x.Details,
                    ImageUrl = x.Image,
                    Price = x.AppCatPrices
                        .OrderByDescending(x => x.DateCreate)
                        .FirstOrDefault().Price
                })
                .OrderBy(x=>x.Name)
                .Skip(0)
                .Take(20)
                .ToList();

            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            //Debug.WriteLine("Сідер 1 закінчив свою роботу: " + elapsedTime);
            lblCursorPosition.Text = elapsedTime;
            lblInfoStatus.Text = "Читання даних із БД успішно";

            _cats = new ObservableCollection<CatVM>(list);
            dgSimple.ItemsSource = _cats;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddCatWindow addCat = new AddCatWindow(this._cats);
            addCat.Show();
        }


        public void Resume() => _mrse.Set();
        public void Pause() => _mrse.Reset();

        private void btnValidation_Click(object sender, RoutedEventArgs e)
        {
            UserView window = new UserView();
            window.Show();
        }

        private void btnPauseAddRange_Click(object sender, RoutedEventArgs e)
        {
            this.Pause();
        }
        
        private void btnCancelAddRange_Click(object sender, RoutedEventArgs e)
        {
            //ShowMessage();
            _catService.CanselAsyncMethod = true;
            //abort = true;
        }
        
        private async void btnAddRange_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Thread id: {0}", Thread.CurrentThread.ManagedThreadId);
            //btnAddRange.IsEnabled = false;
            //ShowMessage();
            //Action action = ShowMessage;
            //Task task = new Task(action);
            //Task task = new Task(() => ShowMessage());
            //task.Start();
            //MessageBox.Show(Environment.ProcessorCount.ToString());
            //Task.Run(() => ShowMessage());

            btnAddRange.IsEnabled = false;

            this.Resume();
            
            int count = 3340;
            pbCats.Maximum = count;
            await _catService.InsertCatsAsync(count, _mrse);
            btnAddRange.IsEnabled = true;

            //Thread thread = new Thread(ShowMessage);
            //thread.IsBackground = true;
            //thread.Start();
        }
        
        private void ShowMessage()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                btnAddRange.IsEnabled = false;
            }));
            ICatService catService = new CatService();
            catService.EventInsertItem += UpdateUIAsync;
            catService.InsertCats(240, _mrse);
            Dispatcher.Invoke(new Action(() =>
            {
                btnAddRange.IsEnabled = true;
            }));

        }

        void UpdateUIAsync(int i)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                btnAddRange.Content = $"{i}";
                pbCats.Value = i;
                //Debug.WriteLine("Thread id: {0}", Thread.CurrentThread.ManagedThreadId);
            }));
            
        }


        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgSimple.SelectedItem != null)
            {
                if (dgSimple.SelectedItem is CatVM)
                {
                    var userView = dgSimple.SelectedItem as CatVM;
                    userView.Birthday = new DateTime(2003, 1, 23);
                    userView.Details = "Пішов в гори!";
                    userView.ImageUrl = "https://i.pinimg.com/originals/ec/5a/a9/ec5aa93a38113ea5b346cb87b5c2c941.jpg";
                }
            }
        }
    }
}
