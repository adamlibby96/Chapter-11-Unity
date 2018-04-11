using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour, IGameManager {

    [SerializeField] private AudioSource soundSource;
    [SerializeField] private AudioSource music1Source;
    [SerializeField] private AudioSource music2Source;
    [SerializeField] private string ambientSoundName;
    [SerializeField] private string ambientSoundName2;

    private AudioSource _activeMusic;
    private AudioSource _inactiveMusic;

    public float crossFadeRate = 1.5f;
    private bool _corssFading;
    private float _musicVolume;

    public ManagerStatus status { get; private set; }

    public float musicVolume
    {
        get
        {
            return _musicVolume;
        }
        set
        {
            _musicVolume = value;
            if (music1Source != null && !_corssFading)
            {
                music1Source.volume = _musicVolume;
                music2Source.volume = _musicVolume;
            }
        }
    }
    public bool musicMute
    {
        get
        {
            if (music1Source != null)
            {
                return music1Source.mute;
            }
            return false;
        }
        set
        {
            if (music1Source != null)
            {
                music1Source.mute = value;
                music2Source.mute = value;
            }
        }
    }

    public void PlayIntroMusic()
    {
        PlayMusic(Resources.Load("Music/" + ambientSoundName) as AudioClip);
    }

    public void PlayLevelMusic()
    {
        PlayMusic(Resources.Load("Music/" + ambientSoundName2) as AudioClip);
    }

    private void PlayMusic(AudioClip clip)
    {
        if (_corssFading)
        {
            return;
        }
        StartCoroutine(CrossFadeMusic(clip));
        //music1Source.clip = clip;
        //music1Source.Play();
    }

    private IEnumerator CrossFadeMusic(AudioClip clip)
    {
        _corssFading = true;

        _inactiveMusic.clip = clip;
        _inactiveMusic.volume = 0;
        _inactiveMusic.Play();

        float scaledRate = crossFadeRate * _musicVolume;
        while (_activeMusic.volume > 0)
        {
            _activeMusic.volume -= scaledRate * Time.deltaTime;
            _inactiveMusic.volume += scaledRate * Time.deltaTime;
            yield return null;
        }

        AudioSource temp = _activeMusic;

        _activeMusic = _inactiveMusic;
        _activeMusic.volume = _musicVolume;

        _inactiveMusic = temp;
        _inactiveMusic.Stop();

        _corssFading = false; 
    }

    public void StopMusic()
    {
       // music1Source.Stop();
        _activeMusic.Stop();
        _inactiveMusic.Stop();
    }

    public void PlaySound(AudioClip clip)
    {
        soundSource.PlayOneShot(clip);
    }

    public void Startup()
    {
        Debug.Log("Audio manager starting...");
        

        music1Source.ignoreListenerVolume = true;
        music1Source.ignoreListenerPause = true;
        music2Source.ignoreListenerVolume = true;
        music2Source.ignoreListenerPause = true;

        musicVolume = 1;
        soundVolume = 1;

        _activeMusic = music1Source;
        _inactiveMusic = music2Source;
        
        status = ManagerStatus.Started;
    }

    private IEnumerator startCrossFade()
    {
        yield return new WaitForSeconds(3);
        PlayLevelMusic();
    }

    public float soundVolume
    {
        get { return AudioListener.volume; }
        set { AudioListener.volume = value; }
    }

    public bool soundMute
    {
        get { return AudioListener.pause; }
        set { AudioListener.pause = value; }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
