<template v-if="errors.length > 0">
  <div class="error-report">
    <div v-for="(group, name) in errors" :key="name">
      <ErrorGroup :errors="group.items" :title="name" :info="group.info" />
    </div>
  </div>
</template>


<script lang="ts">
import Vue from "vue";
import { Component, Prop } from "vue-property-decorator";
import ErrorGroup from "shared/vue/ErrorReporting/ErrorGroup.vue";
import SchemaExceptionGroup from "shared/vue/ErrorReporting/SchemaExceptionGroup.vue";
import MissingArtifactGroup from "shared/vue/ErrorReporting/MissingArtifactGroup.vue";

@Component({ components: { ErrorGroup, SchemaExceptionGroup, MissingArtifactGroup } })
export default class ErrorReport extends Vue {
  @Prop({ type: Array, required: false, default: () => [] }) errors: any;

  public static ReportErrors(
    target: any,
    propertyKey: string | symbol,
    descriptor: PropertyDescriptor
  ) {
    var originalMethod = descriptor.value;
    descriptor.value = function() {
      var context = this;
      var args = arguments;
      var promise: Promise<void> = originalMethod.apply(context, args);
      promise.catch(e => {
        context.errors = e.response.data.errors;
      });
    };
    return descriptor;
  }
}
</script>

<style lang="scss" scoped>
@import "~@syncfusion/ej2-vue-inplace-editor/styles/bootstrap.scss";
</style>

