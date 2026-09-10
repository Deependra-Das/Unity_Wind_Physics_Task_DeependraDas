using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Audio_SO _audio_SO;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource _audioSource;

    public static AudioManager Instance { get; private set; }

    private Dictionary<AudioTypeEnum, AudioData> _audioDictionary;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        InitializeAudioDictionary();
    }

    private void InitializeAudioDictionary()
    {
        _audioDictionary = new Dictionary<AudioTypeEnum, AudioData>();

        foreach (AudioData data in _audio_SO.audioDataList)
        {
            if (_audioDictionary.ContainsKey(data.audioType))
            {
                Debug.LogWarning($"Duplicate audio type found: {data.audioType}");
                continue;
            }

            _audioDictionary.Add(data.audioType, data);
        }
    }

    public void PlaySFX(AudioTypeEnum audioType)
    {
        if (!_audioDictionary.TryGetValue(audioType, out AudioData data))
        {
            Debug.LogWarning($"Audio not found for type: {audioType}");
            return;
        }

        if (data.audioClip == null)
        {
            Debug.LogWarning($"Audio clip is missing for: {audioType}");
            return;
        }

        _audioSource.PlayOneShot(data.audioClip);
    }
}
