using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Core
{
    [DataContract]  //** If there is any data that is not handled it's gonna be stored in the implemented prop
    public abstract class EntityBase : IExtensibleDataObject
    {
        #region IExtensibleDataObject

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
    }
}
