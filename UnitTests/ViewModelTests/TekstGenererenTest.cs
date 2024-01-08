using Moq;
using NUnit.Framework;
using Solution.Services;
using Solution.ViewModels;

namespace UnitTests.ViewModelTests;

[TestFixture]
public class OnPropertyChangedTests
{
  private Mock<INavigationService> navigationMock;
  private Mock<SendPrompt> sendPromptMock;
  private Mock<PassTestStats> passTestStatsMock;
  private ScoreViewModel viewModel;
  private Mock<ApiClient> mockApiClient;


  [SetUp]
  public void SetUp()
  {
    // Arrange
    navigationMock = new Mock<INavigationService>();
    sendPromptMock = new Mock<SendPrompt>();
    passTestStatsMock = new Mock<PassTestStats>();
    mockApiClient = new Mock<ApiClient>();

    viewModel = new ScoreViewModel(navigationMock.Object, sendPromptMock.Object, passTestStatsMock.Object,mockApiClient.Object);
  }
  [Test]
  public void OnPropertyChanged_ResponseText()
  {
    // Arrange
    bool propertyChangedRaised = false;
    viewModel.PropertyChanged += (sender, args) => propertyChangedRaised = true;

    // Act
    viewModel.ResponseText = "New ResponseText";

    // Assert
    Assert.IsTrue(propertyChangedRaised, "PropertyChanged event not raised.");
  }

  [Test]
  public void OnPropertyChanged_TextLength()
  {
    // Arrange
    bool propertyChangedRaised = false;

    viewModel.PropertyChanged += (sender, args) => propertyChangedRaised = true;

    // Act
    viewModel.TextLength = 40;

    // Assert
    Assert.IsTrue(propertyChangedRaised, "PropertyChanged event not raised.");
  }

  [Test]
  public void OnPropertyChanged_TextTypes()
  {
    // Arrange
    bool propertyChangedRaised = false;

    viewModel.PropertyChanged += (sender, args) => propertyChangedRaised = true;

    // Act
    viewModel.TextType = "story";

    // Assert
    Assert.IsTrue(propertyChangedRaised, "PropertyChanged event not raised.");
  }

  [Test]
  public void OnPropertyChanged_ComplexityLevel()
  {
    // Arrange
    bool propertyChangedRaised = false;

    viewModel.PropertyChanged += (sender, args) => propertyChangedRaised = true;

    // Act
    viewModel.ComplexityLevel = "Basic";

    // Assert
    Assert.IsTrue(propertyChangedRaised, "PropertyChanged event not raised.");
  }

  [Test]
  public void OnPropertyChanged_Language()
  {
    // Arrange
    bool propertyChangedRaised = false;

    viewModel.PropertyChanged += (sender, args) => propertyChangedRaised = true;

    // Act
    viewModel.Language = "English";

    // Assert
    Assert.IsTrue(propertyChangedRaised, "PropertyChanged event not raised.");
  }

  [Test]
  public void OnPropertyChanged_TextSubject()
  {
    // Arrange

    bool propertyChangedRaised = false;

    viewModel.PropertyChanged += (sender, args) => propertyChangedRaised = true;

    // Act
    viewModel.TextSubject = "Dairy cows";

    // Assert
    Assert.IsTrue(propertyChangedRaised, "PropertyChanged event not raised.");
  }
}
