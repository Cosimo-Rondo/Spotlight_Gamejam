using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public class SceneBGM
    {
        public string sceneName;
        public string bgmName;
    }

    public List<SceneBGM> 场景BGM列表;
    // 存储单个音频的信息
    [System.Serializable]
    public class Sound
    {
        [Header("音频剪辑")]
        public AudioClip clip;
        [Header("音频分组")]
        public AudioMixerGroup outputGroup;
        [Header("音频音量")]
        [Range(0, 1)]
        public float volume = 1;
        [Header("音频是否开局播放")]
        public bool playOnAwake;
        [Header("音频是否循环播放")]
        public bool loop;
    }

    // 存储所有的音频信息
    public List<Sound> sounds;
    // 每一个音频剪辑的名称对应一个音频组件
    private Dictionary<string, AudioSource> audiosDic;
    // 场景名对应BGM的字典
    private Dictionary<string, string> 场景名对应BGM = new Dictionary<string, string>();

    // 单例模式
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<AudioManager>();
            return _instance;
        }
    }

    private void Awake()
    {
        // 将场景BGM列表中的数据填充到字典中
        foreach (var sceneBGM in 场景BGM列表)
        {
            场景名对应BGM[sceneBGM.sceneName] = sceneBGM.bgmName;
        }

        // 播放当前场景对应的BGM
        if (场景名对应BGM.ContainsKey(SceneManager.GetActiveScene().name))
        {
            PlayAudio(场景名对应BGM[SceneManager.GetActiveScene().name], false);
        }

        // 初始化
        if (_instance == null)
        {
            _instance = this;
            audiosDic = new Dictionary<string, AudioSource>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // 初始化
        foreach (var sound in sounds)
        {
            GameObject obj = new GameObject(sound.clip.name);
            obj.transform.parent = transform;

            AudioSource source = obj.AddComponent<AudioSource>();
            source.clip = sound.clip;
            source.playOnAwake = sound.playOnAwake;
            source.loop = sound.loop;
            source.volume = sound.volume;
            source.outputAudioMixerGroup = sound.outputGroup;

            if (sound.playOnAwake)
                source.Play();

            audiosDic.Add(sound.clip.name, source);
        }
    }

    /// <summary>
    /// 播放音频
    /// </summary>
    /// <param name="name">音频名称</param>
    /// <param name="isWait">是否等音乐播放完</param>
    public static void PlayAudio(string name, bool isWait = false)
    {
        if (!_instance.audiosDic.ContainsKey(name))
        {
            Debug.LogWarning($"名为{name}音频不存在");
            return;
        }
        if (isWait)
        {
            if (!_instance.audiosDic[name].isPlaying)
                _instance.audiosDic[name].Play();
        }
        else
        {
            if (_instance.audiosDic[name] != null)
                _instance.audiosDic[name].Play();
        }
    }

    public static void PlayAudioNoWait(string name)
    {
        PlayAudio(name, false);
    }

    public static void PlayAudioWait(string name)
    {
        PlayAudio(name, true);
    }

    /// <summary>
    /// 停止某一音频的播放
    /// </summary>
    /// <param name="name">音频名称</param>
    public static void StopAudio(string name)
    {
        if (!_instance.audiosDic.ContainsKey(name))
        {
            Debug.LogWarning($"名为{name}音频不存在");
            return;
        }
        _instance.audiosDic[name].Stop();
    }
}