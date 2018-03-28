using System.Collections.Generic;
using System.IO;

namespace DataProcessing.Data
{
    public static class DataMiner
    {
        private static string Header { get; set; }
        private static string TokenExample { get; set; }

        public static string[] GetData(string path)
        {
            string[] data = Read(path);
            TokenExample = data[1];
            return data;
        }

        private static string[] Read(string path)
        {
            List<string> data = new List<string>();
            using (StreamReader reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    data.Add(reader.ReadLine());
                }
            }
            Header = data[0];
            data.RemoveAt(0);
            return data.ToArray();
        }

        public static string[] GetHeader(char separator)
        {
            if (Header == null)
            {
                throw new FileNotParsedException("Source file is not parsed yet");
            }

            return Header.Split(separator);
        }

        public static DataType[] GetMask(char separator)
        {
            if (TokenExample == null)
            {
                throw new FileNotParsedException("Source file is not parsed yet");
            }

            string[] points = TokenExample.Split(separator);
            DataType[] result = new DataType[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                double value;
                if (double.TryParse(points[i], out value))
                {
                    result[i] = DataType.Numerical;
                }
                else
                {
                    result[i] = DataType.Categorial;
                }
            }
            return result;
        }
    }
}
