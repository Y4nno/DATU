using UnityEngine;
public class AudioManager : MonoBehaviour
{
    [Header("-----------------AUDIO SOURCE-----------------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    [Header("-----------------AUDIO CLIP-----------------")]
    public AudioClip background;
    public AudioClip attack;
    public AudioClip hurt;
    public AudioClip heal;
    public AudioClip dash;
    public AudioClip projectile;
    public AudioClip jump;
    public AudioClip land;

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