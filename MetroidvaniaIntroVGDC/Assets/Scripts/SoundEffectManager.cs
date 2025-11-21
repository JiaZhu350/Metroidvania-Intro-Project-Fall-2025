using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    public static SoundEffectManager Instance;
    [SerializeField] private AudioSource soundFXPObject;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySoundFXClip(AudioClip clip, Transform spawnTransform, float volume = 1.0f)
    {
        AudioSource audioSource = Instantiate(soundFXPObject, spawnTransform.position, Quaternion.identity);
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();

        float clipLength = clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }
}
