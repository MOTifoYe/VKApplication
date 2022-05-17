using DevExpress.Mvvm;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VKApplication.Model;
using VKApplication.App.Model;

namespace VKApplication.App.ViewModel
{
    class EditItemViewModel : BaseVM
    {
        public Item ItemInfo { get; set; }

        public DelegateCommand AddKeyWord
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    ItemInfo.KeyWords.Add(new KeyWordItem(""));
                });
            }
        }

        public DelegateCommand<KeyWordItem> DeleteKeyWord
        {
            get
            {
                return new DelegateCommand<KeyWordItem>((keyword) =>
                {
                    if (keyword != null)
                    {
                        ItemInfo.KeyWords.Remove(keyword);
                    }
                });
            }
        }

        public DelegateCommand<Window> Save
        {
            get
            {
                return new DelegateCommand<Window>((w) =>
                {
                    foreach (var key in ItemInfo.KeyWords)
                    {
                        if (DataBase.GetInstance().KeyWords.FirstOrDefault(s => key.Value == s) == null)
                        {
                            DataBase.GetInstance().KeyWords.Add(key.Value);
                        }
                    }
                    w?.Close();
                });
            }
        }
    }
}

