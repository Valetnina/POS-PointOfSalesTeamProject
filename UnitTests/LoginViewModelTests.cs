using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POS_PointOfSales.ViewModels;
using POS_ViewsLibrary;

namespace UnitTests
{
    [TestClass]
    public class LoginViewModelTests
    {
       

              [TestMethod]
        public void IsViewModel()
        {
            Assert.IsTrue(typeof(LoginViewModel).BaseType == typeof(ViewModel));
        }

        [TestMethod]
        public void ValidationErrorWhenUserNameIsEmpty()
        {
            var viewModel = new LoginViewModel
            {
                UserName = ""
            };

            Assert.IsNotNull(viewModel["UserName"]);
        }
        [TestMethod]
        public void ValidationErrorWhenUserNameIsNotProvided()
        {
            var viewModel = new LoginViewModel
            {
                UserName = null
            };
            Assert.IsNotNull(viewModel["UserName"]);
        }
        [TestMethod]
        public void NoValidationErrorWhenUserNameMeetsAllRequirements()
        {
            var viewModel = new LoginViewModel
            {
                UserName = "Tina"
            };

            Assert.IsNull(viewModel["UserName"]);
        }
        [TestMethod]
        public void LoginComamndCannotExecuteWhenUserNameIsNotValid()
        {
            var viewModel = new LoginViewModel
            {
                UserName = null,
                Password = "123456",
            };
            Assert.IsFalse(viewModel.Login.CanExecute(null));
        }
    }
}
