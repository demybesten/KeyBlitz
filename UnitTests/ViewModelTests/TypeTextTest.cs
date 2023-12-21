using NUnit.Framework;
using Moq;
using System.Threading.Tasks;
using System.Windows.Input;
using Solution.Services;
using Solution.ViewModels;

namespace UnitTests.ViewModelTests
{
  [TestFixture]
  public class TypeTextViewModelTests
  {
    private TypeTextViewModel viewModel;
    private Mock<INavigationService> navigationMock;
    private Mock<IDataService> passTestStatsMock;

    [SetUp]
    public void SetUp()
    {
      navigationMock = new Mock<INavigationService>();
      passTestStatsMock = new Mock<IDataService>();

      viewModel = new TypeTextViewModel(navigationMock.Object, passTestStatsMock.Object);
    }

    [Test]
    public void Constructor_Initialization()
    {
      // Assert
      Assert.IsNotNull(viewModel);
      Assert.IsNotNull(viewModel.MyCommand);
      Assert.IsNotNull(viewModel.PressChar);
      Assert.IsNotNull(viewModel.PressBackspace);
      // Add more assertions for other properties and commands
    }

    [Test]
    public void PressChar_AddsSpaceToUserInput()
    {
      // Arrange
      const string space = " ";

      // Act
      viewModel.PressChar.Execute(space);

      // Assert
      Assert.AreEqual(2, viewModel.UserInput.Count);
      Assert.AreEqual(string.Empty, viewModel.UserInput[1]);
    }
  }
}
