using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using Disunity.Store.Data;
using Disunity.Store.Data.Services;

using EFCoreHooks.Attributes;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;


namespace Disunity.Store.Entities {

    public class Org : ICreatedAt {

        public int Id { get; set; }
        [Required] [MaxLength(128)] public string DisplayName { get; set; }

        public string Slug { get; set; }

        public bool? ShowUsers { get; set; }

        public List<OrgMember> Members { get; set; }

        public List<Mod> Mods { get; set; }


        [OnBeforeCreate(typeof(UserIdentity), WatchDescendants = false)]
        public static void OnBeforeCreateUser(UserIdentity user, IServiceProvider services) {

            if (user.ShadowOrg != null) {
                return;
            }

            var slugifier = services.GetRequiredService<ISlugifier>();

            user.ShadowOrg = new Org() {
                DisplayName = user.UserName,
                Slug = slugifier.Slugify(user.UserName),
                ShowUsers = false,
                Members = new List<OrgMember>()
                    {new OrgMember() {Role = OrgMemberRole.Owner, User = user}}
            };

        }

        [OnBeforeCreate]
        public static void OnBeforeCreate(Org org, IServiceProvider services) {
            if (org.Slug == null) {
                org.Slug = services.GetRequiredService<ISlugifier>().Slugify(org.DisplayName);
            }
        }

        public class OrgConfiguration : IEntityTypeConfiguration<Org> {

            public void Configure(EntityTypeBuilder<Org> builder) {
                builder.HasIndex(o => o.Slug).IsUnique();

                builder.Property(o => o.ShowUsers).HasDefaultValue(true);

                builder.HasMany(o => o.Mods)
                       .WithOne(m => m.Owner);
            }

        }

        public DateTime CreatedAt { get; set; }

    }

}