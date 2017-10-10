/*
MIT License

Copyright (c) 2017 David Cassady

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/


//using System;
//using System.Collections.Generic;
//using System.Collections.Concurrent;
//using System.ComponentModel;
//using System.Text;
//using System.Xml;



//namespace csReporter
//{
//    class csObject
//    {
//        private string strCSdn;
//        private string strID;
//        private string strObjectType;
//        private Delta unappliedExport;
//        private Delta escrowedExport;
//        private Delta unconfirmedExport;
//        private Delta pendingImport;
//        private Entry syncHologram;
//        private Entry unappliedExportHologram;
//        private Entry escrowedExportHologram;
//        private Entry unconfirmedExportHologram;
//        private Entry pendingImportHologram;
//        private bool? bConnector;
//        private string strConnState;
//        private bool bImportSeen;
//        private bool bRebuilding;
//        private bool bObsoletion;
//        private bool bNeedFS;
//        private bool bPlaceHolderParent;
//        private bool bPlaceHolderLink;
//        private bool bPlaceHolderDelete;
//        private bool bPending;
//        private bool bRefRetry;
//        private bool bRenameRetry;
//        private string strImportDeltaOp;
//        private string strExportDeltaOp;
//        private bool bPendingRefDelete;
//        private string strMAid;
//        private string strMAname;
//        private string strPartionID;
//        private string strDisconnectionType;
//        private string strDisconnectionID;
//        private DateTime disconTime;
//        private DateTime lastImportDeltaTime;
//        private DateTime lastExportDeltaTime;
//        private string strFQDN;
//        private string strDomain;
//        private string strAccountName;
//        private string strPartitionName;
//        private Sequencer csSequencer;
//        private string strUPN;
//        private DateTime connectedTime;
//        private string connectionOperation;
//        private ExportError exportError;

//        public csObject(XmlNode csObjectNode)
//        {
//            try
//            {
//                foreach (XmlAttribute xmlAttr in csObjectNode.Attributes)
//                {
//                    switch (xmlAttr.Name)
//                    {
//                        case "cs-dn":
//                            strCSdn = xmlAttr.Value;
//                            break;
//                        case "object-type":
//                            strObjectType = xmlAttr.Value;
//                            break;
//                        case "id":
//                            strID = xmlAttr.Value;
//                            break;
//                    }
//                }

//                foreach (XmlNode childNode in csObjectNode.ChildNodes)
//                {
//                    switch (childNode.Name)
//                    {
//                        case "connector":
//                            bConnector = Convert.ToBoolean(Convert.ToInt16(childNode.InnerText));
//                            break;
//                        case "connector-state":
//                            strConnState = childNode.InnerText;
//                            break;
//                        case "seen-by-import":
//                            bImportSeen = Convert.ToBoolean(Convert.ToInt16(childNode.InnerText));
//                            break;
//                        case "rebuild-in-progress":
//                            bRebuilding = Convert.ToBoolean(Convert.ToInt16(childNode.InnerText));
//                            break;
//                        case "obsoletion":
//                            bObsoletion = Convert.ToBoolean(Convert.ToInt16(childNode.InnerText));
//                            break;
//                        case "need-full-sync":
//                            bNeedFS = Convert.ToBoolean(Convert.ToInt16(childNode.InnerText));
//                            break;
//                        case "placeholder-parent":
//                            bPlaceHolderParent = Convert.ToBoolean(Convert.ToInt16(childNode.InnerText));
//                            break;
//                        case "placeholder-link":
//                            bPlaceHolderLink = Convert.ToBoolean(Convert.ToInt16(childNode.InnerText));
//                            break;
//                        case "placeholder-delete":
//                            bPlaceHolderDelete = Convert.ToBoolean(Convert.ToInt16(childNode.InnerText));
//                            break;
//                        case "pending":
//                            bPending = Convert.ToBoolean(Convert.ToInt16(childNode.InnerText));
//                            break;
//                        case "ref-retry":
//                            bRefRetry = Convert.ToBoolean(Convert.ToInt16(childNode.InnerText));
//                            break;
//                        case "rename-retry":
//                            bRenameRetry = Convert.ToBoolean(Convert.ToInt16(childNode.InnerText));
//                            break;
//                        case "import-delta-operation":
//                            strImportDeltaOp = childNode.InnerText;
//                            break;
//                        case "export-delta-operation":
//                            strExportDeltaOp = childNode.InnerText;
//                            break;
//                        case "pending-ref-delete":
//                            bPendingRefDelete = Convert.ToBoolean(Convert.ToInt16(childNode.InnerText));
//                            break;
//                        case "ma-id":
//                            strMAid = childNode.InnerText;
//                            break;
//                        case "ma-name":
//                            strMAname = childNode.InnerText;
//                            break;
//                        case "partition-id":
//                            strPartionID = childNode.InnerText;
//                            break;
//                        case "disconnection-type":
//                            strDisconnectionType = childNode.InnerText;
//                            break;
//                        case "disconnection-id":
//                            strDisconnectionID = childNode.InnerText;
//                            break;
//                        case "disconnection-time":
//                            disconTime = Convert.ToDateTime(childNode.InnerText);
//                            break;
//                        case "last-import-delta-time":
//                            lastImportDeltaTime = Convert.ToDateTime(childNode.InnerText);
//                            break;
//                        case "last-export-delta-time":
//                            lastExportDeltaTime = Convert.ToDateTime(childNode.InnerText);
//                            break;
//                        case "full-qualified-domain-name":
//                            strFQDN = childNode.InnerText;
//                            break;
//                        case "domain-name":
//                            strDomain = childNode.InnerText;
//                            break;
//                        case "account-name":
//                            strAccountName = childNode.InnerText;
//                            break;
//                        case "partition-name":
//                            strPartitionName = childNode.InnerText;
//                            break;
//                        case "user-principal-name":
//                            strUPN = childNode.InnerText;
//                            break;
//                        case "cs-mv-links":
//                            foreach (XmlNode subNode in childNode)
//                            {
//                                if (subNode.Name == "cs-mv-value")
//                                {
//                                    foreach (XmlAttribute xmlAttr in subNode.Attributes)
//                                    {
//                                        switch (xmlAttr.Name)
//                                        {
//                                            case "lineage-time":
//                                                connectedTime = Convert.ToDateTime(xmlAttr.Value);
//                                                break;
//                                            case "lineage-type":
//                                                connectionOperation = xmlAttr.Value;
//                                                break;
//                                        }
//                                    }
//                                }
//                            }
//                            break;
//                        case "mv-link":
//                            foreach (XmlAttribute xmlAttr in childNode.Attributes)
//                            {
//                                switch (xmlAttr.Name)
//                                {
//                                    case "lineage-time":
//                                        connectedTime = Convert.ToDateTime(xmlAttr.Value);
//                                        break;
//                                    case "lineage-type":
//                                        connectionOperation = xmlAttr.Value;
//                                        break;
//                                }
//                            }
//                            break;
//                        case "unapplied-export":
//                            unappliedExport = new Delta(childNode.FirstChild);
//                            break;
//                        case "escrowed-export":
//                            escrowedExport = new Delta(childNode.FirstChild);
//                            break;
//                        case "unconfirmed-export":
//                            unconfirmedExport = new Delta(childNode.FirstChild);
//                            break;
//                        case "pending-import":
//                            pendingImport = new Delta(childNode.FirstChild);
//                            break;
//                        case "synchronized-hologram":
//                            if (childNode.FirstChild != null)
//                            {
//                                syncHologram = new Entry(childNode.FirstChild, this);
//                            }
//                            break;
//                        case "unapplied-export-hologram":
//                            if (childNode.FirstChild != null)
//                            {
//                                unappliedExportHologram = new Entry(childNode.FirstChild, this);
//                            }
//                            break;
//                        case "escrowed-export-hologram":
//                            if (childNode.FirstChild != null)
//                            {
//                                escrowedExportHologram = new Entry(childNode.FirstChild, this);
//                            }
//                            break;
//                        case "unconfirmed-export-hologram":
//                            if (childNode.FirstChild != null)
//                            {
//                                unconfirmedExportHologram = new Entry(childNode.FirstChild, this);
//                            }
//                            break;
//                        case "pending-import-hologram":
//                            if (childNode.FirstChild != null)
//                            {
//                                pendingImportHologram = new Entry(childNode.FirstChild, this);
//                            }
//                            break;
//                        case "sequencers":
//                            csSequencer = new Sequencer(childNode);
//                            break;
//                        case "export-errordetail":
//                            exportError = new ExportError(childNode);
//                            break;
//                    }
//                }
//                //if (syncHologram == null)              { syncHologram = new Entry(); }
//                //if (unappliedExportHologram == null)   { unappliedExportHologram = new Entry(); }
//                //if (escrowedExportHologram == null)    { escrowedExportHologram = new Entry(); }
//                //if (unconfirmedExportHologram == null) { unconfirmedExportHologram = new Entry(); }
//                //if (pendingImportHologram == null)     { pendingImportHologram = new Entry(); }
//            }
//            catch (Exception ex)
//            {
//                if (strCSdn != null)
//                {
//                    throw new Exception("Error occurred in csObject object constructor.  csDN=" + strCSdn, ex);
//                }
//                else
//                {
//                    throw new Exception("Error occurred in csObject object constructor.", ex);
//                }
//            }
//        }

//        public string csDN
//        {
//            get
//            {
//                return strCSdn;
//            }
//        }
//        public string ObjectType
//        {
//            get
//            {
//                return strObjectType;
//            }
//        }
//        public string ID
//        {
//            get
//            {
//                return strID;
//            }
//        }
//        public bool? Connector
//        {
//            get
//            {
//                return bConnector;
//            }
//        }
//        public string ConnectorState
//        {
//            get
//            {
//                return strConnState;
//            }
//        }
//        public bool ImportSeen
//        {
//            get
//            {
//                return bImportSeen;
//            }
//        }
//        public bool Rebuilding
//        {
//            get
//            {
//                return bRebuilding;
//            }
//        }
//        public bool Obsoletion
//        {
//            get
//            {
//                return bObsoletion;
//            }
//        }
//        public bool NeedFullSync
//        {
//            get
//            {
//                return bNeedFS;
//            }
//        }
//        public bool PlaceHolderParent
//        {
//            get
//            {
//                return bPlaceHolderParent;
//            }
//        }
//        public bool PlaceHolderLink
//        {
//            get
//            {
//                return bPlaceHolderLink;
//            }
//        }
//        public bool PlaceHolderDelete
//        {
//            get
//            {
//                return bPlaceHolderDelete;
//            }
//        }
//        public bool Pending
//        {
//            get
//            {
//                return Pending;
//            }
//        }
//        public bool ReferenceRetry
//        {
//            get
//            {
//                return bRefRetry;
//            }
//        }
//        public bool RenameRetry
//        {
//            get
//            {
//                return bRenameRetry;
//            }
//        }
//        public string ImportDeltaOperation
//        {
//            get
//            {
//                return strImportDeltaOp;
//            }
//        }
//        public string ExportDeltaOperation
//        {
//            get
//            {
//                return strExportDeltaOp;
//            }
//        }
//        public bool PendingRefeDelete
//        {
//            get
//            {
//                return bPendingRefDelete;
//            }
//        }
//        public string MAid
//        {
//            get
//            {
//                return strMAid;
//            }
//        }
//        public string MAname
//        {
//            get
//            {
//                return strMAname;
//            }
//        }
//        public string PartitionID
//        {
//            get
//            {
//                return strPartionID;
//            }
//        }
//        public string DisconnectionType
//        {
//            get
//            {
//                return strDisconnectionType;
//            }
//        }
//        public string DisconnectionID
//        {
//            get
//            {
//                return strDisconnectionID;
//            }
//        }
//        public DateTime DisconnectionTime
//        {
//            get
//            {
//                return disconTime;
//            }
//        }
//        public DateTime LastImportDeltaTime
//        {
//            get
//            {
//                return lastImportDeltaTime;
//            }
//        }
//        public DateTime LastExportDeltaTime
//        {
//            get
//            {
//                return lastImportDeltaTime;
//            }
//        }
//        public string FQDN
//        {
//            get
//            {
//                return strFQDN;
//            }
//        }
//        public string Domain
//        {
//            get
//            {
//                return strDomain;
//            }
//        }
//        public string AccountName
//        {
//            get
//            {
//                return strAccountName;
//            }
//        }
//        public string PartitionName
//        {
//            get
//            {
//                return strPartitionName;
//            }
//        }
//        public Delta UnappliedExport
//        {
//            get
//            {
//                return unappliedExport;
//            }
//        }
//        public Delta EscrowedExport
//        {
//            get
//            {
//                return escrowedExport;
//            }
//        }
//        public Delta UnconfirmedExport
//        {
//            get
//            {
//                return unconfirmedExport;
//            }
//        }
//        public Delta PendingImport
//        {
//            get
//            {
//                return pendingImport;
//            }
//        }
//        public Delta Delta(State state)
//        {
//            switch (state)
//            {
//                case State.EscrowedExport:
//                    return escrowedExport;
//                case State.None:
//                    return null;
//                case State.PendingImport:
//                    return pendingImport;
//                case State.UnappliedExport:
//                    return unappliedExport;
//                case State.UnconfirmedExport:
//                    return unconfirmedExport;
//                default:
//                    return null;
//            }
//        }
//        public Entry SynchronizedHologram
//        {
//            get
//            {
//                return syncHologram;
//            }
//        }
//        public Entry UnappliedExportHologram
//        {
//            get
//            {
//                return unappliedExportHologram;
//            }
//        }
//        public Entry EscrowedExportHologram
//        {
//            get
//            {
//                return escrowedExportHologram;
//            }
//        }
//        public Entry UnconfirmedExportHologram
//        {
//            get
//            {
//                return unconfirmedExportHologram;
//            }
//        }
//        public Entry PendingImportHologram
//        {
//            get
//            {
//                return pendingImportHologram;
//            }
//        }
//        public Entry Hologram(State state)
//        {
//            switch (state)
//            {
//                case State.EscrowedExport:
//                    return escrowedExportHologram;
//                case State.None:
//                    return syncHologram;
//                case State.PendingImport:
//                    return pendingImportHologram;
//                case State.UnappliedExport:
//                    return unappliedExportHologram;
//                case State.UnconfirmedExport:
//                    return unconfirmedExportHologram;
//                default:
//                    return null;
//            }
//        }
//        public Sequencer Sequencers
//        {
//            get
//            {
//                return csSequencer;
//            }
//        }
//        public string UserPrincipalName
//        {
//            get
//            {
//                return strUPN;
//            }
//        }
//        public DateTime ConnectionTime
//        {
//            get
//            {
//                return connectedTime;
//            }
//        }
//        public string ConnectionOperation
//        {
//            get
//            {
//                return connectionOperation;
//            }
//        }
//        public ExportError ExportError
//        {
//            get
//            {
//                return exportError;
//            }
//        }
//    }

//    class Delta
//    {
//        private operation opOperation;
//        private string strDN;
//        private List<Attribute> attr = new List<Attribute>();

//        public Delta(XmlNode deltaNode)
//        {
//            try
//            {
//                foreach (XmlAttribute xmlAttr in deltaNode.Attributes)
//                {
//                    switch (xmlAttr.Name)
//                    {
//                        case "operation":
//                            switch (xmlAttr.Value)
//                            {
//                                case "delete-add":
//                                    opOperation = operation.deleteAdd;
//                                    break;
//                                case "add":
//                                    opOperation = operation.add;
//                                    break;
//                                case "replace":
//                                    opOperation = operation.replace;
//                                    break;
//                                case "update":
//                                    opOperation = operation.update;
//                                    break;
//                                case "delete":
//                                    opOperation = operation.delete;
//                                    break;
//                                case "none":
//                                    opOperation = operation.none;
//                                    break;
//                            }
//                            break;
//                        case "dn":
//                            strDN = xmlAttr.Value;
//                            break;
//                        case "newdn":
//                            attr.Add(new Attribute(xmlAttr.Value));
//                            break;
//                    }
//                }

//                foreach (XmlNode childNode in deltaNode.ChildNodes)
//                {
//                    switch (childNode.Name)
//                    {
//                        case "attr":
//                            attr.Add(new Attribute(childNode));
//                            break;
//                        case "dn-attr":
//                            attr.Add(new Attribute(childNode));
//                            break;
//                    }
//                }

//            }
//            catch (Exception ex)
//            {
//                throw new Exception("Error occurred in Delta object constructor.", ex);
//            }
//        }

//        public operation Operation
//        {
//            get
//            {
//                return opOperation;
//            }
//        }
//        public string DN
//        {
//            get
//            {
//                return strDN;
//            }
//        }
//        public List<Attribute> Attributes
//        {
//            get
//            {
//                return attr;
//            }
//        }
//        public List<string> AttributeNames
//        {
//            get
//            {
//                if (attr == null)
//                {
//                    return null;
//                }
//                else
//                {
//                    List<string> Names = new List<string>();
//                    foreach (Attribute bute in attr)
//                    {
//                        Names.Add(bute.Name);
//                    }
//                    return Names;
//                }
//            }
//        }

//        public Attribute AttributeByName(string AttributeName)
//        {
//            foreach (Attribute attrib in attr)
//            {
//                if (attrib.Name == AttributeName)
//                {
//                    return attrib;
//                }
//            }
//            return null;
//        }

//        public int AttribIndexByName(string AttributeName)
//        {
//            for (int i = 0; i < attr.Count; i++)
//            {
//                if (attr[i].Name == AttributeName)
//                {
//                    return i;
//                }
//            }
//            return -1;
//        }
//    }

//    class Entry
//    {
//        private string strDN;
//        private string strPrimaryObjectClass;
//        private string[] strObjectClass = new string[0];
//        private List<Attribute> attr = new List<Attribute>();
//        private csObject parent;

//        public Entry(XmlNode entryNode, csObject parentCSobject)
//        {
//            try
//            {
//                if (parentCSobject != null)
//                {
//                    parent = parentCSobject;
//                }
//                if (entryNode.Attributes.GetNamedItem("dn") != null)
//                {
//                    strDN = entryNode.Attributes.GetNamedItem("dn").Value;
//                    attr.Add(new Attribute(strDN));
//                }
//                //Attribute tempAttribute = new Attribute();
//                foreach (XmlNode childNode in entryNode.ChildNodes)
//                {
//                    switch (childNode.Name)
//                    {
//                        case "primary-objectclass":
//                            strPrimaryObjectClass = childNode.InnerText;
//                            break;
//                        case "objectclass":
//                            strObjectClass = new string[entryNode.SelectNodes("oc-value").Count];
//                            int counter = 0;
//                            foreach (XmlNode ocVal in entryNode.SelectNodes("oc-value"))
//                            {
//                                strObjectClass[counter] = ocVal.InnerText;
//                                counter++;
//                            }
//                            break;
//                        case "attr":
//                            //tempAttribute = new Attribute(childNode);
//                            //attr.Add(tempAttribute);
//                            attr.Add(new Attribute(childNode));
//                            break;
//                        case "dn-attr":
//                            //tempAttribute = new Attribute(childNode);
//                            //attr.Add(tempAttribute);
//                            attr.Add(new Attribute(childNode));
//                            break;

//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("Error occurred in Entry object constructor.", ex);
//            }
//        }

//        public string DN
//        {
//            get
//            {
//                return strDN;
//            }
//        }
//        public string PrimaryObjectClass
//        {
//            get
//            {
//                return strPrimaryObjectClass;
//            }
//        }
//        public string[] ObjectClass
//        {
//            get
//            {
//                return strObjectClass;
//            }
//        }
//        public List<Attribute> Attributes
//        {
//            get
//            {
//                return attr;
//            }
//        }
//        public List<string> AttributeNames
//        {
//            get
//            {
//                if (attr == null)
//                {
//                    return null;
//                }
//                else
//                {
//                    List<string> Names = new List<string>();
//                    foreach (Attribute bute in attr)
//                    {
//                        Names.Add(bute.Name);
//                    }
//                    return Names;
//                }
//            }
//        }
//        public Attribute AttributeByName(string AttributeName)
//        {
//            foreach (Attribute attrib in attr)
//            {
//                if (attrib.Name == AttributeName)
//                {
//                    return attrib;
//                }
//            }
//            return null;
//        }
//        public int AttribIndexByName(string AttributeName)
//        {
//            for (int i = 0; i < attr.Count; i++)
//            {
//                if (attr[i].Name == AttributeName)
//                {
//                    return i;
//                }
//            }
//            return -1;
//        }
//        public csObject Parent
//        {
//            get
//            {
//                return parent;
//            }
//        }
//    }

//    class Attribute
//    {
//        private string strName;
//        private string strType;
//        private operation opOperation;
//        private bool bMultivalued;
//        private List<AttributeValue> vals = new List<AttributeValue>();
//        private bool bDNattr;

//        public Attribute(XmlNode attributeNode)
//        {
//            try
//            {
//                foreach (XmlAttribute xmlAttr in attributeNode.Attributes)
//                {
//                    switch (xmlAttr.Name)
//                    {
//                        case "name":
//                            strName = xmlAttr.Value;
//                            break;
//                        case "type":
//                            strType = xmlAttr.Value;
//                            break;
//                        case "operation":
//                            switch (xmlAttr.Value)
//                            {
//                                case "delete-add":
//                                    opOperation = operation.deleteAdd;
//                                    break;
//                                case "add":
//                                    opOperation = operation.add;
//                                    break;
//                                case "replace":
//                                    opOperation = operation.replace;
//                                    break;
//                                case "update":
//                                    opOperation = operation.update;
//                                    break;
//                                case "delete":
//                                    opOperation = operation.delete;
//                                    break;
//                                case "none":
//                                    opOperation = operation.none;
//                                    break;
//                            }
//                            break;
//                        case "multivalued":
//                            bMultivalued = Convert.ToBoolean(xmlAttr.Value);
//                            break;
//                    }
//                }

//                switch (attributeNode.Name)
//                {
//                    case "attr":
//                        bDNattr = false;
//                        foreach (XmlNode childNode in attributeNode.ChildNodes)
//                        {
//                            if (childNode.Name == "value")
//                            {
//                                vals.Add(new AttributeValue(childNode));
//                            }
//                        }
//                        break;
//                    case "dn-attr":
//                        bDNattr = true;
//                        foreach (XmlNode childNode in attributeNode.ChildNodes)
//                        {
//                            if (childNode.Name == "dn-value")
//                            {
//                                foreach (XmlNode subNode in childNode)
//                                {
//                                    if (subNode.Name == "dn")
//                                    {
//                                        vals.Add(new AttributeValue(subNode));
//                                    }
//                                }
//                            }
//                        }
//                        break;
//                }
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("Error occurred in Attribute object constructor.", ex);
//            }
//        }
//        public Attribute() { }

//        //additional contructor created to support DN changes
//        //no XML to parse
//        public Attribute(string dnValue)
//        {
//            strName = "DN";
//            strType = "DN";
//            opOperation = operation.update;
//            bMultivalued = false;
//            bDNattr = true;
//            vals.Add(new AttributeValue(dnValue));
//        }

//        public string Name
//        {
//            get
//            {
//                return strName;
//            }
//        }
//        public string Type
//        {
//            get
//            {
//                return strType;
//            }
//        }
//        public operation Operation
//        {
//            get
//            {
//                return opOperation;
//            }
//        }
//        public bool Multivalued
//        {
//            get
//            {
//                return bMultivalued;
//            }
//        }
//        public List<AttributeValue> Values
//        {
//            get
//            {
//                return vals;
//            }
//        }
//        public List<string> StringValues
//        {
//            get
//            {
//                List<string> strValues = new List<string>(vals.Count);
//                if (vals.Count > 0)
//                {
//                    strValues = new List<string>(vals.Count);
//                    for (int i = 0; i < vals.Count; i++)
//                    {
//                        strValues.Add(vals[i].Value);
//                    }
//                }
//                else
//                {
//                    strValues.Add("");
//                }
//                return strValues;
//            }
//        }
//        public List<string> ADStringValues
//        {
//            get
//            {
//                List<string> strValues = new List<string>(vals.Count);
//                for (int i = 0; i < vals.Count; i++)
//                {
//                    strValues.Add(vals[i].ADvalue);
//                }
//                return strValues;
//            }
//        }
//        public bool isDN
//        {
//            get
//            {
//                return bDNattr;
//            }
//        }

//    }

//    class AttributeValue
//    {
//        private operation opOperation;
//        private string val;
//        private string ADval;

//        public AttributeValue(XmlNode valueNode)
//        {
//            try
//            {
//                if (valueNode.InnerText != null)
//                {
//                    val = valueNode.InnerText;
//                }
//                else
//                {
//                    val = "";
//                }
//                foreach (XmlAttribute xmlAttr in valueNode.Attributes)
//                {
//                    switch (xmlAttr.Name)
//                    {
//                        case "operation":
//                            switch (xmlAttr.Value)
//                            {
//                                case "delete-add":
//                                    opOperation = operation.deleteAdd;
//                                    break;
//                                case "add":
//                                    opOperation = operation.add;
//                                    break;
//                                case "replace":
//                                    opOperation = operation.replace;
//                                    break;
//                                case "update":
//                                    opOperation = operation.update;
//                                    break;
//                                case "delete":
//                                    opOperation = operation.delete;
//                                    break;
//                                case "none":
//                                    opOperation = operation.none;
//                                    break;
//                            }
//                            break;
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("Exception occurred in AttributeValue contructor", ex);
//            }
//        }

//        //additional contructor created to support DN changes
//        //no XML to parse
//        public AttributeValue(string value)
//        {
//            val = value;
//            opOperation = operation.none;
//        }

//        public operation Operation
//        {
//            get
//            {
//                return opOperation;
//            }
//        }
//        public string Value
//        {
//            get
//            {
//                return val;
//            }
//        }
//        public string ADvalue
//        {
//            get
//            {
//                return ADval;
//            }
//            set
//            {
//                ADval = value;
//            }
//        }
//    }

//    class Sequencer
//    {
//        private int iCurrentBatchNumber;
//        private int iCurrentSequenceNumber;
//        private int iUnappliedBatchNumber;
//        private int iUnappliedSequenceNumber;
//        private int iOriginalBatchNumber;
//        private int iOriginalSequenceNumber;

//        public Sequencer(XmlNode sequencerNode)
//        {
//            try
//            {
//                foreach (XmlNode childNode in sequencerNode)
//                {
//                    switch (childNode.Name)
//                    {
//                        case "orginal":
//                            foreach (XmlNode subNode in childNode)
//                            {
//                                switch (subNode.Name)
//                                {
//                                    case "batch-number":
//                                        iOriginalBatchNumber = Convert.ToInt32(subNode.InnerText);
//                                        break;
//                                    case "sequence-number":
//                                        iOriginalSequenceNumber = Convert.ToInt32(subNode.InnerText);
//                                        break;
//                                }
//                            }
//                            break;
//                        case "current":
//                            foreach (XmlNode subNode in childNode)
//                            {
//                                switch (subNode.Name)
//                                {
//                                    case "batch-number":
//                                        iCurrentBatchNumber = Convert.ToInt32(subNode.InnerText);
//                                        break;
//                                    case "sequence-number":
//                                        iCurrentSequenceNumber = Convert.ToInt32(subNode.InnerText);
//                                        break;
//                                }
//                            }
//                            break;
//                        case "unapplied":
//                            foreach (XmlNode subNode in childNode)
//                            {
//                                switch (subNode.Name)
//                                {
//                                    case "batch-number":
//                                        iUnappliedBatchNumber = Convert.ToInt32(subNode.InnerText);
//                                        break;
//                                    case "sequence-number":
//                                        iUnappliedSequenceNumber = Convert.ToInt32(subNode.InnerText);
//                                        break;
//                                }
//                            }
//                            break;
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("Exception occurred in Sequencer contructor", ex);
//            }
//        }

//        public int CurrentBatchNumber
//        {
//            get
//            {
//                return iCurrentBatchNumber;
//            }
//        }
//        public int CurrentSequenceNumber
//        {
//            get
//            {
//                return iCurrentSequenceNumber;
//            }
//        }
//        public int UnappliedBatchNumber
//        {
//            get
//            {
//                return iUnappliedBatchNumber;
//            }
//        }
//        public int UnappliedSequenceNumber
//        {
//            get
//            {
//                return iUnappliedSequenceNumber;
//            }
//        }
//        public int OriginalBatchNumber
//        {
//            get
//            {
//                return iOriginalBatchNumber;
//            }
//        }
//        public int OriginalSequenceNumber
//        {
//            get
//            {
//                return iOriginalSequenceNumber;
//            }
//        }
//    }

//    class ExportError
//    {
//        private DateTime dateOccurred;
//        private DateTime firstOccurred;
//        private string retryCount;
//        private string errorType;
//        private string errorCode;
//        private string errorLiteral;
//        private string serverErrorDetail;

//        public ExportError(XmlNode exportErrorNode)
//        {
//            try
//            {
//                foreach (XmlAttribute attrib in exportErrorNode.Attributes)
//                {
//                    switch (attrib.Name)
//                    {
//                        case "first-occurred":
//                            firstOccurred = Convert.ToDateTime(attrib.InnerText);
//                            break;
//                        case "date-occurred":
//                            dateOccurred = Convert.ToDateTime(attrib.InnerText);
//                            break;
//                        case "retry-count":
//                            retryCount = attrib.InnerText;
//                            break;
//                        case "error-type":
//                            errorType = attrib.InnerText;
//                            break;
//                    }
//                }
//                foreach (XmlNode childNode in exportErrorNode)
//                {
//                    switch (childNode.Name)
//                    {
//                        case "export-status":
//                            foreach (XmlNode cNode in childNode)
//                            {
//                                if (cNode.Name == "cd-error")
//                                {
//                                    foreach (XmlNode ChildN in cNode)
//                                    {
//                                        switch (ChildN.Name)
//                                        {
//                                            case "error-literal":
//                                                errorLiteral = ChildN.InnerText;
//                                                break;
//                                            case "error-code":
//                                                errorCode = ChildN.InnerText;
//                                                break;
//                                            case "server-error-detail":
//                                                serverErrorDetail = ChildN.InnerText;
//                                                break;
//                                        }
//                                    }
//                                }
//                            }
//                            break;
//                        case "export-error":
//                            foreach (XmlAttribute attrib in exportErrorNode.Attributes)
//                            {
//                                switch (attrib.Name)
//                                {
//                                    case "first-occurred":
//                                        firstOccurred = Convert.ToDateTime(attrib.InnerText);
//                                        break;
//                                    case "date-occurred":
//                                        dateOccurred = Convert.ToDateTime(attrib.InnerText);
//                                        break;
//                                    case "retry-count":
//                                        retryCount = attrib.InnerText;
//                                        break;
//                                    case "error-type":
//                                        errorType = attrib.InnerText;
//                                        break;
//                                }
//                            }
//                            foreach (XmlNode cNode in childNode)
//                            {
//                                if (cNode.Name == "cd-error")
//                                {
//                                    foreach (XmlNode ChildN in cNode)
//                                    {
//                                        switch (ChildN.Name)
//                                        {
//                                            case "error-literal":
//                                                errorLiteral = ChildN.InnerText;
//                                                break;
//                                            case "error-code":
//                                                errorCode = ChildN.InnerText;
//                                                break;
//                                            case "server-error-detail":
//                                                serverErrorDetail = ChildN.InnerText;
//                                                break;
//                                        }
//                                    }
//                                }
//                            }
//                            break;
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("Exception occurred in ExportError constructor", ex);
//            }
//        }

//        public DateTime DateOccurred
//        {
//            get
//            {
//                return dateOccurred;
//            }
//        }
//        public DateTime FirstOccurred
//        {
//            get
//            {
//                return firstOccurred;
//            }
//        }
//        public string RetryCount
//        {
//            get
//            {
//                return retryCount;
//            }
//        }
//        public string ErrorType
//        {
//            get
//            {
//                return errorType;
//            }
//        }
//        public string ErrorCode
//        {
//            get
//            {
//                return errorCode;
//            }
//        }
//        public string ErrorLiteral
//        {
//            get
//            {
//                return errorLiteral;
//            }
//        }
//        public string ServerErrorDetail
//        {
//            get
//            {
//                return serverErrorDetail;
//            }
//        }
//    }

//    public enum operation { none, add, replace, update, delete, deleteAdd };

//    //public enum pendingAction { Import, Export, None };
//    public enum State { UnappliedExport, EscrowedExport, UnconfirmedExport, PendingImport, None };

//    class FilterObject
//    {
//        State filterState;
//        List<string> objTypes = new List<string>();
//        List<operation> ops = new List<operation>();
//        List<string> availableAttribs = new List<string>();
//        List<string> reportAttribs = new List<string>();
//        BindingList<FilterAttribute> attribFilters = new BindingList<FilterAttribute>();
//        private FilterLevel lev;
//        public enum FilterLevel { ImportExport, ObjectType, Operation, AttributeValue };

//        public FilterObject()
//        { }
//        public FilterObject(State FilterState, FilterLevel level)
//        {
//            filterState = FilterState;
//            lev = level;
//        }
//        public FilterObject(State FilterState, FilterLevel level, List<string> objectTypes)
//        {
//            filterState = FilterState;
//            lev = level;
//            objTypes = objectTypes;
//        }
//        public FilterObject(State FilterState, FilterLevel level, List<string> objectTypes, List<operation> operations)
//        {
//            filterState = FilterState;
//            lev = level;
//            objTypes = objectTypes;
//            ops = operations;
//        }
//        public FilterObject(State FilterState, FilterLevel level, List<string> objectTypes, List<operation> operations, List<string> attributes)
//        {
//            filterState = FilterState;
//            lev = level;
//            objTypes = objectTypes;
//            ops = operations;
//            availableAttribs = attributes;
//        }
//        public FilterObject(State FilterState, FilterLevel level, List<string> objectTypes, List<operation> operations, List<string> attributes, BindingList<FilterAttribute> filters)
//        {
//            filterState = FilterState;
//            lev = level;
//            objTypes = objectTypes;
//            ops = operations;
//            availableAttribs = attributes;
//            attribFilters = filters;
//        }

//        public State FilterState
//        {
//            get
//            {
//                return filterState;
//            }
//            set
//            {
//                filterState = value;
//            }
//        }

//        public List<string> ObjectTypes
//        {
//            get
//            {
//                return objTypes;
//            }
//            set
//            {
//                objTypes = value;
//            }
//        }

//        public List<operation> Operations
//        {
//            get
//            {
//                return ops;
//            }
//            set
//            {
//                ops = value;
//            }
//        }

//        public List<string> AvailableAttributes
//        {
//            get
//            {
//                return availableAttribs;
//            }
//            set
//            {
//                availableAttribs = value;
//            }
//        }

//        public List<string> ReportAttributes
//        {
//            get
//            {
//                return reportAttribs;
//            }
//            set
//            {
//                reportAttribs = value;
//            }
//        }

//        public BindingList<FilterAttribute> AttributeFilters
//        {
//            get
//            {
//                return attribFilters;
//            }
//            set
//            {
//                attribFilters = value;
//            }
//        }

//        public FilterLevel Level
//        {
//            get
//            {
//                return lev;
//            }
//            set
//            {
//                lev = value;
//            }
//        }

//        public void Clear()
//        {
//            //filterState = null;
//            ObjectTypes.Clear();
//            ops.Clear();
//            availableAttribs.Clear();
//            reportAttribs.Clear();
//            attribFilters.Clear();
//            lev = new FilterLevel();
//        }
//    }

//    //implements interfaces for binding list and DataGrig control
//    class FilterAttribute : INotifyPropertyChanged, IEquatable<FilterAttribute>
//    {
//        private string attrib;
//        private string comparator;
//        private string val;

//        //required for binding list and DataGrid control
//        public event PropertyChangedEventHandler PropertyChanged;


//        public FilterAttribute(string attribute, string operation, string value)
//        {
//            attrib = attribute;
//            comparator = operation;
//            val = value;
//            this.NotifyPropertyChanged(attribute);
//        }

//        public string Attribute
//        {
//            get
//            {
//                return attrib;
//            }
//        }

//        public string Operation
//        {
//            get
//            {
//                return comparator;
//            }
//        }

//        public string Value
//        {
//            get
//            {
//                return val;
//            }
//        }

//        public bool Equals(FilterAttribute other)
//        {
//            return this.Attribute == other.Attribute && this.Value == other.Value && this.Operation == other.Operation;
//        }

//        //required for binding list and DataGrid control
//        private void NotifyPropertyChanged(string name)
//        {
//            if (PropertyChanged != null)
//            {
//                PropertyChanged(this, new PropertyChangedEventArgs(name));
//            }
//        }
//    }
//}
