using System;

namespace WinApp.Estructuras.SunatAggregateComponents
{
    [Serializable]
    public class SunatCosts
    {
        public SunatRoadTransport RoadTransport { get; set; }

        public SunatCosts()
        {
            RoadTransport = new SunatRoadTransport();
        }
    }
}
