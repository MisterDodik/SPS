using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SFXmanager_script;

public class SFXmanager_script : MonoBehaviour
{

    /********************************************************************************************************
     * 
     * 
     * To play any sound use:
     * 
     * 
     * SFXmanager_script.PlayUI_Sound(SFXmanager_script.UI_Sound.ButtonHover);
     *                          or
     * SFXmanager_script.PlaySound(SFXmanager_script.GameSound.PlayerWalk);
     *                          or  
     * SFXmanager_script.PlaySound3D(SFXmanager_script.GameSound.PlayerWalk, transform.position);
     * 
     * 
     * 
     * *****************************************************************************************************/






    public static SFXmanager_script instance { get; private set; }     

    private void Awake()
    {
        Initialize();       // This function initializes a dictionary below
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }


        if (oneShotGameObject == null)
        {
            oneShotGameObject = new GameObject("One Shot Sound");
            oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>();
        }
    }



    public enum UI_Sound
    {
       
    }
    public enum GameSound
    {
        PlayerMove
    }


    public UI_SoundAudioClip[] UI_SoundAudioClipArray;          

    [System.Serializable]
    public class UI_SoundAudioClip
    {
        public UI_Sound sound;         
        public AudioClip audioClip; 
    }


    public GameSoundAudioClip[] gameSoundAudioClipArray;            

    [System.Serializable]
    public class GameSoundAudioClip
    {
        public GameSound sound;         
        public AudioClip audioClip; 
    }


    public float sfxVolume = 1;


    public static void PlaySound3D(GameSound sound, Vector2 position)     
    {
        if (canPlaySound(sound))  
        {
            GameObject soundGameObject= new GameObject("Sound");
            soundGameObject.transform.position = position; 
            AudioSource audioSource=soundGameObject.AddComponent<AudioSource>();

            audioSource.clip=instance.GetAudioClip(null, sound);

            // Adjustments
            audioSource.maxDistance = 100f;                      
            audioSource.spatialBlend = 1f;
            audioSource.rolloffMode=AudioRolloffMode.Linear;
            audioSource.dopplerLevel = 0f;
            
            audioSource.volume = instance.sfxVolume;

            audioSource.Play();

            Destroy(soundGameObject, audioSource.clip.length);
        }
    }
    
    public static GameObject oneShotGameObject;         
    public static AudioSource oneShotAudioSource;      
    public static void PlaySound(GameSound sound)          
    {
        if (canPlaySound(sound))        
        {
            //oneShotAudioSource.pitch = UnityEngine.Random.Range(0.8f, 1f);        // To prevent ear fatigue
            oneShotAudioSource.PlayOneShot(instance.GetAudioClip(null, sound));
        }
    }

    public static void PlayUI_Sound(UI_Sound sound)          
    {
        oneShotAudioSource.PlayOneShot(instance.GetAudioClip(sound, null));
    }

    private AudioClip GetAudioClip(UI_Sound? UI_Sound, GameSound? gameSound)             //this just returns the right audioclip based on the name of the sound
    {
        if (UI_Sound != null)        
        {
            foreach(UI_SoundAudioClip soundAudioClip in UI_SoundAudioClipArray)
            {
                if(soundAudioClip.sound == UI_Sound)
                {
                    return soundAudioClip.audioClip;
                }
            }
            Debug.LogError("Sound " + UI_Sound + "not found :(");    
        }

        else if (gameSound != null)    
        {
            foreach (GameSoundAudioClip soundAudioClip in gameSoundAudioClipArray)
            {
                if (soundAudioClip.sound == gameSound)
                {
                    return soundAudioClip.audioClip;
                }
            }
            Debug.LogError("Sound " + gameSound + "not found :(");
        }

        return null;
    }



    public static Dictionary<GameSound, float> soundTimerDictionary = new Dictionary<GameSound, float>();       //  This is used for certain sounds that need to be delayed
    public static void Initialize()
    {
        soundTimerDictionary= new Dictionary<GameSound, float>();
        soundTimerDictionary[GameSound.PlayerMove] = 0;           
    }
    private static bool canPlaySound(GameSound sound)
    {
        switch (sound)
        {
            case GameSound.PlayerMove:     
                if (soundTimerDictionary.ContainsKey(GameSound.PlayerMove))
                {
                    float lastTimePlayed = soundTimerDictionary[GameSound.PlayerMove];
                    float playerMoveTimerMax = 0.3f;
                    if (lastTimePlayed + playerMoveTimerMax < Time.time)
                    {
                        soundTimerDictionary[GameSound.PlayerMove] = Time.time;
                        return true;
                    }
                    else
                        return false;

                }
                else
                    return false;


            default:            // Basically most of the sounds can be played anytime, but sound like player walk need to be delayed a bit
                return true;
        }
    }
    


    public void changeSfxVolume(float value)
    {
        if(oneShotAudioSource!= null)
            oneShotAudioSource.volume = Mathf.Clamp01(value);
        instance.sfxVolume = Mathf.Clamp01(value);
    }
  
}
