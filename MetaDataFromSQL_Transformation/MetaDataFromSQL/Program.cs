using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Diagnostics;


namespace MetaDataFromSQL
{
    class Program
    {
        static void Main(string[] args)
        {
            MetaData Data = new MetaData("Census_Data");
            DataSet RawData = new DataSet();
            DataSet CleanData;
            DataTable CleanDataTable;
            Transformation DataTransformation = new Transformation();
            Importer DataLoader = new Importer();
            LoadData Loader;
            Stopwatch sw = new Stopwatch();

            sw.Start();
            Data.GetMetaData();
            RawData = DataLoader.ImportFile(Data.SourceFileLocation, Data.DataSetName, Data.DelimiterCharacter, Data.NumberOfColumns);
            DataTransformation.TransformData(RawData, Data);
            CleanDataTable = DataTransformation.AcceptableData();
            CleanData = new DataSet();
            CleanData.Tables.Add(CleanDataTable);
            Loader = new LoadData(CleanData, Data);

            sw.Stop();
            Console.Write(sw.Elapsed.Seconds.ToString());
            Console.Write(sw.Elapsed.Minutes.ToString());
            Console.Read();
        }
    }
}
