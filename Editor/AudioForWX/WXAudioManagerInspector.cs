using UnityEditor;
using UnityEngine;

namespace HT.Framework
{
    [CustomEditor(typeof(WXAudioManager))]
    [GiteeURL("https://gitee.com/SaiTingHu/HTFramework")]
    [GithubURL("https://github.com/SaiTingHu/HTFramework")]
    [CSDNBlogURL("https://wanderer.blog.csdn.net/article/details/89874351")]
    internal sealed class WXAudioManagerInspector : InternalModuleInspector<WXAudioManager, IWXAudioHelper>
    {
        protected override string Intro => "Audio Manager, manage all audio playback, pause, stop, etc.";

        protected override void OnInspectorDefaultGUI()
        {
            base.OnInspectorDefaultGUI();

            GUI.enabled = !EditorApplication.isPlaying;

            PropertyField(nameof(AudioManager.MuteDefault), "Mute");
            
            GUILayout.BeginHorizontal();
            FloatSlider(Target.BackgroundVolumeDefault, out Target.BackgroundVolumeDefault, 0f, 1f, "Background Volume");
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            FloatSlider(Target.VolumeDefault, out Target.VolumeDefault, 0f, 1f, "Volume");
            GUILayout.EndHorizontal();

            GUI.enabled = true;
        }
        protected override void OnInspectorRuntimeGUI()
        {
            base.OnInspectorRuntimeGUI();
        }
    }
}