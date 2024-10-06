using UnityEngine;
using System.Collections.Generic;

[ExecuteAlways] // Allows the script to run in edit mode
public class GameManager : MonoBehaviour
{
    public List<GameObject> characters = new List<GameObject>();
    private int currentCharacterIndex = 0;

    // Lighting settings
    [Header("Lighting Settings")]
    public Color ambientLightColor = Color.black;
    public Material skyboxMaterial;
    public bool useSkybox = true;

    [Tooltip("Intensity of all lights in the scene (excluding flashlights)")]
    [Range(0f, 8f)]
    public float sceneLightIntensity = 0.5f;

    [Tooltip("Intensity of the characters' flashlights")]
    [Range(0f, 8f)]
    public float flashlightIntensity = 1.0f;

    void Start()
    {
        // Apply lighting settings at the start of the game
        ApplyLightingSettings();

        // Existing code to find and manage characters
        PlayerController[] players = FindObjectsOfType<PlayerController>();
        RobotController[] robots = FindObjectsOfType<RobotController>();

        // Add them to the characters list
        characters.Clear();
        foreach (var player in players)
        {
            characters.Add(player.gameObject);
        }

        foreach (var robot in robots)
        {
            characters.Add(robot.gameObject);
        }

        // Disable control for all characters except the first one
        for (int i = 0; i < characters.Count; i++)
        {
            if (i != currentCharacterIndex)
            {
                DisableControl(characters[i]);
            }
            else
            {
                EnableControl(characters[i]);
            }
        }
    }

    void OnValidate()
    {
        // Apply lighting settings in the editor when values change
        ApplyLightingSettings();
    }

    void ApplyLightingSettings()
    {
        // Set ambient light
        RenderSettings.ambientLight = ambientLightColor;

        // Set or clear skybox
        if (useSkybox && skyboxMaterial != null)
        {
            RenderSettings.skybox = skyboxMaterial;
        }
        else
        {
            RenderSettings.skybox = null;
        }

        // Adjust scene lights and flashlights
        Light[] allLights = FindObjectsOfType<Light>();
        foreach (Light light in allLights)
        {
            if (IsFlashlight(light))
            {
                light.intensity = flashlightIntensity;
            }
            else
            {
                light.intensity = sceneLightIntensity;
            }
        }
    }

    bool IsFlashlight(Light light)
    {
        // Check if the light is a flashlight attached to a character
        foreach (GameObject character in characters)
        {
            Light flashlight = character.GetComponentInChildren<Light>();
            if (flashlight == light)
            {
                return true;
            }
        }
        return false;
    }

    void Update()
    {
        // Switch character with Tab key
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchCharacter();
        }
    }

    void SwitchCharacter()
    {
        // Disable current character
        DisableControl(characters[currentCharacterIndex]);

        // Increment index
        currentCharacterIndex = (currentCharacterIndex + 1) % characters.Count;

        // Enable new character
        EnableControl(characters[currentCharacterIndex]);
    }

    void DisableControl(GameObject character)
    {
        // Disable control scripts
        if (character.GetComponent<PlayerController>())
            character.GetComponent<PlayerController>().enabled = false;

        if (character.GetComponent<RobotController>())
            character.GetComponent<RobotController>().enabled = false;

        // Disable camera and audio listener
        Camera cam = character.GetComponentInChildren<Camera>();
        if (cam != null)
            cam.enabled = false;

        AudioListener audioListener = character.GetComponentInChildren<AudioListener>();
        if (audioListener != null)
            audioListener.enabled = false;

        // Disable MouseLook script
        MouseLook mouseLook = character.GetComponentInChildren<MouseLook>();
        if (mouseLook != null)
            mouseLook.enabled = false;
    }

    void EnableControl(GameObject character)
    {
        // Enable control scripts
        if (character.GetComponent<PlayerController>())
            character.GetComponent<PlayerController>().enabled = true;

        if (character.GetComponent<RobotController>())
            character.GetComponent<RobotController>().enabled = true;

        // Enable camera and audio listener
        Camera cam = character.GetComponentInChildren<Camera>();
        if (cam != null)
            cam.enabled = true;

        AudioListener audioListener = character.GetComponentInChildren<AudioListener>();
        if (audioListener != null)
            audioListener.enabled = true;

        // Enable MouseLook script
        MouseLook mouseLook = character.GetComponentInChildren<MouseLook>();
        if (mouseLook != null)
            mouseLook.enabled = true;
    }
}
