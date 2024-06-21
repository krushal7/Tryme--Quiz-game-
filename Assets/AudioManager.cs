using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("-----Audio Source ------")]
    [SerializeField] AudioSource musicSource;

    public AudioClip background;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }
}
