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
using VKApplication.ViewModel;

namespace VKApplication.ViewModel
{
    public class MainViewModel : BaseVM
    {
        public ObservableCollection<Item> Items { get; set; }
        public ICollectionView ItemsView { get; set; }
        public Page MainContent { get; set; }
        public Item SelectedItem { get; set; }

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
                                    Comment = DateTime.Now.ToString(),
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
    }
}


/*
 * И ТАК. 
 * МОЖНО ДОБАВИТЬ ФАЙЛ (ТОЛЬКО ИМЯ)
 *       и комент (дата добавления)
 * 
 * НУЖНО ДОБАВИТЬ ВОЗМОЖНОСТЬ ПРОСМОТРА СОДЕРЖИМОГО
 * НЕ ВАЖНО В КАКОМ ФОРМАТЕ БУДЕТ ФАЙЛ
 * 
 * ЖЕСТТЬ ДА
 *
 */