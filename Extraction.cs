using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ELTManagement
{
    class Extraction
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
