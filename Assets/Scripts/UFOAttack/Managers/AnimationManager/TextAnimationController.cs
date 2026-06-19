using TMPro;
using UnityEngine;

public class TextAnimationController : MonoBehaviour
{
    public TextMeshProUGUI myText;
    public AudioSource countdownAudio;

    // Appelée par un Animation Event
    public void SetText(string newText)
    {
        myText.text = newText;
    }

    public void PlayCountdownSound()
    {
        if (countdownAudio != null)
        {
            countdownAudio.Play();
        }
    }   
}