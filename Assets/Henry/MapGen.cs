using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class MapGen : MonoBehaviour
{

    
    [SerializeField] private GameObject cube;
    [SerializeField] private GameObject line;

    private GameObject[] lines_l;
    private GameObject[] lines_r;
    private LineRenderer[] line_r_l;
    private LineRenderer[] line_r_r;
    
    [SerializeField] private int l;
    [SerializeField] private int depth;
    private float[] noise;
    private float[] w;
    private int offset;

    [SerializeField] private int der_length;
    [SerializeField] private float der;
    
    void Start()
    {
        lines_l = new GameObject[depth];
        lines_r = new GameObject[depth];
        line_r_l = new LineRenderer[depth];
        line_r_r = new LineRenderer[depth];
        
        
        offset = 0;

        for (int i = 0; i < depth; i++)
        {
            UpdateMap();
        }
    }

    void UpdateMap()
    {
        for (int i = 0; i < depth-1; i++)
        {
            lines_l[i] = lines_l[i + 1];
            line_r_l[i] = line_r_l[i + 1];
            lines_r[i] = lines_r[i + 1];
            line_r_r[i] = line_r_r[i + 1];
        }
        lines_l[depth-1] = Instantiate(line);
        line_r_l[depth-1] = lines_l[depth-1].GetComponent<LineRenderer>();
        line_r_l[depth-1].positionCount = l;
        lines_r[depth-1] = Instantiate(line);
        line_r_r[depth-1] = lines_r[depth-1].GetComponent<LineRenderer>();
        line_r_r[depth-1].positionCount = l;


        noise = new float[l];
        w = new float[l];

        for (int i = 0; i < l; i++)
        {
            noise[i] = 50 * (Mathf.PerlinNoise(1, (offset) * 0.07f) - 0.5f);
            //noise[i] = -offset;
            w[i] = 30 * Mathf.PerlinNoise(1.5f, (offset) * 0.07f) + 3f;
            //w[i] = offset;
            //Debug.Log(noise[i]);
            
            line_r_l[depth-1].SetPosition(i, new Vector3(noise[i] - w[i], -offset));
            line_r_r[depth-1].SetPosition(i, new Vector3(noise[i] + w[i], -offset));
            //Instantiate(cube, new Vector3(noise[i]-w[i], -i), Quaternion.Euler(Vector3.zero), gameObject.transform);
            //Instantiate(cube, new Vector3(noise[i]+w[i], -i), Quaternion.Euler(Vector3.zero), gameObject.transform);
            offset += 1;
        }

        offset -= 1;
        
        ArrayList goodPlacesOnTheLeft = GETGoodPlacesOnTheLeft();
        if (goodPlacesOnTheLeft.Count > 0)
        {
            //print(goodPlaces.Count);
            //print(goodPlaces[0]);
            foreach (int place in goodPlacesOnTheLeft)
            {
                int placing = place + (int) Math.Floor(der_length / 2f);
                Vector3 koht;
                
                if (placing >= 0)
                {
                    koht = line_r_l[depth - 1].GetPosition(placing);
                }
                else
                {
                    koht = line_r_l[depth - 2].GetPosition(l + placing);
                }
                Instantiate(cube, koht,
                    quaternion.Euler(Vector3.zero));
            }
        }

        ArrayList goodPlacesOnTheRight = GETGoodPlacesOnTheRight();
        if (goodPlacesOnTheRight.Count > 0)
        {
            //print(goodPlaces.Count);
            //print(goodPlaces[0]);
            foreach (int place in goodPlacesOnTheRight)
            {
                int placing = place + (int) Math.Floor(der_length / 2f);
                Vector3 koht;
                
                if (placing >= 0)
                {
                    koht = line_r_r[depth - 1].GetPosition(placing);
                }
                else
                {
                    koht = line_r_r[depth - 2].GetPosition(l + placing);
                }
                Instantiate(cube, koht,
                    quaternion.Euler(Vector3.zero));
            }
        }

    }

    ArrayList GETGoodPlacesOnTheLeft()
    {
        ArrayList good = new ArrayList();
        
        float first;
        float second;
        float deriv;

        if (line_r_l[depth-2] != null)
        {
            for (int i = 0; i < der_length; i++)
            {
                first = line_r_l[depth - 2].GetPosition(l - der_length + i - 1).x;
                second = line_r_l[depth - 1].GetPosition(i).x;
                if (first != second)
                {
                    deriv = (float) (Math.Atan(der_length / (second - first)) * (180 / Math.PI));
                }
                else
                {
                    deriv = 90;
                }

                //Debug.Log(deriv);
                if (deriv < der && deriv > 0)
                {
                    good.Add(-der_length + i);
                }
            }
        }

        for (int i = 0; i < l-der_length-1; i++)
        {
            first = line_r_l[depth - 1].GetPosition(i).x;
            second = line_r_l[depth - 1].GetPosition(i+der_length).x;
            if (first != second)
            {
                deriv = (float) (Math.Atan(der_length / (second - first))*(180/Math.PI));
            }
            else
            {
                deriv = 90;
            }
            //Debug.Log(deriv);
            if (deriv < der && deriv > 0)
            {
                good.Add(i);
            }
        }
        return good;
    }
    
    ArrayList GETGoodPlacesOnTheRight()
    {
        ArrayList good = new ArrayList();
        
        float first;
        float second;
        float deriv;

        if (line_r_r[depth-2] != null)
        {
            for (int i = 0; i < der_length; i++)
            {
                first = line_r_r[depth - 2].GetPosition(l - der_length + i - 1).x;
                second = line_r_r[depth - 1].GetPosition(i).x;
                if (first != second)
                {
                    deriv = (float) (Math.Atan(der_length / (first - second)) * (180 / Math.PI));
                }
                else
                {
                    deriv = 90;
                }

                //Debug.Log(deriv);
                if (deriv < der && deriv > 0)
                {
                    good.Add(-der_length + i);
                }
            }
        }

        for (int i = 0; i < l-der_length-1; i++)
        {
            first = line_r_r[depth - 1].GetPosition(i).x;
            second = line_r_r[depth - 1].GetPosition(i+der_length).x;
            if (first != second)
            {
                deriv = (float) (Math.Atan(der_length / (first - second))*(180/Math.PI));
            }
            else
            {
                deriv = 90;
            }
            //Debug.Log(deriv);
            if (deriv < der && deriv > 0)
            {
                good.Add(i);
            }
        }
        return good;
    }

}
