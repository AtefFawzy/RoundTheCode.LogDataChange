using RoundTheCode.LogDataChange.Testing.CudOperations.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RoundTheCode.LogDataChange.Testing.CudOperations
{
    public partial class DeleteTest : BaseTest
    {
        [Fact]
        public virtual async Task VideoGameDelete()
        {
            var testVideoGame = new TestVideoGame();
            testVideoGame.Title = "My New Video Game";

            var entity = await _testVideoGameService.CreateAsync(testVideoGame);

            await _testVideoGameService.DeleteAsync(entity.Id);

            var change = await RunChangeTestsAsync<TestVideoGame>(2);
            Assert.NotNull(change);
            Assert.Equal("Soft Delete", change.ChangeData.CUD);

            RunChangePropertyTests(change.ChangeData, "Deleted", null, false, expectedCurrent: true, expectedOriginal: false);
        }

        [Fact]
        public virtual async Task VideoGameDeleteCannotRead()
        {
            var testVideoGame = new TestVideoGame();
            testVideoGame.Title = "My New Video Game";

            var entity = await _testVideoGameService.CreateAsync(testVideoGame);

            await _testVideoGameService.DeleteAsync(entity.Id);

            entity = await _testVideoGameService.ReadAsync(entity.Id);

            Assert.Null(entity);
        }


    }
}
