using UnityEngine;

public class MusicManager : MonoBehaviour
{
    /*****************************************************************************************************************************************
    * 
    *                                                  Game Main Music Handler
    *
    *****************************************************************************************************************************************/
    
    
    public static SFXmanager_script instance { get; private set; }
    private void Awake()
    {
        if (MusicGameObject == null)
        {
            MusicGameObject = new GameObject("Music GameObject");
            MusicAudioSource = MusicGameObject.AddComponent<AudioSource>();
        }
    }

    //main menu music
    public enum MenuMusic
    {

    }

    public MenuMusicAudioClip[] MenuMusicAudioClipArray;            

    [System.Serializable]
    public class MenuMusicAudioClip
    {
        public MenuMusic sound;        
        public AudioClip audioClip;     
    }


    //main game music
    public enum MainMusic
    {

    }

    public MainMusicAudioClip[] MainMusicAudioClipArray;            

    [System.Serializable]
    public class MainMusicAudioClip
    {
        public MainMusic sound;         
        public AudioClip audioClip;     
    }


    //---Playing music logic
    public static GameObject MusicGameObject;         
    public static AudioSource MusicAudioSource;      
    public void PlayMainMusic()
    {
        MainMusic randomSong = GetRandomMainSong(); // Fetch a random song
        AudioClip clip = GetMusicClip(randomSong, null);

        if (clip != null)
        {
            CancelInvoke(nameof(PlayMainMusic));
            MusicAudioSource.clip = clip;
            MusicAudioSource.Play();
            
            Invoke(nameof(PlayMainMusic), clip.length); // Schedule next song
        }
        else
        {
            Debug.LogError("No clip found, cannot play music.");
        }

    }

    public void PlayMenuMusic()
    {
        MenuMusic randomSong = GetRandomMenuSong(); // Fetch a random song
        AudioClip clip = GetMusicClip(null, randomSong);

        if (clip != null)
        {
            MusicAudioSource.clip = clip;
            MusicAudioSource.Play();
            Invoke(nameof(PlayMenuMusic), clip.length); // Schedule next song
        }
        else
        {
            Debug.LogError("No clip found, cannot play music.");
        }

    }
    //song randomizer
    private MainMusic GetRandomMainSong()
    {
        int randomIndex = UnityEngine.Random.Range(0, MainMusicAudioClipArray.Length);
        return MainMusicAudioClipArray[randomIndex].sound;
    }
    private MenuMusic GetRandomMenuSong()
    {
        int randomIndex = UnityEngine.Random.Range(0, MenuMusicAudioClipArray.Length);
        return MenuMusicAudioClipArray[randomIndex].sound;
    }


    //returns an actual music clip for a certain song name
    private AudioClip GetMusicClip(MainMusic? mainSong, MenuMusic? menuSong)
    {
        if (mainSong != null)
        {
            foreach (MainMusicAudioClip soundAudioClip in MainMusicAudioClipArray)
            {
                if (soundAudioClip.sound == mainSong)
                {
                    return soundAudioClip.audioClip;
                }
            }

            Debug.LogError($"Sound {mainSong} not found!");
        }

        else if (menuSong != null)
        {
            foreach (MenuMusicAudioClip soundAudioClip in MenuMusicAudioClipArray)
            {
                if (soundAudioClip.sound == menuSong)
                {
                    return soundAudioClip.audioClip;
                }
            }

            Debug.LogError($"Sound {menuSong} not found!");
        }
        return null;
    }



    public void changeMusicVolume(float value)
    {
        if (MusicAudioSource != null)
            MusicAudioSource.volume = Mathf.Clamp01(value);
    }
}
