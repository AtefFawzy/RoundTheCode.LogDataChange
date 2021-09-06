using RoundTheCode.LogDataChange.Testing.CudOperations.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RoundTheCode.LogDataChange.Testing.CudOperations
{
    public partial class CreateTest : BaseTest
    {
        [Fact]
        public virtual async Task VideoGameNoDeveloper()
        {
            var testVideoGame = new TestVideoGame();
            testVideoGame.Title = "My New Video Game";

            await _testVideoGameService.CreateAsync(testVideoGame);

            var change = await RunChangeTestsAsync<TestVideoGame>(1);
            Assert.NotNull(change);
            Assert.Equal("Create", change.ChangeData.CUD);

            RunChangePropertyTests(change.ChangeData, "Title", null, false, expectedCurrent: "My New Video Game");
        }

        [Fact]
        public virtual async Task VideoGameWithDeveloper()
        {
            var testVideoGame = new TestVideoGame();
            testVideoGame.Title = "My New Video Game";
            testVideoGame.TestEngineId = 1;

            await _testVideoGameService.CreateAsync(testVideoGame);

            var change = await RunChangeTestsAsync<TestVideoGame>(1);
            Assert.NotNull(change);
            Assert.Equal("Create", change.ChangeData.CUD);

            RunChangePropertyTests(change.ChangeData, "Id", "TestEngine", false, expectedCurrent: 1);
            RunChangePropertyTests(change.ChangeData, "Name", "TestEngine", false, expectedCurrent: "Unreal Engine 4");
            RunChangePropertyTests(change.ChangeData, "TestEngineDeveloperId", "TestEngine", true);

            RunChangePropertyTests(change.ChangeData, "Id", "TestEngineDeveloper", true);
            RunChangePropertyTests(change.ChangeData, "Name", "TestEngineDeveloper", true);

            RunChangePropertyTests(change.ChangeData, "Id", "TestSecondaryEngineDeveloper", false, expectedCurrent: 2, expectedReferenceDisplayName: "Test Secondary Engine Developer");
            RunChangePropertyTests(change.ChangeData, "Name", "TestSecondaryEngineDeveloper", false, expectedCurrent: "Unity Technologies", expectedDisplayName: "The Name", expectedReferenceDisplayName: "Test Secondary Engine Developer");

            RunChangePropertyTests(change.ChangeData, "Size", "TestEngineDeveloperCompanySize", true);
            RunChangePropertyTests(change.ChangeData, "Size", "TestSecondaryEngineDeveloperCompanySize", true);
        }

        [Fact]
        public virtual async Task VideoTwoGames()
        {
            var testVideoGame = new TestVideoGame();
            testVideoGame.Title = "My New Video Game";

            await _testVideoGameService.CreateAsync(testVideoGame);

            testVideoGame = new TestVideoGame();
            testVideoGame.Title = "My New Video Game 2";

            await _testVideoGameService.CreateAsync(testVideoGame);

            var change = await RunChangeTestsAsync<TestVideoGame>(2);
            Assert.NotNull(change);
            Assert.Equal("Create", change.ChangeData.CUD);

            RunChangePropertyTests(change.ChangeData, "Title", null, false, expectedCurrent: "My New Video Game 2");
        }
    }
}
