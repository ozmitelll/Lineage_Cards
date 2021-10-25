using TMPro;
using UnityEngine;

public class AuthUIManager : MonoBehaviour
{
    public static AuthUIManager instance;
    public AudioSource click_clip;
    [Header("References")]
    [SerializeField]
    private GameObject checkingForAccountUI;
    [SerializeField]
    private GameObject loginUI;
    [SerializeField]
    private GameObject registerUI;
    [SerializeField]
    private GameObject verifyEmailUI;
    [SerializeField]
    private TMP_Text verifyEmailText;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void CleanUI()
    {
        registerUI.SetActive(false);
        loginUI.SetActive(false);
        FirebaseManager.instance.ClearOutputs();
        //checkingForAccountUI.SetActive(false);
    }
    public void LoginScreen()
    {
        click_clip.Play();
        CleanUI();
        loginUI.SetActive(true);
    }
    public void RegisterScreen()
    {
        click_clip.Play();
        CleanUI();
        registerUI.SetActive(true);
    }
}
