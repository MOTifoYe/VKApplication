using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKApplication.Model
{
    public class Item : BaseVM
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Descrition { get; set; }
        public string Path { get; set; }
        public double Size { get; set; }
        public DateTime UploadDate{ get; set; }
        public ObservableCollection<KeyWordItem> KeyWords { get; set; } = new ObservableCollection<KeyWordItem>();
    }
}
