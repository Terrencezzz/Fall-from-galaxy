using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;

[ExecuteAlways]
public class GameManager : MonoBehaviour
{
    public List<CharacterControllerBase> characters = new List<CharacterControllerBase>();
    private int currentCharacterIndex = 0;

    [Header("Lighting Settings")]
    public bool useAmbientLight = true;
    [ColorUsage(false, true)]
    public Color ambientLightColor = Color.white;
    [Range(0f, 8f)] public float sceneLightIntensity = 0f;
    [Range(0f, 8f)] public float flashlightIntensity = 1.0f;

    public GameObject robotPrefab;

    [Header("Message UI")]
    public Canvas messageCanvas;
    public TextMeshProUGUI messageTextUI;

    public static GameManager Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        InitializeCharacters();
    }

    void Start()
    {
        ApplyLightingSettings();
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
    }

    void ApplyLightingSettings()
    {
        if (useAmbientLight)
        {
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = ambientLightColor;
        }
        else
        {
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = Color.black;
        }

        // Update all lights in the scene
        Light[] allLights = FindObjectsOfType<Light>();
        foreach (Light light in allLights)
        {
            light.intensity = IsFlashlight(light) ? flashlightIntensity : sceneLightIntensity;
        }
    }

    void InitializeCharacters()
    {
        characters.Clear();
        CharacterControllerBase[] allCharacters = FindObjectsOfType<CharacterControllerBase>();
        characters.AddRange(allCharacters);

        for (int i = 0; i < characters.Count; i++)
            characters[i].EnableControl(i == currentCharacterIndex);
    }

    void SwitchCharacter()
    {
        characters[currentCharacterIndex].EnableControl(false);
        currentCharacterIndex = (currentCharacterIndex + 1) % characters.Count;
        characters[currentCharacterIndex].EnableControl(true);
    }

    void TrySpawnRobot()
    {
        CharacterControllerBase player = characters[currentCharacterIndex];
        if (!(player is PlayerController) || !IsGroundFlat(player.transform.position))
        {
            Debug.Log("Cannot spawn robot here.");
            return;
        }

        if (FindSpawnPosition(player.transform.position, out Vector3 spawnPosition))
        {
            GameObject newRobot = Instantiate(robotPrefab, spawnPosition, Quaternion.identity);
            CharacterControllerBase robotController = newRobot.GetComponent<CharacterControllerBase>();
            if (robotController != null)
                characters.Add(robotController);
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
        if (!IsGroundFlat(position))
            return false;
        return Physics.OverlapSphere(position, 0.5f).Length == 0;
    }

    public void DisplayMessage(MessageTrigger messageTrigger)
    {
        messageTextUI.text = messageTrigger.messageText;
        messageTextUI.color = messageTrigger.fontColor;
        messageTextUI.fontSize = messageTrigger.fontSize;

        RectTransform rectTransform = messageTextUI.GetComponent<RectTransform>();
        rectTransform.anchorMin = messageTrigger.position;
        rectTransform.anchorMax = messageTrigger.position;
        rectTransform.anchoredPosition = Vector2.zero;

        messageCanvas.enabled = true;
        StartCoroutine(HideMessageAfterDelay(messageTrigger.displayDuration));
    }

    IEnumerator HideMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        messageCanvas.enabled = false;
    }

    bool IsFlashlight(Light light)
    {
        foreach (var character in characters)
        {
            if (character.flashlight == light)
                return true;
        }
        return false;
    }
}
