using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

/// <summary>
/// IDs of all inputFields
/// </summary>
public enum FIELD_ID
{
    // Adjust as needed
    SERVER_IP,
    IDLE_INTERVAL,
    GAME_DURATION,
    COMBO_BONUS,
    REWARD_AMT,

    MIN_SPAWN_TIME,
    MAX_SPAWN_TIME,
    MIN_NUM_SPAWN,
    MAX_NUM_SPAWN,

    MIN_FORCE_GUMMY,
    MAX_FORCE_GUMMY,
    MIN_SIDE_FORCE_GUMMY,
    MAX_SIDE_FORCE_GUMMY,

    MIN_FORCE_BOMB,
    MAX_FORCE_BOMB,
    MIN_SIDE_FORCE_BOMB,
    MAX_SIDE_FORCE_BOMB,

    MIN_SPAWN_IN_ROW,
    MAX_SPAWN_IN_ROW,

    FREEZE_DURATION,
    FEVER_DURATION,
    THIRD_TIER_DURATION,
    SECOND_TIER_DURATION,
    FIRST_TIER_DURATION,

    TIMETOTOGGLESPAWNERS,
}

/// <summary>
/// IDs of all toggles
/// </summary>
public enum TOGGLE_ID
{
    // Adjust as needed
    USE_SERVER,
    USE_MOCKY,
    //USE_KEYBOARD,
    MUTE_SOUND,
    ENABLE_COMBO,
    USE_MOUSE,
}

/// <summary>
/// IDs of all sliders
/// </summary>
public enum SLIDER_ID
{
    // Adjust as needed
    MASTER,
    MUSIC,
    SFX,
    SFX2,
    SFX3,
    SFX4,
    UI_SFX,
    UI_SFX2,
}

/// <summary>
/// Admin Panel
/// </summary>
public class TrinaxAdminPanel : MonoBehaviour
{
    [Header("InputFields")]
    [Space(3)]
    public TMP_InputField[] inputFields;

    [Header("Toggles")]
    [Space(3)]
    public Toggle[] toggles;

    [Header("Sliders")]
    [Space(3)]
    public Slider[] sliders;
    [Header("Slider values")]
    [Space(3)]
    public TextMeshProUGUI[] sliderValue;

    [Header("Panel Buttons")]
    [Space(3)]
    public Button closeBtn;
    public Button submitBtn;
    public Button pageBtn;
    public Button reporterBtn;

    [Header("Text")]
    [Space(3)]
    public TextMeshProUGUI result;

    [Header("Pages")]
    [Space(3)]
    public CanvasGroup[] pageList;

    [Header("Components")]
    [Space(3)]
    public Reporter reporter;

    private Color red = Color.red;
    private Color green = Color.green;

    private int selected = 0;
    private int maxInputFieldCount;
    private int current_InputField_Page = 0;
    private const int inputFields_per_page = 14; // number of inputfields per page

    private float hideResultText = 0f;
    private float DURATION_RESULT = 5f;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void Start()
    {
        maxInputFieldCount = inputFields.Length;

        InitListeners();
        PopulateSettings();
    }

    private void OnEnable()
    {
        result.gameObject.SetActive(false);
        SetPage(current_InputField_Page);
    }

    private void InitListeners()
    {
        closeBtn.onClick.AddListener(Close);
        submitBtn.onClick.AddListener(Submit);
        pageBtn.onClick.AddListener(() => CycleThroughPages());
        reporterBtn.onClick.AddListener(OpenReporter);
        //clearLB.onClick.AddListener(() => { LocalLeaderboardJson.Instance.Clear(); });

        toggles[(int)TOGGLE_ID.MUTE_SOUND].onValueChanged.AddListener(delegate { OnMuteAllSounds(toggles[(int)TOGGLE_ID.MUTE_SOUND]); });
    }

    private void Update()
    {
        HandleInputs();
        UpdateSliderValueText();

        if (hideResultText > 0)
        {
            hideResultText -= Time.deltaTime;
        }

        else
        {
            result.gameObject.SetActive(false);
            hideResultText = 0f;
        }
    }

    private void OnMuteAllSounds(Toggle toggle)
    {
        TrinaxAudioManager.Instance.MuteAllSounds(toggle.isOn);
    }

    private void HandleInputs()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            Submit();

        if (Input.GetKeyDown(KeyCode.Escape))
            Close();

        if (Input.GetKeyDown(KeyCode.Tab))
            CycleThroughInputFields(++selected);
    }

    private void UpdateSliderValueText()
    {
        sliderValue[(int)SLIDER_ID.MASTER].text = sliders[(int)SLIDER_ID.MASTER].value.ToString("0.0");
        sliderValue[(int)SLIDER_ID.MUSIC].text = sliders[(int)SLIDER_ID.MUSIC].value.ToString("0.0");
        sliderValue[(int)SLIDER_ID.SFX].text = sliders[(int)SLIDER_ID.SFX].value.ToString("0.0");
        sliderValue[(int)SLIDER_ID.SFX2].text = sliders[(int)SLIDER_ID.SFX2].value.ToString("0.0");
        sliderValue[(int)SLIDER_ID.SFX3].text = sliders[(int)SLIDER_ID.SFX3].value.ToString("0.0");
        sliderValue[(int)SLIDER_ID.SFX4].text = sliders[(int)SLIDER_ID.SFX4].value.ToString("0.0");
        sliderValue[(int)SLIDER_ID.UI_SFX].text = sliders[(int)SLIDER_ID.UI_SFX].value.ToString("0.0");
        sliderValue[(int)SLIDER_ID.UI_SFX2].text = sliders[(int)SLIDER_ID.UI_SFX2].value.ToString("0.0");
    }

    private void CycleThroughPages()
    {
        current_InputField_Page++;

        if (current_InputField_Page >= pageList.Length)
            current_InputField_Page = 0;

        if (current_InputField_Page == 0)
            selected = 0;
        else
            selected = current_InputField_Page * inputFields_per_page + selected;

        SetPage(current_InputField_Page);
    }

    private void SetPage(int index)
    {
        if (pageList.Length == 0) { Debug.LogWarning("Page list is empty!"); return; }

        for (int i = 0; i < pageList.Length; i++)
        {
            CanvasGroup page = pageList[i];
            if (i == index)
            {
                page.gameObject.SetActive(true);
                page.DOFade(1.0f, 0.25f).OnComplete(() =>
                {
                    page.interactable = true;
                    inputFields[current_InputField_Page * inputFields_per_page + selected].Select();
                    inputFields[current_InputField_Page * inputFields_per_page + selected].ActivateInputField();
                });
            }
            else
            {
                page.interactable = false;
                page.DOFade(0.0f, 0.25f);
                page.gameObject.SetActive(false);
            }
        }
    }

    private void CycleThroughInputFields(int index)
    {
        if (index % inputFields_per_page == 0)
        {
            CycleThroughPages();
            index = 0;
            selected = 0;
        }
        else
        {
            for (int i = 0; i < pageList.Length; i++)
            {
                if (i == current_InputField_Page)
                {
                    inputFields[i * inputFields_per_page + index].Select();
                    inputFields[i * inputFields_per_page + index].ActivateInputField();
                }
            }
        }
    }

    private void PopulateSettings()
    {
        //inputFields[(int)FIELD_ID.SERVER_IP].text = TrinaxAsyncServerManager.Instance.ip.ToString();
        inputFields[(int)FIELD_ID.IDLE_INTERVAL].text = TrinaxGlobal.Instance.gSettings.idleInterval.ToString();
        inputFields[(int)FIELD_ID.GAME_DURATION].text = TrinaxGlobal.Instance.gameSettings.gameDuration.ToString();
        inputFields[(int)FIELD_ID.REWARD_AMT].text = TrinaxGlobal.Instance.gameSettings.rewardAmt.ToString();

        inputFields[(int)FIELD_ID.MIN_SPAWN_TIME].text = TrinaxGlobal.Instance.gameSettings.minSpawnTime.ToString();
        inputFields[(int)FIELD_ID.MAX_SPAWN_TIME].text = TrinaxGlobal.Instance.gameSettings.maxSpawnTime.ToString();
        inputFields[(int)FIELD_ID.MIN_NUM_SPAWN].text = TrinaxGlobal.Instance.gameSettings.minToSpawn.ToString();
        inputFields[(int)FIELD_ID.MAX_NUM_SPAWN].text = TrinaxGlobal.Instance.gameSettings.maxToSpawn.ToString();

        inputFields[(int)FIELD_ID.MIN_FORCE_GUMMY].text = TrinaxGlobal.Instance.gameSettings.minForceCupcake.ToString();
        inputFields[(int)FIELD_ID.MAX_FORCE_GUMMY].text = TrinaxGlobal.Instance.gameSettings.maxForceCupcake.ToString();
        inputFields[(int)FIELD_ID.MIN_SIDE_FORCE_GUMMY].text = TrinaxGlobal.Instance.gameSettings.minSideForceCupcake.ToString();
        inputFields[(int)FIELD_ID.MAX_SIDE_FORCE_GUMMY].text = TrinaxGlobal.Instance.gameSettings.maxSideForceCupcake.ToString();

        inputFields[(int)FIELD_ID.MIN_FORCE_BOMB].text = TrinaxGlobal.Instance.gameSettings.minForceBomb.ToString();
        inputFields[(int)FIELD_ID.MAX_FORCE_BOMB].text = TrinaxGlobal.Instance.gameSettings.maxForceBomb.ToString();
        inputFields[(int)FIELD_ID.MIN_SIDE_FORCE_BOMB].text = TrinaxGlobal.Instance.gameSettings.minSideForceBomb.ToString();
        inputFields[(int)FIELD_ID.MAX_SIDE_FORCE_BOMB].text = TrinaxGlobal.Instance.gameSettings.maxSideForceBomb.ToString();

        inputFields[(int)FIELD_ID.MIN_SPAWN_IN_ROW].text = TrinaxGlobal.Instance.gameSettings.minSpawnInRow.ToString();
        inputFields[(int)FIELD_ID.MAX_SPAWN_IN_ROW].text = TrinaxGlobal.Instance.gameSettings.maxSpawnInRow.ToString();

        inputFields[(int)FIELD_ID.FREEZE_DURATION].text = TrinaxGlobal.Instance.gameSettings.freezeDuration.ToString();
        inputFields[(int)FIELD_ID.FEVER_DURATION].text = TrinaxGlobal.Instance.gameSettings.feverDuration.ToString();
        inputFields[(int)FIELD_ID.THIRD_TIER_DURATION].text = TrinaxGlobal.Instance.gameSettings.third_tier_score.ToString();
        inputFields[(int)FIELD_ID.SECOND_TIER_DURATION].text = TrinaxGlobal.Instance.gameSettings.second_tier_score.ToString();
        inputFields[(int)FIELD_ID.FIRST_TIER_DURATION].text = TrinaxGlobal.Instance.gameSettings.first_tier_score.ToString();
        inputFields[(int)FIELD_ID.TIMETOTOGGLESPAWNERS].text = TrinaxGlobal.Instance.gameSettings.timeToToggleSpawners.ToString();
        inputFields[(int)FIELD_ID.COMBO_BONUS].text = TrinaxGlobal.Instance.gameSettings.comboBonus.ToString();

        toggles[(int)TOGGLE_ID.USE_SERVER].isOn = TrinaxGlobal.Instance.gSettings.useServer;
        toggles[(int)TOGGLE_ID.USE_MOCKY].isOn = TrinaxGlobal.Instance.gSettings.useMocky;
        //toggles[(int)TOGGLE_ID.USE_KEYBOARD].isOn = TrinaxGlobal.Instance.gSettings.useKeyboard;
        toggles[(int)TOGGLE_ID.MUTE_SOUND].isOn = TrinaxGlobal.Instance.gSettings.muteAllSounds;
        toggles[(int)TOGGLE_ID.ENABLE_COMBO].isOn = TrinaxGlobal.Instance.gameSettings.EnableCombo;
        toggles[(int)TOGGLE_ID.USE_MOUSE].isOn = TrinaxGlobal.Instance.gSettings.useMouse;
    }

    /// <summary>
    /// Saves the value to respective fields.
    /// </summary>
    void Submit()
    {
        string resultText = "Empty";
        //if (string.IsNullOrEmpty(inputFields[(int)FIELD_ID.SERVER_IP].text.Trim()))
        //{
        //    Debug.Log("Mandatory fields in admin panel is empty!");
        //    result.color = red;
        //    resultText = "Need to fill mandatory fields!";
        //}
        //else
        //{
            result.color = green;
            resultText = "Success!";
            //TrinaxGlobal.Instance.gSettings.IP = inputFields[(int)FIELD_ID.SERVER_IP].text.Trim();
            TrinaxGlobal.Instance.gSettings.idleInterval = float.Parse(inputFields[(int)FIELD_ID.IDLE_INTERVAL].text);
            TrinaxGlobal.Instance.gameSettings.gameDuration = int.Parse(inputFields[(int)FIELD_ID.GAME_DURATION].text);
            TrinaxGlobal.Instance.gameSettings.rewardAmt = int.Parse(inputFields[(int)FIELD_ID.REWARD_AMT].text);
            TrinaxGlobal.Instance.gameSettings.minSpawnTime = float.Parse(inputFields[(int)FIELD_ID.MIN_SPAWN_TIME].text);
            TrinaxGlobal.Instance.gameSettings.maxSpawnTime = float.Parse(inputFields[(int)FIELD_ID.MAX_SPAWN_TIME].text);
            TrinaxGlobal.Instance.gameSettings.minToSpawn = int.Parse(inputFields[(int)FIELD_ID.MIN_NUM_SPAWN].text);
            TrinaxGlobal.Instance.gameSettings.maxToSpawn = int.Parse(inputFields[(int)FIELD_ID.MAX_NUM_SPAWN].text);

            TrinaxGlobal.Instance.gameSettings.minForceCupcake = float.Parse(inputFields[(int)FIELD_ID.MIN_FORCE_GUMMY].text);
            TrinaxGlobal.Instance.gameSettings.maxForceCupcake = float.Parse(inputFields[(int)FIELD_ID.MAX_FORCE_GUMMY].text);
            TrinaxGlobal.Instance.gameSettings.minSideForceCupcake = float.Parse(inputFields[(int)FIELD_ID.MIN_SIDE_FORCE_GUMMY].text);
            TrinaxGlobal.Instance.gameSettings.maxSideForceCupcake = float.Parse(inputFields[(int)FIELD_ID.MAX_SIDE_FORCE_GUMMY].text);

            TrinaxGlobal.Instance.gameSettings.minForceBomb = float.Parse(inputFields[(int)FIELD_ID.MIN_FORCE_BOMB].text);
            TrinaxGlobal.Instance.gameSettings.maxForceBomb = float.Parse(inputFields[(int)FIELD_ID.MAX_FORCE_BOMB].text);
            TrinaxGlobal.Instance.gameSettings.minSideForceBomb = float.Parse(inputFields[(int)FIELD_ID.MIN_SIDE_FORCE_BOMB].text);
            TrinaxGlobal.Instance.gameSettings.maxSideForceBomb = float.Parse(inputFields[(int)FIELD_ID.MAX_SIDE_FORCE_BOMB].text);

            TrinaxGlobal.Instance.gameSettings.minSpawnInRow = int.Parse(inputFields[(int)FIELD_ID.MIN_SPAWN_IN_ROW].text);
            TrinaxGlobal.Instance.gameSettings.maxSpawnInRow = int.Parse(inputFields[(int)FIELD_ID.MAX_SPAWN_IN_ROW].text);


            TrinaxGlobal.Instance.gameSettings.comboBonus = int.Parse(inputFields[(int)FIELD_ID.COMBO_BONUS].text);

            TrinaxGlobal.Instance.gameSettings.freezeDuration = int.Parse(inputFields[(int)FIELD_ID.FREEZE_DURATION].text);
            TrinaxGlobal.Instance.gameSettings.feverDuration = float.Parse(inputFields[(int)FIELD_ID.FEVER_DURATION].text);
            TrinaxGlobal.Instance.gameSettings.third_tier_score = int.Parse(inputFields[(int)FIELD_ID.THIRD_TIER_DURATION].text);
            TrinaxGlobal.Instance.gameSettings.second_tier_score = int.Parse(inputFields[(int)FIELD_ID.SECOND_TIER_DURATION].text);
            TrinaxGlobal.Instance.gameSettings.first_tier_score = int.Parse(inputFields[(int)FIELD_ID.FIRST_TIER_DURATION].text);

            TrinaxGlobal.Instance.gameSettings.timeToToggleSpawners = float.Parse(inputFields[(int)FIELD_ID.TIMETOTOGGLESPAWNERS].text);

            TrinaxGlobal.Instance.gSettings.useServer = toggles[(int)TOGGLE_ID.USE_SERVER].isOn;
            TrinaxGlobal.Instance.gSettings.useMocky = toggles[(int)TOGGLE_ID.USE_MOCKY].isOn;
            //TrinaxGlobal.Instance.gSettings.useKeyboard = toggles[(int)TOGGLE_ID.USE_KEYBOARD].isOn;
            TrinaxGlobal.Instance.gSettings.muteAllSounds = toggles[(int)TOGGLE_ID.MUTE_SOUND].isOn;
            TrinaxGlobal.Instance.gameSettings.EnableCombo = toggles[(int)TOGGLE_ID.ENABLE_COMBO].isOn;
            TrinaxGlobal.Instance.gSettings.useMouse = toggles[(int)TOGGLE_ID.USE_MOUSE].isOn;

            TrinaxSaveManager.Instance.SaveJson();

            GameManager.Instance.RefreshSettings(TrinaxGlobal.Instance.gSettings, TrinaxGlobal.Instance.gameSettings, TrinaxGlobal.Instance.kinectSettings);
        //}

        result.text = resultText;
        result.gameObject.SetActive(true);
        hideResultText = DURATION_RESULT;
    }

    private void Close()
    {
        gameObject.SetActive(false);
    }

    private void OpenReporter()
    {
        reporter.doShow();
    }
}
