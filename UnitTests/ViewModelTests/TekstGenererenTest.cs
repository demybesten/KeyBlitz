using NUnit.Framework;
using Solution.ViewModels;

namespace UnitTests.ViewModelTests;

[TestFixture]
public class OnPropertyChangedTests
{
  [Test]
  public void OnPropertyChanged_ResponseText()
  {
    // Arrange
    var viewModel = new NewTestViewModel();
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
    var viewModel = new NewTestViewModel();
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
    var viewModel = new NewTestViewModel();
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
    var viewModel = new NewTestViewModel();
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
    var viewModel = new NewTestViewModel();
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
    var viewModel = new NewTestViewModel();
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
    var viewModel = new NewTestViewModel();

    // Act
    await viewModel.SendPrompt();

    // Assert
    Assert.That(viewModel.ResponseText, Is.Not.Null.And.Not.Empty);
  }

  [Test]
  public async Task SendPrompt_EmptySubject_DefaultToRandom()
  {
    // Arrange
    var viewModel = new NewTestViewModel();
    viewModel.TextSubject = "";

    // Act
    await viewModel.SendPrompt();

    // Assert
    Assert.That(viewModel.TextSubject, Is.EqualTo("random"));
  }

  // Add more tests for different scenarios
}
