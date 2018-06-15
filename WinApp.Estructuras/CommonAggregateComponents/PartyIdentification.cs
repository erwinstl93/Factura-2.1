using System;
using WinApp.Estructuras.CommonBasicComponents;

namespace WinApp.Estructuras.CommonAggregateComponents
{
    [Serializable]
    public class PartyIdentification
    {
        public PartyIdentificationId Id { get; set; }

        public PartyIdentification()
        {
            Id = new PartyIdentificationId();
        }
    }
}
