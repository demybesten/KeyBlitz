using NUnit.Framework;
using Moq;
using System.Threading.Tasks;
using System.Windows.Input;
using Solution.Services;
using Solution.ViewModels;

namespace UnitTests.ViewModelTests
{
  using NUnit.Framework;
  using Moq;
  using Solution.Services;

  [TestFixture]
  public class ScoreTest
  {
    private ScoreViewModel viewModel;
    private Mock<INavigationService> mockNavigation;
    private Mock<SendPrompt> mockSendPrompt;
    private Mock<IDataService> mockDataService;
    private Mock<ApiClient> mockApiClient;

    [SetUp]
    public void Setup()
    {
      mockNavigation = new Mock<INavigationService>();
      mockSendPrompt = new Mock<SendPrompt>();
      mockDataService = new Mock<IDataService>();
      mockApiClient = new Mock<ApiClient>();

      viewModel = new ScoreViewModel(mockNavigation.Object, mockSendPrompt.Object, mockDataService.Object,mockApiClient.Object);
    }

    [Test]
    public void ShowPopupCommand_Should_Set_IsPopupVisible_To_True()
    {
      // Act
      viewModel.ShowPopupCommand.Execute(null);

      // Assert
      Assert.IsTrue(viewModel.IsPopupVisible);
    }

    [Test]
    public void HidePopupCommand_Should_Set_IsPopupVisible_To_False()
    {
      // Act
      viewModel.HidePopupCommand.Execute(null);

      // Assert
      Assert.IsFalse(viewModel.IsPopupVisible);
    }
  }

}
