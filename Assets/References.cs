using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class References
{
    //Participants
    public static PlayerBehaviour thePlayer;
    public static VotingPostScript votingPost;
    public static List<ElectorBehaviour> electors = new List<ElectorBehaviour>();
    public static List<ElectorBehaviour> targetElectors = new List<ElectorBehaviour>();
    //Navigation
    public static List<SpawnPoint> spawnPoints = new List<SpawnPoint>();
    public static List<LeaveArea> leaveAreaPoints = new List<LeaveArea>();
    public static int pointsForPlayerCandidate;
    public static int pointsForOppositeCandidate;
    //Experience Management
    public static int gamesCount = 0;
    //Level Progress Tracking
    public static List<FighterScript> blueFighters = new List<FighterScript>();
    public static List<FighterScript> redFighters = new List<FighterScript>();
    public static bool electionsEnded;
    public static bool fightEnded;
    public static LevelManager levelManager;
    //Map Nav
    public static GameObject currentPad;
    public static GameObject currentlyHighlightedPad;
    public static List<GameObject> pads = new List<GameObject>();
    //Camera
    public static CameraTools cameraTools;

}
