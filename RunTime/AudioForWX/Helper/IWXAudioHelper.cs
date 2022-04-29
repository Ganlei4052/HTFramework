using System.Collections.Generic;
using UnityEngine;

namespace HT.Framework
{
    /// <summary>
    /// 音频管理器的助手接口
    /// </summary>
    public interface IWXAudioHelper : IInternalModuleHelper
    {
        /// <summary>
        /// 静音
        /// </summary>
        bool Mute { get; set; }
        
        /// <summary>
        /// 背景音乐是否播放中
        /// </summary>
        bool IsBackgroundPlaying { get; }
        
        /// <summary>
        /// 音效是否播放中
        /// </summary>
        bool IsSoundPlaying { get; }
        
        /// <summary>
        /// 背景音乐音量
        /// </summary>
        float BackgroundVolume { get; set; }
        
        /// <summary>
        /// 音效音量
        /// </summary>
        float SoundVolume { get; set; }
        
        /// <summary>
        /// 播放背景音乐
        /// </summary>
        /// <param name="clipPath">音乐文件在Assets下的路径如 Source/Sounds/ShootingSound/card.wav</param>
        /// <param name="isLoop">是否循环</param>
        /// <param name="speed">播放速度。范围 0.5-2.0，默认为 1。（Android 需要 6 及以上版本）</param>
        /// <param name="startTime">开始播放的位置（单位：s），默认为 0</param>
        void PlayBackgroundMusic(string clipPath, bool isLoop = true, float speed = 1,float startTime = 0);
        
        /// <summary>
        /// 暂停播放背景音乐
        /// </summary>
        void PauseBackgroundMusic();
        
        /// <summary>
        /// 恢复播放背景音乐
        /// </summary>
        void ResumeBackgroundMusic();
        
        /// <summary>
        /// 停止播放背景音乐
        /// </summary>
        void StopBackgroundMusic();
        
        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="clipPath">音乐文件在Assets下的路径如 Source/Sounds/ShootingSound/card.wav</param>
        /// <param name="isLoop">是否循环</param>
        /// <param name="speed">播放速度</param>
        /// <param name="startTime">开始播放的位置（单位：s），默认为 0</param>
        void PlaySound(string clipPath, bool isLoop = false ,float speed = 1,float startTime = 0);
        
        /// <summary>
        /// 停止播放音效
        /// </summary>
        void StopSound();
    }
}