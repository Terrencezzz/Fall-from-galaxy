using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

// [ExecuteAlways] // Allows the script to run in edit mode
public class TutorialGameManager : MonoBehaviour
{
    public List<GameObject> characters = new List<GameObject>();
    private int currentCharacterIndex = 0;

    [Header("Lighting Settings")]
    public bool useAmbientLight = true;
    [ColorUsage(false, true)]
    public Color ambientLightColor = Color.white;

    public Material skyboxMaterial;
    public bool useSkybox = true;
    public bool useSkyboxAmbientLight = true;
    [Range(0f, 8f)]
    public float skyboxAmbientIntensity = 0f;

    [Range(0f, 8f)]
    public float sceneLightIntensity = 0f;
    [Range(0f, 8f)]
    public float flashlightIntensity = 1.0f;

    public GameObject player;
    public GameObject robotPrefab; // Robot prefab for spawning

    // Message UI 
    [Header("Message UI")]
    public Canvas messageCanvas;
    public TextMeshProUGUI messageText;
    bool messageDisplayed = false;
    bool msg1 = false;
    bool msg2 = false;
    bool msg3 = false;
    bool msg4 = false;
    bool msg5 = false;
    bool msg6 = false;

    public static TutorialGameManager Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        ApplyLightingSettings();
        InitializeCharacters();
    }

    void OnValidate()
    {
        ApplyLightingSettings();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            SwitchCharacter();

        if (Input.GetKeyDown(KeyCode.Q))
            TrySpawnRobot();

        if (Input.GetKeyDown(KeyCode.L))
        {
            useAmbientLight = !useAmbientLight;
            ApplyLightingSettings();
        }
        DisplayTutorialMessages();
    }

    void ApplyLightingSettings()
    {
        if (useAmbientLight)
        {
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = ambientLightColor;
        }
        else if (useSkyboxAmbientLight && skyboxMaterial != null)
        {
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Skybox;
            RenderSettings.ambientIntensity = skyboxAmbientIntensity;
        }
        else
        {
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = Color.black;
        }

        RenderSettings.skybox = useSkybox ? skyboxMaterial : null;

        Light[] allLights = FindObjectsOfType<Light>();
        foreach (Light light in allLights)
        {
            light.intensity = IsFlashlight(light) ? flashlightIntensity : sceneLightIntensity;
        }
    }

    void InitializeCharacters()
    {
        PlayerController[] players = FindObjectsOfType<PlayerController>();
        RobotController[] robots = FindObjectsOfType<RobotController>();

        characters.Clear();
        characters.AddRange(System.Array.ConvertAll(players, p => p.gameObject));
        characters.AddRange(System.Array.ConvertAll(robots, r => r.gameObject));

        for (int i = 0; i < characters.Count; i++)
        {
            if (i == currentCharacterIndex)
                EnableControl(characters[i]);
            else
                DisableControl(characters[i]);
        }
    }

    void SwitchCharacter()
    {
        DisableControl(characters[currentCharacterIndex]);
        currentCharacterIndex = (currentCharacterIndex + 1) % characters.Count;
        EnableControl(characters[currentCharacterIndex]);
    }

    void TrySpawnRobot()
    {
        GameObject player = characters[currentCharacterIndex];
        if (!player.CompareTag("Player") || !IsGroundFlat(player.transform.position))
        {
            Debug.Log("Cannot spawn robot here.");
            return;
        }

        if (FindSpawnPosition(player.transform.position, out Vector3 spawnPosition))
        {
            GameObject newRobot = Instantiate(robotPrefab, spawnPosition, Quaternion.identity);
            characters.Add(newRobot);
        }
        else
        {
            Debug.Log("No space to spawn robot.");
        }
    }

    bool IsGroundFlat(Vector3 position)
    {
        if (Physics.Raycast(position, Vector3.down, out RaycastHit hit, 1f))
            return Vector3.Angle(hit.normal, Vector3.up) < 5f;
        return false;
    }

    bool FindSpawnPosition(Vector3 playerPosition, out Vector3 spawnPosition)
    {
        float spawnDistance = 5f;
        for (int i = 0; i < 8; i++)
        {
            Vector3 direction = Quaternion.Euler(0, i * 45, 0) * Vector3.forward;
            Vector3 checkPosition = playerPosition + direction * spawnDistance;
            if (IsPositionSuitable(checkPosition))
            {
                spawnPosition = checkPosition;
                return true;
            }
        }
        spawnPosition = Vector3.zero;
        return false;
    }

    bool IsPositionSuitable(Vector3 position)
    {
        if (!IsGroundFlat(position)) return false;
        return Physics.OverlapSphere(position, 0.5f).Length == 0;
    }

    // public void DisplayMessage(MessageTrigger messageTrigger)
    // {
    //     messageTextUI.text = messageTrigger.messageText;
    //     messageTextUI.color = messageTrigger.fontColor;
    //     messageTextUI.fontSize = messageTrigger.fontSize;

    //     RectTransform rectTransform = messageTextUI.GetComponent<RectTransform>();
    //     rectTransform.anchorMin = messageTrigger.position;
    //     rectTransform.anchorMax = messageTrigger.position;
    //     rectTransform.anchoredPosition = Vector2.zero;

    //     messageCanvas.enabled = true;
    //     StartCoroutine(HideMessageAfterDelay(messageTrigger.displayDuration));
    // }

    IEnumerator HideMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        messageCanvas.enabled = false;
    }

    void EnableControl(GameObject character)
    {
        ToggleCharacterComponents(character, true);
    }

    void DisableControl(GameObject character)
    {
        ToggleCharacterComponents(character, false);
    }

    void ToggleCharacterComponents(GameObject character, bool isEnabled)
    {
        var animator = character.GetComponentInChildren<Animator>();
        if (animator != null) animator.enabled = isEnabled;

        var playerController = character.GetComponent<PlayerController>();
        if (playerController != null) playerController.enabled = isEnabled;

        var robotController = character.GetComponent<RobotController>();
        if (robotController != null) robotController.enabled = isEnabled;

        var cam = character.GetComponentInChildren<Camera>();
        if (cam != null) cam.enabled = isEnabled;

        var audioListener = character.GetComponentInChildren<AudioListener>();
        if (audioListener != null) audioListener.enabled = isEnabled;

        var mouseLook = character.GetComponentInChildren<MouseLook>();
        if (mouseLook != null) mouseLook.enabled = isEnabled;
    }

    bool IsFlashlight(Light light)
    {
        foreach (var character in characters)
        {
            if (character.GetComponentInChildren<Light>() == light) return true;
        }
        return false;
    }

    // Writes given text to the HUD in a scolling manner 
    IEnumerator WriteText(string message) {
        messageDisplayed = true;
        for (int i = 0; i < message.Length; i++) {
            messageText.text += message[i];
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(3f);
        messageText.text = "";
        messageDisplayed = false;
        Debug.Log("Flag =" + msg1);
    }

    IEnumerator Wait(float time) {
        yield return new WaitForSeconds(time);
    }

    // Event manager to display tutorial messages 
    void DisplayTutorialMessages() {
        if (!msg1 && !messageDisplayed) {
            StartCoroutine(WriteText("Lilith:\nHello, I am lilith, your sentient suit AI. Your vitals are looking stable, but you should take it slow. It has been a long trip, after all!"));
            msg1 = true;
        }
        if (msg1 && !msg2 && !messageDisplayed) {
            Wait(2);
            StartCoroutine(WriteText("Lilith:\nUse [W][A][S][D] to move.\nYou can toggle your flashlight with [F]. This docking bay looks quite empty - try explore the rest of the ship!"));
            msg2 = true;
        }
        if (msg2 && !msg3 && !messageDisplayed && player.transform.position.x < -12) {
            StartCoroutine(WriteText("Lilith:\nOh look! A drone! These can help investigate parts of the ship while you stay somewhere safe ... Who knows what could be lurking in these dark corridors.\nUse [E] to collect a drone, [Q] to deploy a drone, and [R] to switch between them."));
            msg3 = true;
        }
        if (msg3 && !msg4 && !messageDisplayed && player.transform.position.z > 45) {
            StartCoroutine(WriteText("Lilith:\nUse [SHIFT] to sprint!"));
            msg4 = true;
        }
        if (msg4 && !msg5 && !messageDisplayed && player.transform.position.z > 45) {
            StartCoroutine(WriteText("Lilith:\nThis catwalk looks a little worse for wear. Use [SPACE] to jump over this chasm!"));
            msg5 = true;
        }
        if (msg5 && !msg6 && !messageDisplayed && player.transform.position.z > 9 && player.transform.position.x < 15) {
            StartCoroutine(WriteText("Lilith:\nA console! Maybe we can use that to unlock this door ... I wonder what else these consoles can give us access to?"));
            msg6 = true;
        }
    }
}
