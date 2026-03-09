using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("----------Audio Source----------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] public AudioSource SFXSource;

    [Header("----------Audio Clip----------")]
    public AudioClip background;
    public AudioClip cartsHitting;
    public AudioClip winScreenMusic;
    public AudioClip pickingUpItem;
    public AudioClip cartMoving;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }
    public AudioSource cartMoveSource;


    public void PlayCartMove()
    {
        if (!cartMoveSource.isPlaying)
        {
            cartMoveSource.clip = cartMoving;
            cartMoveSource.loop = true;
            cartMoveSource.Play();
        }
    }

    public void StopCartMove()
    {
        if (cartMoveSource.isPlaying)
        {
            cartMoveSource.Stop();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

}
