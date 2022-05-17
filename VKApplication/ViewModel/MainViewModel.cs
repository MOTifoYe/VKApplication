using DevExpress.Mvvm;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using VKApplication.Model;
using VKApplication.App.Model;
using VKApplication.App.ViewModel;
using VKApplication.App.Views;

namespace VKApplication.ViewModel
{
    public class MainViewModel : BaseVM
    {
        public ObservableCollection<Item> Items { get; set; }
        public ICollectionView ItemsView { get; set; }
        public Page MainContent { get; set; }
        public Item SelectedItem { get; set; }

        private string _SearchText { get; set; }
        public string SearchText
        {
            get => _SearchText;
            set
            {
                _SearchText = value;
                ItemsView.Filter = (obj) =>
                {
                    if (obj is Item item)
                    {
                        switch (SearchText.FirstOrDefault())
                        {
                            case '@': return item.KeyWords.FirstOrDefault(s => s.Value.ToLower().Contains(SearchText.Remove(0, 1).ToLower())) != null;
                            case '$':
                                if (DateTime.TryParse(SearchText.Remove(0, 1), out DateTime date))
                                    return (item.DateUpload.Date == date.Date) || (item.DateOfChange.Date == date.Date);
                                return false;

                            default: return item.Name.ToLower().Contains(SearchText.ToLower());
                        }
                    }

                    return false;
                };
                ItemsView.Refresh();

            }
        }
        
        public MainViewModel()
        {
            OverlayService.GetInstance().Show = (str) =>
            {
                OverlayService.GetInstance().Text = str;
            };
            Items = File.Exists("ItemsData.json")
                  ? JsonConvert.DeserializeObject<ObservableCollection<Item>>
                  (File.ReadAllText("ItemsData.json")) : new ObservableCollection<Item>();

            Items.CollectionChanged += (s, e) =>
            {
                File.WriteAllText("ItemsData.json", JsonConvert.SerializeObject
                    (Items));
            };
            BindingOperations.EnableCollectionSynchronization(Items, new object());
            ItemsView = CollectionViewSource.GetDefaultView(Items);
        }

        public ICommand Sort
        {
            get
            {
                return new DelegateCommand(() =>
                {


                    if (ItemsView.SortDescriptions.Count > 0)
                    {
                        ItemsView.SortDescriptions.Clear();
                    }
                    else
                    {
                        ItemsView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                    }
                });
            }
        }
        public ICommand DeleteItem
        {
            get
            {
                return new DelegateCommand<Item>((item) =>
                {
                    Items.Remove(item);
                    SelectedItem = Items.FirstOrDefault();

                }, (item) => item != null);
            }
        }
        public ICommand AddItem
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    var opd = new OpenFileDialog();
                    opd.Multiselect = true;
                    if (opd.ShowDialog() == true)
                    {
                        await Task.Factory.StartNew(() =>
                        {
                            OverlayService.GetInstance().Show("Загрузка информации...");

                            for (int i = 0; i < opd.FileNames.Length; i++)
                            {
                                OverlayService.GetInstance().Show($"Загрузка информации...{Environment.NewLine}{i}/{opd.FileNames.Length}");
                                var file = opd.FileNames[i];

                                Items.Add(new Item
                                {
                                    Name = Path.GetFileNameWithoutExtension(file),
                                    Type = Path.GetExtension(file),
                                    Size = new FileInfo(file).Length / 1024.0,
                                    DateUpload = DateTime.Now,
                                    DateOfChange = new FileInfo(file).LastWriteTime,
                                    Path = Path.GetFullPath(file),
                                });

                                Task.Delay(500).Wait();
                            }
                            SelectedItem = Items.FirstOrDefault(s => s.Path == opd.FileNames.FirstOrDefault());

                            OverlayService.GetInstance().Close();
                        });
                    }

                });
            }
        }
        public ICommand EditItem
        {
            get
            {
                return new DelegateCommand<Item>((item) =>
                {
                    var w = new EditItemWindow();
                    var vm = new EditItemViewModel
                    {
                        ItemInfo = item,
                    };
                    w.DataContext = vm;
                    w.ShowDialog();
                    File.WriteAllText("VideosData.json", JsonConvert.SerializeObject(Items));

                }, (item) => item != null);
            }
        }
        public ICommand GoToUrl
        {
            get
            {
                return new DelegateCommand<string>((url) =>
                {
                    if (new Uri(url).IsFile)
                    {
                        Process.Start(new ProcessStartInfo("explorer.exe", " /select, " + url));
                    }
                    else
                    {
                        Process.Start(url);
                    }


                });
            }
        }
        public ICommand DataClick
        {
            get
            {
                return new DelegateCommand<DateTime>((date) =>
                {
                    SearchText = "$" + date.Date.ToShortDateString();

                });
            }
        }


    }
}


/*
 * И ТАК. 
 * МОЖНО ДОБАВИТЬ ФАЙЛ 
 * 
 * ДОБАВИТЬ КОМАНДУ ИЗМЕНЕНИЯ
 * 
 * ЖЕСТТЬ ДА
 *
 */