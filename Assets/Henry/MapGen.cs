using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGen : MonoBehaviour
{

    
    [SerializeField] private GameObject cube;
    [SerializeField] private GameObject line;
    [SerializeField] private bool random;
    [SerializeField] private float laius;
    [SerializeField] private float ulatuvus;

    [SerializeField] private GameObject endofsection;

    private GameObject[] lines_l;
    private GameObject[] lines_r;
    private LineRenderer[] line_r_l;
    private LineRenderer[] line_r_r;
    
    [SerializeField] private int l;
    [SerializeField] private int depth;
    private float[] noise;
    private float[] w;
    private int offset;
    private float noise_seed;

    [SerializeField] private int der_length;
    [SerializeField] private float der;
    
    //NONO
    private Vector3[] punktid_l;
    private Vector3[] punktid_r;
    
    
    void Start()
    {
        if (random)
        {
            noise_seed = Random.Range(0f, 1f);
        }
        else
        {
            noise_seed = 0;
        }
        
        lines_l = new GameObject[depth];
        lines_r = new GameObject[depth];
        line_r_l = new LineRenderer[depth];
        line_r_r = new LineRenderer[depth];

        endofsection = Instantiate(endofsection);
        
        offset = 0;

        for (int i = 0; i < depth; i++)
        {
            UpdateMap();
        }
    }

    public void UpdateMap()
    {
        Destroy(lines_l[0]);
        Destroy(lines_r[0]);
        Destroy(line_r_l[0]);
        Destroy(line_r_r[0]);
        for (int i = 0; i < depth-1; i++)
        {
            lines_l[i] = lines_l[i + 1];
            line_r_l[i] = line_r_l[i + 1];
            lines_r[i] = lines_r[i + 1];
            line_r_r[i] = line_r_r[i + 1];
        }
        
        if (lines_l[depth-2] != null)
        {
            endofsection.transform.position = new Vector3(0, -offset);
        }

        lines_l[depth-1] = Instantiate(line);
        line_r_l[depth-1] = lines_l[depth-1].GetComponent<LineRenderer>();
        line_r_l[depth-1].positionCount = l;
        lines_r[depth-1] = Instantiate(line);
        line_r_r[depth-1] = lines_r[depth-1].GetComponent<LineRenderer>();
        line_r_r[depth-1].positionCount = l;

        noise = new float[l];
        w = new float[l];

        punktid_l = new Vector3[l];
        punktid_r = new Vector3[l];
        
        for (int i = 0; i < l; i++)
        {
            noise[i] = ulatuvus * (Mathf.PerlinNoise(noise_seed, (offset) * 0.07f) - 0.5f);
            //noise[i] = -offset;
            w[i] = laius * Mathf.PerlinNoise(noise_seed, (offset) * 0.07f) + 3f;
            //w[i] = offset;
            //Debug.Log(noise[i]);
            
            
            //NONO
            punktid_l[i] = new Vector3(noise[i] - w[i], -offset);
            punktid_r[i] = new Vector3(noise[i] + w[i], -offset);
            
            
            
            //line_r_l[depth-1].SetPosition(i, new Vector3(noise[i] - w[i], -offset));
            //line_r_r[depth-1].SetPosition(i, new Vector3(noise[i] + w[i], -offset));
            
            
            
            //Instantiate(cube, new Vector3(noise[i]-w[i], -i), Quaternion.Euler(Vector3.zero), gameObject.transform);
            //Instantiate(cube, new Vector3(noise[i]+w[i], -i), Quaternion.Euler(Vector3.zero), gameObject.transform);
            offset += 1;
        }
        //NONO
        line_r_l[depth-1].SetPositions(punktid_l);
        line_r_r[depth-1].SetPositions(punktid_r);

        offset -= 1;

        //collider also NONO
        EdgeCollider2D ec_l = lines_l[depth-1].AddComponent<EdgeCollider2D>();
        EdgeCollider2D ec_r = lines_r[depth-1].AddComponent<EdgeCollider2D>();
        ec_l.points = punktid_l.Select(i => { return new Vector2(i.x, i.y);}).ToArray();
        ec_r.points = punktid_r.Select(i => { return new Vector2(i.x, i.y);}).ToArray();
        

        /*
        MeshCollider mc_l = lines_l[depth - 1].AddComponent<MeshCollider>();
        MeshCollider mc_r = lines_r[depth - 1].AddComponent<MeshCollider>();
        Mesh mesh_l = new Mesh();
        Mesh mesh_r = new Mesh();
        line_r_l[depth - 1].BakeMesh(mesh_l, true);
        line_r_r[depth - 1].BakeMesh(mesh_r, true);
        mc_l.sharedMesh = mesh_l;
        mc_r.sharedMesh = mesh_r;
        */
        



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
                //Instantiate(cube, koht, quaternion.Euler(Vector3.zero));
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
                //Instantiate(cube, koht, quaternion.Euler(Vector3.zero));
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EndOfSection")
        {
            UpdateMap();
        }
    }
}
