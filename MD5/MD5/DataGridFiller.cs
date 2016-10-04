using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MD5
{
    class DataGridFiller
    {
        public void Fill (DataGrid dataGrid, Dictionary<string, List<string>> duplicates)
        {
            Dictionary<string, string> temp = new Dictionary<string, string>();
            foreach (string name in duplicates.Keys)
            {
                temp.Add(name, String.Join(",\n", duplicates[name]));
            }
            
            dataGrid.ItemsSource = temp;


        }
    }
}
