using System.Threading.Tasks;
using WinApp.Comun.Dto.Intercambio;
using System.Security.Cryptography.X509Certificates;
using System;
using System.Xml;
using System.Text;
using WinApp.Comun.Constantes;
using System.IO;
using System.Security.Cryptography.Xml;

namespace WinApp.Firmado
{
    public class Certificador : ICertificador
    {
        public Task<FirmadoResponse> FirmarXml(FirmadoRequest request)
        {
            throw new System.NotImplementedException();
        }

        async Task<FirmadoResponse> ICertificador.FirmarXml(FirmadoRequest request)
        {
            var task = Task.Factory.StartNew(() =>
            {

                var response = new FirmadoResponse();

                var certificate = new X509Certificate2();
                certificate.Import(Convert.FromBase64String(request.CertificadoDigital),
                    request.PasswordCertificado, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.UserKeySet);

                var xmlDoc = new XmlDocument();

                string resultado;

                var betterBytes = Encoding.Convert(Encoding.GetEncoding(Formatos.EncodingIso),
                    Encoding.GetEncoding(Formatos.EncodingIso),
                    Convert.FromBase64String(request.TramaXmlSinFirma));

                using (var documento = new MemoryStream(betterBytes))
                {
                    xmlDoc.PreserveWhitespace = false;
                    xmlDoc.Load(documento);

                   
                    var indiceNodo = 0;

                    var nodoExtension = xmlDoc.GetElementsByTagName("ExtensionContent", EspacioNombres.ext)
                        .Item(indiceNodo);
                    if (nodoExtension == null)
                        throw new InvalidOperationException("No se pudo encontrar el nodo ExtensionContent en el XML");
                    nodoExtension.RemoveAll();

                    // Creamos el objeto SignedXml.
                    var signedXml = new SignedXml(xmlDoc) { SigningKey = certificate.PrivateKey };
                    var xmlSignature = signedXml.Signature;

                    var env = new XmlDsigEnvelopedSignatureTransform();

                    var reference = new Reference(string.Empty);
                    reference.AddTransform(env);
                    xmlSignature.SignedInfo.AddReference(reference);

                    var keyInfo = new KeyInfo();
                    var x509Data = new KeyInfoX509Data(certificate);

                    x509Data.AddSubjectName(certificate.Subject);

                    keyInfo.AddClause(x509Data);
                    xmlSignature.KeyInfo = keyInfo;
                    xmlSignature.Id = "SignOpenInvoicePeru";
                    signedXml.ComputeSignature();

                    // Recuperamos el valor Hash de la firma para este documento.
                    if (reference.DigestValue != null)
                        response.ResumenFirma = Convert.ToBase64String(reference.DigestValue);
                    response.ValorFirma = Convert.ToBase64String(signedXml.SignatureValue);

                    nodoExtension.AppendChild(signedXml.GetXml());

                    using (var memDoc = new MemoryStream())
                    {

                        using (var writer = XmlWriter.Create(memDoc,
                            new XmlWriterSettings { Encoding = Encoding.GetEncoding(Formatos.EncodingIso) }))
                        {
                            xmlDoc.WriteTo(writer);
                        }

                        resultado = Convert.ToBase64String(memDoc.ToArray());

                    }
                }
                response.TramaXmlFirmado = resultado;

                return response;
            });

            return await task;
        }
    }
}
