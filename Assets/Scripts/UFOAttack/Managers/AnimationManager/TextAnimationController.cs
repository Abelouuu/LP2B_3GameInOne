using TMPro;
using UnityEngine;

public class TextAnimationController : MonoBehaviour
{
    // Texte TMP qui sera modifié pendant l'animation
    public TextMeshProUGUI myText;

    // Source audio utilisée pour jouer le son du compte à rebours
    public AudioSource countdownAudio;

    // Fonction appelée par un Animation Event pour changer le texte affiché
    public void SetText(string newText)
    {
        myText.text = newText;
    }

    public void PlayCountdownSound()
    {
        // Joue le son seulement si la source audio est bien assignée
        if (countdownAudio != null)
        {
            countdownAudio.Play();
        }
    }   
}