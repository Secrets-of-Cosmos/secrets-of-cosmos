using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

[RequireComponent(typeof(MenuDescriptionController))]
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
    [SerializeField] private GameObject planetPercentageText;
    
    [Header("Map")]
    [SerializeField] private GameObject mapGameObject;
    [SerializeField] private Material mapMaterial;
    [SerializeField] private Material defaultMapMaterial;
    [SerializeField] private Mesh defaultMapMesh;
    [SerializeField] [ColorUsage(true, true)] private Color defaultMapColor;

    [Header("Spacecrafts")]
    [SerializeField] private List<Mesh> spacecraftsMeshes;
    [SerializeField] private float spacecraftsRotationSpeed = 20f;
    
    [Header("DescriptionController")]
    [SerializeField] private MenuDescriptionController menuDescriptionController;

    private TabType currentTab { get; set; } = TabType.NOT_SELECTED;
    private int _currentPlanet;
    private int _currentSpacecraft;
    
    private Transform _terrainParent;
    private MeshFilter _mapMeshFilter;
    private MeshRenderer _mapRenderer;
    private bool _isMapActive;

    private VisualEffect _hologramEffect;
    private VFXEventAttribute _eventAttribute;
    private static readonly int BaseTexture = Shader.PropertyToID("_BaseTexture");
    private static readonly int PlanetEvent = Shader.PropertyToID("OnPlanetsSelected");
    private static readonly int SpacecraftEvent = Shader.PropertyToID("OnSpacecraftsSelected");
    private static readonly int SelectionChangedEvent = Shader.PropertyToID("OnSelectionChanged");
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
    private static readonly int PlanetRayColor = Shader.PropertyToID("PlanetRayColor");
    private static readonly int MapEvent = Shader.PropertyToID("OnMapSelected");

    public static HologramMenuController instance { get; private set; }
    public UnityEvent<TabType> tabSelectedEvent = new();
    
    private void Awake() {
        instance = this;
    }

    private void Start() {
        _hologramEffect = hologramEffectGameObject.GetComponent<VisualEffect>();
        _eventAttribute = _hologramEffect.CreateVFXEventAttribute();

        gameObject.SetActive(false);
        leftButton.SetActive(false);
        rightButton.SetActive(false);
        mapGameObject.SetActive(false);
        planetGameObject.SetActive(false);
        planetPercentageText.SetActive(false);
        tabButtons.ForEach(button => button.SetActive(false));
        
        _mapRenderer = mapGameObject.GetComponent<MeshRenderer>();
        _mapMeshFilter = mapGameObject.GetComponent<MeshFilter>();

        planetGameObject.GetComponent<MeshRenderer>().material.SetTexture(BaseTexture, planetsProperties[_currentPlanet].planetTexture);
        _eventAttribute.SetVector4(PlanetRayColor, planetsProperties[_currentPlanet].rayColor);
        
        // var vertices = defaultMapMesh.vertices;
        // for (var i = 0; i < vertices.Length; i++) {
        //     vertices[i] = Quaternion.Euler(-90, 0, 0) * vertices[i];
        // }
        // defaultMapMesh.vertices = vertices;
        // defaultMapMesh.RecalculateNormals();
        // defaultMapMesh.RecalculateBounds();
        // defaultMapMesh.Optimize();
    }

    private void Update() {
        if (currentTab is TabType.SPACECRAFTS or TabType.PLANETS) {
            hologramEffectGameObject.transform.Rotate(Vector3.up, spacecraftsRotationSpeed * Time.deltaTime);
        }
    }

    public void OpenMenu() {
        gameObject.SetActive(true);
        var mostLeft = tabButtons.Count / 2 * -1;
        for (var i = 0; i < tabButtons.Count; i++, mostLeft++) {
            tabButtons[i].transform.localPosition = tabButtonsStartPosition + new Vector3(mostLeft * tabButtonsStartGap + tabButtonsStartGap / 2, 0f, 0f);
        }
        tabButtons.ForEach(button =>
        {
            button.transform.localScale = new Vector3(0f, 0f, 0f);
            button.SetActive(true);
            button.transform.SetAsLastSibling();
            LeanTween.scale(button, new Vector3(1.2f, 1.2f, 1.2f), 0.5f).setEaseOutBack();
        });
    }
    
    public void CloseMenu() {
        currentTab = TabType.NOT_SELECTED;
        _currentPlanet = 0;
        _currentSpacecraft = 0;
        leftButton.SetActive(false);
        rightButton.SetActive(false);
        planetGameObject.SetActive(false);
        planetPercentageText.SetActive(false);
        mapGameObject.SetActive(false);
        hologramEffectGameObject.transform.rotation = Quaternion.identity;
        gameObject.SetActive(false);
        menuDescriptionController.Hide();
        tabButtons.ForEach(button => {
            LeanTween.scale(button, new Vector3(1f, 1f, 1f), 0.5f).setEaseOutBack();
            button.GetComponent<MeshRenderer>().material.SetColor(EmissionColor, currentTabColor);
        });
    }
    
    public void OnTabSelected(TabType tab) {
        if (currentTab == tab) return;
        tabSelectedEvent.Invoke(tab);
        if (currentTab == TabType.NOT_SELECTED) {
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
        currentTab = tab;
        planetGameObject.SetActive(false);
        mapGameObject.SetActive(false);
        hologramEffectGameObject.transform.rotation = Quaternion.identity;
        planetPercentageText.SetActive(false);
        switch (tab) {
            case TabType.PLANETS:
                planetPercentageText.SetActive(true);
                ChangeControlButtonStatus(true);
                SetDescriptionMenuMiddleLenght(false);
                planetGameObject.SetActive(true);
                _hologramEffect.SendEvent(PlanetEvent, _eventAttribute);
                menuDescriptionController.Show(true, true);
                break;
            case TabType.MISSIONS:
                ChangeControlButtonStatus(false);
                SetDescriptionMenuMiddleLenght(true);
                menuDescriptionController.Show(true, false);
                break;
            case TabType.SPACECRAFTS:
                ChangeControlButtonStatus(true);
                SetDescriptionMenuMiddleLenght(false);
                _hologramEffect.SetMesh("SpacecraftMesh", spacecraftsMeshes[_currentSpacecraft]);
                _hologramEffect.SendEvent(SpacecraftEvent, _eventAttribute);
                menuDescriptionController.Show(true, false);
                break;
            case TabType.MAPS:
                menuDescriptionController.Hide();
                ChangeControlButtonStatus(false);
                try {
                    _terrainParent = GameObject.Find("Map Generator").transform;
                    InstantiateMap();
                    mapGameObject.SetActive(true);
                }
                catch (NullReferenceException) {
                    OpenDefaultMap();
                    mapGameObject.SetActive(true);
                    Console.WriteLine("Map Generator not found");
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(tab), tab, null);
        }
    }
    
    private void InstantiateMap() {
        mapGameObject.transform.localScale = new Vector3(0.00015f, 0.0003f, 0.00015f);
        mapGameObject.transform.localRotation = Quaternion.Euler(0, 0, -25);
        // instantiate terrain meshes
        var meshFilters = (from Transform terrain in _terrainParent where terrain.gameObject.activeSelf select terrain.GetComponent<MeshFilter>()).ToList();

        // combine meshes
        var combine = new CombineInstance[meshFilters.Count];
        
        // scale and combine meshes
        for (var i = 0; i < meshFilters.Count; i++) {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
        }
        
        var mapMesh = new Mesh {
            indexFormat = UnityEngine.Rendering.IndexFormat.UInt32
        };
        
        mapMesh.CombineMeshes(combine);
        mapMesh.Optimize();
        mapMesh.name = "Map";
        _mapMeshFilter.sharedMesh = mapMesh;
        _mapRenderer.sharedMaterial = mapMaterial;
        
        _hologramEffect.SetMesh("MapMesh", mapMesh);
        _hologramEffect.SetVector3("MapTransform_position", mapGameObject.transform.localPosition);
        _hologramEffect.SetVector3("MapTransform_angles", mapGameObject.transform.localRotation.eulerAngles);
        _hologramEffect.SetVector3("MapTransform_scale", mapGameObject.transform.localScale);
        _hologramEffect.SetVector3("MapLineScale", new Vector3(0, -2.6f, 0));
        
        _hologramEffect.SendEvent(MapEvent, _eventAttribute);

    }
    
    private void OpenDefaultMap() {
        mapGameObject.transform.localRotation = Quaternion.Euler(-90, 0, -25);
        mapGameObject.transform.localScale = new Vector3(0.005f, 0.007f, 0.005f);
        _mapMeshFilter.sharedMesh = defaultMapMesh;
        _mapRenderer.sharedMaterial = defaultMapMaterial;
        
        _hologramEffect.SetMesh("MapMesh", _mapMeshFilter.sharedMesh);
        _hologramEffect.SetVector3("MapTransform_position", mapGameObject.transform.localPosition);
        _hologramEffect.SetVector3("MapTransform_angles", mapGameObject.transform.localRotation.eulerAngles);
        _hologramEffect.SetVector3("MapTransform_scale", mapGameObject.transform.localScale);
        _hologramEffect.SetVector3("MapLineScale", new Vector3(0, -2.6f, 0));
        _hologramEffect.SetVector4("MapLineColor", defaultMapColor);
        
        _hologramEffect.SendEvent(MapEvent, _eventAttribute);
    }
    
    public void OnControlButtonsPressed(ControlButtonsType button) {
        var direction = button == ControlButtonsType.LEFT ? -1 : 1;
        switch (currentTab) {
            case TabType.PLANETS:
                _currentPlanet += direction;
                _currentPlanet = _currentPlanet < 0 ? planetsProperties.Count - 1 : (_currentPlanet >= planetsProperties.Count ? 0 : _currentPlanet);
                planetGameObject.transform.localScale = planetsProperties[_currentPlanet].planetSize * Vector3.one * 0.1f;
                _hologramEffect.SetVector3("PlanetTransform_scale", planetsProperties[_currentPlanet].planetSize * Vector3.one * 0.1f);
                _hologramEffect.SetVector4(PlanetRayColor, planetsProperties[_currentPlanet].rayColor);
                planetGameObject.GetComponent<MeshRenderer>().material.SetTexture(BaseTexture, planetsProperties[_currentPlanet].planetTexture);
                break;
            case TabType.MISSIONS:
                break;
            case TabType.SPACECRAFTS:
                _currentSpacecraft += direction;
                _currentSpacecraft = _currentSpacecraft < 0 ? spacecraftsMeshes.Count - 1 : (_currentSpacecraft >= spacecraftsMeshes.Count ? 0 : _currentSpacecraft);
                _hologramEffect.SetMesh("SpacecraftMesh", spacecraftsMeshes[_currentSpacecraft]);
                break;
            case TabType.MAPS:
                break;
            case TabType.NOT_SELECTED:
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ChangeControlButtonStatus(bool isActive) {
        leftButton.SetActive(isActive);
        rightButton.SetActive(isActive);
    }

    private void SetDescriptionMenuMiddleLenght(bool extend) {
        menuDescriptionController.middleTopHeightEndRatio = extend ? 0.99f : 0.7f;
    }
}
[Serializable]
public struct PlanetProperties {
    public string planetName;
    public Texture2D planetTexture;
    public float planetSize;
    [ColorUsage(true, true)] public Color rayColor;
}
