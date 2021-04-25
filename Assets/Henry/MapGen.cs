using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class MapGen : MonoBehaviour
{

    
    //kuubik, mida ma kasutasin, et leida erinevaid kohti
    [SerializeField] private GameObject cube;
    //joon, mida ma joonistan, et näha ala, kus saab liikuda
    [SerializeField] private GameObject line;
    //kas teha random map või mitte
    [SerializeField] private bool random;
    //max laius, mis võib mapis olla
    [SerializeField] private float laius;
    //max kaugus, kuhu map ulatab x pidi
    [SerializeField] private float ulatuvus;

    //trigger, mis lõpetab sektsiooni ja kutsub update map funk
    [SerializeField] private GameObject endofsection;

    //left lines
    private GameObject[] lines_l;
    //right lines
    private GameObject[] lines_r;
    //left line renderers
    private LineRenderer[] line_r_l;
    //right line renderers
    private LineRenderer[] line_r_r;
    
    //ühe sektsiooni sügavus (+1, et need connectiksid)
    [SerializeField] private int l;
    //mitu sektsiooni korraga on
    [SerializeField] private int depth;
    //keskjoone vasakule/paremale liikumise noise
    private float[] noise;
    //laiuse noise
    private float[] w;
    //kui sügavale jõudnud oleme y pidi
    private int offset;
    //noise seed, kui on random
    private float noise_seed;

    //tuletise sügavus (mitu ühikut on vahet nendel kahel punktil, mille vahel sa tõusu vaatad)
    [SerializeField] private int der_length;
    //max nurk, millel saab veel spawnida asju
    [FormerlySerializedAs("min_nurk")] [FormerlySerializedAs("der")] [SerializeField] private float max_nurk;
    
    //jätame meelde vasaku ja parema joone punktid
    private Vector3[] punktid_l;
    private Vector3[] punktid_r;

    //see on mesh prefab, millest teeme meshid, mis töötavad tänu laasile, aitäh laas
    [SerializeField] private GameObject mesh_prefab;
    
    //need on laevavrakid ja allveelaevad
    [SerializeField] private GameObject[] props;
    //need on erinevad ründavad kalad
    [SerializeField] private GameObject[] kalad;
    //need on jellyfishid ja kastid ja pufferfishid ja muud exrad
    [SerializeField] private GameObject[] extras;
    //see on seaweed prefab, et teha seaweed
    [SerializeField] private GameObject seaweed;
    //see on probability, et mingile sobivale kohale tuleb seaweed
    [SerializeField] private float seaweed_prob;
    
    void Start()
    {
        //kui on valitud, ss teeb random noise
        if (random)
        {
            noise_seed = Random.Range(0f, 1f);
        }
        else
        {
            noise_seed = 0;
        }
        
        //teeme jooned ja joonte renderid
        lines_l = new GameObject[depth];
        lines_r = new GameObject[depth];
        line_r_l = new LineRenderer[depth];
        line_r_r = new LineRenderer[depth];

        //teeme triggeri
        endofsection = Instantiate(endofsection);
        
        //offset = 0
        offset = 0;

        //updateme mapi alguses depth korda, et oleks depth sektsiooni olemas
        for (int i = 0; i < depth; i++)
        {
            UpdateMap();
        }
    }

    //kaotame ära esimese sektsiooni ja teeme ühe juurde
    public void UpdateMap()
    {
        //kustutame esimese sektsiooni ära (neid ei ole eriti palju, seega all is well, brait)
        Destroy(lines_l[0]);
        Destroy(lines_r[0]);
        Destroy(line_r_l[0]);
        Destroy(line_r_r[0]);
        //ja tõstame teised ette poole (siin ka)
        for (int i = 0; i < depth-1; i++)
        {
            lines_l[i] = lines_l[i + 1];
            line_r_l[i] = line_r_l[i + 1];
            lines_r[i] = lines_r[i + 1];
            line_r_r[i] = line_r_r[i + 1];
        }
        
        //kui veel pole sektsioone, paneme endsection triggeri õigesse kohta
        if (lines_l[depth-2] != null)
        {
            endofsection.transform.position = new Vector3(0, -offset);
        }

        //teeme uuue joone ja rendereri mõlemale poole
        lines_l[depth-1] = Instantiate(line);
        line_r_l[depth-1] = lines_l[depth-1].GetComponent<LineRenderer>();
        line_r_l[depth-1].positionCount = l;
        lines_r[depth-1] = Instantiate(line);
        line_r_r[depth-1] = lines_r[depth-1].GetComponent<LineRenderer>();
        line_r_r[depth-1].positionCount = l;

        //teeme noise ja w tühjaks
        noise = new float[l];
        w = new float[l];

        //teeme need tühjaks, et uusi salvestada
        punktid_l = new Vector3[l];
        punktid_r = new Vector3[l];
        
        //teeme l uut punkti
        for (int i = 0; i < l; i++)
        {
            //loeme "häälest" noise ja w
            noise[i] = ulatuvus * (Mathf.PerlinNoise(noise_seed, (offset) * 0.07f) - 0.5f);
            w[i] = laius * Mathf.PerlinNoise(noise_seed, (offset) * 0.07f) + 3f;
            
            //paneme vajalikud väärtused kirja punktidesse
            punktid_l[i] = new Vector3(noise[i] - w[i], -offset);
            punktid_r[i] = new Vector3(noise[i] + w[i], -offset);
            
            
            
            //kui vaja, ss saad sinna joonele kuubikud teha
            //Instantiate(cube, new Vector3(noise[i]-w[i], -i), Quaternion.Euler(Vector3.zero), gameObject.transform);
            //Instantiate(cube, new Vector3(noise[i]+w[i], -i), Quaternion.Euler(Vector3.zero), gameObject.transform);
            
            //liidame offset 1 juurde, sest me liigume alla poole
            offset += 1;
        }
        
        //joonistame jooned
        line_r_l[depth-1].SetPositions(punktid_l);
        line_r_r[depth-1].SetPositions(punktid_r);

        //võtame ühe maha, sest 1 peab kattuma järgmisega, et oleks pidev joon
        offset -= 1;

        //teeme edge colliderid ja lisame neile kõik need punktid, mis me enne kirja panime ja joontele lisasime
        EdgeCollider2D ec_l = lines_l[depth-1].AddComponent<EdgeCollider2D>();
        EdgeCollider2D ec_r = lines_r[depth-1].AddComponent<EdgeCollider2D>();
        ec_l.points = punktid_l.Select(i => { return new Vector2(i.x, i.y);}).ToArray();
        ec_r.points = punktid_r.Select(i => { return new Vector2(i.x, i.y);}).ToArray();

        //teeme meshid, millele pärast tekstuuri anname
        GameObject left_mesh_prefab = Instantiate(mesh_prefab);
        GameObject right_mesh_prefab = Instantiate(mesh_prefab);
        
        //teeme uued meshid ja paneme need peale 
        Mesh mesh_l = new Mesh();
        Mesh mesh_r = new Mesh();
        left_mesh_prefab.GetComponent<MeshFilter>().mesh = mesh_l;
        right_mesh_prefab.GetComponent<MeshFilter>().mesh = mesh_r;

        //kutsume createMesh, mis lisab antud meshile vertices ja triangles ja mingi random UV mapi, sest ma ei tea miks sellga saab repeati peale olevat tekstuuri ilusti kuvada
        createMesh(mesh_l, punktid_l, true, offset);
        createMesh(mesh_r, punktid_r, false, offset);
        
        //saame teada need head kohad vasakul, mis on horisontaalsemad kui max_nurk lubab, ehk head kohad, kuhu prop panna
        ArrayList goodPlacesOnTheLeft = GETGoodPlacesOnTheLeft(max_nurk);
        //kui vähemalt 1 on olemas, ss tee sellega asju
        if (goodPlacesOnTheLeft.Count > 0)
        {
            //käi kõik head kohad läbi
            foreach (float[] place in goodPlacesOnTheLeft)
            {
                //place sisaldab {viimase üksuse indeskit, mis sobis; ja selle nurka}
                //arvutame välja selle keskmise koha, kuhu prolly sobib prop panna
                int placing = (int) place[0] + (int) Math.Floor(der_length / 2f);
                //see on hea koht
                Vector3 koht;

                //kui on üle 0, ss see on selles viimases sektsioonis
                if (placing >= 0)
                {
                    //ehk võtame viimase sektsiooni õige indeksiga elemendi ja saame selle positsiooni
                    koht = line_r_l[depth - 1].GetPosition(placing);
                }
                //kui on alla 0, ss on eelviimases sekstioonis
                else
                {
                    //ehk võtame eelviimase sektsiooni õige indeksiga elemendi ja saame selle koha (siin ei saa -1 ja muid taoliseid kasutada, seega peab kasutama length + indeks, mis on alla 0)
                    koht = line_r_l[depth - 2].GetPosition(l + placing);
                }
                
                //kui rnd value on soodne seaweedi kasvamiseks
                if (Random.value < seaweed_prob)
                {
                    //teeme seaweed
                    GameObject a = Instantiate(seaweed);
                    //ja muudame selle asukohta ja rotationit
                    a.transform.GetChild(0).GetComponent<SeaweedBush>().setPos(koht, Quaternion.Euler(0, 0, -place[1]));
                }
            }
        }
        //saame teada need head kohad paremal, mis on horisontaalsemad kui max_nurk lubab. ehk head kohad, kuhu panna props
        ArrayList goodPlacesOnTheRight = GETGoodPlacesOnTheRight(max_nurk);
        //kui vähemalt üks on olemas, ss tee sellega asju
        if (goodPlacesOnTheRight.Count > 0)
        {
            //käi kõik head kohad läbi
            foreach (float[] place in goodPlacesOnTheRight)
            {
                //place sisaldab {viimase üksuse indeskit, mis sobis; ja selle nurka}
                //arvutame välja selle keskmise koha, kuhu prolly sobib prop panna
                int placing = (int) place[0] + (int) Math.Floor(der_length / 2f);
                //see on hea koht
                Vector3 koht;

                //kui on üle 0, ss see on selles viimases sektsioonis
                if (placing >= 0)
                {
                    //ehk võtame viimase sektsiooni õige indeksiga elemendi ja saame selle positsiooni
                    koht = line_r_r[depth - 1].GetPosition(placing);
                }
                else
                {
                    //ehk võtame eelviimase sektsiooni õige indeksiga elemendi ja saame selle koha (siin ei saa -1 ja muid taoliseid kasutada, seega peab kasutama length + indeks, mis on alla 0)
                    koht = line_r_r[depth - 2].GetPosition(l + placing);
                }
                
                //kui tahad, võid panna sinna kuubiku, kuhu saaks asju panna
                //Instantiate(cube, koht, quaternion.Euler(Vector3.zero));

                //kui rnd value on soodne seaweedi kasvamiseks
                if (Random.value < seaweed_prob)
                {
                    //teeme seaweed
                    GameObject a = Instantiate(seaweed);
                    //ja muudame selle asukohta ja rotationit
                    a.transform.GetChild(0).GetComponent<SeaweedBush>().setPos(koht, Quaternion.Euler(0, 0, place[1]));
                }
            }
        }

        //saame teada head kohad, kus saaks spawnida vastaseid (ehk kohad, kus sein on ~~90 kraadi ehk püstine)
        //kahjuks see funktsioon õigesti ei tööta, te võiks selle korda teha, ma pol kindel kus viga on, sest me kustiga vaatasime selle koos üle ja me ei leidnud viga ples, see peaks õige olema, aga see miskipärast paneb haid seina sisse, idk
        goodPlacesOnTheLeft = GETGoodPlacesOnTheLeft(90, 75);
        //kui neid on
        if (goodPlacesOnTheLeft.Count > 0)
        {
            //käime kõik läbi
            foreach (float[] place in goodPlacesOnTheLeft)
            {
                //nagu enne
                int placing = (int) place[0] + (int) Math.Floor(der_length / 2f);
                //nagu enne
                float koht;
                
                //nagu enne
                if (placing >= 0)
                {
                    //nagu enne
                    koht = (line_r_l[depth - 1].GetPosition(placing).x + line_r_r[depth - 1].GetPosition(placing).x)/2;
                }
                else
                {
                    //nagu enne
                    koht = (line_r_l[depth - 2].GetPosition(l + placing).x + line_r_r[depth - 2].GetPosition(l + placing).x)/2;
                }
                //nagu enne
                //Instantiate(cube, koht, quaternion.Euler(Vector3.zero));
                
                
                //teeme 0-2, et oleks 50% võimalus, et tuleb kala
                float prob = Random.Range(0f, 2f);
                //vaatame, milline kala tuleb (küigil sama võimalus tulla)
                for (int i = 0; i < kalad.Length; i++)
                {
                    //kui on soodne kala spawnimis võimalus, ss tee seda
                    if (prob <= (i+1) / kalad.Length && prob > (i)/kalad.Length)
                    {
                        //pane see kala
                        GameObject kala = Instantiate(kalad[i]);
                        //ja pane ta õigesse kohta
                        kala.transform.position = new Vector3(koht, -offset+placing, 0);
                    }
                }
            }
        }
    }

    //saa teada head kohad, kuhu saaks panna mingid asjad, ma tegin nii, et saaks ise panna nurkade vahemiku
    ArrayList GETGoodPlacesOnTheLeft(float max, float min = 0)
    {
        //see list, mida tagastab pärast
        ArrayList good = new ArrayList();
        
        //paar muutujat, esimene punkt, mida vaatab, viimane punkt, mida vaatab, ja nende vaheline tuletis/tõus, kuigi see tglt on nende vaheline nurk
        float first;
        float second;
        float deriv;

        //kui pole, esimesed paar korda ehk on ikka olemas asjad, mida vaadta
        if (line_r_l[depth-2] != null)
        {
            //käime läbi eelmise sektsiooni viimased punktid
            for (int i = 0; i < der_length; i++)
            {
                //paneme kirja esimese punkti
                first = line_r_l[depth - 2].GetPosition(l - der_length + i - 1).x;
                //paneme kirja teise punkti, mis on mõne punkti kaugusel esimeset
                second = line_r_l[depth - 1].GetPosition(i).x;
                //kui need on võrdsed, ss oleks 0-ga jagamine, ja seda me ei taha
                if (first != second)
                {
                    //see on ühe x - teise x ehk üks kaatet jagada teine kaatet, mis ongi see nende vaheline kaugus, aga teistpidi ja ss see on tehtud kraadideks
                    deriv = (float) (Math.Atan(der_length / (second - first)) * (180 / Math.PI));
                }
                //kui need on võrdsed, tuleks nulliga jagamine
                else
                {
                    //seega paneme selle võrduma 90-ga
                    deriv = 90;
                }

                //kui see nurk jääb õigesse vahemikku
                if (deriv < max && deriv > min)
                {
                    //liidame selle sinna listi, mille pärast tagastame
                    good.Add(new float[]{-der_length + i, deriv});
                }
            }
        }

        //käime nüüd selle uue sekstsiooni ka läbi
        for (int i = 0; i < l-der_length-1; i++)
        {
            //nagu enne
            first = line_r_l[depth - 1].GetPosition(i).x;
            //nagu enne
            second = line_r_l[depth - 1].GetPosition(i+der_length).x;
            //nagu ene
            if (first != second)
            {
                //nagu enne
                deriv = (float) (Math.Atan(der_length / (second - first))*(180/Math.PI));
            }
            //nagu enne
            else
            {
                //nagu enne
                deriv = 90;
            }
            //nagu enne
            if (deriv < max && deriv > min)
            {
                //nagu enne
                good.Add(new float[]{i, deriv});
            }
        }
        //ja lõpuks tagastame selle listi kõikide sobivate kohtade ja  vastavagte nurkadega
        return good;
    }
    
    //see on sama nagu GETGoodPlacesOnLeft, aga siin on l asendatud r-ga, et saada paremal joonel asuvaid kohti ja siin on second ja first lahutatud teistpidi
    ArrayList GETGoodPlacesOnTheRight(float max, float min = 0)
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
                if (deriv < max && deriv > min)
                {
                    good.Add(new float[] {-der_length + i, deriv});
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
            if (deriv < max && deriv > min)
            {
                good.Add(new float[]{i, deriv});
            }
        }
        return good;
    }

    //see on see createMesh meetod, kus me teeme selle meshi, millele me paneme kivi tekstuuri, mis algusese ei töö(t/d)anud
    //anname sisse selle meshi, joonel olevad punktid, kas see on vasak või mitte(kas panna x negatiivne või positiivne), kui palju juba offset on
    void createMesh(Mesh mesh, Vector3[] line_points, bool left, int offset)
    {
        //teeme selle uue points listi, mis me anname meshile
        Vector3[] points = new Vector3[line_points.Length*2];
        //käime kõik joonel olevad punktid läbi
        for (int i = 0; i < line_points.Length; i++)
        {
            //ja lisame sinna joonel olevad punktid
            points[i] = line_points[i];
            //ja neile vastavad punktid, mis on 70 ühiku kaugusel, vastavalt + või -
            points[line_points.Length + i] = new Vector3(left ? -70 : 70, points[i].y, 0);
        }

        //teem listi nende kolmnurkade jaoks
        int[] tri = new int[(line_points.Length - 1) * 6];
        //ja käime kõik läbi
        for (int i = 0; i < tri.Length; i+=3)
        {
            //kui on vasak pool
            if (left)
            {
                //iga esimene on ühelt poole 2 ja teiselt poole 1
                if (i % 2 == 0)
                {
                    tri[i] = i/3 - i/6;
                    tri[i + 1] = i/3 + 1 - i/6;
                    tri[i + 2] = points.Length / 2 + i / 3 - i/6;
                }
                //ja iga teine in ühelt poole 1 ja teiselt poolt 2
                else
                {
                    tri[i] = i/3 - i/6;
                    tri[i + 2] = points.Length / 2 + i / 3 - 1 - i/6;
                    tri[i + 1] = points.Length / 2 + i / 3 - i/6;
                }
            }
            //kui on parem pool, on vastupidi, sest unity tahab neid päripäevas, et neid õigesti näidata, kinda retarded, aga still, ig peab
            else
            {
                if (i % 2 == 0)
                {
                    tri[i] = i/3 - i/6;
                    tri[i + 2] = i/3 + 1 - i/6;
                    tri[i + 1] = points.Length / 2 + i / 3 - i/6;
                }
                else
                {
                    tri[i] = i/3 - i/6;
                    tri[i + 1] = points.Length / 2 + i / 3 - 1 - i/6;
                    tri[i + 2] = points.Length / 2 + i / 3 - i/6;
                }
            }
            
        }

        //paneme need ss meshile punktideks ja kolmnurkadeks
        mesh.vertices = points;
        mesh.triangles = tri;

        //ja genereerime selle random UV mapi, mis miskipärast aitab tekstuuri ilusti kuvada, aga meist keegi ei tea, miks tglt 
        mesh.uv = points.Select(i => { return new Vector2(i.x/(left ? -7000 : 7000), (i.y-offset)/-7000); }).ToArray();
    }
    
}
