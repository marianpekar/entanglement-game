using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] audioClips;

    [SerializeField]
    private AudioSource audioSource;

    public void Play(int i) 
    {
        audioSource.clip = audioClips[i];
        audioSource.Play();
    }
}
