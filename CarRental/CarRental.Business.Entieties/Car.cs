using Core.Common.Contracts;
using Core.Common.Core;
using System.Runtime.Serialization;

namespace CarRental.Business.Entities
{   // we can point data contract namespace explicitly via namespace attr /*(Namespace = "http://www.pluralsight.com/carrental")*/
    [DataContract] //For serialization, only caries data the easiest one
    public class Car : EntityBase, IIdentifiableEntity
    {
		//test
        [DataMember]
        public int CarId { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Color { get; set; }

        [DataMember]
        public int Year { get; set; }

        [DataMember]
        public decimal RentalPrice { get; set; }

        [DataMember]
        public bool CurrentlyRented { get; set; }

        #region IIdentifiableEntity members

        public int EntityId
        {
            get => CarId;
            set => CarId = value;
        }

        #endregion
    }
}
