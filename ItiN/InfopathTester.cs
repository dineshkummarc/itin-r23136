#region WatiN Copyright (C) 2006-2007 Jeroen van Menen

//Copyright 2006-2007 Jeroen van Menen
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

#endregion Copyright


using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.Office.Interop.InfoPath;
using Microsoft.Office.Interop.InfoPath.Xml;
using ItiN;
using ItiN.UtilityClasses;
using ItiN.Exceptions;
using System.Diagnostics;
using mshtml;

namespace ItiN
{
    public class InfopathTester : DomContainer
    {

        #region Fields

        private Application InfopathApplicationTester;
        private IntPtr infopathHwnd;
        private int infopathProcessId;
        private string InfoPathWarningStyleInHex = "94C82084";
        private IHTMLDocument2 InternalHTMLDOMDocument;
        private XDocument InternalInfopathXDocument;
        private IXMLDOMDocument2 InternalInfopathXMLDOMDocument;
        private DialogWatcher ItiNdialogWatcher;
        private static Settings settings = new Settings();

        #endregion

        # region Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="InfopathTester"/> class.
        /// </summary>
        /// <param name="FormUri">The form URI.</param>
        public InfopathTester(string FormUri)
        {
            CreateNewInfopathAndLoadForm(FormUri, null, false);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InfopathTester"/> class.
        /// </summary>
        /// <param name="FormUri">The form URI.</param>
        /// <param name="InfoPathLoginHandler">An InfoPath login handler.</param>
        public InfopathTester(string FormUri, LogonDialogHandler InfoPathLoginHandler)
        {
            CreateNewInfopathAndLoadForm(FormUri, InfoPathLoginHandler, false);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InfopathTester"/> class.
        /// </summary>
        /// <param name="FormUri">The form URI.</param>
        /// <param name="readOnlyMode">if set to <c>true</c> the form is opened in [read only mode].</param>
        public InfopathTester(string FormUri, bool readOnlyMode)
        {
            CreateNewInfopathAndLoadForm(FormUri, null, readOnlyMode);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InfopathTester"/> class.
        /// </summary>
        /// <param name="FormUri">The form URI.</param>
        /// <param name="InfoPathLoginHandler">The info path login handler.</param>
        /// <param name="readOnlyMode">if set to <c>true</c> the form is opened in [read only mode].</param>
        public InfopathTester(string FormUri, LogonDialogHandler InfoPathLoginHandler, bool readOnlyMode)
        {
            CreateNewInfopathAndLoadForm(FormUri, InfoPathLoginHandler, readOnlyMode);
        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        public void Close()
        {
            Close(true);
        }


        /// <summary>
        /// Closes the specified force close.
        /// </summary>
        /// <param name="forceClose">if set to <c>true</c> [force close].</param>
        public void Close(bool forceClose)
        {
            PopupDialogs.CloseAll();
            WindowsCollection InfopathWinows = InfopathApplicationTester.Windows;
            foreach (Window2 win in InfopathWinows)
            {
                win.Close(forceClose);
            }

        }

        /// <summary>
        /// Closes the and quit.
        /// </summary>
        public void CloseAndQuit()
        {
            Close();
            Quit();
        }
                            
        /// <summary>
        /// Creates the new infopath and load form.
        /// </summary>
        /// <param name="FormUri">The form URI.</param>
        /// <param name="InfoPathLoginHandler">The info path login handler.</param>
        /// <param name="readOnly">if set to <c>true</c> [read only].</param>
        private void CreateNewInfopathAndLoadForm(string FormUri, LogonDialogHandler InfoPathLoginHandler, bool readOnly)
        {
            // Kill off any existing infopath processes
            foreach (Process proc in Process.GetProcessesByName("infopath"))
	        {    
                proc.Kill();
	        }
            
            InitInfopathAndStartDialogWatcher(new Application());
            SecurityAlertHandler securityHandler = new SecurityAlertHandler();
            ReplaceFormAlertHandler replaceFormHandler = new ReplaceFormAlertHandler();
            DialogWatcher.Add(securityHandler);
            DialogWatcher.Add(replaceFormHandler);
            if (InfoPathLoginHandler != null)
            {
                // remove other logon dialog handlers since only one handler
                // can effectively handle the logon dialog.
                DialogWatcher.RemoveAll(new LogonDialogHandler("a", "b"));

                // Add the (new) logonHandler
                DialogWatcher.Add(InfoPathLoginHandler);
            }
            if (readOnly)
            {
                InternalInfopathXDocument = InfopathApplicationTester.XDocuments.Open(FormUri, (int)XdDocumentVersionMode.xdCanOpenInReadOnlyMode);
            }
            else
            {
                InternalInfopathXDocument = InfopathApplicationTester.XDocuments.Open(FormUri, (int)XdDocumentVersionMode.xdFailOnVersionOlder);
            }
            
            InternalInfopathXMLDOMDocument = InternalInfopathXDocument.DOM as IXMLDOMDocument2;
            InternalHTMLDOMDocument = IEDom.IEDOMFromhWnd(this.hWnd);
        }

        /// <summary>
        /// Finds the popup dialog.
        /// </summary>
        /// <param name="findBy">The find by.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns></returns>
        private PopupDialog findPopupDialog(Attribute findBy, int timeout)
        {
            Logger.LogAction("Busy finding PopupDialog with " + findBy.AttributeName + " '" + findBy.Value + "'");

            SimpleTimer timeoutTimer = new SimpleTimer(timeout);

            do
            {
                Thread.Sleep(500);

                foreach (PopupDialog popupDialog in PopupDialogs)
                {
                    if (findBy.Compare(popupDialog))
                    {
                        return popupDialog;
                    }
                }
            } while (!timeoutTimer.Elapsed);

            throw new PopupDialogNotFoundException(findBy.AttributeName, findBy.Value, timeout);
        }

        /// <summary>
        /// Inits the infopath and start dialog watcher.
        /// </summary>
        /// <param name="InfopathApplication">The infopath application.</param>
        private void InitInfopathAndStartDialogWatcher(Application InfopathApplication)
        {
            InfopathApplicationTester = InfopathApplication;

            // The dialog watcher needs to start before we attempt to load the infopath document
            StartDialogWatcher();

        }

        /// <summary>
        /// This method must be overriden by all sub classes
        /// </summary>
        /// <returns></returns>
        internal override IHTMLDocument2 OnGetHtmlDocument()
        {
            if (InternalHTMLDOMDocument == null)
            {
                InternalHTMLDOMDocument = IEDom.IEDOMFromhWnd(this.hWnd);
            }

            return InternalHTMLDOMDocument;
        }

        /// <summary>
        /// Find a PopupDialog by an attribute within the given <paramref name="timeout" /> period.
        /// Currently Find.ByTitle are supported.
        /// </summary>
        /// <param name="findBy">The title of the popup shown in the dialog</param>
        /// <param name="timeout">Number of seconds before the search times out.</param>
        public PopupDialog PopupDialog(Attribute findBy, int timeout)
        {
            return findPopupDialog(findBy, timeout);
        }


        /// <summary>
        /// Quits this instance.
        /// </summary>
        public void Quit()
        {
            Quit(true);
        }

        /// <summary>
        /// Quits the specified force quit.
        /// </summary>
        /// <param name="forceQuit">if set to <c>true</c> [force quit].</param>
        public void Quit(bool forceQuit)
        {
            InfopathApplicationTester.Quit(forceQuit);
        }

        /// <summary>
        /// Saves the document.
        /// </summary>
        public void SaveDocument()
        {
            InternalInfopathXDocument.Save();
        }

        /// <summary>
        /// Saves the document as.
        /// </summary>
        /// <param name="FileURL">The file URL.</param>
        public void SaveDocumentAs(string FileURL)
        {
            InternalInfopathXDocument.SaveAs(FileURL);
        }

        /// <summary>
        /// Sets the form value.
        /// </summary>
        /// <param name="Control">The control.</param>
        /// <param name="value">The value.</param>
        /// <param name="removeNilAttribute">if set to <c>true</c> [remove nil attribute].</param>
        public void SetFormValue(string Control, string value, bool removeNilAttribute)
        {
            IXMLDOMNode node = InternalInfopathXMLDOMDocument.selectSingleNode(Control);
            if (removeNilAttribute && node.attributes.getNamedItem("xsi:nil") != null)
            {
                node.attributes.removeNamedItem("xsi:nil");
            }
            node.text = value;
        }

        /// <summary>
        /// Sets the form value.
        /// </summary>
        /// <param name="xPath">The XPath expression.</param>
        /// <param name="value">The value.</param>
        /// <param name="index">The index.</param>
        /// <param name="removeNilAttribute">if set to <c>true</c> [remove nil attribute].</param>
        public void SetFormValue(string xPath, string value, int index, bool removeNilAttribute)
        {
            SetNodeValue(InternalInfopathXMLDOMDocument.selectNodes(xPath)[index - 1], value, removeNilAttribute);
        }

        /// <summary>
        /// Sets the node value.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="value">The value.</param>
        /// <param name="removeNilAttribute">if set to <c>true</c> remove nil attribute.</param>
        private void SetNodeValue(IXMLDOMNode node, string value, bool removeNilAttribute)
        {
            if (removeNilAttribute && node.attributes.getNamedItem("xsi:nil") != null)
            {
                node.attributes.removeNamedItem("xsi:nil");
            }
            node.text = value;
        }

        /// <summary>
        /// Gets the form value
        /// </summary>
        /// <param name="Control">The control</param>
        /// <returns>The text value of the control</returns>
        public string GetFormValue(string Control)
        {
            IXMLDOMNode node = InternalInfopathXMLDOMDocument.selectSingleNode(Control);
            if (node.attributes.getNamedItem("xsi:nil") != null)
            {
                return null;
            }
            return node.text;
        }

        /// <summary>
        /// Gets the form values for the given control.
        /// </summary>
        /// <param name="xPath">The XPath.</param>
        /// <returns>An array of values</returns>
        public string[] GetFormValues(string xPath)
        {
            IXMLDOMNodeList nodes = InternalInfopathXMLDOMDocument.selectNodes(xPath);
            string[] values = new string[nodes.length];
            for (int i = 0; i < nodes.length; i++)
            {
                values[i] = nodes[i].attributes.getNamedItem("xsi:nil") != null ? null : nodes[i].text;
            }
            return values;
        }

        /// <summary>
        /// Gets whether a node exists
        /// </summary>
        /// <param name="NodePath">The path to the node</param>
        /// <returns>Whether or not the node exists</returns>
        public bool NodeExists(string NodePath)
        {
            return InternalInfopathXMLDOMDocument.selectSingleNode(NodePath) != null;
        }

        /// <summary>
        /// Return an attribute value of a node with default removeNilAttribute = true
        /// </summary>
        /// <param name="nodeName"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public string GetNodeAttributeValue(string nodeName, string attributeName)
        {
            return GetNodeAttributeValue(nodeName, attributeName, true);
        }

        /// <summary>
        /// Return an attribute value of a node
        /// </summary>
        /// <param name="nodeName">xml node</param>
        /// <param name="attributeName">attribute name</param>
        /// <returns></returns>
        public string GetNodeAttributeValue(string nodeName, string attributeName, bool removeNilAttribute)
        {
            IXMLDOMNode node = InternalInfopathXMLDOMDocument.selectSingleNode(nodeName);

            if (removeNilAttribute && node.attributes.getNamedItem("xsi:nil") != null)
            {
                node.attributes.removeNamedItem("xsi:nil");
            }

            bool attributeFound = false;
            string attributeValue = string.Empty;

            foreach (IXMLDOMAttribute attribute in node.attributes)
            { 
                if(attribute.name.ToLower().Equals(attributeName))
                {
                    attributeFound = true;
                    attributeValue = attribute.text.Trim();
                    break;
                }
            }
            
            if(!attributeFound)
            {
                throw new ArgumentException("Attribute " + attributeName + " cant be found");
            }
            else
            {
                return attributeValue;
            }
        }

        /// <summary>
        /// Sets the form value.
        /// </summary>
        /// <param name="Control">The control.</param>
        /// <param name="value">The value.</param>
        public void SetFormValue(string Control, string value)
        {
            SetFormValue(Control, value, true);
        }

        /// <summary>
        /// Sets the form value.
        /// </summary>
        /// <param name="xPath">The XPath.</param>
        /// <param name="value">The value.</param>
        /// <param name="index">The index.</param>
        public void SetFormValue(string xPath, string value, int index)
        {
            SetFormValue(xPath, value, index, true);
        }

        /// <summary>
        /// Sets the infopath namespace.
        /// </summary>
        /// <param name="Namespace">The namespace.</param>
        public void SetInfopathNamespace(string Namespace)
        {
            InfopathDocument.setProperty("SelectionNamespaces", Namespace);

        }

        /// <summary>
        /// Sets the process id and window handle.
        /// </summary>
        private void SetProcessIdAndWindowHandle()
        {

            Process[] updatedInfopathProcessList = Process.GetProcessesByName("infopath");
            if (updatedInfopathProcessList[0].Id == 0)
            {
                throw new ApplicationException("No Infopath processes exist");

            }
            infopathProcessId = updatedInfopathProcessList[0].Id;
            infopathHwnd = updatedInfopathProcessList[0].MainWindowHandle;


        }

        /// <summary>
        /// Starts the dialog watcher.
        /// </summary>
        private void StartDialogWatcher()
        {
            if (ItiNdialogWatcher == null)
            {
                ItiNdialogWatcher = DialogWatcher.GetDialogWatcherForProcess(ProcessId);
                ItiNdialogWatcher.IncreaseReferenceCount();
            }
        }

        /// <summary>
        /// Submits the document.
        /// </summary>
        public void SubmitDocument()
        {
            InternalInfopathXDocument.Submit();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the dialog watcher.
        /// </summary>
        /// <value>The dialog watcher.</value>
        public DialogWatcher DialogWatcher
        {
            get
            {
                return this.ItiNdialogWatcher;
            }
        }

        /// <summary>
        /// Returns the 'raw' html document for the internet explorer DOM.
        /// </summary>
        /// <value></value>
        public override IHTMLDocument2 HtmlDocument
        {
            get
            {
                if (InternalHTMLDOMDocument == null)
                {
                    InternalHTMLDOMDocument = IEDom.IEDOMFromhWnd(this.hWnd);
                }

                return InternalHTMLDOMDocument;
            }
        }

        public override IntPtr hWnd
        {
            get
            {
                if (infopathHwnd.ToInt32() == 0)
                {
                    SetProcessIdAndWindowHandle();
                }
                else { }

                return infopathHwnd;
            }
        }


        /// <summary>
        /// Gets the infopath document.
        /// </summary>
        /// <value>The infopath document.</value>
        public IXMLDOMDocument2 InfopathDocument
        {
            get { return InternalInfopathXMLDOMDocument; }

        }

        /// <summary>
        /// Returns a collection of open Popup dialogs.
        /// </summary>
        /// <value>The Popup dialogs.</value>
        public PopupDialogCollection PopupDialogs
        {
            get
            {
                Process p = Process.GetProcessById(ProcessID);
                PopupDialogCollection popupDialogCollection = new PopupDialogCollection(p);

                return popupDialogCollection;
            }
        }

        /// <summary>
        /// Gets the process id.
        /// </summary>
        /// <value>The process id.</value>
        public int ProcessId
        {
            get
            {
                if (infopathProcessId == 0)
                {
                    SetProcessIdAndWindowHandle();
                }
                return infopathProcessId;
            }
        }

        /// <summary>
        /// Gets or sets the settings.
        /// </summary>
        /// <value>The settings.</value>
        public static Settings Settings
        {
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                settings = value;
            }
            get { return settings; }
        }

        /// <summary>
        /// Checks for popups so that the user can properly manage the popups that match this style. Defaults to "94C82084".
        /// </summary>
        /// <value>The warning style in hex.</value>
        public string WarningStyleInHex
        {
            get { return InfoPathWarningStyleInHex; }
            set
            {
                DialogWatcher.WarningStyleInHex = value;
                InfoPathWarningStyleInHex = value;
            }
        }

        #endregion
    }
}
