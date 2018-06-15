using System;
using WinApp.Estructuras.SunatAggregateComponents;

namespace WinApp.Estructuras.CommonExtensionComponents
{
    [Serializable]
    public class ExtensionContent
    {
        public AdditionalInformation AdditionalInformation { get; set; }

        public ExtensionContent()
        {
            AdditionalInformation = new AdditionalInformation();
        }
    }
}
