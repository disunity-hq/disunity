using System;
using System.ComponentModel.DataAnnotations;

using Disunity.Store.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Disunity.Store.Entities {

    public class ModVersionDownloadEvent : ICreatedAt, IUpdatedAt {

        private const double CountedDownloadDebounceTime = 10 * 60f;

        [Required] public int ModVersionId { get; set; }

        public ModVersion ModVersion { get; set; }

        [DataType(DataType.ImageUrl)] public string SourceIp { get; set; }

        [DataType(DataType.DateTime)] public DateTime LatestDownload { get; set; }

        public int? TotalDownloads { get; set; }
        public int? CountedDownloads { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public class ModVersionDownloadEventConfiguration : IEntityTypeConfiguration<ModVersionDownloadEvent> {

            public void Configure(EntityTypeBuilder<ModVersionDownloadEvent> builder) {
                builder.HasKey(e => new {SourceIP = e.SourceIp, e.ModVersionId});
                builder.Property(e => e.TotalDownloads).HasDefaultValue(1);
                builder.Property(e => e.CountedDownloads).HasDefaultValue(1);
            }

        }

        /// <summary>
        /// Increment <see cref="TotalDownloads"/> and try to increment <see cref="CountedDownloads"/>
        /// </summary>
        /// <returns></returns>
        public bool TryCountDownload() {
            var now = DateTime.Now;
            var diff = now - LatestDownload;

            TotalDownloads++;

            if (diff.TotalSeconds < CountedDownloadDebounceTime) {
                return false;
            }

            CountedDownloads++;
            LatestDownload = now;
            return true;

        }

    }

}