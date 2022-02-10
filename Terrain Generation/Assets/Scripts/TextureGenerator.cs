using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//No need for monobehaviour 
//Only one instance
public static class TextureGenerator{

    //Generates a 2D texture from the provided colour map
    public static Texture2D TextureFromColourMap(Color[] colourMap, int width, int height)
    {
        Texture2D texture = new Texture2D(width, height);
        //Fixing blurred texture and the removing the wrap around of the texture on the plane
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colourMap);
        texture.Apply();
        return texture;
    }

    //Generates a 2D texture from the provided height map
    public static Texture2D TextureFromHeightMap(float[,] heightMap)
    {
        int width = heightMap.GetLength(0);//Gets index 0 in the float array
        int height = heightMap.GetLength(1);//Gets index 1 in the float array
;

        Color[] colourMap = new Color[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colourMap[y * width + x] = Color.Lerp(Color.black, Color.white, heightMap[x, y]);
            }
        }

        return TextureFromColourMap(colourMap, width, height);

    }
}
