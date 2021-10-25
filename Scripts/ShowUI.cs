using UnityEngine;
using UnityEngine.Video;

public class ShowUI : MonoBehaviour
{
    public VideoPlayer player;
    public GameObject UI;
    public AudioSource audio;
    private void Start()
    {
        UI.SetActive(false);
    }
    public void SkipVideo()
    {
        audio.Play();
        player.Stop();
        UI.SetActive(true);
    }
}
