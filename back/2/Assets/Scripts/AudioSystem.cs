using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// Global audio system puts itself on every scene
/// </summary>
public class AudioSystem : MonoBehaviour
{
    public AudioSource musicNotCompeting;
    public AudioSource musicCompeting;
    public AudioSource musicSucceeded;
    public AudioSource musicFailed;

    /// <summary>
    /// The scene number indicating current scene
    /// </summary>
    private int SceneNumber;

    /// <summary>
    /// This is called to track the scene load event
    /// </summary>
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /// <summary>
    /// We keep the AudioSystem on each scene load so that
    /// the music won't stop playing
    /// </summary>
    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    /// <summary>
    /// We check the scene and decided which music to play
    /// </summary>
    /// <param name="scene">The scene loaded</param>
    /// <param name="mode">Scene load mode</param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "NormalMode")
        {
            Debug.Log("[AudioSystem:OnSceneLoaded()] Normal mode loaded, play musicCompeting.");
            musicNotCompeting.Stop();
            musicCompeting.Play();
            SceneNumber = 0;
        }
        else if (scene.name == "OccupationPattern")
        {
            Debug.Log("[AudioSystem:OnSceneLoaded()] Occupation mode loaded, play musicCompeting");
            musicNotCompeting.Stop();
            musicCompeting.Play();
            SceneNumber = 1;
        }
        else if (scene.name == "ending")
        {
            // in the ending, we need to play the ending music first, then the non-competing music
            Debug.Log("[AudioSystem:OnSceneLoaded()] Ending mode loaded, another method should handle this.");
            SceneNumber = 2;
        }
        else
        {
            Debug.Log("[AudioSystem:OnSceneLoaded()] Other scene loaded, try to play musicNonCompeting (if not already playing)");

            // stop competing music and all other musics
            musicCompeting.Stop();
            musicSucceeded.Stop();
            musicFailed.Stop();

            // play non-competing music if not already playing
            if (!musicNotCompeting.isPlaying)
            {
                musicNotCompeting.Play();
            }
            SceneNumber = 3;
        }
    }

    /// <summary>
    /// This is called when the game is terminated, so remove the handler
    /// </summary>
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// Play ending music based on win or lose, this should be called from outside
    /// so that the status flag can be properly set
    /// </summary>
    /// <param name="status">0 for lose, 1 for win</param>
    public void PlayEndMusic(int status)
    {
        // Stop all music currently playing
        musicFailed.Stop();
        musicSucceeded.Stop();
        musicCompeting.Stop();
        musicNotCompeting.Stop();

        // Play music based on status
        if (status == 0)
        {
            musicFailed.Play();
        }
        else if (status == 1)
        {
            musicSucceeded.Play();
        }
        else
        {
            Debug.LogError("[AudioSystem:PlayEndMusic()] BUG: PlayEndMusic Called with wrong status!");
        }
    }

    /// <summary>
    /// This is only used when we are at ending scene and need to play the BGM after
    /// lose (win) BGM
    /// </summary>
    private void Update()
    {
        if (SceneNumber == 2 && !musicFailed.isPlaying && !musicSucceeded.isPlaying)
        {
            Debug.Log("[AudioSystem:Update()] Start playing BGM after win/lose music.");
            musicNotCompeting.Play();
        }
    }
}
