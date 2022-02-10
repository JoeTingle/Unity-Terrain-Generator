using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour {

    public enum DrawMode
    {
        NOISEMAP,
        COLOURMAP,
        DRAWMESH
    }
    [SerializeField]
    private DrawMode drawMode;

    [SerializeField]
    private int mapWidth;
    [SerializeField]
    private int mapHeight;
    [SerializeField]
    private float noiseScale;

    [SerializeField]
    private int octaves;
    [SerializeField]
    [Range(0,1)]
    private float persistance;
    [SerializeField]
    private float lacunarity;

    [SerializeField]
    private int seed;
    [SerializeField]
    private Vector2 offset;

    [SerializeField]
    private bool autoUpdate;

    [System.Serializable]
    public struct TerrainTypes
    {
        public string name;
        public float height;
        public Color colour;

    }
    [SerializeField]
    private TerrainTypes[] regions;

    #region Getters
    public DrawMode GetDrawMode() { return drawMode;}
    public int GetMapWidth() { return mapWidth; }
    public int GetMapHeight() { return mapHeight; }
    public float GetNoiseScale() { return noiseScale; }
    public int GetOctaves() { return octaves; }
    public float GetPersistance() { return persistance; }
    public float GetLacunarity() { return lacunarity; }
    public int GetSeed() { return seed; }
    public Vector2 GetOffsets() { return offset; }
    public bool GetAutoUpdate() { return autoUpdate; }
    public TerrainTypes[] GetRegions() { return regions; }
    #endregion

    #region Setters
    public void SetDrawMode(DrawMode newDrawMode) {drawMode = newDrawMode; }
    public void SetMapWidth(int newMapWidth) { mapWidth = newMapWidth; }
    public void SetMapHeight(int newMapHeight) { mapHeight = newMapHeight; }
    public void SetNoiseScale(float newScale) {noiseScale = newScale; }
    public void SetOctaves(int newOctaves) {octaves = newOctaves; }
    public void SetPersistance(float newPersistance) {persistance = newPersistance; }
    public void SetLacunarity(float newLacunarity) {lacunarity = newLacunarity; }
    public void SetSeed(int newSeed) {seed = newSeed; }
    public void SetOffsets(Vector2 newOffsets) {offset = newOffsets; }
    public void SetRegions(TerrainTypes[] newTerrainTypes) {regions = newTerrainTypes; }
    public void SetAutoUpdate(bool newUpdate) { autoUpdate = newUpdate; }
    #endregion

    public void GenerateMap()
    {
        //Calling the perlin noise function
        float[,] noisemap = PerlinNoise.GeneratePerlinNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves,persistance,lacunarity, offset);

        Color[] colourMap = new Color[mapWidth * mapHeight];

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float currentHeight = noisemap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[i].height)
                    {
                        colourMap[y * mapWidth + x] = regions[i].colour;
                        break;
                    }
                }
            }
        }



        //Calling the map display class to actually create a texture from it.
        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.NOISEMAP)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noisemap));
        }
        else if (drawMode == DrawMode.COLOURMAP)
        {
            display.DrawTexture(TextureGenerator.TextureFromColourMap(colourMap, mapHeight, mapHeight));
        }
        else if (drawMode == DrawMode.DRAWMESH)
        {
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noisemap), TextureGenerator.TextureFromColourMap(colourMap, mapHeight, mapHeight));
        }
    }

    //Instead of using ranges on serialize field, can use this method to clamp manually
    private void OnValidate()
    {
        if (mapWidth < 1)
        {
            mapWidth = 1;
        }
        if (mapHeight < 1)
        {
            mapHeight = 1;
        }
        if (lacunarity < 1)
        {
            lacunarity = 1;
        }
        if (octaves < 0)
        {
            octaves = 0;
        }
    }
}
