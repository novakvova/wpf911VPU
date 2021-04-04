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
        bool abort = false;
        public MainWindow()
        {
            InitializeComponent();
            
            DataSeed.SeedDataAsync(_context);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var list = _context.Cats
                .Select(x => new CatVM()
                {
                    Name = x.Name,
                    Birthday = x.Birthday,
                    Details = x.Details,
                    ImageUrl = x.Image,
                    Price = x.AppCatPrices
                        .OrderByDescending(x=>x.DateCreate)
                        .FirstOrDefault().Price
                }).ToList();
            _cats = new ObservableCollection<CatVM>(list);
            dgSimple.ItemsSource = _cats;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddCatWindow addCat = new AddCatWindow(this._cats);
            addCat.Show();
        }

        private void btnValidation_Click(object sender, RoutedEventArgs e)
        {
            UserView window = new UserView();
            window.Show();
        }

        private void btnPauseAddRange_Click(object sender, RoutedEventArgs e)
        {
        }
        private void btnCancelAddRange_Click(object sender, RoutedEventArgs e)
        {
            //ShowMessage();
            abort = true;
        }
        private void btnAddRange_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Thread id: {0}", Thread.CurrentThread.ManagedThreadId);
            //btnAddRange.IsEnabled = false;
            //ShowMessage();
            Thread thread = new Thread(ShowMessage);
            thread.Start();
        }
        private void ShowMessage()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                btnAddRange.IsEnabled = false;
            }));
            ICatService catService = new CatService();
            catService.EventInsertItem += UpdateUIAsync;
            catService.InsertCats(240);
            
        }

        void UpdateUIAsync(int i)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                btnAddRange.Content = $"{i}";
                Debug.WriteLine("Thread id: {0}", Thread.CurrentThread.ManagedThreadId);
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
