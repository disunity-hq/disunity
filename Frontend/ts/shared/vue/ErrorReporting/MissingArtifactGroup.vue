<template extends="./ErrorGroup.vue">
  <extensions>
    <extension point="label">Missing Artifact Errors</extension>
    <extension point="header">
      <div>Filename</div>
      <div>Message</div>
    </extension>
    <extension point="item">
      <div>{{error.filename}}</div>
      <div>{{error.message}}</div>
    </extension>
  </extensions>
</template>


<script lang="ts">
import { Component, Prop } from "vue-property-decorator";
import ErrorGroup from "./ErrorGroup.vue";

@Component
export default class MissingArtifactGroup extends ErrorGroup {

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
    return "The manifest's `Artifacts` section names a file which doesn't exist within the archive's `artifacts` directory.";
  }
}
</script>