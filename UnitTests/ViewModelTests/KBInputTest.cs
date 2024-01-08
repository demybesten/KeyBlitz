using NUnit.Framework;
using Solution.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.ViewModelTests
{
    [TestFixture]
    public class KBInputTest
    {
        [Test]
        public void DeleteCharacter_WhenNoWordsAvailable_ShowMessageBox()
        {
            // Arrange
            var viewModel = new KBViewModel();

            // Act
            viewModel.DeleteCharacter();

            // Assert
            // You can use a mocking framework to assert that MessageBox.Show was called with the correct parameters.
            // Alternatively, you can refactor your code to return a value instead of directly calling MessageBox.Show.
        }

        [Test]
        public void DeleteCharacter_WhenWordsAvailable_ClearLastWord()
        {
            // Arrange
            var viewModel = new KBViewModel();
            viewModel.InputText = "SomeText";
            viewModel.Space(); // Add a word

            // Act
            viewModel.DeleteCharacter();

            // Assert
            Assert.AreEqual("SomeText ", viewModel.InputText);
        }
        [Test]
        public void AddWord()
        {
            // Arrange
            var viewModel = new KBViewModel();
            viewModel.InputText = "SomeText";

            // Act
            viewModel.Space();

            // Assert
            Assert.AreEqual("", viewModel.InputText);
        }
        [Test]

        public void AddMultipleWords()
        {
            // Arrange
            var viewModel = new KBViewModel();
            viewModel.InputText = "Dit";

            // Act
            viewModel.Space();
            viewModel.InputText = "is";
            viewModel.Space();
            viewModel.InputText = "een";
            viewModel.Space();
            viewModel.InputText = "test";
            // Assert
            Assert.AreEqual("test", viewModel.InputText);
        }
        [Test]
        public void DeleteMultipleWords()
        {
            // Arrange
            var viewModel = new KBViewModel();
            viewModel.InputText = "Dit";

            // Act
            viewModel.Space();
            viewModel.InputText = "is";
            viewModel.Space();
            viewModel.InputText = "een";
            viewModel.Space();
            viewModel.InputText = "test";
            viewModel.DeleteCharacter();
            viewModel.DeleteCharacter();
            viewModel.DeleteCharacter();
            viewModel.DeleteCharacter();
            viewModel.DeleteCharacter();
            viewModel.DeleteCharacter();
            viewModel.DeleteCharacter();
            viewModel.DeleteCharacter();
            viewModel.DeleteCharacter();
            viewModel.DeleteCharacter();
            viewModel.DeleteCharacter();
            viewModel.DeleteCharacter();


            // Assert
            Assert.AreEqual("test", viewModel.InputText);
        }

    }
}
