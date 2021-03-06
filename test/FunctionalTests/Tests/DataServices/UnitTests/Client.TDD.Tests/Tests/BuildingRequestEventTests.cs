﻿//---------------------------------------------------------------------
// <copyright file="BuildingRequestEventTests.cs" company="Microsoft">
//      Copyright (C) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.
// </copyright>
//---------------------------------------------------------------------

namespace AstoriaUnitTests.TDD.Tests.Client
{
    using System;
    using Microsoft.OData.Client;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit tests for the BuildingRequestEventArgs class.
    /// </summary>
    [TestClass]
    public class BuildingRequestEventTests
    {
        [TestMethod]
        public void MethodComesFromRequestMessageArgs()
        {
            SetupTest("ABCD", new Uri("https://www.example.com/odata.svc/Customers(1)?$filter=something&Custom=Value")).Method.Should().Be("ABCD");
        }

        [TestMethod]
        public void RequestUriComesFromRequestMessageArgs()
        {
            SetupTest("ABCD", new Uri("https://www.example.com/odata.svc/Customers(1)?$filter=something&Custom=Value")).RequestUri.Should().Be(new Uri("https://www.example.com/odata.svc/Customers(1)?$filter=something&Custom=Value"));
        }

        [TestMethod]
        public void DescriptorIsSetInConstructor()
        {
            Descriptor descriptor = new ActionDescriptor();
            SetupTest("ABCD", new Uri("https://www.example.com/odata.svc/"), null, descriptor).Descriptor.Should().BeSameAs(descriptor);
        }

        [TestMethod]
        public void InitialHeadersComesFromConstructor()
        {
            var headers = new HeaderCollection();
            headers.SetHeader("Header1", "Value1");
            headers.SetHeader("Header #2", "A Second *value*");
            SetupTest("ABCD", new Uri("https://www.example.com/odata.svc/"), headers).Headers.Should().Contain("Header1", "Value1").And.Contain("Header #2", "A Second *value*");
        }

        [TestMethod]
        public void ConstructingEventArgsWithNullHeadersShouldCreateEmptyDictionary()
        {
            var eventArgs = SetupTest("ABCD", new Uri("https://www.example.com/odata.svc/"), null);
            eventArgs.Headers.Should().BeEmpty();
        }

        [TestMethod]
        public void CanTotallyReplaceTheRequestUri()
        {
            var eventArgs = SetupTest("ABCD", new Uri("https://www.example.com/odata.svc/Customers(1)?$filter=something&Custom=Value"));
            Uri uri = new Uri("http://something.com");
            eventArgs.RequestUri = uri;
            eventArgs.RequestUri.Should().Be(uri);
        }

        [TestMethod]
        public void CanSetMethodInBuildingRequestEventArgs()
        {
            var eventArgs = SetupTest("PUT", new Uri("https://www.example.com/odata.svc/"));
            eventArgs.Method = "PATCH";
            eventArgs.Method.Should().Be("PATCH");
        }

        private static BuildingRequestEventArgs SetupTest(string method, Uri uri, HeaderCollection headers, Descriptor descriptor)
        {
            return new BuildingRequestEventArgs(method, uri, headers, descriptor, HttpStack.Auto);
        }

        private static BuildingRequestEventArgs SetupTest(string method, Uri uri, HeaderCollection headers)
        {
            return new BuildingRequestEventArgs(method, uri, headers, null, HttpStack.Auto);
        }

        private static BuildingRequestEventArgs SetupTest(string method, Uri uri)
        {
            return SetupTest(method, uri, new HeaderCollection());
        }
    }
}
