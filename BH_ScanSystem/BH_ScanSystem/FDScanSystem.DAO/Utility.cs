using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WisDragon.Framework.Data.DataAccess;

namespace OfficeShop.DAL
{
    public class Utility
    {
        public static void BuilderCommandParameter(Dictionary<string, object> parameters, DataCommand command)
        {
            foreach (KeyValuePair<string, object> kp in parameters)
            {
                if (kp.Value != null && kp.Value.ToString().Length >= 0)
                {
                    command.SetParameterValue(string.Format("@{0}", kp.Key), kp.Value);
                }
            }
        }
    }
}
