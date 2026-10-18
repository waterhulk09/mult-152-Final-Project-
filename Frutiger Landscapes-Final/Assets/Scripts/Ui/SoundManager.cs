using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    private bool isMuted = false;

    // Call this function if you are using a standard UI Button
    public void ToggleSoundButton()
    {
        isMuted = !isMuted;
        AudioListener.pause = isMuted;
    }

    // Call this function if you are using a UI Toggle (Checkbox)
    public void ToggleSoundCheckbox(bool isSoundOn)
    {
        // If sound is ON, pause is FALSE. If sound is OFF, pause is TRUE.
        AudioListener.pause = !isSoundOn;
    }
}