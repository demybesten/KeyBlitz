using Moq;
using NUnit.Framework;
using Solution.Services;
using Solution.ViewModels;

namespace UnitTests.ViewModelTests;

[TestFixture]
public class OnPropertyChangedTests
{
  [Test]
  public void OnPropertyChanged_ResponseText()
  {
    // Arrange
    var navigationMock = new Mock<INavigationService>();
    var viewModel = new NewTestViewModel(navigationMock.Object);
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
    var navigationMock = new Mock<INavigationService>();
    var viewModel = new NewTestViewModel(navigationMock.Object);
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
    var navigationMock = new Mock<INavigationService>();
    var viewModel = new NewTestViewModel(navigationMock.Object);
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
    var navigationMock = new Mock<INavigationService>();
    var viewModel = new NewTestViewModel(navigationMock.Object);
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
    var navigationMock = new Mock<INavigationService>();
    var viewModel = new NewTestViewModel(navigationMock.Object);
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
    var navigationMock = new Mock<INavigationService>();
    var viewModel = new NewTestViewModel(navigationMock.Object);
    bool propertyChangedRaised = false;

    viewModel.PropertyChanged += (sender, args) => propertyChangedRaised = true;

    // Act
    viewModel.TextSubject = "Dairy cows";

    // Assert
    Assert.IsTrue(propertyChangedRaised, "PropertyChanged event not raised.");
  }
}

[TestFixture]
public class SendPromptTests
{
  [Test]
  public async Task SendPrompt_SetsResponseText()
  {
    // Arrange
    var navigationMock = new Mock<INavigationService>();
    var viewModel = new NewTestViewModel(navigationMock.Object);

    // Act
    await viewModel.SendPrompt();

    // Assert
    Assert.That(viewModel.ResponseText, Is.Not.Null.And.Not.Empty);
  }

  [Test]
  public async Task SendPrompt_EmptySubject_DefaultToRandom()
  {
    // Arrange
    var navigationMock = new Mock<INavigationService>();
    var viewModel = new NewTestViewModel(navigationMock.Object);
    viewModel.TextSubject = "";

    // Act
    await viewModel.SendPrompt();

    // Assert
    Assert.That(viewModel.TextSubject, Is.EqualTo("random"));
  }

  // Add more tests for different scenarios
}
