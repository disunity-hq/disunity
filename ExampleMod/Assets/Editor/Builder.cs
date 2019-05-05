using UnityEngine;
using Disunity.Editor;
    
class Builder 
{
    static void PerformBuild()
    {
        Debug.Log("### BUILDING ###");
        ExporterEditorWindow.ExportMod();
    }
}