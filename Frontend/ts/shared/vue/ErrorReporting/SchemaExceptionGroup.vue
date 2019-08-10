<template extends="./ErrorGroup.vue">
  <extensions>
    <extension point="label">Manifest Schema Errors</extension>
    <extension point="header">
      <div>Line</div>
      <div>Column</div>
      <div>Field</div>
      <div>Message</div>
    </extension>
    <extension point="item">
      <div>{{error.lineNumber}}</div>
      <div>{{error.linePosition}}</div>
      <div>{{error.path}}</div>
      <div>{{error.message}}</div>
    </extension>
  </extensions>
</template>


<script lang="ts">
import { Component, Prop } from "vue-property-decorator";
import ErrorGroup from "./ErrorGroup.vue";

@Component
export default class SchemaExceptionGroup extends ErrorGroup {

  constructor() {
    super();
    this.errors = this.errors.sort((a, b) => { 
      var lineA = (Number) (a.lineNumber);
      var lineB = (Number) (b.lineNumber);
      return lineA < lineB ? -1 : lineA > lineB ? 1 : 0;
    });
    console.log("Sorted.");
  }

  public Tooltip() {
    return "There was a problem with the format or validity of the mod archive's manifest.";
  }
}
</script>