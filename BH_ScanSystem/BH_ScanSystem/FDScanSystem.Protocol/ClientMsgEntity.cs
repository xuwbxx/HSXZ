using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FDScanSystem.Protocol
{
    public class ClientMsgEntity
    {

        public ClientMsgEntity()
        {
            Paramters = new Dictionary<string, object>();
            Identity = Guid.NewGuid().ToString();
        }

        public string Identity { get; set; }

        public string Command { get; set; }

        public Dictionary<string, object> Paramters { get; set; }

        public void AddParameter(string key, object val)
        {
            if (Paramters == null) Paramters = new Dictionary<string, object>();
            if (Paramters.ContainsKey(key))
            {
                Paramters.Add(key, val);
            }
            else
            {
                Paramters[key] = val;
            }
        }


        public string ToString()
        {
            return ZJFramework10.Serialize.Helper.SerializeString(this);
        }


        public static ClientMsgEntity DeSerialize(string str)
        {
            return ZJFramework10.Serialize.Helper.DeserializeString<ClientMsgEntity>(str);
        }

    }
}
