<template extendable>
  <div class="error-group">
    <header>
      <h1>
        <extension-point name="label">{{ title }}</extension-point>
      </h1>
      <ejs-tooltip position="BottomLeft" :content="info" :target="'#' + id">
        <i :id="id" class="fas fa-question" />
      </ejs-tooltip>
    </header>

    <div class="group-headers">
        <div class="error-message" v-for="field in Fields()" :key="field">{{field}}</div>
        <div class="error-message" >Message</div>
    </div>

    <div v-for="(error, index) in errors" :key="index" class="error-components">
      <button class="error-copy btn-primary" v-on:click="CopyError(error)">
        <i class="fas fa-copy" />
      </button>
      <div class="error-component" v-for="field in Fields()" :key="field">{{error[field]}}</div>
      <div class="error-component error-message">{{ error.message }}</div>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import { Component, Prop } from "vue-property-decorator";
import copy from "copy-to-clipboard";
import "../syncfusion";

@Component
export default class ErrorGroup extends Vue {
  readonly id: string;

  @Prop({ required: true })
  errors: any;

  @Prop({ default: "Generic Errors" })
  title: string;

  @Prop({ default: "Various uncategorized errors" })
  info: string;

  constructor() {
    super();
    this.id = this.GetId();
  }

  private GetId(): string {
    return "e" + Math.floor(Math.random() * 1000);
  }

  protected GetCopyableError(error) {
    let message = this.Message(error);
    let context = error.context ? "@" + error.context : "";
    return `${error.name}${context}: ${message}`;
  }

  public async CopyError(error) {
    var text = this.GetCopyableError(error);
    copy(text);
  }

  public Message(error): string {
    return error.message;
  }

  public Fields(): string[] {
    return Object.getOwnPropertyNames(this.errors[0]).filter(o => [
      "name", "context", "message", "__ob__"
    ].indexOf(o) == -1);
  }

  public Tooltip() {
    return "Generic errors that don't relate to each other.";
  }
}
</script>

<style lang="scss" scoped>
@import "~@syncfusion/ej2-vue-inplace-editor/styles/bootstrap.scss";
@import "css/variables.scss";

.error-group {
  header {
    display: flex;
    color: $white;
    background: $red;
    font-size: 3em;
    padding: 0.2em;
    h1 {
      flex: 1;
      margin: 0px;
      width: 100%;
    }

    .fas {
      margin: 1em;
    }
  }

  .group-headers {
    font-weight: 600;
    padding: 0em 1em;
    font-size: 1em;
    color: $white;
    background: $black;
    border-top: 1px solid $white;
  }

  .error-components {
    padding: 1em;
    border-bottom: 1px solid lightgray;

    &:nth-child(even) {
      background: #f3eeee;
    }

    &:hover {
      button {
        opacity: 1;
      }
    }
  }

  .group-headers,
  .error-components {
    display: flex;

    > div {
      max-width: 5%;
      min-width: 4em;
      text-align: center;

      &:last-child {
        flex: 1;
        min-width: none;
        max-width: none;
        width: none;
        text-align: left;
        margin-left: 2em;
      }
    }
  }

  button {
    order: 999;
    opacity: 0.05;
  }
}

.e-popup {
  margin: 0px !important;
  padding: 0px !important;
  background: $primary;
  color: $white;
}
</style>
