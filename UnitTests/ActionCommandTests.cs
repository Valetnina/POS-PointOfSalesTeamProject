using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POS_PointOfSales.ViewModels;
using POS_ViewsLibrary;
using POS_DataLibrary;
using POS_SellersApp.ViewModels;

namespace UnitTests
{
    [TestClass]
    public class ActionCommandTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowsExceptionIfActionparameterISNull()
        {
            var commanvd = new ActionCommand(null);
        }
        [TestMethod]
        public void ExecuteInvokesAction()
        {
            var invoked = false;
            Action<Object> action = obj => invoked = true;
            var command = new ActionCommand(action);
            command.Execute();
            Assert.IsTrue(invoked);
        }
    }
}
