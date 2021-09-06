using RoundTheCode.LogDataChange.Testing.CudOperations.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RoundTheCode.LogDataChange.Testing.CudOperations
{
    public partial class UpdateTest : BaseTest
    {
        [Fact]
        public virtual async Task VideoGameChangeTitle()
        {
            var testVideoGame = new TestVideoGame();
            testVideoGame.Title = "My New Video Game";

            var entity = await _testVideoGameService.CreateAsync(testVideoGame);

            var updateEntity = await _testVideoGameService.ReadAsync(entity.Id, false);
            updateEntity.Title = "My Better Game";

            await _testVideoGameService.UpdateAsync(entity.Id, updateEntity);

            var change = await RunChangeTestsAsync<TestVideoGame>(2);
            Assert.NotNull(change);
            Assert.Equal("Update", change.ChangeData.CUD);

            RunChangePropertyTests(change.ChangeData, "Title", null, false, expectedCurrent: "My Better Game", expectedOriginal: "My New Video Game");
        }

        [Fact]
        public virtual async Task VideoGameUpdateEngineFromNull()
        {
            var testVideoGame = new TestVideoGame();
            testVideoGame.Title = "My New Video Game";

            var entity = await _testVideoGameService.CreateAsync(testVideoGame);

            var updateEntity = await _testVideoGameService.ReadAsync(entity.Id, false);
            updateEntity.TestEngineId = 1;

            await _testVideoGameService.UpdateAsync(entity.Id, updateEntity);

            var change = await RunChangeTestsAsync<TestVideoGame>(2);
            Assert.NotNull(change);
            Assert.Equal("Update", change.ChangeData.CUD);

            RunChangePropertyTests(change.ChangeData, "TestEngineId", null, false, expectedCurrent: 1);
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
        public virtual async Task VideoGameUpdateEngine()
        {
            var testVideoGame = new TestVideoGame();
            testVideoGame.Title = "My New Video Game";
            testVideoGame.TestEngineId = 1;

            var entity = await _testVideoGameService.CreateAsync(testVideoGame);

            var updateEntity = await _testVideoGameService.ReadAsync(entity.Id, false);
            updateEntity.TestEngineId = 2;

            await _testVideoGameService.UpdateAsync(entity.Id, updateEntity);

            var change = await RunChangeTestsAsync<TestVideoGame>(2);
            Assert.NotNull(change);
            Assert.Equal("Update", change.ChangeData.CUD);

            RunChangePropertyTests(change.ChangeData, "TestEngineId", null, false, expectedCurrent: 2, expectedOriginal: 1);
            RunChangePropertyTests(change.ChangeData, "Id", "TestEngine", false, expectedCurrent: 2, expectedOriginal: 1);
            RunChangePropertyTests(change.ChangeData, "Name", "TestEngine", false, expectedCurrent: "Unity", expectedOriginal: "Unreal Engine 4");
            RunChangePropertyTests(change.ChangeData, "TestEngineDeveloperId", "TestEngine", true);

            RunChangePropertyTests(change.ChangeData, "Id", "TestSecondaryEngineDeveloper", false, expectedCurrent: 1, expectedOriginal: 2, expectedReferenceDisplayName: "Test Secondary Engine Developer");
            RunChangePropertyTests(change.ChangeData, "Name", "TestSecondaryEngineDeveloper", false, expectedCurrent: "Epic Games", expectedOriginal: "Unity Technologies", expectedDisplayName: "The Name", expectedReferenceDisplayName: "Test Secondary Engine Developer");

            RunChangePropertyTests(change.ChangeData, "Size", "TestEngineDeveloperCompanySize", true);
            RunChangePropertyTests(change.ChangeData, "Size", "TestSecondaryEngineDeveloperCompanySize", true);
        }

        [Fact]
        public virtual async Task VideoGameUpdateEngineBackToNull()
        {
            var testVideoGame = new TestVideoGame();
            testVideoGame.Title = "My New Video Game";
            testVideoGame.TestEngineId = 1;

            var entity = await _testVideoGameService.CreateAsync(testVideoGame);

            var updateEntity = await _testVideoGameService.ReadAsync(entity.Id, false);
            updateEntity.TestEngineId = null;

            await _testVideoGameService.UpdateAsync(entity.Id, updateEntity);

            var change = await RunChangeTestsAsync<TestVideoGame>(2);
            Assert.NotNull(change);
            Assert.Equal("Update", change.ChangeData.CUD);

            RunChangePropertyTests(change.ChangeData, "TestEngineId", null, false, expectedOriginal: 1);
            RunChangePropertyTests(change.ChangeData, "Id", "TestEngine", false, expectedOriginal: 1);
            RunChangePropertyTests(change.ChangeData, "Name", "TestEngine", false, expectedOriginal: "Unreal Engine 4");
            RunChangePropertyTests(change.ChangeData, "TestEngineDeveloperId", "TestEngine", true);

            RunChangePropertyTests(change.ChangeData, "Id", "TestSecondaryEngineDeveloper", false, expectedOriginal: 2, expectedReferenceDisplayName: "Test Secondary Engine Developer");
            RunChangePropertyTests(change.ChangeData, "Name", "TestSecondaryEngineDeveloper", false, expectedOriginal: "Unity Technologies", expectedDisplayName: "The Name", expectedReferenceDisplayName: "Test Secondary Engine Developer");

            RunChangePropertyTests(change.ChangeData, "Size", "TestEngineDeveloperCompanySize", true);
            RunChangePropertyTests(change.ChangeData, "Size", "TestSecondaryEngineDeveloperCompanySize", true);
        }

        [Fact]
        public virtual async Task VideoGameUpdateNoChange()
        {
            var testVideoGame = new TestVideoGame();
            testVideoGame.Title = "My New Video Game";

            var entity = await _testVideoGameService.CreateAsync(testVideoGame);

            var updateEntity = await _testVideoGameService.ReadAsync(entity.Id, false);
            updateEntity.Title = "My New Video Game";

            await _testVideoGameService.UpdateAsync(entity.Id, updateEntity);

            var change = await RunChangeTestsAsync<TestVideoGame>(1);
            Assert.NotNull(change);
            Assert.Equal("Create", change.ChangeData.CUD);
        }
    }
}
