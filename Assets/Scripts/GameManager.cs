using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

[ExecuteAlways] // Allows the script to run in edit mode
public class GameManager : MonoBehaviour
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

    public GameObject robotPrefab; // Robot prefab for spawning

    // Message UI 
    [Header("Message UI")]
    public Canvas messageCanvas;
    public TextMeshProUGUI messageText;

    public static GameManager Instance;

    // Doors to open and close
    public GameObject closedDoorReactor; 
    public GameObject openedDoorReactor; 
    public bool openReactor = false;
    public GameObject closedDoorWarehouse1; 
    public GameObject openedDoorWarehouse1; 
    public GameObject closedDoorWarehouse2; 
    public GameObject openedDoorWarehouse2; 
    public bool openWarehouse = false;

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
        WriteText("");
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
        // Open reactor room
        if (openReactor && (closedDoorReactor != null))
            OpenDoor(closedDoorReactor, openedDoorReactor);
        if (openWarehouse && (closedDoorWarehouse1 != null)) {
            OpenDoor(closedDoorWarehouse1, openedDoorWarehouse1);
            OpenDoor(closedDoorWarehouse2, openedDoorWarehouse2);
        }
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
        for (int i = 0; i < message.Length; i++) {
            messageText.text += message[i];
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(3f);
        messageText.text = "";
        yield return null;
    }

    IEnumerator Wait(float time) {
        yield return new WaitForSeconds(time);
    }

    // Takes the closed door gameobject (in scene), and a prefab of an open door
    void OpenDoor(GameObject closed, GameObject open) 
    {
        Instantiate(open, closed.transform.position, closed.transform.rotation);
        Destroy(closed);
    }
}
