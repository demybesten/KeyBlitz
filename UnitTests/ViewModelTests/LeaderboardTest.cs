using NUnit.Framework;
using Moq;
using Solution.Services;
using Solution.ViewModels;
using Solution.Helpers;
using System.Threading.Tasks;

[TestFixture]
public class LeaderboardTest
{
    private LeaderboardViewModel leaderboardViewModel;
    private Mock<ApiClient> apiClientMock;
    private Mock<INavigationService> navigationServiceMock;

    [SetUp]
    public void SetUp()
    {
        apiClientMock = new Mock<ApiClient>();
        navigationServiceMock = new Mock<INavigationService>();
        leaderboardViewModel = new LeaderboardViewModel(navigationServiceMock.Object, apiClientMock.Object);
    }
    [Test]
    public void ChartFilterChangeTest()
    {
        // Arrange
        const string newFilter = "last week";

        // Act
        leaderboardViewModel.ChartFilter = newFilter;

        // Assert
        Assert.AreEqual(LeaderboardTimeperiod.Week, leaderboardViewModel.FilterToTimePeriod());
    }

    [Test]
    public void NavigationTest()
    {
        // Arrange
        navigationServiceMock.Setup(x => x.NavigateTo<NewTestViewModel>());

        // Act
        leaderboardViewModel.NavigateToNewTestView.Execute(null);

        // Assert
        navigationServiceMock.Verify(x => x.NavigateTo<NewTestViewModel>(), Times.Once);
    }

    [Test]
    public void PropertyChangedTest()
    {
        // Arrange
        bool propertyChangedRaised = false;
        leaderboardViewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(LeaderboardViewModel.ChartFilter))
            {
                propertyChangedRaised = true;
            }
        };

        // Act
        leaderboardViewModel.ChartFilter = "last week";

        // Assert
        Assert.IsTrue(propertyChangedRaised);
    }
}
