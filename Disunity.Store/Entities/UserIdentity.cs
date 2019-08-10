using System;
using System.Collections.Generic;

using Disunity.Store.Data.Services;

using EFCoreHooks.Attributes;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;


namespace Disunity.Store.Entities {

    public class UserIdentity : IdentityUser {

        public IList<OrgMember> Orgs { get; set; }

        public int ShadowOrgId { get; set; }
        public Org ShadowOrg { get; set; }

        public class UserIdentityConfiguration : IEntityTypeConfiguration<UserIdentity> {

            public void Configure(EntityTypeBuilder<UserIdentity> builder) { }

        }

    }

}