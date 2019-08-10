import ModUploader from 'shared/vue/ModUploader.vue';

export function InitPageScript() {
  new ModUploader({
    el: '#ArchiveUpload',
  });
}