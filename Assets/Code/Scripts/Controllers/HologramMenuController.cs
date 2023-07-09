using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class HologramMenuController : MonoBehaviour
{
    [Header("Hologram Effect")]
    [SerializeField] private GameObject hologramEffectGameObject;
        
    [Header("Menu Settings")]
    [SerializeField] private List<GameObject> tabButtons;
    [SerializeField] private Vector3 tabButtonsStartPosition;
    [SerializeField] private float tabButtonsStartGap;
    [SerializeField] private float tabButtonsFinalGap;
    [SerializeField] private Vector3 tabButtonsFinalPosition;
    [SerializeField] private GameObject leftButton;
    [SerializeField] private GameObject rightButton;
    [SerializeField] private Color currentTabColor;

    [Header("Planets")] 
    [SerializeField] private List<PlanetProperties> planetsProperties;
    [SerializeField] private GameObject planetGameObject;

    [Header("Spacecrafts")]
    [SerializeField] private List<Mesh> spacecraftsMeshes;
    [SerializeField] private float spacecraftsRotationSpeed = 20f;

    private TabsType _currentTab = TabsType.NOT_SELECTED;
    private int _currentPlanet;
    private int _currentSpacecraft;
    private VisualEffect _hologramEffect;
    private VFXEventAttribute _eventAttribute;
    private static readonly int BaseTexture = Shader.PropertyToID("_BaseTexture");
    private static readonly int PlanetEvent = Shader.PropertyToID("OnPlanetsSelected");
    private static readonly int SpacecraftEvent = Shader.PropertyToID("OnSpacecraftsSelected");
    private static readonly int SelectionChangedEvent = Shader.PropertyToID("OnSelectionChanged");
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
    private static readonly int PlanetRayColor = Shader.PropertyToID("PlanetRayColor");

    public static HologramMenuController Instance { get; private set; }
    
    private void Awake() {
        Instance = this;
    }

    private void Start() {
        _hologramEffect = hologramEffectGameObject.GetComponent<VisualEffect>();
        _eventAttribute = _hologramEffect.CreateVFXEventAttribute();

        planetGameObject.SetActive(false);
        gameObject.SetActive(false);
        leftButton.SetActive(false);
        rightButton.SetActive(false);
        tabButtons.ForEach(button => button.SetActive(false));
        
        planetGameObject.GetComponent<MeshRenderer>().material.SetTexture(BaseTexture, planetsProperties[_currentPlanet].planetTexture);
        _eventAttribute.SetVector4(PlanetRayColor, planetsProperties[_currentPlanet].rayColor);
    }

    private void Update() {
        if (_currentTab == TabsType.SPACECRAFTS || _currentTab == TabsType.PLANETS) {
            hologramEffectGameObject.transform.Rotate(Vector3.up, spacecraftsRotationSpeed * Time.deltaTime);
        }
    }

    public void OpenMenu() {
        gameObject.SetActive(true);
        var mostLeft = tabButtons.Count / 2 * -1;
        for (var i = 0; i < tabButtons.Count; i++, mostLeft++) {
            tabButtons[i].transform.localPosition = tabButtonsStartPosition + new Vector3(mostLeft * tabButtonsStartGap + tabButtonsStartGap/2, 0f, 0f);
        }
        tabButtons.ForEach(button => button.transform.localScale = new Vector3(0f, 0f, 0f));
        tabButtons.ForEach(button => button.SetActive(true));
        tabButtons.ForEach(button => button.transform.SetAsLastSibling());
        tabButtons.ForEach(button => LeanTween.scale(button, new Vector3(1.2f, 1.2f, 1.2f), 0.5f).setEaseOutBack());
    }
    
    public void CloseMenu() {
        _currentTab = TabsType.NOT_SELECTED;
        _currentPlanet = 0;
        _currentSpacecraft = 0;
        leftButton.SetActive(false);
        rightButton.SetActive(false);
        planetGameObject.SetActive(false);
        hologramEffectGameObject.transform.rotation = Quaternion.identity;
        gameObject.SetActive(false);
        tabButtons.ForEach(button => {
            LeanTween.scale(button, new Vector3(1f, 1f, 1f), 0.5f).setEaseOutBack();
            button.GetComponent<MeshRenderer>().material.SetColor(EmissionColor, currentTabColor);
        });
    }
    
    public void OnTabSelected(TabsType tab) {
        if (_currentTab == tab) return;
        if (_currentTab == TabsType.NOT_SELECTED) {
            var mostLeft = tabButtons.Count/2*-1;
            for (var i = 0; i < tabButtons.Count; i++, mostLeft++) {
                LeanTween.moveLocal(tabButtons[i], new Vector3(tabButtonsFinalPosition.x + mostLeft * tabButtonsFinalGap + tabButtonsFinalGap/2, tabButtonsFinalPosition.y, tabButtonsFinalPosition.z), 0.5f).setEaseOutBack();
                LeanTween.scale(tabButtons[i], new Vector3(0.5f, 0.5f, 0.5f), 0.5f).setEaseOutBack();
            }
        }
        _hologramEffect.SendEvent(SelectionChangedEvent, _eventAttribute);
        tabButtons.ForEach(button => {
            LeanTween.scale(button, new Vector3(0.5f, 0.5f, 0.5f), 0.5f).setEaseOutBack();
            button.GetComponent<MeshRenderer>().material.SetColor(EmissionColor, currentTabColor);
        });
        _currentTab = tab;
        leftButton.SetActive(true);
        rightButton.SetActive(true);
        planetGameObject.SetActive(false);
        hologramEffectGameObject.transform.rotation = Quaternion.identity;
        switch (tab) {
            case TabsType.PLANETS:
                planetGameObject.SetActive(true);
                _hologramEffect.SendEvent(PlanetEvent, _eventAttribute);
                break;
            case TabsType.MISSIONS:
                Debug.Log("Missions");
                break;
            case TabsType.SPACECRAFTS:
                _hologramEffect.SetMesh("SpacecraftMesh", spacecraftsMeshes[_currentSpacecraft]);
                _hologramEffect.SendEvent(SpacecraftEvent, _eventAttribute);
                break;
            case TabsType.INVENTORY:
                Debug.Log("Inventory");
                break;
            case TabsType.MAPS:
                Debug.Log("Maps");
                break;
            case TabsType.DICTIONARY:
                Debug.Log("Dictionary");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(tab), tab, null);
        }
    }
    public void OnControlButtonsPressed(ControlButtonsType button) {
        var direction = button == ControlButtonsType.LEFT ? -1 : 1;
        switch (_currentTab) {
            case TabsType.PLANETS:
                _currentPlanet += direction;
                _currentPlanet = _currentPlanet < 0 ? planetsProperties.Count - 1 : (_currentPlanet >= planetsProperties.Count ? 0 : _currentPlanet);
                planetGameObject.transform.localScale = planetsProperties[_currentPlanet].planetSize * Vector3.one * 0.1f;
                _hologramEffect.SetVector3("PlanetTransform_scale", planetsProperties[_currentPlanet].planetSize * Vector3.one * 0.1f);
                _hologramEffect.SetVector4(PlanetRayColor, planetsProperties[_currentPlanet].rayColor);
                planetGameObject.GetComponent<MeshRenderer>().material.SetTexture(BaseTexture, planetsProperties[_currentPlanet].planetTexture);
                break;
            case TabsType.MISSIONS:
                break;
            case TabsType.SPACECRAFTS:
                _currentSpacecraft += direction;
                _currentSpacecraft = _currentSpacecraft < 0 ? spacecraftsMeshes.Count - 1 : (_currentSpacecraft >= spacecraftsMeshes.Count ? 0 : _currentSpacecraft);
                _hologramEffect.SetMesh("SpacecraftMesh", spacecraftsMeshes[_currentSpacecraft]);
                break;
            case TabsType.INVENTORY:
                break;
            case TabsType.MAPS:
                break;
            case TabsType.DICTIONARY:
                break;
            case TabsType.NOT_SELECTED:
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
[Serializable]
public struct PlanetProperties {
    public string planetName;
    public Texture2D planetTexture;
    public float planetSize;
    [ColorUsage(true, true)] public Color rayColor;
}
