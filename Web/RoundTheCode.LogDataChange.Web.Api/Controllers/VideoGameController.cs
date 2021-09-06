using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using RoundTheCode.LogDataChange.Data.DbContexts;
using RoundTheCode.LogDataChange.Data.Entities.Data;
using RoundTheCode.LogDataChange.Services.Data;

namespace RoundTheCode.LogDataChange.Web.Api.Controllers
{
    [Route("api/video-game")]
    public class VideoGameController : BaseChangeController<DataDbContext, ChangeDbContext, VideoGame, IVideoGameService>
    {
        public VideoGameController([NotNull] IVideoGameService service) : base(service) { }
    }
}
