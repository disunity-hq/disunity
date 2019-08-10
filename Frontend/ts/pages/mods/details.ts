import { ChangeEventArgs } from '@syncfusion/ej2/dropdowns';
import axios from 'axios';

export function NavigateToVersion(args: ChangeEventArgs) {
  if (args.value) {
    window.location.href = args.value + '';
  }
}

export async function publishMod(
  orgSlug: string,
  modSlug: string,
  versionNumber: string
) {
  const response = await axios.post(
    `/api/v1/mods/${orgSlug}/${modSlug}/${versionNumber}/publish`
  );

  if (response.status === 204) {
    window.location.reload();
  }
}

export async function deleteMod(
  orgSlug: string,
  modSlug: string,
  versionNumber: string
) {
  const response = await axios.delete(
    `/api/v1/mods/${orgSlug}/${modSlug}/${versionNumber}`
  );

  if (response.status === 204) {
    window.location.href = `/u/${orgSlug}`;
  }
}

export function InitPageScript(
  orgSlug: string,
  modSlug: string,
  versionNumber: string
) {
  const publishBtn = document.getElementById('publishButton');
  if (publishBtn)
    publishBtn.onclick = () => publishMod(orgSlug, modSlug, versionNumber);

  const deleteBtn = document.getElementById('deleteButton');
  if (deleteBtn)
    deleteBtn.onclick = () => deleteMod(orgSlug, modSlug, versionNumber);
}
