using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace Domain
{
    [Serializable]
    public class Item: TableEntity
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
