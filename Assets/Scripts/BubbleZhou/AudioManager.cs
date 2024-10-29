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

    //public List<SceneBGM> 场景BGM列表;
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
    //private Dictionary<string, string> 场景名对应BGM = new Dictionary<string, string>();
    // 当前播放的BGM
    private string currentBGM;

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
        // 使用 DontDestroyOnLoad 确保这个物体在场景切换时不会被销毁
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            audiosDic = new Dictionary<string, AudioSource>();
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // 将场景BGM列表中的数据填充到字典中
        /*foreach (var sceneBGM in 场景BGM列表)
        {
            场景名对应BGM[sceneBGM.sceneName] = sceneBGM.bgmName;
        }*/
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

        /*// 播放当前场景对应的BGM
        if (场景名对应BGM.ContainsKey(SceneManager.GetActiveScene().name))
        {
            PlayBGM(场景名对应BGM[SceneManager.GetActiveScene().name]);
        }*/
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

    public static void PlaySFXNoWait(string name)
    {
        if (!_instance.audiosDic.ContainsKey(name) || _instance.audiosDic[name].outputAudioMixerGroup.name != "SFX")
        {
            Debug.LogWarning($"名为{name}的音频不在SFX组，或不存在");
            return;
        }
        {
            PlayAudio(name, false);
        }
    }
    public static void PlaySFXWait(string name)
    {
        if (!_instance.audiosDic.ContainsKey(name) || _instance.audiosDic[name].outputAudioMixerGroup.name != "SFX")
        {
            Debug.LogWarning($"名为{name}的音频不在SFX组，或不存在");
            return;
        }
        {
            PlayAudio(name, true);
        }
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
        _instance.StartCoroutine(_instance.FadeOut(_instance.audiosDic[name], 1.0f));
    }

    /// <summary>
    /// 停止当前播放的 BGM
    /// </summary>
    public static void StopBGM()
    {
        if (!string.IsNullOrEmpty(_instance.currentBGM) && _instance.audiosDic.ContainsKey(_instance.currentBGM))
        {
            _instance.StartCoroutine(_instance.FadeOut(_instance.audiosDic[_instance.currentBGM], 2.0f));
            _instance.currentBGM = null;
        }
    }

    /// <summary>
    /// 停止当前播放的 BGM 并播放新 BGM
    /// </summary>
    private IEnumerator StopCurrentBGMAndPlayNew(string newBGMName)
    {
        if (!string.IsNullOrEmpty(currentBGM) && audiosDic.ContainsKey(currentBGM))
        {
            yield return StartCoroutine(FadeOut(audiosDic[currentBGM], 2.0f));
        }
        currentBGM = newBGMName;
        StartCoroutine(FadeIn(audiosDic[newBGMName], 2.0f));
    }

    /// <summary>
    /// 播放 BGM
    /// </summary>
    /// <param name="name">BGM 名称</param>
    public static void PlayBGM(string name)
    {
        Debug.Log(name);
        if (!_instance.audiosDic.ContainsKey(name) || _instance.audiosDic[name].outputAudioMixerGroup.name != "BGM")
        {
            Debug.LogWarning($"名为{name}的音频不在BGM组，或不存在");
            return;
        }
        {
            _instance.StartCoroutine(_instance.StopCurrentBGMAndPlayNew(name));
        }
    }
    /// <summary>
    /// 停止所有 SFX 的播放
    /// </summary>
    public static void StopAllSFX()
    {
        foreach (var audio in _instance.audiosDic)
        {
            if (audio.Value.outputAudioMixerGroup.name == "SFX")
            {
                _instance.StartCoroutine(_instance.FadeOut(audio.Value, 1.0f));
            }
        }
    }

    private IEnumerator FadeIn(AudioSource source, float duration)
    {
        source.volume = 0;
        source.Play();
        float startVolume = source.volume;
        while (source.volume < 1)
        {
            source.volume += Time.deltaTime / duration;
            yield return null;
        }
        source.volume = 1;
    }

    private IEnumerator FadeOut(AudioSource source, float duration)
    {
        float startVolume = source.volume;
        while (source.volume > 0)
        {
            source.volume -= Time.deltaTime / duration;
            yield return null;
        }
        source.Stop();
        source.volume = startVolume; // reset to original volume for next play
    }
}
