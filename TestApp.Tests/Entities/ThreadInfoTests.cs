using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestApp.Entity;

namespace TestApp.Tests.Entities
{
    [TestClass]
    public class ThreadInfoTests
    {
        [TestMethod]
        public void ThreadProcedure_ExecutesThreadProcAndCallbackInOrder()
        {
            var executionOrder = new List<string>();

            Action proc = () => executionOrder.Add("proc");
            Action callback = () => executionOrder.Add("callback");

            var info = new ThreadInfo(50, proc, callback);
            info.ThreadProcedure();

            Assert.AreEqual(2, executionOrder.Count);
            Assert.AreEqual("proc", executionOrder[0]);
            Assert.AreEqual("callback", executionOrder[1]);
        }

        [TestMethod]
        public void ThreadProcedure_ModifiesSharedStateCorrectly()
        {
            int counter = 0;
            bool callbackFired = false;

            var info = new ThreadInfo(
                100,
                () => { counter += 42; },
                () => { callbackFired = true; }
            );

            info.ThreadProcedure();

            Assert.AreEqual(42, counter);
            Assert.IsTrue(callbackFired);
        }
    }
}
