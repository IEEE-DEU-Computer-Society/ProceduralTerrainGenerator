using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UIManager : MonoBehaviour
{
    public static event Action OnUIChange;

    [Header("Assign - Seed")]
    [SerializeField] private TMP_InputField seedInputField;

    [Header("Assign - Octave")]
    [SerializeField] private Slider octaveSlider;
    [SerializeField] private TextMeshProUGUI octaveValueText;
    
    [Header("Assign - Lacunarity")]
    [SerializeField] private Slider lacunaritySlider;
    [SerializeField] private TextMeshProUGUI lacunarityValueText;
    
    [Header("Assign - Persistence")]
    [SerializeField] private Slider persistenceSlider;
    [SerializeField] private TextMeshProUGUI persistenceValueText;
    
    [Header("Assign - Offset")]
    [SerializeField] private TMP_InputField xOffsetInputField;
    [SerializeField] private TMP_InputField yOffsetInputField;
    
    [Header("Assign - Noise Scale")]
    [SerializeField] private TMP_InputField noiseScaleInputField;

    private void Awake()
    {
        seedInputField.onValueChanged.AddListener((string input) =>
        {
            TerrainGenerator.Singleton.ChangeSeed(int.Parse(input));
            OnUIChange?.Invoke();
        });
        
        octaveSlider.onValueChanged.AddListener((float input) =>
        {
            TerrainGenerator.Singleton.octaveNumber = (int)input;
            octaveValueText.text = $"{input}";
            OnUIChange?.Invoke();
        });
        
        lacunaritySlider.onValueChanged.AddListener((float input) =>
        {
            TerrainGenerator.Singleton.lacunarity = input;
            lacunarityValueText.text = input.ToString("F2");
            OnUIChange?.Invoke();
        });
        
        persistenceSlider.onValueChanged.AddListener((float input) =>
        {
            TerrainGenerator.Singleton.persistence = input;
            persistenceValueText.text = input.ToString("F2");
            OnUIChange?.Invoke();
        });
        
        xOffsetInputField.onValueChanged.AddListener((string input) =>
        {
            TerrainGenerator.Singleton.offset.x = int.Parse(input);
            OnUIChange?.Invoke();
        });
        
        yOffsetInputField.onValueChanged.AddListener((string input) =>
        {
            TerrainGenerator.Singleton.offset.y = int.Parse(input);
            OnUIChange?.Invoke();
        });
        
        noiseScaleInputField.onValueChanged.AddListener((string input) =>
        {
            TerrainGenerator.Singleton.noiseScale = int.Parse(input);
            OnUIChange?.Invoke();
        });

        MovementManager.OnMovement += UpdateOffset;
        MovementManager.OnMovement += UpdateNoiseScale;
    }

    private void UpdateOffset()
    {
        xOffsetInputField.text = $"{TerrainGenerator.Singleton.offset.x}";
        yOffsetInputField.text = $"{TerrainGenerator.Singleton.offset.y}";
    }

    private void UpdateNoiseScale()
    {
        noiseScaleInputField.text = $"{TerrainGenerator.Singleton.noiseScale}";
    }
}
