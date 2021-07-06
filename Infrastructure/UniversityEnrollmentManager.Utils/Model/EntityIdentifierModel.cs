using System;
using System.Collections.Generic;
using System.Text;
using UniversityEnrollmentManager.Utils.Interfaces;

namespace UniversityEnrollmentManager.Utils
{
    public class EntityIdentifierModel<TKey> : IWithIdentifier<TKey>
    {
        public TKey Id { get; set; }
    }
}
