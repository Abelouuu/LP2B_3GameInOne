using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public AudioSource buttonClickSound;

    public void PlayButtonClickSound()
    {
        if (buttonClickSound != null)
        {
            buttonClickSound.Play();
        }
    }
}
