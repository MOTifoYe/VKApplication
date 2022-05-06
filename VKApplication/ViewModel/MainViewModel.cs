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
        public ObservableCollection<Test> Tests { get; set; }
        public ICollectionView TestsView { get; set; }
        public Page MainContent { get; set; }
        public Test SelectedTest { get; set; }

        public MainViewModel()
        {
            OverlayService.GetInstance().Show = (str) =>
            {
                OverlayService.GetInstance().Text = str;
            };
            Tests = File.Exists("TestsData.json")
                ? JsonConvert.DeserializeObject<ObservableCollection<Test>>
                (File.ReadAllText("TestsData.json")) : new ObservableCollection<Test>();

            Tests.CollectionChanged += (s, e) =>
            {
                File.WriteAllText("TestsData.json", JsonConvert.SerializeObject
                    (Tests));
            };
            BindingOperations.EnableCollectionSynchronization(Tests, new object());
            TestsView = CollectionViewSource.GetDefaultView(Tests);
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

                                Tests.Add(new Test
                                {
                                    Name = Path.GetFileNameWithoutExtension(file),
                                    Comment = DateTime.Now.ToString(),
                                });

                                Task.Delay(500).Wait();
                            }
                            SelectedTest = Tests.FirstOrDefault(s => s.Path == opd.FileNames.FirstOrDefault());

                            OverlayService.GetInstance().Close();
                        });
                    }

                });
            }
        }
    }
}
