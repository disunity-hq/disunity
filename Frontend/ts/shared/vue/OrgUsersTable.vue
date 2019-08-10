<template>
  <div>
    <table class="table no-top-border">
      <thead>
        <tr>
          <th scope="col">Username</th>
          <th scope="col">Role</th>
          <th scope="col"></th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="(membership, index) in members" v-bind:key="index">
          <td>{{ membership.userName }}</td>
          <td>
            <ejs-inplaceeditor
              :ref="membership.userName"
              v-if="canManageRoles && membership.role !== 'Owner'"
              type="DropDownList"
              mode="Inline"
              :value="membership.role"
              :model="rolesModel"
              v-on:actionSuccess="changeRole($event, membership.userName)"
            ></ejs-inplaceeditor>
            <span v-else>{{membership.role}}</span>
          </td>
          <td class="smallCell" v-if="canManageMembers">
            <button
              id="btn-remove-member"
              v-if="canManageRoles && membership.role !== 'Owner'"
              type="button"
              class="btn btn-primary"
              v-on:click="removeMember(membership.userName)"
            >
              <i class="fas fa-minus" />
            </button>
          </td>
        </tr>
        <tr v-if="addingMember">
          <td>
            <div class="form-group">
              <ejs-autocomplete
                autofill="true"
                v-on:filtering="getUsernames($event)"
                v-on:change="userName=$event.value"
              ></ejs-autocomplete>
            </div>
          </td>
          <td>
            <div class="form-group">
              <select class="form-control" v-model="role">
                <option v-for="role in rolesModel.dataSource" v-bind:key="role">{{role}}</option>
              </select>
            </div>
          </td>
          <td class="smallCell">
            <button type="button" class="btn btn-primary" v-on:click="addMember">
              <i class="fas fa-save" />
            </button>
            <button type="button" class="btn btn-primary" v-on:click="addingMember = false">
              <i class="fas fa-trash" />
            </button>
          </td>
        </tr>
        <tr v-else-if="canManageRoles">
          <td></td>
          <td></td>
          <td key="member-add" id="ctrl-member-add">
            <button
              type="button"
              class="btn btn-primary"
              v-on:click="addingMember = !addingMember"
            >Add member</button>
          </td>
        </tr>
      </tbody>
    </table>
    <ErrorReport :errors="errors" />
  </div>
</template>

<script lang="ts">
import "./syncfusion";
import { ActionEventArgs } from "@syncfusion/ej2-vue-inplace-editor";
import { Component, Prop } from "vue-property-decorator";
import { DataManager, UrlAdaptor, Query } from "@syncfusion/ej2-data";
import { FilteringEventArgs } from "@syncfusion/ej2-dropdowns";
import axios from "axios";
import Error from "../ErrorReporter";
import ErrorReport from "shared/vue/ErrorReporting/ErrorReport.vue";
import Vue from "vue";

enum MemberRole {
  Member,
  Admin,
  Owner
}

interface IMembership {
  userName: string;
  role: string;
}

@Component({
  components: {
    ErrorReport
  }
})
export default class OrgMembersTable extends Vue {
  @Prop({ type: Array, required: false }) errors: any[];
  @Prop({ type: Boolean, default: false }) readonly canManageRoles: boolean;
  @Prop({ type: Boolean, default: false }) readonly canManageMembers: boolean;
  @Prop({ type: String, required: true }) readonly orgSlug: string;

  members: IMembership[] = [];
  addingMember = false;

  userName: string = "";
  role: string = MemberRole[MemberRole.Member];

  readonly rolesModel = {
    dataSource: Object.keys(MemberRole).filter(k => isNaN(Number(k)))
  };

  readonly baseUrl = `/api/v1/orgs/${this.orgSlug}/members`;

  public async mounted() {
    try {
      this.members = (await axios.get<IMembership[]>(this.baseUrl)).data;
    } catch (e) {
      this.errors = e.response.data.errors;
    }
  }

  @ErrorReport.ReportErrors
  public async addMember() {
    const membership: IMembership = {
      userName: this.userName,
      role: this.role
    };
    const response = await axios.post(this.baseUrl, membership);
    this.members.push(membership);
    this.addingMember = false;
    this.userName = "";
    this.role = "Member";
  }

  @ErrorReport.ReportErrors
  public async removeMember(username: string) {
    const response = await axios.delete(`${this.baseUrl}/${username}`);
    this.members = this.members.filter(m => m.userName != username);
  }

  public async changeRole($event: ActionEventArgs, userName: string) {
    const existingMember = this.members.find(m => m.userName === userName);
    const existingRole = existingMember.role;
    try {
      existingMember.role = $event.value;
      await axios.put(this.baseUrl, {
        userName,
        role: $event.value
      });
    } catch (e) {
      existingMember.role = existingRole;
      this.errors = e.response.data.errors;
    }
  }

  public async getUsernames($event: FilteringEventArgs) {
    $event.preventDefaultAction = true;

    if ($event.text.length < 3) {
      $event.cancel = true;
      return;
    }

    const response = await axios.get(
      `/api/v1/users/autocomplete/?username=${$event.text}`
    );

    if (response.status === 200) {
      $event.updateData(response.data);
    }
  }
}
</script>

<style lang="scss" scoped>
@import "~@syncfusion/ej2-vue-inplace-editor/styles/bootstrap.scss";

.e-inplaceeditor {
  margin-left: 2em !important;
}

table {
  text-align: center;
  tr {
    td:first-child {
      width: 50%;
    }
    td:nth-child(2) {
      width: 50%;
    }
    td:last-child {
      min-width: 16em;
      #btn-remove-member {
        opacity: 0.1;
      }
    }

    &:hover {
      td:last-child {
        #btn-remove-member {
          opacity: 1;
        }
      }
    }
  }
}
</style>
