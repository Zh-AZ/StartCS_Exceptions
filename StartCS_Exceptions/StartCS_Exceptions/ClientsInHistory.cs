using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartCS_Exceptions
{
    public class ClientsInHistory
    {
        public string WhoChanged { get; set; }
        public string WhatChanged { get; set;}
        public DateTime TimeChanged { get; set;}
    
        public ClientsInHistory() { }   
        
        public ClientsInHistory(string whoChanged, string whatChanged, DateTime timeChanged)
        {
            WhoChanged = whoChanged;
            WhatChanged = whatChanged;
            TimeChanged = timeChanged;
        }
    }
}
