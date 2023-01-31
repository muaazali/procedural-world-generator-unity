using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WorldGenerator : MonoBehaviour
{
    public float noiseScale = 1f;
    public float maximumHeight = 10f;
	public AnimationCurve heightCurve;

    [Header("Land Cubes")]
    public GameObject sandCube;
    public GameObject grassCube;
    public GameObject waterCube;


    [Header("Noise Sample Origin (Preferred Random)")]
    public bool isNoiseSampleRandom = true;
    public float xOrg = 10f, yOrg = 10f;

	[Header("Terrain")]
	public Terrain terrain;
	public TerrainData terrainData;

	private Texture2D perlinTexture;

    private GameObject worldParent;

	void Start()
	{
		terrainData = terrain.terrainData;
	}

	public void GenerateWorld()
	{
        if (worldParent == null) {
            worldParent = new GameObject("World");
        }
		DeleteExistingWorld();
		if (isNoiseSampleRandom)
		{
            xOrg = Random.Range(0, 8096);
            yOrg = Random.Range(0, 8096);
        }
		int heightmapResolution = terrainData.heightmapResolution;
		int alphamapResolution = terrainData.alphamapResolution;
		float[,] heights = terrainData.GetHeights(0, 0, heightmapResolution, heightmapResolution);
		float[,,] alphas = terrainData.GetAlphamaps(0, 0, alphamapResolution, alphamapResolution);

		perlinTexture = new Texture2D(heightmapResolution, heightmapResolution);
		for (float x = 0.0f; x < heightmapResolution; x++)
		{
			for (float y = 0.0f; y < heightmapResolution; y++)
			{
				float xValue = xOrg + x / heightmapResolution * noiseScale;
				float yValue = yOrg + y / heightmapResolution * noiseScale;
                float perlinValue = Mathf.PerlinNoise(xValue, yValue);

				if (x < alphamapResolution && y < alphamapResolution)
				{
					if (perlinValue > 0.7f)
					{
						alphas[Mathf.FloorToInt(x), Mathf.FloorToInt(y), 1] = 1f;
					}
					if (perlinValue < 0.75f && perlinValue > 0.3f)
						alphas[Mathf.FloorToInt(x), Mathf.FloorToInt(y), 2] = 1f;
					if (perlinValue < 0.35f)
					{
						alphas[Mathf.FloorToInt(x), Mathf.FloorToInt(y), 0] = 1f;
					}


				}
				heights[Mathf.FloorToInt(x), Mathf.FloorToInt(y)] = maximumHeight * heightCurve.Evaluate(perlinValue);
			}
        }
		terrainData.SetHeights(0, 0, heights);
		terrainData.SetAlphamaps(0, 0, alphas);
	}

    public void DeleteExistingWorld()
    {
		int alphamapResolution = terrainData.alphamapResolution;
		float[,,] alphas = terrainData.GetAlphamaps(0, 0, alphamapResolution, alphamapResolution);
		for (int x = 0; x < alphamapResolution; x++)
		{
			for (int y = 0; y < alphamapResolution; y++)
			{
				for (int i = 0; i < terrainData.terrainLayers.Length; i++)
				{
					alphas[Mathf.FloorToInt(x), Mathf.FloorToInt(y), i] = 0f;
				}
			}
		}
		terrainData.SetAlphamaps(0, 0, alphas);
	}
}
