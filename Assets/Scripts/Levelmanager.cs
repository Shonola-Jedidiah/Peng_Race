﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levelmanager : MonoBehaviour
{
    public bool Show_Collider = true;//$$
    public static Levelmanager Instance { set; get; }

   
    //Level spawning

    private const float Distance_Before_Spawn = 100.0f;
    private const int Initial_Segments = 10;
    private const int Initial_Transition_Segments = 2;
    private const int Max_Segments_On_Screen = 14;
    private Transform cameraContainer;
    private int amountOfActiveSegments;
    private int continousSegments;
    private int currentSpawnZ;
    private int currentLevel;
    private int y1, y2, y3;


    //list of Pieces
    public List<Pieces> ramps = new List<Pieces>();
    public List<Pieces> Longblocks = new List<Pieces>();
    public List<Pieces> jumps = new List<Pieces>();
    public List<Pieces> slides = new List<Pieces>();
    [HideInInspector]
    public List<Pieces> pieces = new List<Pieces>(); //All the pieces in the pool

    //List of segments
    public List<Segment> availableSegments = new List<Segment>();
    public List<Segment> availableTransitions = new List<Segment>();
    [HideInInspector]
    public List<Segment> segments = new List<Segment>();


    //GamePlay
    private bool isMoving = false;

    private void Awake()
    {
        Instance = this;
        cameraContainer = Camera.main.transform;
        currentSpawnZ = 0;
        currentLevel = 0;
    }

    private void Start()
    {
        for (int i = 0; i < Initial_Segments; i++)
        {
            if (i < Initial_Transition_Segments)
                SpawnTransition();
            else
                GenerateSegment();
        }
    }

    private void Update()
    {
        if (currentSpawnZ - cameraContainer.position.z < Distance_Before_Spawn)

            GenerateSegment();

        if (amountOfActiveSegments >= Max_Segments_On_Screen)
        {
            segments[amountOfActiveSegments - 1].DeSpawn();
            amountOfActiveSegments--;
        }

    }
    private void GenerateSegment()
    {
        SpawnSegment();

        if (Random.Range(0f, 1f) < (continousSegments * 0.25f))
        {
            //Spawn Transition
            continousSegments = 0;
            SpawnTransition();
        }
        else
        {
            continousSegments++;
        }
    }

    private void SpawnSegment()
    {
        List<Segment> possibleSeg = availableSegments.FindAll(x => x.beginY1 == y1 || x.beginY2 == y2 || x.beginY3 == y3);

        int id = Random.Range(0, possibleSeg.Count);


        Segment s = GetSegment(id, false);

        y1 = s.endY1;
        y2 = s.endY2;
        y3 = s.endY3;

        s.transform.SetParent(transform);
        s.transform.localPosition = Vector3.forward * currentSpawnZ;

        currentSpawnZ += s.lenght;
        amountOfActiveSegments++;
        s.Spawn();
    }


    private void SpawnTransition()
    {
        List<Segment> possibleTransition = availableTransitions.FindAll(x => x.beginY1 == y1 || x.beginY2 == y2 || x.beginY3 == y3);

        int id = Random.Range(0, possibleTransition.Count);


        Segment s = GetSegment(id, true);

        y1 = s.endY1;
        y2 = s.endY2;
        y3 = s.endY3;

        s.transform.SetParent(transform);
        s.transform.localPosition = Vector3.forward * currentSpawnZ;

        currentSpawnZ += s.lenght;
        amountOfActiveSegments++;
        s.Spawn();
    }

    public Segment GetSegment(int id, bool transition)
    {
        Segment s = null;
        s = segments.Find(x => x.SegId == id && x.transition == transition && !x.gameObject.activeSelf);

        if (s == null)
        {
            GameObject go = Instantiate((transition) ? availableTransitions[id].gameObject : availableSegments[id].gameObject as GameObject);
            s = go.GetComponent<Segment>();

            s.SegId = id;
            s.transition = transition;

            segments.Insert(0, s);
        }
        else
        {
            segments.Remove(s);
            segments.Insert(0, s);
        }
        return s;
    }

    public Pieces GetPieces(PieceType pt, int visualIndex)
    {
        Pieces p = pieces.Find(x => x.type == pt && x.visualIndex == visualIndex && !x.gameObject.activeSelf);

        if (p == null)
        {
            GameObject go = null;
            if (pt == PieceType.ramp)
            {
                go = ramps[visualIndex].gameObject;
            }

            else if (pt == PieceType.jump)
            {
                go = jumps[visualIndex].gameObject;
            }
            else if (pt == PieceType.slide)
            {
                go = slides[visualIndex].gameObject;
            }
            else if (pt == PieceType.longblock)
            {
                go = Longblocks[visualIndex].gameObject;
            }

            go = Instantiate(go);
            p = go.GetComponent<Pieces>();
            pieces.Add(p);
        }

        return p;
    }
}
