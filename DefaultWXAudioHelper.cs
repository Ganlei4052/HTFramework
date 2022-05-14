using System.Collections.Generic;
using System.Linq;
using WeChatWASM;

namespace HT.Framework
{
    /// <summary>
    /// 默认的音频管理器助手
    /// </summary>
    public sealed class DefaultWXAudioHelper : IWXAudioHelper
    {
        private WXAudioManager _module;
        private bool _isMute = false;
        private float _backgroundVolume = 0.6f;
        private float _soundVolume = 1;
        private float _tempSoundVolume = 0;
        private bool _isSoundPlaying = false;
        
        /// <summary>
        /// 背景音乐播放器
        /// </summary>
        private WXInnerAudioContext _backgroundAudioContext = null;
        
        /// <summary>
        /// 音效播放器
        /// </summary>
        private WXInnerAudioContext _SoundAudioContext = null;
        
        /// <summary>
        /// 音频管理器
        /// </summary>
        public IModuleManager Module { get; set; }

        /// <summary>
        /// 静音
        /// </summary>
        public bool Mute
        {
            get { return _isMute; }
            set 
            { 
                _isMute = value;
                if (_backgroundAudioContext!=null)
                {
                    _backgroundAudioContext.mute= _isMute;
                }
                if (_isMute)
                {
                    _tempSoundVolume = _soundVolume;
                    _soundVolume = 0;
                }
                else
                {
                    _soundVolume = _tempSoundVolume;
                }
            }
        }

        /// <summary>
        /// 背景音乐是否播放中
        /// </summary>
        public bool IsBackgroundPlaying
        {
            get
            {
                if (_backgroundAudioContext == null)
                    return false;

                return _backgroundAudioContext.isPlaying;
            }
        }

        /// <summary>
        /// 音效是否播放中
        /// </summary>
        public bool IsSoundPlaying
        {
            get 
            {
                if (_SoundAudioContext == null)
                    return false;
                
                return _SoundAudioContext.isPlaying;
            }
        }

        /// <summary>
        /// 背景音乐音量
        /// </summary>
        public float BackgroundVolume
        {
            get { return _backgroundVolume; }
            set
            {
                if (!_backgroundVolume.Approximately(value))
                {
                    _backgroundVolume = value;
                    if (_backgroundAudioContext != null)
                        _backgroundAudioContext.volume = _backgroundVolume;
                }
            }
        }

        /// <summary>
        /// 音效音量
        /// </summary>
        public float SoundVolume
        {
            get { return _soundVolume; }
            set
            {
                if (!_soundVolume.Approximately(value))
                {
                    _soundVolume = value;
                }
            }
        }

        /// <summary>
        /// 初始化助手
        /// </summary>
        public void OnInit()
        {
            _module = Module as WXAudioManager;
        }

        private List<string> AudioContexts = new List<string>();
        public void PreDownloadAudios(string[] clipPaths)
        {
            WX.PreDownloadAudios(clipPaths, (int res) =>
            {
                if (res == 0)
                {
                    for (int i = 0; i < clipPaths.Length-1; i++)
                    {
                        WX.CreateInnerAudioContext(new InnerAudioContextParam()
                        {
                            src = clipPaths[i],
                            needDownload = true
                        });
                        AudioContexts.Add(clipPaths[i]);
                    }
                }
            });
        }

        /// <summary>
        /// 助手准备工作
        /// </summary>
        public void OnReady()
        {
        }

        /// <summary>
        /// 刷新助手
        /// </summary>
        public void OnUpdate()
        {
        }

        /// <summary>
        /// 终结助手
        /// </summary>
        public void OnTerminate()
        {
        }

        /// <summary>
        /// 暂停助手
        /// </summary>
        public void OnPause()
        {
            Mute = true;
        }

        /// <summary>
        /// 恢复助手
        /// </summary>
        public void OnResume()
        {
            Mute = false;
        }

        /// <summary>
        /// 播放背景音乐
        /// </summary>
        /// <param name="clipPath">音乐文件在Asset下的路径</param>
        /// <param name="isLoop">是否循环</param>
        /// <param name="speed">播放速度。范围 0.5-2.0，默认为 1。（Android 需要 6 及以上版本）</param>
        /// <param name="startTime">开始播放的位置（单位：s），默认为 0</param>>
        public void PlayBackgroundMusic(string clipPath, bool isLoop = true, float speed = 1, float startTime = 0)
        {
            if (AudioContexts.Contains(clipPath))
            {
                WX.WriteLog(StringToolkit.Concat("当前音频：",clipPath));
                _backgroundAudioContext = WX.CreateInnerAudioContext(new InnerAudioContextParam()
                {
                    src = clipPath,
                });
                _backgroundAudioContext.Play();
                _backgroundAudioContext.loop = isLoop;
                _backgroundAudioContext.playbackRate = speed;
                _backgroundAudioContext.startTime = startTime;
                _backgroundAudioContext.mute = Mute;
                _backgroundAudioContext.volume = _backgroundVolume;
            }
            else
            {
                _backgroundAudioContext = WX.CreateInnerAudioContext(new InnerAudioContextParam()
                {
                    src = clipPath,
                    needDownload = true
                });
            }
            _backgroundAudioContext.OnCanplay(() =>
            {
                //如果音频缓存中没有这个音频，这添加到缓存
                if (!AudioContexts.Contains(clipPath))
                {
                    AudioContexts.Add(clipPath);
                }
                _backgroundAudioContext.Play();
                _backgroundAudioContext.loop = isLoop;
                _backgroundAudioContext.playbackRate = speed;
                _backgroundAudioContext.startTime = startTime;
                _backgroundAudioContext.mute = Mute;
                _backgroundAudioContext.volume = _backgroundVolume;
            });
        }
        
        /// <summary>
        /// 暂停播放背景音乐
        /// </summary>
        public void PauseBackgroundMusic()
        {
            if (!_backgroundAudioContext.paused)
            {
                _backgroundAudioContext.Pause();
            }
        }
        
        /// <summary>
        /// 恢复播放背景音乐
        /// </summary>
        public void ResumeBackgroundMusic()
        {
            if (_backgroundAudioContext.paused)
            {
                _backgroundAudioContext.Play();
            }
        }
        
        /// <summary>
        /// 停止播放背景音乐
        /// </summary>
        public void StopBackgroundMusic()
        {
            if (_backgroundAudioContext.isPlaying)
            {
                _backgroundAudioContext.Stop();
            }
        }
        
        /// <summary>
        /// 播放音效 
        /// </summary>
        /// <param name="clipPath">音乐文件在Assets下的路径如 Source/Sounds/ShootingSound/card.wav</param>
        /// <param name="isLoop">是否循环</param>
        /// <param name="speed">播放速度。范围 0.5-2.0，默认为 1。（Android 需要 6 及以上版本）</param>
        public void PlaySound(string clipPath, bool isLoop = false, float speed = 1,float startTime = 1)
        {
            // WX.ShortAudioPlayer.StopOthersAndPlay(clipPath,_soundVolume);
            if (AudioContexts.Contains(clipPath))
            {
                WX.WriteLog(StringToolkit.Concat("当前音频：",clipPath));
                _SoundAudioContext = WX.CreateInnerAudioContext(new InnerAudioContextParam()
                {
                    src = clipPath,
                });
                _SoundAudioContext.Play();
                _SoundAudioContext.loop = isLoop;
                _SoundAudioContext.playbackRate = speed;
                _SoundAudioContext.startTime = startTime;
                _SoundAudioContext.mute = Mute;
                _SoundAudioContext.volume = _backgroundVolume;
            }
            else
            {
                _SoundAudioContext = WX.CreateInnerAudioContext(new InnerAudioContextParam()
                {
                    src = clipPath,
                    needDownload = true
                });
                _SoundAudioContext.OnCanplay(() =>
                {
                    //如果音频缓存中没有这个音频，这添加到缓存
                    if (!AudioContexts.Contains(clipPath))
                    {
                        AudioContexts.Add(clipPath);
                    }
                    _SoundAudioContext.Play();
                    _SoundAudioContext.loop = isLoop;
                    _SoundAudioContext.playbackRate = speed;
                    _SoundAudioContext.startTime = startTime;
                    _SoundAudioContext.mute = Mute;
                    _SoundAudioContext.volume = _backgroundVolume;
                });
            }
        }
        
        /// <summary>
        /// 停止播放音效
        /// </summary>
        /// <param name="clipPath">音乐文件在Assets下的路径如 Source/Sounds/ShootingSound/card.wav</param>
        public void StopSound()
        {
            // WX.ShortAudioPlayer.Stop(clipPath);
            _SoundAudioContext.Stop();
        }
    }
}