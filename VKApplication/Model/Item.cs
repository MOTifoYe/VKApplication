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
        public string Comment { get; set; }
        public string Path { get; set; }
    }
}
