using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Disunity.Store.Entities;


namespace Disunity.Client {

    [GeneratedCode("NSwag", "13.0.4.0 (NJsonSchema v10.0.21.0 (Newtonsoft.Json v11.0.0.0))")]
    public interface IOrgMemberClient
    {
        /// <summary>Get a list of the memberships for all users in the given org</summary>
        /// <param name="orgSlug">The human-readable unique identifier for this org</param>
        /// <returns>Success</returns>
        /// <exception cref="ApiClientException">A server side error occurred.</exception>
        Task<List<OrgMemberDto>> GetMembersAsync(string orgSlug);
    
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Get a list of the memberships for all users in the given org</summary>
        /// <param name="orgSlug">The human-readable unique identifier for this org</param>
        /// <returns>Success</returns>
        /// <exception cref="ApiClientException">A server side error occurred.</exception>
        Task<List<OrgMemberDto>> GetMembersAsync(string orgSlug, CancellationToken cancellationToken);
    
        /// <summary>Update a users role within an org</summary>
        /// <param name="orgSlug">The human-readable unique identifier for this org</param>
        /// <returns>Success</returns>
        /// <exception cref="ApiClientException">A server side error occurred.</exception>
        Task UpdateUserRoleAsync(OrgMemberDto membershipDto, string orgSlug);
    
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Update a users role within an org</summary>
        /// <param name="orgSlug">The human-readable unique identifier for this org</param>
        /// <returns>Success</returns>
        /// <exception cref="ApiClientException">A server side error occurred.</exception>
        Task UpdateUserRoleAsync(OrgMemberDto membershipDto, string orgSlug, CancellationToken cancellationToken);
    
        /// <summary>Adds a user to an existing organization</summary>
        /// <param name="membershipDto">The username of the user to add and the role with which to grant them</param>
        /// <param name="orgSlug">The human-readable unique identifier for this org</param>
        /// <returns>Success</returns>
        /// <exception cref="ApiClientException">A server side error occurred.</exception>
        Task AddMembersAsync(OrgMemberDto membershipDto, string orgSlug);
    
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Adds a user to an existing organization</summary>
        /// <param name="membershipDto">The username of the user to add and the role with which to grant them</param>
        /// <param name="orgSlug">The human-readable unique identifier for this org</param>
        /// <returns>Success</returns>
        /// <exception cref="ApiClientException">A server side error occurred.</exception>
        Task AddMembersAsync(OrgMemberDto membershipDto, string orgSlug, CancellationToken cancellationToken);
    
        /// <summary>Remove a user from an organization</summary>
        /// <param name="orgSlug">The human-readable unique identifier for this org</param>
        /// <returns>Success</returns>
        /// <exception cref="ApiClientException">A server side error occurred.</exception>
        Task DeleteMembersAsync(string username, string orgSlug);
    
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Remove a user from an organization</summary>
        /// <param name="orgSlug">The human-readable unique identifier for this org</param>
        /// <returns>Success</returns>
        /// <exception cref="ApiClientException">A server side error occurred.</exception>
        Task DeleteMembersAsync(string username, string orgSlug, CancellationToken cancellationToken);
    
    }

}