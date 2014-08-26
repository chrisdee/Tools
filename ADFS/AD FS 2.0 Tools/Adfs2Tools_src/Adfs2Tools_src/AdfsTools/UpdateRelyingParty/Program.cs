using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityServer.PowerShell.Commands;
using Microsoft.IdentityServer.PowerShell.Resources;

namespace UpdateRelyingParty
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: UpdateRelyingParty name filename");
                return;
            }

            XElement relyingPartyTrustXml = XElement.Load(args[1]);

            string name = relyingPartyTrustXml.Element("Name").Value;

            string[] identifiers = null;
            foreach (XElement identifiersXml in relyingPartyTrustXml.Elements("Identifiers"))
            {
                int i = 0;
                identifiers = new string[identifiersXml.Elements("Identifier").Count()];
                foreach (XElement identifierXml in identifiersXml.Elements("Identifier"))
                {
                    identifiers[i++] = identifierXml.Value;
                }
            }

            SamlEndpoint[] samlEndpoints = null;
            foreach (XElement samlEndpointsXml in relyingPartyTrustXml.Elements("SamlEndpoints"))
            {
                int i = 0;
                samlEndpoints = new SamlEndpoint[samlEndpointsXml.Elements("SamlEndpoint").Count()];
                foreach (XElement samlEndpointXml in samlEndpointsXml.Elements("SamlEndpoint"))
                {
                    NewSamlEndpointCommand newSamlEndpointCommand = new NewSamlEndpointCommand();
                    newSamlEndpointCommand.Binding = samlEndpointXml.Element("Binding").Value;
                    newSamlEndpointCommand.Uri = new Uri(samlEndpointXml.Element("Location").Value);
                    newSamlEndpointCommand.IsDefault = bool.Parse(samlEndpointXml.Element("IsDefault").Value);
                    newSamlEndpointCommand.Index = int.Parse(samlEndpointXml.Element("Index").Value);
                    newSamlEndpointCommand.Protocol = samlEndpointXml.Element("Protocol").Value;
                    newSamlEndpointCommand.ResponseUri = String.IsNullOrEmpty(samlEndpointXml.Element("ResponseLocation").Value) ? null : new Uri(samlEndpointXml.Element("ResponseLocation").Value);
                    IEnumerable commandResults = newSamlEndpointCommand.Invoke();
                    SamlEndpoint samlEndpoint = null;
                    foreach (SamlEndpoint commandResult in commandResults)
                    {
                        samlEndpoint = commandResult;
                    }
                    samlEndpoints[i++] = samlEndpoint;
                }
            }

            Uri wsFedEndpoint = String.IsNullOrEmpty(relyingPartyTrustXml.Element("WsFedEndpoint").Value) ? null : new Uri(relyingPartyTrustXml.Element("WsFedEndpoint").Value);

            X509Certificate2[] requestSigningCertificates = null;
            foreach (XElement requestSigningCertificatesXml in relyingPartyTrustXml.Elements("RequestSigningCertificates"))
            {
                int i = 0;
                requestSigningCertificates = new X509Certificate2[requestSigningCertificatesXml.Elements("RequestSigningCertificate").Count()];
                foreach (XElement requestSigningCertificateXml in requestSigningCertificatesXml.Elements("RequestSigningCertificate"))
                {
                    requestSigningCertificates[i++] = new X509Certificate2(Convert.FromBase64String(requestSigningCertificateXml.Value));
                }
            }

            X509Certificate2 encryptionCertificate = null;
            if (!String.IsNullOrEmpty(relyingPartyTrustXml.Element("EncryptionCertificate").Value))
            {
                encryptionCertificate = new X509Certificate2(Convert.FromBase64String(relyingPartyTrustXml.Element("EncryptionCertificate").Value));
            }

            string issuanceTransformRules = relyingPartyTrustXml.Element("IssuanceTransformRules").Value;
            string issuanceAuthorizationRules = relyingPartyTrustXml.Element("IssuanceAuthorizationRules").Value;
            string delegationAuthorizationRules = relyingPartyTrustXml.Element("DelegationAuthorizationRules").Value;
            bool autoUpdateEnabled = bool.Parse(relyingPartyTrustXml.Element("AutoUpdateEnabled").Value);
            bool monitoringEnabled = bool.Parse(relyingPartyTrustXml.Element("MonitoringEnabled").Value);
            Uri metadataUrl = String.IsNullOrEmpty(relyingPartyTrustXml.Element("MetadataUrl").Value) ? null : new Uri(relyingPartyTrustXml.Element("MetadataUrl").Value);
            string signatureAlgorithm = relyingPartyTrustXml.Element("SignatureAlgorithm").Value;
            bool encryptClaims = bool.Parse(relyingPartyTrustXml.Element("EncryptClaims").Value);
            bool encryptedNameIdRequired = bool.Parse(relyingPartyTrustXml.Element("EncryptedNameIdRequired").Value);
            string encryptionCertificateRevocationCheck = relyingPartyTrustXml.Element("EncryptionCertificateRevocationCheck").Value;
            int notBeforeSkew = int.Parse(relyingPartyTrustXml.Element("NotBeforeSkew").Value);
            string notes = relyingPartyTrustXml.Element("Notes").Value;
            string protocolProfile = relyingPartyTrustXml.Element("ProtocolProfile").Value;
            string samlResponseSignature = relyingPartyTrustXml.Element("SamlResponseSignature").Value;
            bool signedSamlRequestsRequired = bool.Parse(relyingPartyTrustXml.Element("SignedSamlRequestsRequired").Value);
            string signingCertificateRevocationCheck = relyingPartyTrustXml.Element("SigningCertificateRevocationCheck").Value;
            int tokenLifetime = int.Parse(relyingPartyTrustXml.Element("TokenLifetime").Value);

            SetRelyingPartyTrustCommand setRelyingPartyTrustCommand = new SetRelyingPartyTrustCommand();
            setRelyingPartyTrustCommand.TargetName = args[0];
            setRelyingPartyTrustCommand.Identifier = identifiers;
            setRelyingPartyTrustCommand.SamlEndpoint = samlEndpoints;
            setRelyingPartyTrustCommand.WSFedEndpoint = wsFedEndpoint;
            setRelyingPartyTrustCommand.RequestSigningCertificate = requestSigningCertificates;
            setRelyingPartyTrustCommand.IssuanceTransformRules = issuanceTransformRules;
            setRelyingPartyTrustCommand.IssuanceAuthorizationRules = issuanceAuthorizationRules;
            setRelyingPartyTrustCommand.DelegationAuthorizationRules = delegationAuthorizationRules;
            setRelyingPartyTrustCommand.AutoUpdateEnabled = autoUpdateEnabled;
            setRelyingPartyTrustCommand.MonitoringEnabled = monitoringEnabled;
            setRelyingPartyTrustCommand.MetadataUrl = metadataUrl;
            setRelyingPartyTrustCommand.SignatureAlgorithm = signatureAlgorithm;
            setRelyingPartyTrustCommand.EncryptClaims = encryptClaims;
            setRelyingPartyTrustCommand.EncryptedNameIdRequired = encryptedNameIdRequired;
            setRelyingPartyTrustCommand.EncryptionCertificate = encryptionCertificate;
            setRelyingPartyTrustCommand.EncryptionCertificateRevocationCheck = encryptionCertificateRevocationCheck;
            setRelyingPartyTrustCommand.NotBeforeSkew = notBeforeSkew;
            setRelyingPartyTrustCommand.Notes = notes;
            setRelyingPartyTrustCommand.ProtocolProfile = protocolProfile;
            setRelyingPartyTrustCommand.SamlResponseSignature = samlResponseSignature;
            setRelyingPartyTrustCommand.SignedSamlRequestsRequired = signedSamlRequestsRequired;
            setRelyingPartyTrustCommand.SigningCertificateRevocationCheck = signingCertificateRevocationCheck;
            setRelyingPartyTrustCommand.TokenLifetime = tokenLifetime;

            IEnumerable result = setRelyingPartyTrustCommand.Invoke();

            try
            {
                result.GetEnumerator().MoveNext();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Relying party cannot be updated.");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
