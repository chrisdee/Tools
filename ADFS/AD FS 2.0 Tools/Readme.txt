Introduction
This article covers two different problems that might be encountered in large AD FS 2.0 deployments with multiple AD FS 2.0 federation servers in multiple environments.

Create relying party - Creating a relying party with the wizard could be cumbersome and time consuming. It is convenient to have a tool which will export a relying party definition (i.e. name, identifiers, SAML endpoints, ...) in an XML file. The XML file can be used on another server (in another environment) for the relying party trust creation or for the modification of the parameters of the existing relying party.
Search event log for activity ID - Finding the event log entry with the specific activity ID could be cumbersome and time consuming. It is convenient to have a tool which will search AD FS 2.0 admin event logs on multiple servers to find the event log entries with the specific activity ID.
1. Create Relying Party
In AD FS 2.0 that acts as Identity Provider in a large deployment with dozens (or hundreds) of Relying Parties, there are some common tasks that have to be performed:

Create relying party on one server that is the same as the one already defined on a different server (e.g. in different environments, e.g. test and production)
Create a new relying party that is similar to an already existing one
Backup the relying party definition
A relying party in AD FS 2.0 can be created in different ways:

Import data about the relying party published online or on a local network
Import data about the relying party from a file
Enter data about the relying party manually
All three ways are not convenient for common tasks mentioned further above.

The AD FS tools provide three console applications, GetRelyingParty, AddRelyingParty and UpdateRelyingParty. The console applications must be executed on a (primary) AD FS 2.0 Federation Server.

GetRelyingParty

The GetRelyingParty takes the name of the AD FS relying party as a parameter and produces the XML file with the same name as relying party name in the folder where it is executed. For example, the statement ...

 Collapse | Copy Code
GetRelyingParty "salesforce test"
...produces an XML file with the name salesforce test.xml. The XML file has the information needed for the creation of the same relying party on another server. The XML file can be used as a basis for creation of the similar relying party. The relying party properties that are stored in the XML file are:

Name
Identifiers
SamlEndpoints
WsFedEndpoint
AutoUpdateEnabled
MonitoringEnabled
MetadataUrl
SignatureAlgorithm
RequestSigningCertificates
IssuanceTransformRules
IssuanceAuthorizationRules
DelegationAuthorizationRules
EncryptClaims
EncryptedNameIdRequired
EncryptionCertificate
EncryptionCertificateRevocationCheck
NotBeforeSkew
Notes
ProtocolProfile
SamlResponseSignature
SignedSamlRequestsRequired
SigningCertificateRevocationCheck
TokenLifetime
The certificates in the XML file are in base64 encoded DER format. The Convert.ToBase64String(X509Certificate2.RawData) is used for converting the certificate object to a string. The new X509Certificate2(Convert.FromBase64String(base64DERString)) is used to create certificate object from the base64 encoded DER string. You can use certificate management tool to create base64 DER from the certificates installed on a server. An example of the XML file is given below:

 Collapse | Copy Code
<?xml version="1.0" encoding="utf-8"?>
<RelyingPartyTrust>
  <Name>WIF sample application (with SAML 2.0 extension CTP)</Name>
  <Identifiers>
    <Identifier>https://idp.company.com/ServiceProvider</Identifier>
  </Identifiers>
  <SamlEndpoints>
    <SamlEndpoint>
      <Binding>POST</Binding>
      <Protocol>SAMLAssertionConsumer</Protocol>
      <Index>0</Index>
      <IsDefault>true</IsDefault>
      <Location>https://idp.company.com/ServiceProvider/</Location>
      <ResponseLocation />
    </SamlEndpoint>
    <SamlEndpoint>
      <Binding>Artifact</Binding>
      <Protocol>SAMLAssertionConsumer</Protocol>
      <Index>1</Index>
      <IsDefault>false</IsDefault>
      <Location>https://idp.company.com/ServiceProvider/</Location>
      <ResponseLocation />
    </SamlEndpoint>
    <SamlEndpoint>
      <Binding>Redirect</Binding>
      <Protocol>SAMLLogout</Protocol>
      <Index>0</Index>
      <IsDefault>false</IsDefault>
      <Location>https://idp.company.com/ServiceProvider/</Location>
      <ResponseLocation />
    </SamlEndpoint>
  </SamlEndpoints>
  <WsFedEndpoint />
  <AutoUpdateEnabled>false</AutoUpdateEnabled>
  <MonitoringEnabled>false</MonitoringEnabled>
  <MetadataUrl />
  <SignatureAlgorithm>http://www.w3.org/2001/04/xmldsig-more#rsa-sha256
  </SignatureAlgorithm>
  <RequestSigningCertificates />
  <IssuanceTransformRules>@RuleTemplate = "LdapClaims"
@RuleName = "Retrieve AD attributes and send them as outgoing claims"
c:[Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/windowsaccountname", 
Issuer == "AD AUTHORITY"]
 =&gt; issue(store = "Active Directory", 
 types = ("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", 
 "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress", 
 "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn", 
 "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"), 
 query = ";displayName,mail,userPrincipalName,employeeType;{0}", param = c.Value);
 
@RuleTemplate = "LdapClaims"
@RuleName = "Sent userAccountControl"
c:[Type == http://schemas.microsoft.com/ws/2008/06/identity/claims/windowsaccountname, 
 Issuer == "AD AUTHORITY"]
 =&gt; issue(store = "Active Directory", 
 types = (http://idp.company.com/useraccountcontrol), 
 query = ";userAccountControl;{0}", param = c.Value);
 
@RuleTemplate = "MapClaims"
@RuleName = "Calculate status from userAccountControl"
c:[Type == "http://idp.company.com/useraccountcontrol", Value =~ "^(?i)512$"]
 =&gt; issue(Type = http://idp.company.com/useraccountstatus, 
Issuer = c.Issuer, OriginalIssuer = c.OriginalIssuer, Value = "active", 
ValueType = c.ValueType);
 
  </IssuanceTransformRules>
  <IssuanceAuthorizationRules>@RuleTemplate = "AllowAllAuthzRule"
 =&gt; issue(Type = http://schemas.microsoft.com/authorization/claims/permit, 
 Value = "true");
  </IssuanceAuthorizationRules>
  <DelegationAuthorizationRules></DelegationAuthorizationRules>
  <EncryptClaims>true</EncryptClaims>
  <EncryptedNameIdRequired>false</EncryptedNameIdRequired>
  <EncryptionCertificate>MIeC6DtaAdCgA...HgTe7Q==</EncryptionCertificate>
  <EncryptionCertificateRevocationCheck>CheckChainExcludeRoot
  </EncryptionCertificateRevocationCheck>
  <NotBeforeSkew>0</NotBeforeSkew>
  <Notes>The SAML 2.0 extension test.</Notes>
  <ProtocolProfile>WsFed-SAML</ProtocolProfile>
  <SamlResponseSignature>AssertionOnly</SamlResponseSignature>
  <SignedSamlRequestsRequired>false</SignedSamlRequestsRequired>
  <SigningCertificateRevocationCheck>CheckChainExcludeRoot
  </SigningCertificateRevocationCheck>
  <TokenLifetime>0</TokenLifetime>
</RelyingPartyTrust>
AddRelyingParty

The AddRelyingParty takes the XML file as a parameter and creates a relying party on the (primary) AD FS 2.0 Federation Server where it is executed. For example:

 Collapse | Copy Code
AddRelyingParty "salesforce test.xml"
creates a new relying party with the parameters specified in the file "salesforce test.xml".

UpdateRelyingParty

The UpdateRelyingParty takes the relying party name and an XML file as parameters and updates the relying party with the specified name on the (primary) AD FS 2.0 Federation Server where it is executed. The relying party name cannot be changed using UpdateRelyingParty. However the identifier(s) and all other relying party parameters can be changed. For example:

 Collapse | Copy Code
UpdateRelyingParty "salesforce test" "salesforce test.xml"
updates the relying party with the name "salesforce test" using the parameters from the file "salesforce test.xml". The name itself cannot be changed.

2. Search Event Log for Activity ID
When an error happens in AD FS 2.0 during a token issuance, the activity ID is displayed in the generic error page in browser based scenarios (as Reference number or Additional data).

Generic error page
The activity ID is of type GUID. The error message is logged in the event log of the federation server which was processing the token issuance.

Event log
The AD FS tools provide the console applications GetEventLog. The console application must be executed under the account that has rights to read event log on the federation servers. For example:

 Collapse | Copy Code
GetEventLog "0724A8D0-D873-4CB3-8762-C9A70084DD98" 
		"server1.company.com,server2.company.com"
... searches for the event log entry with the Correlation Activity ID set to 0724A8D0-D873-4CB3-8762-C9A70084DD98 on the servers server1.company.com and server2.company.com in the AD FS 2.0 Admin event log. It writes to console the event ID and the timestamp followed by the event message. An example of the command output is given in the screenshot below:

Command output
Using the Code
The code leverages AD FS PowerShell API and LINQ to XML for XML handling and System.Diagnostics namespace with XPath queries for AD FS 2.0 event log analysis.

History
28-Sep-2011 - Original version posted
29-Sep-2011 - Included all available relying party attributes
04-Oct-2011 - Included event log analysis tool