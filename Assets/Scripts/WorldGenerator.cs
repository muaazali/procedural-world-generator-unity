using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WorldGenerator : MonoBehaviour
{
    public int worldHeight, worldWidth;
    public float noiseScale = 1f;
    public float maximumHeight = 10f;

    [Header("Land Cubes")]
    public GameObject sandCube;
    public GameObject grassCube;
    public GameObject waterCube;


    [Header("Noise Sample Origin (Preferred Random)")]
    public bool isNoiseSampleRandom = true;
    public float xOrg = 10f, yOrg = 10f;

    private Texture2D perlinTexture;

    private GameObject worldParent;

    public void GenerateWorld() {
        if (worldParent == null) {
            worldParent = new GameObject("World");
        }
        DeleteExistingWorld();
        if (isNoiseSampleRandom) {
            xOrg = Random.Range(0, 8096);
            yOrg = Random.Range(0, 8096);
        }
        perlinTexture = new Texture2D(worldHeight, worldWidth);
        for (float x = 0.0f; x < perlinTexture.width; x++) {
            for (float y = 0.0f; y < perlinTexture.height; y++) {
                float xValue = xOrg + x / perlinTexture.width * noiseScale;
                float yValue = yOrg + y / perlinTexture.height * noiseScale;
                float perlinValue = Mathf.PerlinNoise(xValue, yValue);

                GameObject instantiatedObject;
                if (perlinValue > 0.5f) {
                    instantiatedObject = Instantiate(grassCube, new Vector3(x, 0f, y), Quaternion.identity);
                } else if (perlinValue > 0.2f) {
                    instantiatedObject = Instantiate(sandCube, new Vector3(x, 0f, y), Quaternion.identity);
                }
                else instantiatedObject = Instantiate(waterCube, new Vector3(x, 0f, y), Quaternion.identity);
                instantiatedObject.transform.parent = worldParent.transform;
                instantiatedObject.transform.position += Vector3.up * (maximumHeight * perlinValue);
            }
        }
    }

    public void DeleteExistingWorld()
    {
        int totalChildren = worldParent.transform.childCount;
        Debug.Log(totalChildren);
        for (int i = 0; i < totalChildren; i++)
        {
            DestroyImmediate(worldParent.transform.GetChild(0).gameObject);
        }
    }
}
