using UnityEngine;
using Disunity.Editor.Windows;
    
class Builder 
{
    static void PerformBuild()
    {
        Debug.Log("### BUILDING ###");
        ExporterWindow.ExportMod();
    }
}