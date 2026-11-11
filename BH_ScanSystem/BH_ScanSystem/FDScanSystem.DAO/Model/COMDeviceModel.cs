using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FDScanSystem.DAO.Model
{
    public class COMDeviceModel
    {
        public COMDeviceModel() { Devices = new ScanDeviceCollection(); }

        public int ID { get; set; }

        public string DeviceName { get; set; }

        public int DataProtocol { get; set; }

        public int CommunicationType { get; set; }

        public string CommunicationCode { get; set; }

        public ScanDeviceCollection Devices { get; set; }

    }

    public class ScanDeviceCollection : IList<ScanDevice>
    {
        List<ScanDevice> _innerList;
        public ScanDeviceCollection()
        {
            _innerList = new List<ScanDevice>(5);
        }

        public int IndexOf(ScanDevice item)
        {
            return _innerList.IndexOf(item);
        }

        public void Insert(int index, ScanDevice item)
        {
            _innerList.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _innerList.RemoveAt(index);
        }

        public ScanDevice this[int index]
        {
            get
            {
                return _innerList[index];
            }
            set
            {
                _innerList[index] = value;
            }
        }

        public void Add(ScanDevice item)
        {
            _innerList.Add(item);
        }

        public void Clear()
        {
            _innerList.Clear();
        }

        public bool Contains(ScanDevice item)
        {
            return _innerList.Contains(item);
        }

        public void CopyTo(ScanDevice[] array, int arrayIndex)
        {
            _innerList.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _innerList.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(ScanDevice item)
        {
            return _innerList.Remove(item);
        }

        public IEnumerator<ScanDevice> GetEnumerator()
        {
            return _innerList.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _innerList.GetEnumerator();
        }
    }


    public class ScanDevice
    {
        public int ID { get; set; }
        public string DeviceName { get; set; }

        public int CommunicationDeviceId { get; set; }

        public int DeviceProtocol { get; set; }

        public string DeviceIden { get; set; }

    }
}
