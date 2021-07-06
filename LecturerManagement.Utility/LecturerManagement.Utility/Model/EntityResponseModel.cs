using System;
using System.Collections;
using System.Collections.Generic;
namespace LecturerManagement.Utility.Model
{
    public class EntityResponseModel<T>
    {
        //public bool IsLogout { get; set; }
        public bool ReturnStatus { get; set; }
        public List<string> ReturnMessage { get; set; }
        public Hashtable Errors;             
        public T Data;
        public EntityResponseModel()
        {
            ReturnMessage = new List<String>();
            //IsLogout = false;
            ReturnStatus = true;
            Errors = new Hashtable();
            Data = default(T);
        }
    }
}
