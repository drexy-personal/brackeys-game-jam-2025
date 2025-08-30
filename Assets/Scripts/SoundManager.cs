using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public AudioSource soundObject;

    public void PlaySoundClip(AudioClip audioClip)
    {
        AudioSource audioSource = Instantiate(soundObject);

        audioSource.clip = audioClip;

        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);
    }
}
