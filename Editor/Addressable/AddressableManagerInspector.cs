using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.GUI;
using UnityEngine;

namespace HT.Framework
{
    [CustomEditor(typeof(AddressableManager))]
    internal sealed class AddressableManagerInspector : InternalModuleInspector<AddressableManager, IAddressableHelper>
    {
        protected override string Intro =>
            "Addressable Manager, use this to complete the loading and unloading of resources!";

        protected override void OnInspectorDefaultGUI()
        {
            base.OnInspectorDefaultGUI();
            GUILayout.Space(10);
        }

        protected override void OnInspectorRuntimeGUI()
        {
            if (_helper.AssetCache==null)
            {
                return;
            }
            base.OnInspectorRuntimeGUI();
            GUILayout.BeginHorizontal();
            GUILayout.Label("AssetBundles: ", GUILayout.Width(LabelWidth));
            GUILayout.Label(_helper.AssetCache.Count.ToString());
            GUILayout.EndHorizontal();

            foreach (var item in _helper.AssetCache)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                GUILayout.Label(item.Key, GUILayout.Width(LabelWidth - 20));
                EditorGUILayout.ObjectField(item.Value.Result as Object,item.Value.Result.GetType(),false);
                GUILayout.EndHorizontal();
            }
        }
    }
}