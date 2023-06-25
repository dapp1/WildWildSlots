using Pixelplacement;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : Singleton<SoundManager>
{
    public AudioClip winner;
    public AudioClip buttonClick;
    public AudioClip PlusAndMinusClick;
    public AudioClip startSpin;
    public AudioClip loseGame;
    public AudioClip smallWin;
    public Toggle toggle;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        toggle.isOn = audioSource.enabled;

        toggle.onValueChanged.AddListener(ToggleSound);
    }

    private void ToggleSound(bool isSoundOn)
    {
        audioSource.enabled = isSoundOn;
    }

    public void StartSpin()
    {
        audioSource.PlayOneShot(startSpin);
    }

    public void ButtonClick()
    {
        audioSource.PlayOneShot(buttonClick);
    }

    public void PlusAndMinus()
    {
        audioSource.PlayOneShot(PlusAndMinusClick);
    }


    public void AudioPlay(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
