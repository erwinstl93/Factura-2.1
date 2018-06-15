using System;
using WinApp.Estructuras.CommonBasicComponents;

namespace WinApp.Estructuras.CommonAggregateComponents
{
    [Serializable]
    public class OrderReference
    {
        public string Id { get; set; }

        public OrderTypeCode OrderTypeCode { get; set; }

        public OrderReference()
        {
            OrderTypeCode = new OrderTypeCode();
        }
    }
}
