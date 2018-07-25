using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELTManagement
{
    public class Results
    {
        //This class will store the results of each import process
        //The results will be displayed on the Import Results web page

        //private strings to store statistics 
        //Statistics about the Raw Data Imported
        private int rawDataRecordsImported, rawDataRecordsOmitted;
        private List<string> errorsInRawData = new List<string>();
        //Statistics about the transformationProcess
        private int acceptedRecords_transform, rejectedRecords_transform;
        private Dictionary<string, string> transformationErrorsList = new Dictionary<string, string>();
        //Statistics about the insertion process
        private int recordsInsertedToDestination, recordDeletedFromDestination, recordsRejectedFromDestination;
        private List<string> destinationRejectionReasons = new List<string>();


        private List<string> ErrorsInTransformation;

        //Properties to access the import statistics 
        public int RawDataRecordsImported
        {
            get
            {
                return rawDataRecordsImported;
            }

            set
            {
                rawDataRecordsImported = value;
            }
        }

        public int RawDataRecordsOmitted
        {
            get
            {
                return rawDataRecordsOmitted;
            }

            set
            {
                rawDataRecordsOmitted = value;
            }
        }

        public List<string> ErrorsInRawData
        {
            get
            {
                return errorsInRawData;
            }

            set
            {
                errorsInRawData = value;
            }
        }

        public Dictionary<string, string> TransformationErrorsList
        {
            get
            {
                return transformationErrorsList;
            }

            set
            {
                transformationErrorsList = value;
            }
        }

        public int AcceptedRecords_transform
        {
            get
            {
                return acceptedRecords_transform;
            }

            set
            {
                acceptedRecords_transform = value;
            }
        }

        public int RejectedRecords_transform
        {
            get
            {
                return rejectedRecords_transform;
            }

            set
            {
                rejectedRecords_transform = value;
            }
        }

        public int RecordsInsertedToDestination
        {
            get
            {
                return recordsInsertedToDestination;
            }

            set
            {
                recordsInsertedToDestination = value;
            }
        }

        public int RecordDeletedFromDestination
        {
            get
            {
                return recordDeletedFromDestination;
            }

            set
            {
                recordDeletedFromDestination = value;
            }
        }

        public int RecordsRejectedFromDestination
        {
            get
            {
                return recordsRejectedFromDestination;
            }

            set
            {
                recordsRejectedFromDestination = value;
            }
        }

        public List<string> DestinationRejectionReasons
        {
            get
            {
                return destinationRejectionReasons;
            }

            set
            {
                destinationRejectionReasons = value;
            }
        }
    }

}