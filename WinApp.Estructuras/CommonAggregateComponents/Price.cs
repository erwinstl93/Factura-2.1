using System;
using WinApp.Estructuras.CommonBasicComponents;

namespace WinApp.Estructuras.CommonAggregateComponents
{
    [Serializable]
    public class Price
    {
        public PayableAmount PriceAmount { get; set; }

        public Price()
        {
            PriceAmount = new PayableAmount();
        }
    }
}
