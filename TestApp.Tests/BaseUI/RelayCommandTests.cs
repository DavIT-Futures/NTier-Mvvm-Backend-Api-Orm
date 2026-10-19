using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestApp.BaseUI;

namespace TestApp.Tests.BaseUI
{
    [TestClass]
    public class RelayCommandTests
    {
        [TestMethod]
        public void RelayCommand_Execute_InvokesActionWithParameter()
        {
            object? capturedParam = null;
            bool executed = false;

            var command = new RelayCommand(
                param =>
                {
                    executed = true;
                    capturedParam = param;
                },
                null!
            );

            command.Execute("test-param");

            Assert.IsTrue(executed);
            Assert.AreEqual("test-param", capturedParam);
        }

        [TestMethod]
        public void RelayCommand_CanExecute_WhenPredicateIsNull_ReturnsTrue()
        {
            var command = new RelayCommand(_ => { }, null!);
            bool canExecute = command.CanExecute(null);

            Assert.IsTrue(canExecute);
        }

        [TestMethod]
        public void RelayCommand_CanExecute_EvaluatesPredicateCorrectly()
        {
            bool allow = false;
            var command = new RelayCommand(_ => { }, _ => allow);

            Assert.IsFalse(command.CanExecute(null));

            allow = true;
            Assert.IsTrue(command.CanExecute(null));
        }

        [TestMethod]
        public void RelayCommand_CanExecute_PassesParameterToPredicate()
        {
            var command = new RelayCommand(
                _ => { },
                param => param is int intVal && intVal > 10
            );

            Assert.IsFalse(command.CanExecute(5));
            Assert.IsTrue(command.CanExecute(20));
            Assert.IsFalse(command.CanExecute("not an int"));
        }

        [TestMethod]
        public void RelayCommand_Execute_WhenActionIsNull_DoesNotThrow()
        {
            var command = new RelayCommand(null!, null!);
            command.Execute("anything");
            // should not throw
        }

        [TestMethod]
        public void RelayCommand_CanExecuteChanged_EventSubscribeUnsubscribe_DoesNotThrow()
        {
            var command = new RelayCommand(_ => { }, null!);
            EventHandler handler = (s, e) => { };

            command.CanExecuteChanged += handler;
            command.CanExecuteChanged -= handler;
        }
    }
}
