using Core.Common.Contracts;
using Core.Common.Core;
using System;
using System.Runtime.Serialization;


namespace CarRental.Business.Entieties
{
    [DataContract]
    public class Rental : EntityBase, IIdentifiableEntity, IAccountOwnedEntity
    {
        [DataMember]
        public int RentalId { get; set; }

        [DataMember]
        public int AccountId { get; set; }

        [DataMember]
        public int CarId { get; set; }

        [DataMember]
        public DateTime DataRented { get; set; }

        [DataMember]
        public DateTime DateDue { get; set; }

        [DataMember]
        public DateTime? DateReturned { get; set; }

        [DataMember]
        public decimal RentalPrice { get; set; }

        [DataMember]
        public bool CurrentlyRented { get; set; }

        #region IIdentifiableEntity members

        public int EntityId
        {
            get => RentalId;
            set => RentalId = value;
        }

        #endregion

        #region IAccountOwnedEntity members

        int IAccountOwnedEntity.OwnerAccountId => AccountId;

        #endregion
    }
}
