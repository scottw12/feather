﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;
using Telerik.Sitefinity.Frontend.Resources;
using Telerik.Sitefinity.Frontend.Test.TestUtilities;
using Telerik.Sitefinity.Frontend.TestUtilities.DummyClasses.Mvc.Controllers;
using Telerik.Sitefinity.Frontend.TestUtilities.Mvc.Controllers;

namespace Telerik.Sitefinity.Frontend.TestUnit.Resources
{
    /// <summary>
    /// Ensures that VirtualPathBuilder class works correctly.
    /// </summary>
    [TestClass]
    public class VirtualPathBuilderTests
    {
        #region GetVirtualPath invoked with Assembly

        [TestMethod]
        [Owner("Boyko-Karadzhov")]
        [Description("Checks whether GetVirtualPath returns the expected path from a given assembly.")]
        public void GetVirtualPath_TestAssembly_ReturnsAssemblyNameWithPrefix()
        {
            //Arrange
            var controllerAssembly = Assembly.GetExecutingAssembly();

            //Act
            var result = new VirtualPathBuilder().GetVirtualPath(controllerAssembly);

            //Assert
            Assert.AreEqual("Frontend-Assembly/Telerik.Sitefinity.Frontend.TestUnit/", result, "The virtual path is not resolved correctly.");
        }

        [TestMethod]
        [Owner("Boyko-Karadzhov")]
        [Description("Checks if GetVirtualPath throws ArgumentNullException when null is passed as argument.")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetVirtualPath_NullAssembly_ThrowsArgumentNullException()
        {
            new VirtualPathBuilder().GetVirtualPath(assembly: null);
        }

        #endregion

        #region GetVirtualPath invoked with type

        [TestMethod]
        [Owner("Boyko-Karadzhov")]
        [Description("Checks whether GetVirtualPath for a registered controller will return the virtual path for its assembly.")]
        public void GetVirtualPath_RegisteredWidget_ReturnsVirtualPathForItsAssembly()
        {
            //Arrange
            var controllerAssembly = typeof(DummyController).Assembly;
            var vpBuilder = new VirtualPathBuilder();
            var expected = vpBuilder.GetVirtualPath(controllerAssembly);

            using (var factoryReg = new ControllerFactoryRegion<DummyControllerFactory>())
            {
                factoryReg.Factory.ControllerRegistry["Dummy"] = typeof(DummyController);

                //Act
                var result = vpBuilder.GetVirtualPath(typeof(DummyController));

                //Assert
                Assert.AreEqual(expected, result, "The virtual path is not retrieved properly.");
            }
        }

        [TestMethod]
        [Owner("Boyko-Karadzhov")]
        [Description("Checks if GetVirtualPath throws ArgumentNullException when controllerName is null.")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetVirtualPath_PassNull_ThrowsArgumentNullException()
        {
            new VirtualPathBuilder().GetVirtualPath(type: null);
        }

        #endregion

        #region RemoveParams

        [TestMethod]
        [Owner("EGaneva")]
        [Description("Checks whether RemoveParams removes the parameters from the URL correctly.")]
        public void RemoveParams_UrlWithParams_RemovesParams()
        {
            //Arrange
            var vpBuilder = new VirtualPathBuilder(); 
            var urlWithParams = "/sfLayouts/test.master#someParam.master";
            var expectedUrl = "/sfLayouts/test.master";
   
                //Act
            var resultUrl = vpBuilder.RemoveParams(urlWithParams);

                //Assert
            Assert.AreEqual(expectedUrl, resultUrl, "The parameters are not stripped correctly from the URL.");
        }

        [TestMethod]
        [Owner("EGaneva")]
        [Description("Checks whether RemoveParams preserves the URL when no parameters are available.")]
        public void RemoveParams_WithoutParams_PreservesUrl()
        {
            //Arrange
            var vpBuilder = new VirtualPathBuilder();
            var urlWithoutParams = "/sfLayouts/test.master";

            //Act
            var resultUrl = vpBuilder.RemoveParams(urlWithoutParams);

            //Assert
            Assert.AreEqual(urlWithoutParams, resultUrl, "The URL has been changed.");
        }

        #endregion

        #region AddParams

        [TestMethod]
        [Owner("EGaneva")]
        [Description("Checks whether AddParams preserves the URL when no parameters are available.")]
        public void AddParams_WithoutParams_PreservesUrl()
        {
            //Arrange
            var vpBuilder = new VirtualPathBuilder();
            var urlWithoutParams = "/sfLayouts/test.master";

            //Act
            var resultUrl = vpBuilder.AddParams(urlWithoutParams, "");

            //Assert
            Assert.AreEqual(urlWithoutParams, resultUrl, "The URL has been changed.");
        }

        [TestMethod]
        [Owner("EGaneva")]
        [Description("Checks whether AddParams preserves the URL when no parameters are available.")]
        public void AddParams_WithParams_AddsParams()
        {
            //Arrange
            var vpBuilder = new VirtualPathBuilder();
            var urlWithParams = "/sfLayouts/test.master#someParam.master";
            var url = "/sfLayouts/test.master";

            //Act
            var resultUrl = vpBuilder.AddParams(url, "someParam");

            //Assert
            Assert.AreEqual(urlWithParams, resultUrl, "The parameters are not added correctly from the URL.");
        }

        #endregion
    }
}
