using System.Collections;
using System.Collections.Generic;
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
    
    // Start is called before the first frame update
    void Start()
    {
        lines_l = new GameObject[depth];
        lines_r = new GameObject[depth];
        line_r_l = new LineRenderer[depth];
        line_r_r = new LineRenderer[depth];
        
        
        offset = 0;

        for (int i = 0; i < depth; i++)
        {
            update_map();
        }
        
        /*for (int j = 0; j < depth; j++)
        {
            lines_l[j] = Instantiate(line);
            line_r_l[j] = lines_l[j].GetComponent<LineRenderer>();
            line_r_l[j].positionCount = l;
            lines_r[j] = Instantiate(line);
            line_r_r[j] = lines_r[j].GetComponent<LineRenderer>();
            line_r_r[j].positionCount = l;


            noise = new float[l];
            w = new float[l];

            for (int i = 0; i < l; i++)
            {
                //noise[i] = Mathf.PerlinNoise(1, (float)(i)*0.01f);
                noise[i] = 50 * (Mathf.PerlinNoise(1, (offset) * 0.07f) - 0.5f);
                w[i] = 30 * Mathf.PerlinNoise(1.5f, (offset) * 0.07f) + 3f;
                //Debug.Log(noise[i]);
                line_r_l[j].SetPosition(i, new Vector3(noise[i] - w[i], -offset));
                line_r_r[j].SetPosition(i, new Vector3(noise[i] + w[i], -offset));
                //Instantiate(cube, new Vector3(noise[i]-w[i], -i), Quaternion.Euler(Vector3.zero), gameObject.transform);
                //Instantiate(cube, new Vector3(noise[i]+w[i], -i), Quaternion.Euler(Vector3.zero), gameObject.transform);
                offset += 1;
            }

            offset -= 1;
        }*/
    }

    void update_map()
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
            //noise[i] = Mathf.PerlinNoise(1, (float)(i)*0.01f);
            noise[i] = 50 * (Mathf.PerlinNoise(1, (offset) * 0.07f) - 0.5f);
            w[i] = 30 * Mathf.PerlinNoise(1.5f, (offset) * 0.07f) + 3f;
            //Debug.Log(noise[i]);
            line_r_l[depth-1].SetPosition(i, new Vector3(noise[i] - w[i], -offset));
            line_r_r[depth-1].SetPosition(i, new Vector3(noise[i] + w[i], -offset));
            //Instantiate(cube, new Vector3(noise[i]-w[i], -i), Quaternion.Euler(Vector3.zero), gameObject.transform);
            //Instantiate(cube, new Vector3(noise[i]+w[i], -i), Quaternion.Euler(Vector3.zero), gameObject.transform);
            offset += 1;
        }

        offset -= 1;
    }

}
