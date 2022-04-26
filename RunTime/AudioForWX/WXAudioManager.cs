using UnityEngine;
using UnityEngine.Serialization;

namespace HT.Framework
{
    /// <summary>
    /// 音频管理器
    /// </summary>
    [InternalModule(HTFrameworkModule.WXAudio)]
    public sealed class WXAudioManager : InternalModuleBase<IWXAudioHelper>
    {
        /// <summary>
        /// 是否静音初始值【请勿在代码中修改】
        /// </summary>
        [SerializeField] internal bool MuteDefault = false;
        /// <summary>
        /// 背景音乐音量初始值【请勿在代码中修改】
        /// </summary>
        [SerializeField] internal float BackgroundVolumeDefault = 0.6f;
        /// <summary>
        /// 音效音量初始值【请勿在代码中修改】
        /// </summary>
        [SerializeField] internal float VolumeDefault = 1;

        /// <summary>
        /// 静音
        /// </summary>
        public bool Mute
        {
            get
            {
                return _helper.Mute;
            }
            set
            {
                _helper.Mute = value;
            }
        }
        /// <summary>
        /// 背景音乐是否播放中
        /// </summary>
        public bool IsBackgroundPlaying
        {
            get
            {
                return _helper.IsBackgroundPlaying;
            }
        }
        /// <summary>
        /// 音效是否播放中
        /// </summary>
        public bool IsSoundPlaying
        {
            get
            {
                return _helper.IsSoundPlaying;
            }
        }
        /// <summary>
        /// 背景音乐音量
        /// </summary>
        public float BackgroundVolume
        {
            get
            {
                return _helper.BackgroundVolume;
            }
            set
            {
                _helper.BackgroundVolume = value;
            }
        }
        /// <summary>
        /// 音效音量
        /// </summary>
        public float Volume
        {
            get
            {
                return _helper.SoundVolume;
            }
            set
            {
                _helper.SoundVolume = value;
            }
        }
        
        public override void OnInit()
        {
            base.OnInit();
            
            Mute = MuteDefault;
            BackgroundVolume = BackgroundVolumeDefault;
            Volume = VolumeDefault;
        }

        /// <summary>
        /// 播放背景音乐
        /// </summary>
        /// <param name="clipPath">音乐文件在Assets下的路径如 Source/Sounds/ShootingSound/card.wav</param>
        /// <param name="isLoop">是否循环</param>
        /// <param name="speed">播放速度</param>
        /// <param name="startTime">开始播放的位置（单位：s），默认为 0</param>
        public void PlayBackgroundMusic(string clipPath, bool isLoop = true, float speed = 1,float startTime = 0)
        {
            _helper.PlayBackgroundMusic(clipPath, isLoop, speed,startTime);
        }
        /// <summary>
        /// 暂停播放背景音乐
        /// </summary>
        public void PauseBackgroundMusic()
        {
            _helper.PauseBackgroundMusic();
        }
        /// <summary>
        /// 恢复播放背景音乐
        /// </summary>
        public void ResumeBackgroundMusic()
        {
            _helper.ResumeBackgroundMusic();
        }
        /// <summary>
        /// 停止播放背景音乐
        /// </summary>
        public void StopBackgroundMusic()
        {
            _helper.StopBackgroundMusic();
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="clipPath">音乐文件在Assets下的路径如 Source/Sounds/ShootingSound/card.wav</param>
        /// <param name="isLoop">是否循环</param>
        /// <param name="speed">播放速度</param>
        public void PlaySound(string clipPath, bool isLoop = false, float speed = 1)
        {
            _helper.PlaySound(clipPath, isLoop, speed);
        }
        /// <summary>
        /// 停止播放音效
        /// </summary>
        /// <param name="clipPath">音乐文件在Assets下的路径如 Source/Sounds/ShootingSound/card.wav</param>
        public void StopSound(string clipPath)
        {
            _helper.StopSound(clipPath);
        }
    }
}