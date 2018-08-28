using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ETLComponents
{
    //This class is the parent extraction class. Any class that
    //peforms extraction of raw data will inherit from this one.
    public class Extraction
    {
        protected DataProperties dataProp;
        protected DataSet RawData;
        protected int rowImportedFromSource = 0;

        public Extraction(DataProperties DataProp)
        {
            dataProp = DataProp;
        }

        public int RowImportedFromSource
        {
            get
            {
                return rowImportedFromSource;
            }

            set
            {
                rowImportedFromSource = value;
            }
        }
    }
}
