using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace cwg.sourcePEwithPS
{
    class GatherInfo
    {

        public string operatingSystem; //string variable for storing operating system
        public string userName; //string variable for storing username
        public string ipv4address; //string variable for ipv4adress
        public string hostName;

        public GatherInfo()
        {
            operatingSystem = Environment.OSVersion.ToString();
            userName = Environment.UserName;
            hostName = Dns.GetHostName();
            ipv4address = Dns.GetHostByName(hostName).AddressList[1].ToString();
        }
    }


}
