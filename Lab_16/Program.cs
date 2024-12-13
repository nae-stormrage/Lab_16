using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab_16
{
    class RawDataItem
    {
        public String Name { get; set; }
        public float Price { get; set; }
        public float Count { get; set; }
        public String Name_Shop { get; set; }
        public float Sum
        {
            get { return Count * Price; }
        }
    }

    class AverageDataItem
    {
        public String AvgNameShop { get; set; }
        public float AvgAveragePrice { get; set; }
    }

    interface DataInterface
    {
        List<RawDataItem> GetRawData();
        List<AverageDataItem> GetAverageData();
    }

    class DataStorage : DataInterface
    {
        public bool IsReady
        {
            get
            {
                if (rawData == null) return false;
                else return true;
            }
        }

        private List<RawDataItem> rawData;
        private List<AverageDataItem> averageData;
        private DataStorage() { }

        private bool InitData(String datapath)
        {
            rawData = new List<RawDataItem>();
            
            try
            {
                StreamReader sr = new StreamReader(datapath, Encoding.UTF8);
                String line;
                while ((line = sr.ReadLine()) != null) 
                {
                    string[] items = line.Split('/');
                    var item = new RawDataItem()
                    {
                        Name = items[0].Trim(),
                        Price = Convert.ToSingle(items[1].Trim()),
                        Count = Convert.ToSingle(items[2].Trim()),
                        Name_Shop = items[3].Trim(),
                    };
                    rawData.Add(item);
                }

                sr.Close();
                BuildAverage();

            } catch(IOException ex)
            {
                return false;
            }
            
            return true;
        }

        private void BuildAverage()
        {
            Dictionary<string, (float Sum, int Count)> stats = new Dictionary<string, (float Sum, int Count)>();
            foreach (var item in rawData)
            {
                if (stats.ContainsKey(item.Name_Shop))
                {
                    stats[item.Name_Shop] = (stats[item.Name_Shop].Sum + item.Price, stats[item.Name_Shop].Count + 1);
                }
                else
                {
                    stats[item.Name_Shop] = (item.Price, 1);
                }
            }

            averageData = new List<AverageDataItem>();
            foreach (var stat in stats)
            {
                Console.WriteLine(stat);
                averageData.Add(new AverageDataItem
                {
                    AvgNameShop = stat.Key,
                    AvgAveragePrice = stat.Value.Sum / stat.Value.Count
                });
            }
        }

        public static DataStorage DataCreator(string path)
        {
            DataStorage d = new DataStorage();
            if (d.InitData(path))
                return d;
            else return null;
        }

        public List<AverageDataItem> GetAverageData()
        {
            if(this.IsReady)
                return averageData;
            else return null;
        }

        public List<RawDataItem> GetRawData()
        {
            if (this.IsReady)
                return rawData;
            else return null;
        }
    }

    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
