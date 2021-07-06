using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Intellix.Core.CQRS.Queries
{
    public class EntityPagedResult<TReadModel>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public long Total { get; set; }       
        public bool ReturnStatus { get; set; }
        public List<string> ReturnMessage { get; set; }
        public Hashtable Errors;
        public List<TReadModel> Data { get; set; }
        public EntityPagedResult()
        {
            ReturnMessage = new List<String>();
            ReturnStatus = true;
            Errors = new Hashtable();
            Data = new List<TReadModel>();
        }

    }
}
