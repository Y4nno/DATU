using UnityEngine;
public class AudioManager : MonoBehaviour
{
    [Header("-----------------AUDIO SOURCE-----------------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    [Header("-----------------AUDIO CLIP-----------------")]
    public AudioClip background;
    public AudioClip attack;
    public AudioClip attack2;
    public AudioClip hurt;
    public AudioClip hurt2;
    public AudioClip heal;
    public AudioClip dash;
    public AudioClip projectile;
    public AudioClip block;
    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }
    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);

    }
}