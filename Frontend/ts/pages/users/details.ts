import Vue from 'vue';

import OrgUsersTable from 'shared/vue/OrgUsersTable.vue';

export function InitPageScript(
  orgSlug: string,
  canManageMembers: boolean,
  canManageRoles: boolean
) {
  new OrgUsersTable({
    el: '#usersTable',
    propsData: {
      orgSlug,
      canManageMembers,
      canManageRoles
    }
  });
}
