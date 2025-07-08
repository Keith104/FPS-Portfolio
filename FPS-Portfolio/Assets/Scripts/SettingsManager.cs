using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Linq;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance;
    public TMP_Dropdown windowModeDropdown;
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown qualityDropdown;
    public Button vysncButton;
    public TMP_Text vysncLabel;
    public Slider volumeSlider;
    public Slider sensSlider;
    public AudioMixer masterMixer;

    private FullScreenMode[] screenModes = {
        FullScreenMode.Windowed,
        FullScreenMode.ExclusiveFullScreen,
        FullScreenMode.FullScreenWindow
    };
    private Resolution[] resolutions;
    private int tempWindowMode;
    private int tempResolution;
    private int tempQuality;
    private bool tempVsync;
    private float tempVolume;
    private int tempSens;

    void Awake() => instance = this;

    void Start()
    {
        windowModeDropdown.ClearOptions();
        windowModeDropdown.AddOptions(new[] { "Windowed", "Fullscreen", "Borderless" }.ToList());

        resolutions = Screen.resolutions.Distinct().ToArray();
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resolutions.Select(r => $"{r.width}x{r.height}@{r.refreshRate}Hz").ToList());

        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(QualitySettings.names.ToList());

        tempWindowMode = Mathf.Clamp(PlayerPrefs.GetInt("windowMode", System.Array.IndexOf(screenModes, Screen.fullScreenMode)), 0, screenModes.Length - 1);
        tempResolution = Mathf.Clamp(PlayerPrefs.GetInt("resolution", resolutions.Length - 1), 0, resolutions.Length - 1);
        tempQuality = Mathf.Clamp(PlayerPrefs.GetInt("quality", QualitySettings.GetQualityLevel()), 0, QualitySettings.names.Length - 1);
        tempVsync = PlayerPrefs.GetInt("vSync", 1) == 1;
        tempVolume = Mathf.Clamp01(PlayerPrefs.GetFloat("volume", 1f));

        sensSlider.wholeNumbers = true;
        sensSlider.minValue = 0;
        sensSlider.maxValue = 600;
        tempSens = PlayerPrefs.GetInt("sens", 400);

        Debug.Log("Player Pref Loaded as: " +  tempSens);

        windowModeDropdown.value = tempWindowMode;
        resolutionDropdown.value = tempResolution;
        qualityDropdown.value = tempQuality;
        vysncLabel.text = tempVsync ? "ON" : "OFF";

        masterMixer.SetFloat("MasterVolume", Mathf.Lerp(-80f, 0f, tempVolume));
        volumeSlider.value = tempVolume;
        sensSlider.value = tempSens;
    }

    public void SetTempWindowMode(int i) => tempWindowMode = i;
    public void SetTempResolution(int i) => tempResolution = i;
    public void SetTempQuality(int i) => tempQuality = i;
    public void SetTempVolume(float v) => tempVolume = v;
    public void SetTempSens(float s) => tempSens = Mathf.RoundToInt(s);

    public void ToggleTempVysnc()
    {
        tempVsync = !tempVsync;
        vysncLabel.text = tempVsync ? "ON" : "OFF";
    }

    public void ApplySettings()
    {
        tempWindowMode = windowModeDropdown.value;
        tempResolution = resolutionDropdown.value;
        tempQuality = qualityDropdown.value;
        tempVolume = volumeSlider.value;
        tempSens = Mathf.RoundToInt(sensSlider.value);

        var r = resolutions[tempResolution];
        Screen.SetResolution(r.width, r.height, screenModes[tempWindowMode], r.refreshRateRatio);
        QualitySettings.SetQualityLevel(tempQuality);
        QualitySettings.vSyncCount = tempVsync ? 1 : 0;
        masterMixer.SetFloat("MasterVolume", Mathf.Lerp(-80f, 0f, tempVolume));

        PlayerPrefs.SetInt("windowMode", tempWindowMode);
        PlayerPrefs.SetInt("resolution", tempResolution);
        PlayerPrefs.SetInt("quality", tempQuality);
        PlayerPrefs.SetInt("vSync", tempVsync ? 1 : 0);
        PlayerPrefs.SetFloat("volume", tempVolume);
        PlayerPrefs.SetInt("sens", tempSens);
        PlayerPrefs.Save();

        Debug.Log("Player Pref Saved As:" + PlayerPrefs.GetInt("sens"));

        if (GameManager.instance != null)
            GameManager.instance.SetSens(tempSens);
    }

    public void RevertSettings() => Start();
}
