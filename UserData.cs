using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drop
{
    public class UserData
    {
        string name;
        string type;
        object data;
        public string DisplayName
        {
            get { return name; }
            set { name = value; }
        }
        public string DataType
        {
            get { return type; }
            set { type = value.ToLower(); }
        }
        public object Data
        {
            get { return data; }
            set { data = value; }
        }
    }
}
