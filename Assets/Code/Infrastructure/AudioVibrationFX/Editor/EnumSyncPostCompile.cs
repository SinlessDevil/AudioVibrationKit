using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Code
{
    [InitializeOnLoad]
    public static class EnumSyncPostCompile
    {
        [DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            Debug.Log("🔄 Scripts recompiled. Trying to sync SoundTypes...");

            var window = Resources.FindObjectsOfTypeAll<Code.Infrastructure.AudioVibrationFX.Editor.SoundLibraryEditorWindow>().FirstOrDefault();
            if (window != null)
            {
                window.UpdateSoundTypesAfterReload();
                Debug.Log("✅ SoundTypes synced after compilation!");
            }
            else
            {
                Debug.LogWarning("⚠️ SoundLibraryEditorWindow is not open, cannot sync sound types.");
            }
        }
    }
}