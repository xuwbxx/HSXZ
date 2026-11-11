namespace FDScanSystem.DAO
{
    partial class DBDataContext
    {
    }

    partial class FD_DataValue2
    {
        public string XlSlcation { set; get; }
    }

    partial class FD_FileUpload
    {
        public string TypeName
        {
            get
            {
                if (this.FileType == 0)
                {
                    return "月报";
                }
                return "年报";
            }
        }
    }
}
