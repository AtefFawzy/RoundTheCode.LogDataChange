using Microsoft.EntityFrameworkCore;
using RoundTheCode.LogDataChange.Data.Attributes;
using RoundTheCode.LogDataChange.Data.Entities.Change;
using RoundTheCode.LogDataChange.Data.Entities.Data.BaseObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RoundTheCode.LogDataChange.Data.Entities.Data
{
    [Table("VideoGame", Schema = "dbo")]
    [ChangeTable("VideoGame-Change", Schema = "dbo")]
    public partial class VideoGame : Base, IChangeEntity<VideoGame>
    {
        [MaxLength(100)]
        public virtual string Title { get; set; }

        [MaxLength(100)]
        public virtual string Publisher { get; set; }

        [DisplayName("Release Date"), Column(TypeName = "date")]
        public virtual DateTime? ReleaseDate { get; set; }

        public virtual int? EngineId { get; set; }

        [ChangeReference, DisplayName("Engine")]
        public virtual Engine Engine { get; set; }

        public static void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<VideoGame>().HasOne(videoGame => videoGame.Engine)
                .WithMany()
                .HasPrincipalKey(engine => engine.Id)
                .HasForeignKey(videoGame => videoGame.EngineId);
        }
    }
}
