using System;
using WinApp.Estructuras.CommonBasicComponents;

namespace WinApp.Estructuras.CommonAggregateComponents
{
    [Serializable]
    public class AllowanceCharge
    {
        public bool ChargeIndicator { get; set; }

        public PayableAmount Amount { get; set; }

        public AllowanceCharge()
        {
            Amount = new PayableAmount();
        }
    }
}
