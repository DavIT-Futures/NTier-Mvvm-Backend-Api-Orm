using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TestApp.BaseUI;

namespace TestApp.Tests.BaseUI
{
    [TestClass]
    public class MouseDoubleClickTests
    {
        private static void RunInSta(Action action)
        {
            Exception? exception = null;
            var thread = new Thread(() =>
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();

            if (exception != null)
            {
                throw exception;
            }
        }

        [TestMethod]
        public void MouseDoubleClick_DependencyProperties_AreNotNull()
        {
            Assert.IsNotNull(MouseDoubleClick.CommandProperty);
            Assert.IsNotNull(MouseDoubleClick.CommandParameterProperty);
        }

        [TestMethod]
        public void MouseDoubleClick_SetAndGetCommandParameter_WorksProperly()
        {
            RunInSta(() =>
            {
                var target = new Button();
                object expectedParam = "sample-parameter";

                MouseDoubleClick.SetCommandParameter(target, expectedParam);
                object actualParam = MouseDoubleClick.GetCommandParameter(target);

                Assert.AreSame(expectedParam, actualParam);
            });
        }

        [TestMethod]
        public void MouseDoubleClick_SetCommand_WithMockCommand_AttachesSuccessfully()
        {
            RunInSta(() =>
            {
                var target = new Button();
                var mockCommand = new Mock<ICommand>();

                MouseDoubleClick.SetCommand(target, mockCommand.Object);

                var retrieved = target.GetValue(MouseDoubleClick.CommandProperty);
                Assert.AreSame(mockCommand.Object, retrieved);
            });
        }

        [TestMethod]
        public void MouseDoubleClick_SetCommand_NullClearsCommand()
        {
            RunInSta(() =>
            {
                var target = new Button();
                var mockCommand = new Mock<ICommand>();

                MouseDoubleClick.SetCommand(target, mockCommand.Object);
                Assert.IsNotNull(target.GetValue(MouseDoubleClick.CommandProperty));

                MouseDoubleClick.SetCommand(target, null!);
                Assert.IsNull(target.GetValue(MouseDoubleClick.CommandProperty));
            });
        }
    }
}
