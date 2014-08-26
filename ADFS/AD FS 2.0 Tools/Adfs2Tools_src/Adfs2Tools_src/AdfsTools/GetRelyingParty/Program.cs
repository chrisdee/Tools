using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityServer.PowerShell.Commands;
using Microsoft.IdentityServer.PowerShell.Resources;
using Microsoft.IdentityServer.PolicyModel.Configuration;


namespace GetRelyingParty
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: GetRelyingParty name");
                return;
            }

            GetRelyingPartyTrustCommand getRelyingPartyTrustCommand = new GetRelyingPartyTrustCommand();
            getRelyingPartyTrustCommand.Name = new string[1];
            getRelyingPartyTrustCommand.Name[0] = args[0]; // salesforce iHCP
            IEnumerable result = getRelyingPartyTrustCommand.Invoke();

            RelyingPartyTrust rp = null;
            foreach (object obj in result)
            {
                rp = obj as RelyingPartyTrust;
                break;
            }
            if (rp == null)
            {
                Console.WriteLine("No relying party found.");
                return;
            }

            XElement relyingPartyTrustXml = new XElement("RelyingPartyTrust");
            
            XElement nameXml = new XElement("Name", rp.Name);
            relyingPartyTrustXml.Add(nameXml);

            XElement identifiersXml = new XElement("Identifiers");
            int i = 0;
            foreach (string identifier in rp.Identifier)
            {
                XElement identifierXml = new XElement("Identifier", rp.Identifier[i++]);
                identifiersXml.Add(identifierXml);
            }
            relyingPartyTrustXml.Add(identifiersXml);

            XElement samlEndpointsXml = new XElement("SamlEndpoints");
            foreach (SamlEndpoint samlEndpoint in rp.SamlEndpoints)
            {
                XElement samlEndpointXml =
                    new XElement("SamlEndpoint",
                        new XElement("Binding", samlEndpoint.Binding),
                        new XElement("Protocol", samlEndpoint.Protocol),
                        new XElement("Index", samlEndpoint.Index),
                        new XElement("IsDefault", samlEndpoint.IsDefault),
                        new XElement("Location", samlEndpoint.Location == null ? null : samlEndpoint.Location.ToString()),
                        new XElement("ResponseLocation", samlEndpoint.ResponseLocation == null ? null : samlEndpoint.ResponseLocation.ToString())
                    );
                samlEndpointsXml.Add(samlEndpointXml);
            }
            relyingPartyTrustXml.Add(samlEndpointsXml);

            XElement wsFedEndpointXml = new XElement("WsFedEndpoint", rp.WSFedEndpoint == null ? null : rp.WSFedEndpoint.ToString());
            relyingPartyTrustXml.Add(wsFedEndpointXml);

            XElement autoUpdateEnabledXml = new XElement("AutoUpdateEnabled", rp.AutoUpdateEnabled);
            relyingPartyTrustXml.Add(autoUpdateEnabledXml);

            XElement monitoringEnabledXml = new XElement("MonitoringEnabled", rp.MonitoringEnabled);
            relyingPartyTrustXml.Add(monitoringEnabledXml);

            XElement metadataUrlXml = new XElement("MetadataUrl", rp.MetadataUrl == null ? null : rp.MetadataUrl.ToString());
            relyingPartyTrustXml.Add(metadataUrlXml);

            XElement signatureAlgorithmXml = new XElement("SignatureAlgorithm", rp.SignatureAlgorithm);
            relyingPartyTrustXml.Add(signatureAlgorithmXml);

            XElement requestSigningCertificatesXml = new XElement("RequestSigningCertificates");
            foreach (X509Certificate2 x509Certificate2 in rp.RequestSigningCertificate)
            {
                // x509Certificate2.RawData contains ASN.1 DER data
                XElement requestSigningCertificateXml = new XElement("RequestSigningCertificate", Convert.ToBase64String(x509Certificate2.RawData));
                requestSigningCertificatesXml.Add(requestSigningCertificateXml); 
            }
            relyingPartyTrustXml.Add(requestSigningCertificatesXml);

            XElement issuanceTransformRulesXml = new XElement("IssuanceTransformRules", rp.IssuanceTransformRules);
            relyingPartyTrustXml.Add(issuanceTransformRulesXml);

            XElement issuanceAuthorizationRulesXml = new XElement("IssuanceAuthorizationRules", rp.IssuanceAuthorizationRules);
            relyingPartyTrustXml.Add(issuanceAuthorizationRulesXml);

            XElement delegationAuthorizationRulesXml = new XElement("DelegationAuthorizationRules", rp.DelegationAuthorizationRules);
            relyingPartyTrustXml.Add(delegationAuthorizationRulesXml);

            XElement encryptClaimsXml = new XElement("EncryptClaims", rp.EncryptClaims);
            relyingPartyTrustXml.Add(encryptClaimsXml);

            XElement encryptedNameIdRequiredXml = new XElement("EncryptedNameIdRequired", rp.EncryptedNameIdRequired);
            relyingPartyTrustXml.Add(encryptedNameIdRequiredXml);

            XElement encryptionCertificateXml = new XElement("EncryptionCertificate", rp.EncryptionCertificate == null ? null : Convert.ToBase64String(rp.EncryptionCertificate.RawData));
            relyingPartyTrustXml.Add(encryptionCertificateXml);

            XElement encryptionCertificateRevocationCheckXml = new XElement("EncryptionCertificateRevocationCheck", rp.EncryptionCertificateRevocationCheck);
            relyingPartyTrustXml.Add(encryptionCertificateRevocationCheckXml);

            XElement notBeforeSkewXml = new XElement("NotBeforeSkew", rp.NotBeforeSkew);
            relyingPartyTrustXml.Add(notBeforeSkewXml);

            XElement notesXml = new XElement("Notes", rp.Notes);
            relyingPartyTrustXml.Add(notesXml);

            XElement protocolProfileXml = new XElement("ProtocolProfile", rp.ProtocolProfile);
            relyingPartyTrustXml.Add(protocolProfileXml);

            XElement samlResponseSignatureXml = new XElement("SamlResponseSignature", rp.SamlResponseSignature);
            relyingPartyTrustXml.Add(samlResponseSignatureXml);

            XElement signedSamlRequestsRequiredXml = new XElement("SignedSamlRequestsRequired", rp.SignedSamlRequestsRequired);
            relyingPartyTrustXml.Add(signedSamlRequestsRequiredXml);

            XElement signingCertificateRevocationCheckXml = new XElement("SigningCertificateRevocationCheck", rp.SigningCertificateRevocationCheck);
            relyingPartyTrustXml.Add(signingCertificateRevocationCheckXml);

            XElement tokenLifetimeXml = new XElement("TokenLifetime", rp.TokenLifetime);
            relyingPartyTrustXml.Add(tokenLifetimeXml);

            Console.WriteLine(relyingPartyTrustXml);
            relyingPartyTrustXml.Save(rp.Name + ".xml");
        }
    }
}


