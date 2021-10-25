using UnityEngine;
using UnityEngine.UI;

public class Click : MonoBehaviour
{
    public AudioSource source;
    public AudioClip clicking;
    public Button exit_btn;
    public void Clicks()
    {
        source.clip = clicking;
        source.Play();

    }
}
