using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Code.Infrastructure.AudioVibrationFX.Services.Sound;
using Code.Infrastructure.AudioVibrationFX.StaticData;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Code.Infrastructure.AudioVibrationFX.Editor
{
    public class SoundLibraryEditorWindow : OdinEditorWindow
    {
        [MenuItem("Tools/AudioVibrationKit/Sound Library")]
        private static void OpenWindow()
        {
            GetWindow<SoundLibraryEditorWindow>().Show();
        }

        [BoxGroup("Existing Enums"), ReadOnly]
        [MultiLineProperty(4)]
        [SerializeField]
        private string _current2DTypes;

        [BoxGroup("Existing Enums"), ReadOnly]
        [MultiLineProperty(4)]
        [SerializeField]
        private string _current3DTypes;

        [BoxGroup("2D Sounds")]
        [ShowInInspector, Searchable]
        [ListDrawerSettings(
            Expanded = true,
            DraggableItems = true,
            ShowPaging = true,
            ListElementLabelName = "Name"
        )]
        private List<SoundData> _sounds2DDataEditable;

        [BoxGroup("3D Sounds")]
        [ShowInInspector, Searchable]
        [ListDrawerSettings(
            Expanded = true,
            DraggableItems = true,
            ShowPaging = true,
            ListElementLabelName = "Name"
        )]
        private List<SoundData> _sounds3DDataEditable;

        private SoundsData soundsData;

        [BoxGroup("Generation")]
        [Button("Generate Enums", ButtonSizes.Large)]
        [GUIColor(0f, 1f, 0f)]
        private void GenerateEnums()
        {
            GenerateEnumFile("Sound2DType.cs", "Sound2DType", _sounds2DDataEditable);
            GenerateEnumFile("Sound3DType.cs", "Sound3DType", _sounds3DDataEditable);
            SaveSoundsData();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void GenerateEnumFile(string fileName, string enumName, List<SoundData> soundList)
        {
            var enumPath = $"Assets/Code/Infrastructure/AudioVibrationFX/Services/Sound/{fileName}";
            var names = soundList
                .Where(s => !string.IsNullOrWhiteSpace(s.Name))
                .Select(s => s.Name.Replace(" ", "_").Replace("-", "_").Replace(".", "_").Trim())
                .Distinct()
                .ToList();

            using (var writer = new StreamWriter(enumPath))
            {
                writer.WriteLine("using System;");
                writer.WriteLine();
                writer.WriteLine("namespace Code.Infrastructure.AudioVibrationFX.Services.Sound");
                writer.WriteLine("{");
                writer.WriteLine("    [Serializable]");
                writer.WriteLine($"    public enum {enumName}");
                writer.WriteLine("    {");

                for (int i = 0; i < names.Count; i++)
                {
                    writer.WriteLine($"        {names[i]} = {i},");
                }

                writer.WriteLine("    }");
                writer.WriteLine("}");
            }
            Debug.Log($"{enumName} enum generated successfully with values!");
        }
        
        protected override void OnEnable()
        {
            base.OnEnable();

            var loaded = Resources.Load<SoundsData>("StaticData/Sounds/Sounds");

            if (loaded != null)
            {
                soundsData = loaded;
                _sounds2DDataEditable = soundsData.Sounds2DData;
                _sounds3DDataEditable = soundsData.Sounds3DData;
            }
            else
            {
                Debug.LogError("❌ SoundsData asset not found at Resources/StaticData/Sounds/Sounds.asset");
                _sounds2DDataEditable = new List<SoundData>();
                _sounds3DDataEditable = new List<SoundData>();
            }

            _current2DTypes = GetEnumValues(typeof(Sound2DType));
            _current3DTypes = GetEnumValues(typeof(Sound3DType));
        }


        protected override void OnDisable()
        {
            base.OnDisable();
            SaveSoundsData();
        }
        
        private void SaveSoundsData()
        {
            if (soundsData != null)
            {
                EditorUtility.SetDirty(soundsData);
            }
        }

        
        private string GetEnumValues(Type enumType)
        {
            return string.Join(", ", Enum.GetNames(enumType));
        }
    }
}
