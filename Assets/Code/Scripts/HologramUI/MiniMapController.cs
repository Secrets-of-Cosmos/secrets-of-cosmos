using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

public class MiniMapController : MonoBehaviour
{
    [SerializeField] private GameObject mapGameObject;
    [SerializeField] private Material mapMaterial;
    [SerializeField] private GameObject hologramEffectGameObject;

    private Transform _terrainParent;
    private MeshFilter _mapMeshFilter;
    private MeshRenderer _mapRenderer;
    private VisualEffect _hologramEffect;
    private VFXEventAttribute _eventAttribute;
    private bool _isMapActive;
    private static readonly int SelectionChangedEvent = Shader.PropertyToID("OnSelectionChanged");
    private static readonly int MapEvent = Shader.PropertyToID("OnMapSelected");

    private void Start() {
        _mapRenderer = mapGameObject.GetComponent<MeshRenderer>();
        _terrainParent = GameObject.Find("Map Generator").transform;
        _mapMeshFilter = mapGameObject.GetComponent<MeshFilter>();
        _hologramEffect = hologramEffectGameObject.GetComponent<VisualEffect>();
        _eventAttribute = _hologramEffect.CreateVFXEventAttribute();
        mapGameObject.transform.localScale = new Vector3(0.005f, 0.007f, 0.005f);
        mapGameObject.SetActive(false);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.M)) {
            if (!_isMapActive) {
                InstantiateMap();
                mapGameObject.SetActive(true);
            }
            else {
                mapGameObject.SetActive(false);
                _hologramEffect.SendEvent(SelectionChangedEvent, _eventAttribute);
            }
            _isMapActive = !_isMapActive;
        }
    }

    private void InstantiateMap() {
        // instantiate terrain meshes
        var meshFilters = (from Transform terrain in _terrainParent where terrain.gameObject.activeSelf select terrain.GetComponent<MeshFilter>()).ToList();

        // combine meshes
        var combine = new CombineInstance[meshFilters.Count];
        
        // scale and combine meshes
        for (var i = 0; i < meshFilters.Count; i++) {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            // combine[i].transform = Matrix4x4.Scale(new Vector3(0.005f, 0.007f, 0.005f)) * combine[i].transform;
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
        _hologramEffect.SetVector3("MapTransform_position", mapGameObject.transform.position);
        _hologramEffect.SetVector3("MapTransform_scale", mapGameObject.transform.localScale);
        
        _hologramEffect.SendEvent(MapEvent, _eventAttribute);

    }
}
