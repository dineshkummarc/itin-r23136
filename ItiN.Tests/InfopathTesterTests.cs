using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ItiN;

namespace Itin.Tests
{
    /// <summary>
    ///This is a test class for ItiN.InfopathTester and is intended
    ///to contain all ItiN.InfopathTester Unit Tests
    ///</summary>
    [TestClass()]
    public class InfopathTesterTests
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for InfopathTester (Uri, LogonDialogHandler)
        ///</summary>
        [TestMethod()]
        public void IssueTrackingSample()
        {
           // Open test form
            string infopathFileName = @"c:\A0000022.xml";
            
            InfopathTester FormTester = new InfopathTester(infopathFileName);

            FormTester.SetInfopathNamespace("xmlns:iss='http://schemas.microsoft.com/office/infopath/2003/sample/IssueTracking'");
            FormTester.WarningStyleInHex = "94C82084";
           
            //FormTester.SetFormValue(@"//iss:title", "Issue Title");
            FormTester.Button(ItiN.Find.ByValue("Complete form")).ClickNoWait();
            PopupDialog something = FormTester.PopupDialog(Find.ByTitle(new System.Text.RegularExpressions.Regex(".*InfoPath.*")),5);
            string textHere = something.Text;
            FormTester.SaveDocumentAs(@"c:\SavedForm.xml");
            FormTester.CloseAndQuit();

            // TODO: Implement code to verify target
            Assert.Inconclusive("TODO: Implement code to verify target");
        }
    }
}
