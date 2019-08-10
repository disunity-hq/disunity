<template>
  <div>
    <ejs-uploader 
      ref="uploadObj" 
      id="ArchiveUpload" 
      name="ArchiveUpload"
      multiple="false"
      :asyncSettings="asyncSettings"
      :success="uploadSuccess"
      :failure="uploadFailure"></ejs-uploader>
    <ErrorReport :errors="errors" />
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import { Component, Prop } from "vue-property-decorator";
import { FilteringEventArgs } from "@syncfusion/ej2-dropdowns";
import ErrorReport from "shared/vue/ErrorReporting/ErrorReport.vue";


@Component({components: {ErrorReport}})
export default class ModUploader extends Vue {
  @Prop({ type: Array, required: false }) errors: any[];

  asyncSettings: any = {
    saveUrl: "/api/v1/mods/upload"
  };

  uploadSuccess = () => {};

  uploadFailure = (response: any) => {
    const responseBody = response.e.currentTarget.response;
    const responseData = JSON.parse(responseBody);
    this.errors = responseData.errors;
  };

}
</script>

<style lang="scss" scoped>
@import "~@syncfusion/ej2-vue-inplace-editor/styles/bootstrap.scss";
</style>
