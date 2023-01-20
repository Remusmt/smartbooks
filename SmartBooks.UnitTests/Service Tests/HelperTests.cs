using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using SmartBooks.ApplicationCore.Helpers;

namespace SmartBooks.UnitTests.Service_Tests
{
    [TestClass]
    public class HelperTests
    {
        [TestMethod]
        public void GetNumberEmptyString()
        {
            int returnVal = "".GetNumberFromString();
            Assert.AreEqual(0, returnVal);
        }

        [TestMethod]
        public void GetNumberStringWithoutNumber()
        {
            int returnVal = "a string without numbers".GetNumberFromString();
            Assert.AreEqual(0, returnVal);
        }

        [TestMethod]
        public void GetNumberValidString()
        {
            int returnVal = "WS50012".GetNumberFromString();
            Assert.AreEqual(50012, returnVal);
        }

        [TestMethod]
        public void GetNumberWIthMultipleNumbers()
        {
            int returnVal = "W1S002".GetNumberFromString();
            Assert.AreEqual(2, returnVal);
        }

        [TestMethod]
        public void CleanLedgerAccountName()
        {
            Assert.AreEqual("Fees_1".GetCleanLedgerAccountName(), "Fees");
            Assert.AreEqual("Fee_200".GetCleanLedgerAccountName(), "Fee");
            Assert.AreEqual("Management Fees_78".GetCleanLedgerAccountName(), "Management Fees");
        }
    }
}
