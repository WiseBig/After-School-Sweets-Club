using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoOption : MonoBehaviour
{
    public Dropdown resloutionDropdown;
    List<Resolution> resolutions = new List<Resolution>();
    FullScreenMode screenMode;

    public Toggle fullScreenBtn;

    int resolutionNum;
    // Start is called before the first frame update
    void Start()
    {
        InitializeResolutions();
        Display();
    }
    void InitializeResolutions()
    {
        resolutions.Add(new Resolution { width = 800, height = 600 });
        resolutions.Add(new Resolution { width = 960, height = 540 });
        resolutions.Add(new Resolution { width = 1024, height = 768 });
        resolutions.Add(new Resolution { width = 1280, height = 720 });
        resolutions.Add(new Resolution { width = 1600, height = 900});
        resolutions.Add(new Resolution { width = 1920, height = 1080 });
        resolutions.Add(new Resolution { width = 2560, height = 1440 });
    }
    void Display()
    {
        resloutionDropdown.options.Clear();
        int optionNum = 0;
        foreach (Resolution item in resolutions)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = item.width + "x" + item.height;
            resloutionDropdown.options.Add(option);
            if (item.width == Screen.width && item.height == Screen.height)
                resloutionDropdown.value = optionNum;
            optionNum++;
        }
        resloutionDropdown.RefreshShownValue();
        fullScreenBtn.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow)? true : false;
    }
    // Update is called once per frame
    
    public void FullScreenBtn(bool isFull)
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }
    public void OkBtn()
    {
        Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, screenMode);
    }
    public void DropdownOptionChange(int x)
    {
        resolutionNum = x;
    }
}
