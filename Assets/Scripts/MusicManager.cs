using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : Singleton<MusicManager>
{
    private AudioSource fx1;
    private AudioSource fx2;
    private AudioSource music;

    #region Audio Clips

    [Header("Sounds")]
    [SerializeField]
    private AudioClip levelMusic;

    [SerializeField]
    private AudioClip startGame;

    [SerializeField]
    private AudioClip playerDied;


    [SerializeField]
    private AudioClip pelletEaten;

    [SerializeField]
    private AudioClip powerPelletEaten;

    [SerializeField]
    private AudioClip ghostEaten;

    [SerializeField]
    private AudioClip ghostScream;

    [SerializeField]
    private AudioClip fruitSpawned;

    [SerializeField]
    private AudioClip fruitEaten;

    #endregion


    void Start()
    {
        AudioSource[] sources = GetComponents<AudioSource>();

        if (sources.Length == 3)
        {
            fx1 = sources[0];
            fx2 = sources[1];
            music = sources[2];
        }

        SetVolume(fx2, 1.5f);
        music.clip = levelMusic;
        music.loop = true;
        music.Play();
    }

    #region Source Functions

    public void PlayOneShot(Sound sound)
    {
        AudioClip clip = GetClip(sound);

        if (clip == powerPelletEaten || clip == ghostEaten)
        {
            if(fx2.isPlaying && clip == powerPelletEaten)
            {
                fx2.Stop();
                fx2.PlayOneShot(clip);
            }
            else
            {
                fx2.PlayOneShot(clip);
            }
        }
        else
        {
            fx1.PlayOneShot(clip);
        }
    }


    private AudioClip GetClip(Sound sound)
    {
        switch (sound)
        {
            case Sound.PlayerDied:
                return playerDied;

            case Sound.FruitEaten:
                return fruitEaten;

            case Sound.FruitSpawned:
                return fruitSpawned;

            case Sound.GhostEaten:
                return ghostEaten;

            case Sound.PelletEaten:
                return pelletEaten;

            case Sound.PowerPelletEaten:
                return powerPelletEaten;

            case Sound.Start:
                return startGame;

            case Sound.GhostScream:
                return ghostScream;

            //case Sound.Win
            //    return winSound;

            default:
                return null;
        }
    }


    public AudioClip NullCheck(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogError(clip.name);
        }
        return clip;
    }


    public void ResetSource(AudioSource source)
    {
        source.volume = 1f;
        source.pitch = 1f;
    }


    public void SetVolume(AudioSource source, float volume)
    {
        source.volume = volume;
    }


    public void SetVolume(AudioSource source)
    {
        SetVolume(source, 1f);
    }


    public void SetPitch(AudioSource source, float pitch)
    {
        source.pitch = pitch;
    }


    public void SetPitch(AudioSource source)
    {
        SetPitch(source, 1f);
    }

    #endregion
}
