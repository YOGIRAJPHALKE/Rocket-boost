using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float LevelLoadDelay=2f;
    [SerializeField] AudioClip SuccesSfx;
    [SerializeField] AudioClip CreashedSFX;

    [SerializeField] ParticleSystem SuccessParticle;
    [SerializeField] ParticleSystem crashParticle;


    AudioSource audioSource;

    bool isControlable=true;
    bool isCollidiable=true;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    
    private void Update() {
        RespondToDebugKeys();
    }

    void RespondToDebugKeys(){
        if(Keyboard.current.lKey.wasPressedThisFrame)
        {
            LoadNextLevel();
        }
        else if(Keyboard.current.cKey.wasPressedThisFrame)
        {
            isCollidiable= !isCollidiable;
           // Debug.Log("C");
        }

    }

    private void OnCollisionEnter(Collision other)
    {
        if(! isControlable || !isCollidiable){return;}

        switch(other.gameObject.tag)
        {
            case "Friendly":
               Debug.Log("This is Friendly Tagged Object");
               break;

            case "Finish":
              StartSuccesSequence();
              break;
 
            default:
              StartCrashSequence();
              break;
        }
    }

    void StartSuccesSequence()
    {
        isControlable= false;
        audioSource.Stop();
       
        audioSource.PlayOneShot(SuccesSfx);
         SuccessParticle.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", LevelLoadDelay);
    }

    void StartCrashSequence()
    {
        isControlable = false;
        audioSource.Stop();
        crashParticle.Play();
        audioSource.PlayOneShot(CreashedSFX);
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", LevelLoadDelay);
         
    }

    void LoadNextLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int NextScene = currentScene+1;

        if(NextScene == SceneManager.sceneCountInBuildSettings)
        {
            NextScene =0;
        }
        SceneManager.LoadScene(NextScene);
    }
    
    void ReloadLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    } 
}
