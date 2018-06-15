using System;

namespace WinApp.Estructuras.CommonAggregateComponents
{
    [Serializable]
    public class DigitalSignatureAttachment
    {
        public ExternalReference ExternalReference { get; set; }

        public DigitalSignatureAttachment()
        {
            ExternalReference = new ExternalReference();
        }
    }
}
