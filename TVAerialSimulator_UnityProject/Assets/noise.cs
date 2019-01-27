using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class noise : MonoBehaviour
{
    // Width and height of the texture in pixels.
    public int pixWidth;
    public int pixHeight;

    // The origin of the sampled area in the plane.
    public float xOrg;
    public float yOrg;

    // The number of cycles of the basic noise pattern that are repeated
    // over the width and height of the texture.
    private float scale = 100.0F;

    public Texture2D noiseTex;
    static public float alpha;

    private Color[] pix;

    void Start()
    {
        // Set up the texture and a Color array to hold pixels during processing.
        pix = new Color[noiseTex.width * noiseTex.height];
    }

    void CalcNoise()
    {
        float angle = Mathf.Abs(main.angleBetweenTwoVectors);

        alpha = angle / 180.0f;

        // For each pixel in the texture...
        float y = 0.0F;

        while (y < noiseTex.height)
        {
            float x = 0.0F;
            while (x < noiseTex.width)
            {
                float xCoord = xOrg + x / noiseTex.width * scale;
                float yCoord = yOrg + y / noiseTex.height * scale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                pix[(int)y * noiseTex.width + (int)x] = new Color(sample, sample, sample, alpha);

                //pix[(int)y * noiseTex.width + (int)x] = new Color(1.0f, 0, 0, 0.5f);

                x++;
            }
            y++;
        }

        // Copy the pixel data to the texture and load it into the GPU.
        noiseTex.SetPixels(pix);
        noiseTex.Apply();
    }

    void Update()
    {
        //Debug.Log("Generate noise");
        CalcNoise();

        xOrg = Random.Range(0, 512);
        yOrg = Random.Range(0, 512);
    }
}
