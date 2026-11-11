using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.AlertService
{
    public interface ISMSService
    {
        void SMSSend(double? value);
    }
}
