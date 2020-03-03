using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class mapGenerator : MonoBehaviour
{
    private GameObject currCube;

    //ukuran mapnya
    public int sizeX = 50;
    public int sizeY = 5;
    public int sizeZ = 50;

    // array buat cubenya
    public int[, ,] cubeArray;

    // Use this for initialization
    void Start()
    {
        cubeArray = new int[sizeX, sizeY, sizeZ];
        stone();
        cubePosition();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void stone()
    {
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeZ; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    GameObject.Instantiate((GameObject)Resources.Load("Prefab/normCobbleStone"), new Vector3(i, k, j), Quaternion.identity);
                }
            }
        }
    }

    void cubePosition()
    {
        int currheight;
        int octave = 3;

        float[][] pNarray = PerlinNoiseGenerate(sizeX, sizeZ, octave);

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeZ; j++)
            {
                currheight = (int)Mathf.Clamp(pNarray[i][j], 0, sizeY);
                for (int k = 0; k < currheight; k++)
                {
                    GameObject.Instantiate((GameObject)Resources.Load("Prefab/normGrass"), new Vector3(i, k + 2, j), Quaternion.identity);
                    int randomTree = Random.Range(0, 100);
                    if (randomTree < 2 && k == (currheight - 1))
                    {
                        int tree = currheight + Random.Range(2, 5);
                        for (int l = currheight; l < tree; l++)
                        {
                            GameObject.Instantiate((GameObject)Resources.Load("Prefab/normWood"), new Vector3(i, l + 2, j), Quaternion.identity);
                        }
                    }
                }
            }
        }
    }

    static float Interpolate(float posawal, float posakhir, float blend)
    {
        return posawal * (1 - blend) + blend * posakhir;
    }

    static float[][] GenerateWhiteNoise(int widthx, int heightz)
    {
        float[][] noise = new float[widthx][];

        for (int i = 0; i < widthx; i++)
        {
            noise[i] = new float[heightz];
        }

        for (int i = 0; i < widthx; i++)
        {
            for (int j = 0; j < heightz; j++)
            {
                noise[i][j] = (float)Random.Range(0.1f, 0.7f);
            }
        }
        return noise;
    }

    static float[][] GenerateSmoothNoise(float[][] baseNoise, int octave)
    {
        int widthx = baseNoise.Length;
        int heightz = baseNoise[0].Length;

        float[][] smoothNoise = new float[widthx][];

        for (int i = 0; i < widthx; i++)
        {
            smoothNoise[i] = new float[heightz];
        }
        int periode = 1 << octave; // calculates 2 ^ k
        float frekuensi = 1.0f / periode;

        for (int i = 0; i < widthx; i++)
        {
            int posisiawalx = (i / periode) * periode; // perkalian paling gede dari period yang lebih kecil dari i
            int posisiakhirx = (posisiawalx + periode) % widthx;
            float horizontal_blend = (i - posisiawalx) * frekuensi;

            for (int j = 0; j < heightz; j++)
            {
                int posisiawalz = (j / periode) * periode;
                int posisiakhirz = (posisiawalz + periode) % heightz;
                float vertical_blend = (j - posisiawalz) * periode;

                float batasatas = Interpolate(baseNoise[posisiawalx][posisiawalz],
                                              baseNoise[posisiakhirx][posisiawalz], horizontal_blend);

                float batasbawah = Interpolate(baseNoise[posisiawalx][posisiakhirz],
                                               baseNoise[posisiakhirx][posisiakhirz], horizontal_blend);

                smoothNoise[i][j] = Interpolate(batasatas, batasbawah, vertical_blend);
            }
        }

        return smoothNoise;
    }

    static float[][] GeneratePerlinNoise(float[][] baseNoise, int octaveCount)
    {
        int width = baseNoise.Length;
        int height = baseNoise[0].Length;

        float[][][] smoothNoise = new float[octaveCount][][]; //an array of 2D arrays containing

        float persistance = 0.9f;

        //generate smooth noise
        for (int i = 0; i < octaveCount; i++)
        {
            smoothNoise[i] = GenerateSmoothNoise(baseNoise, i);
        }

        float[][] perlinNoise = new float[width][];//an array of floats initialised to 0  

        for (int i = 0; i < width; i++)
        {
            perlinNoise[i] = new float[height];
        }

        float amplitude = 1.00f;
        float totalAmplitude = 0.0f;  //blend noise together  
        for (int octave = octaveCount - 1; octave >= 0; octave--)
        {
            amplitude *= persistance;
            totalAmplitude += amplitude;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    perlinNoise[i][j] += smoothNoise[octave][i][j] * totalAmplitude; ;
                }
            }
        }

        return perlinNoise;

    }

    static float[][] PerlinNoiseGenerate(int width, int height, int octaveCount)
    {
        float[][] baseNoise = GenerateWhiteNoise(width, height);
        return GeneratePerlinNoise(baseNoise, octaveCount);
    }
}
