﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;

namespace LINGYUN.Abp.Identity.EntityFrameworkCore
{
    public class EfCoreIdentityRoleRepository : Volo.Abp.Identity.EntityFrameworkCore.EfCoreIdentityRoleRepository, IIdentityRoleRepository
    {
        public EfCoreIdentityRoleRepository(
            IDbContextProvider<IIdentityDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {
        }

        public virtual async Task<List<OrganizationUnit>> GetOrganizationUnitsAsync(
            Guid id, 
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = from roleOU in DbContext.Set<OrganizationUnitRole>()
                        join ou in DbContext.OrganizationUnits.IncludeDetails(includeDetails) on roleOU.OrganizationUnitId equals ou.Id
                        where roleOU.RoleId == id
                        select ou;

            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<IdentityRole>> GetRolesInOrganizationsListAsync(
            List<Guid> organizationUnitIds, 
            CancellationToken cancellationToken = default)
        {
            var query = from roleOu in DbContext.Set<OrganizationUnitRole>()
                        join user in DbSet on roleOu.RoleId equals user.Id
                        where organizationUnitIds.Contains(roleOu.OrganizationUnitId)
                        select user;
            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<IdentityRole>> GetRolesInOrganizationUnitAsync(
            Guid organizationUnitId, 
            CancellationToken cancellationToken = default)
        {
            var query = from roleOu in DbContext.Set<OrganizationUnitRole>()
                        join user in DbSet on roleOu.RoleId equals user.Id
                        where roleOu.OrganizationUnitId == organizationUnitId
                        select user;
            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<IdentityRole>> GetRolesInOrganizationUnitWithChildrenAsync(
            string code, 
            CancellationToken cancellationToken = default)
        {
            var query = from roleOU in DbContext.Set<OrganizationUnitRole>()
                        join user in DbSet on roleOU.RoleId equals user.Id
                        join ou in DbContext.Set<OrganizationUnit>() on roleOU.OrganizationUnitId equals ou.Id
                        where ou.Code.StartsWith(code)
                        select user;
            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }
    }
}