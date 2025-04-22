using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using Code.Infrastructure.AudioVibrationFX.StaticData;
using Code.Infrastructure.AudioVibrationFX.Services.Sound;
using UnityEngine.Serialization;

namespace Code
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
        [ListDrawerSettings(Expanded = true, DraggableItems = true, ShowPaging = true)]
        private List<SoundData> _sounds2DData => soundsData != null ? soundsData.Sounds2DData : new List<SoundData>();

        [BoxGroup("3D Sounds")]
        [ShowInInspector, Searchable]
        [ListDrawerSettings(Expanded = true, DraggableItems = true, ShowPaging = true)]
        private List<SoundData> _sounds3DData => soundsData != null ? soundsData.Sounds3DData : new List<SoundData>();

        private SoundsData soundsData;

        [BoxGroup("Generation")]
        [Button("Generate Enums", ButtonSizes.Large)]
        [GUIColor(0f, 1f, 0f)]
        private void GenerateEnums()
        {
            GenerateEnumFile("Sound2DType.cs", "Sound2DType", _sounds2DData);
            GenerateEnumFile("Sound3DType.cs", "Sound3DType", _sounds3DData);
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

            AssetDatabase.Refresh();
            Debug.Log($"{enumName} enum generated successfully with values!");
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            soundsData = Resources.Load<SoundsData>("StaticData/Sounds/Sounds");

            if (soundsData == null)
            {
                Debug.LogError("SoundsData asset not found at Resources/StaticData/Sounds/Sounds.asset");
                soundsData = CreateInstance<SoundsData>();
            }

            _current2DTypes = GetEnumValues(typeof(Sound2DType));
            _current3DTypes = GetEnumValues(typeof(Sound3DType));
        }

        private string GetEnumValues(Type enumType)
        {
            return string.Join(", ", Enum.GetNames(enumType));
        }
    }
}
