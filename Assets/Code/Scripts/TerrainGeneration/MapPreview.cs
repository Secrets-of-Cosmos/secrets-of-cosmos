﻿using UnityEngine;
using System.Collections;

public class MapPreview : MonoBehaviour {

	public MeshFilter meshFilter;
	public MeshRenderer meshRenderer;

	public MeshSettings meshSettings;
	public HeightMapSettings heightMapSettings;

	public Material terrainMaterial;

	[Range(0,MeshSettings.numSupportedLODs-1)]
	public int editorPreviewLOD;
	public bool autoUpdate;
	
	public void DrawMapInEditor() {
		HeightMap heightMap = HeightMapGenerator.GenerateHeightMap (meshSettings.numVertsPerLine, meshSettings.numVertsPerLine, heightMapSettings, Vector2.zero);
		
		var meshData = MeshGenerator.GenerateTerrainMesh (heightMap.values,meshSettings, editorPreviewLOD);
		
		meshRenderer.material = terrainMaterial;
		meshFilter.sharedMesh = meshData.CreateMesh ();
		meshFilter.gameObject.SetActive (true);
	}
	
	void OnValuesUpdated() {
		if (!Application.isPlaying) {
			DrawMapInEditor ();
		}
	}
	
	void OnValidate() {

		if (meshSettings != null) {
			meshSettings.OnValuesUpdated -= OnValuesUpdated;
			meshSettings.OnValuesUpdated += OnValuesUpdated;
		}
		if (heightMapSettings != null) {
			heightMapSettings.OnValuesUpdated -= OnValuesUpdated;
			heightMapSettings.OnValuesUpdated += OnValuesUpdated;
		}
	}
}
