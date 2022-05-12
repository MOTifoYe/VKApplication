using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKApplication.Model;

namespace VKApplication.Model
{
    public class KeyWordItem : BaseVM
    {
        public string Value { get; set; }

        public KeyWordItem(string value)
        {
            Value = value;
        }
    }
}
