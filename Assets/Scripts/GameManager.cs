using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

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
    bool textDisplayed = false;

    public static GameManager Instance;

    // Reference to InteractionController
    private InteractionController interactionController;
    
    void Awake()
    {
        // Implement Singleton Pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        ApplyLightingSettings();
        InitializeCharacters();

        // Find the InteractionController in the scene
        interactionController = FindObjectOfType<InteractionController>();
        if (interactionController == null)
        {
            Debug.LogError("GameManager: InteractionController not found in the scene.");
        }

        // Ensure only the active character has AudioListener enabled
        UpdateAudioListeners();
    }

    void OnValidate()
    {
        ApplyLightingSettings();
    }

    void Update()
    {
        // Remove destroyed characters from the list
        CleanUpCharacters();

        // Switch character logic
        if (Input.GetKeyDown(KeyCode.Tab))
            SwitchCharacter();

        // Try to spawn robot
        if (Input.GetKeyDown(KeyCode.Q))
            TrySpawnRobot();

        // Toggle ambient light
        if (Input.GetKeyDown(KeyCode.L))
        {
            useAmbientLight = !useAmbientLight;
            ApplyLightingSettings();
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
        if (characters.Count <= 1)
            return;

        DisableControl(characters[currentCharacterIndex]);
        currentCharacterIndex = (currentCharacterIndex + 1) % characters.Count;

        // If the next character is a robot, ensure it's active
        if (characters[currentCharacterIndex].CompareTag("Robot"))
        {
            // You can add additional checks or initialization here if needed
        }

        EnableControl(characters[currentCharacterIndex]);

        // Update AudioListeners to ensure only active character has it enabled
        UpdateAudioListeners();
    }

    void TrySpawnRobot()
    {
        if (interactionController == null)
        {
            Debug.LogError("GameManager: InteractionController reference is missing.");
            return;
        }

        if (interactionController.robotCount <= 0)
        {
            Debug.Log("No robots available to deploy.");
            DisplayMessage("No robots available to deploy.");
            return;
        }

        GameObject player = characters[currentCharacterIndex];
        if (!player.CompareTag("Player") || !IsGroundFlat(player.transform.position))
        {
            Debug.Log("Cannot spawn robot here.");
            DisplayMessage("Cannot spawn robot here.");
            return;
        }

        if (FindSpawnPosition(player.transform.position, out Vector3 spawnPosition))
        {
            GameObject newRobot = Instantiate(robotPrefab, spawnPosition, Quaternion.identity);
            newRobot.tag = "Robot";

            // Initially disable the robot's control and AudioListener
            DisableControl(newRobot);

            // Add the new robot to the characters list
            characters.Add(newRobot);

            // Reduce the robot count in InteractionController
            interactionController.robotCount--;

            // Display "Robot deployed" message
            DisplayMessage("Robot deployed");
        }
        else
        {
            Debug.Log("No space to spawn robot.");
            DisplayMessage("No space to spawn robot.");
        }
    }

    void CleanUpCharacters()
    {
        // Remove any characters that have been destroyed
        characters.RemoveAll(item => item == null);

        // Ensure currentCharacterIndex is within bounds
        if (currentCharacterIndex >= characters.Count)
            currentCharacterIndex = 0;

        // If no characters left, handle accordingly
        if (characters.Count == 0)
        {
            Debug.LogWarning("No characters left in the game.");
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
        if (cam != null)
        {
            cam.enabled = isEnabled;
        }

        var audioListener = character.GetComponentInChildren<AudioListener>();
        if (audioListener != null)
        {
            audioListener.enabled = isEnabled;
        }

        var mouseLook = character.GetComponentInChildren<MouseLook>();
        if (mouseLook != null) mouseLook.enabled = isEnabled;
    }

    bool IsFlashlight(Light light)
    {
        foreach (var character in characters)
        {
            var characterLight = character.GetComponentInChildren<Light>();
            if (characterLight != null && characterLight == light)
                return true;
        }
        return false;
    }

    // Displays a message to the player
    void DisplayMessage(string message, float duration = 2f)
    {
        if (messageCanvas != null && messageText != null)
        {
            messageCanvas.enabled = true;
            messageText.text = message;
            CancelInvoke("HideMessage");
            Invoke("HideMessage", duration);
        }
    }

    void HideMessage()
    {
        if (messageCanvas != null)
            messageCanvas.enabled = false;
    }

    /// <summary>
    /// Updates AudioListeners to ensure only the active character has it enabled.
    /// </summary>
    void UpdateAudioListeners()
    {
        foreach (GameObject character in characters)
        {
            AudioListener listener = character.GetComponentInChildren<AudioListener>();
            if (listener != null)
            {
                listener.enabled = (character == characters[currentCharacterIndex]);
            }
        }
    }
}
