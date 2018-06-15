using System;
using WinApp.Estructuras.CommonBasicComponents;

namespace WinApp.Estructuras.CommonAggregateComponents
{
    [Serializable]
    public class TaxTotal
    {
        public PayableAmount TaxableAmount { get; set; }

        public PayableAmount TaxAmount { get; set; }

        public TaxSubtotal TaxSubtotal { get; set; }

        public TaxTotal()
        {
            TaxableAmount = new PayableAmount();
            TaxAmount = new PayableAmount();
            TaxSubtotal = new TaxSubtotal();
        }
    }
}
