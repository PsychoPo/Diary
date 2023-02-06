using Diary.Models;
using Diary.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
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

namespace Diary
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string PATH = $"{Environment.CurrentDirectory}\\DiaryDataList.json";
        private BindingList<DiaryModel> _DiaryDataList;
        private FileIOServices _fileIOService;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _fileIOService = new FileIOServices(PATH);
            try
            {
                _DiaryDataList = _fileIOService.LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Close();
            }

            dgDiary.ItemsSource = _DiaryDataList;
            _DiaryDataList.ListChanged += _DiaryDataList_ListChanged;
        }

        private void _DiaryDataList_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded || e.ListChangedType == ListChangedType.ItemDeleted || e.ListChangedType == ListChangedType.ItemChanged)
            {
                try
                {
                    _fileIOService.SaveData(sender);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Close();
                }
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tbx = (TextBox)sender;
            if (tbx.Text != "") 
            {
                var filteredList = _DiaryDataList.Where(x => x.Text.ToLower().Contains(tbx.Text.ToLower()));
                dgDiary.ItemsSource = null;
                dgDiary.ItemsSource = filteredList;
            }
            else
            {
                dgDiary.ItemsSource = _DiaryDataList;
            }
        }
        private void dg_Sorting(object sender, DataGridColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "Дата создания")
            {
                if (e.Column.SortDirection == null || e.Column.SortDirection == ListSortDirection.Descending)
                {
                    dgDiary.ItemsSource = new ObservableCollection<DiaryModel>(from item in _DiaryDataList orderby item.CreationDate ascending select item);
                    e.Column.SortDirection = ListSortDirection.Ascending;
                }
                else
                {
                    dgDiary.ItemsSource = new ObservableCollection<DiaryModel>(from item in _DiaryDataList orderby item.CreationDate descending select item);
                    e.Column.SortDirection = ListSortDirection.Descending;
                }
            }
            else if (e.Column.Header.ToString() == "Сделано")
            {
                if (e.Column.SortDirection == null || e.Column.SortDirection == ListSortDirection.Descending)
                {
                    dgDiary.ItemsSource = new ObservableCollection<DiaryModel>(from item in _DiaryDataList orderby item.IsDone ascending select item);
                    e.Column.SortDirection = ListSortDirection.Ascending;
                }
                else
                {
                    dgDiary.ItemsSource = new ObservableCollection<DiaryModel>(from item in _DiaryDataList orderby item.IsDone descending select item);
                    e.Column.SortDirection = ListSortDirection.Descending;
                }
            }
            else if (e.Column.Header.ToString() == "Текст")
            {
                if (e.Column.SortDirection == null || e.Column.SortDirection == ListSortDirection.Descending)
                {
                    dgDiary.ItemsSource = new ObservableCollection<DiaryModel>(from item in _DiaryDataList orderby item.Text ascending select item);
                    e.Column.SortDirection = ListSortDirection.Ascending;
                }
                else
                {
                    dgDiary.ItemsSource = new ObservableCollection<DiaryModel>(from item in _DiaryDataList orderby item.Text descending select item);
                    e.Column.SortDirection = ListSortDirection.Descending;
                }
            }
        }
    }
}
