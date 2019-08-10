using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using Disunity.Store.Data;

using EFCoreHooks.Attributes;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Disunity.Store.Entities {

    public enum OrgMemberRole {

        Owner,
        Admin,
        Member

    }

    public class OrgMember {

        [Required] public string UserId { get; set; }

        public UserIdentity User { get; set; }

        [Required] public int OrgId { get; set; }

        public Org Org { get; set; }

        [Required] public OrgMemberRole Role { get; set; }

        [OnBeforeCreate]
        public static void EnsureOneOwner(OrgMember membership, ApplicationDbContext context) {
            var oldOwner = context.OrgMembers.SingleOrDefault(m => m.OrgId == membership.OrgId &&
                                                                   m.UserId == membership.UserId &&
                                                                   m.Role == OrgMemberRole.Owner);

            if (oldOwner != null) {
                oldOwner.Role = OrgMemberRole.Admin;
            }
        }

        public class OrgMemberConfiguration : IEntityTypeConfiguration<OrgMember> {

            public void Configure(EntityTypeBuilder<OrgMember> builder) {
                builder.HasKey(m => new {m.UserId, m.OrgId});

                builder.HasIndex(m => new {m.OrgId, m.Role})
                       .HasFilter("\"Role\" = 'owner'")
                       .IsUnique();

                builder.HasOne(m => m.Org)
                       .WithMany(o => o.Members)
                       .HasForeignKey(m => m.OrgId);

                builder.HasOne(m => m.User)
                       .WithMany(u => u.Orgs)
                       .HasForeignKey(m => m.UserId);
            }

        }

    }

}