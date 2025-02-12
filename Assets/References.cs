using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class References
{
    public static PlayerBehaviour thePlayer;
    public static VotingPostScript votingPost;
    //public static CanvasBehaviour canvas;
    public static List<ElectorBehaviour> electors = new List<ElectorBehaviour>();
    public static List<ElectorBehaviour> targetElectors = new List<ElectorBehaviour>();

    public static List<SpawnPoint> spawnPoints = new List<SpawnPoint>();
    public static List<LeaveArea> leaveAreaPoints = new List<LeaveArea>();

    public static List<Useable> useables = new List<Useable>();

    public static int pointsForPlayerCandidate;
    public static int pointsForOppositeCandidate;
    public static int gamesCount = 0;

    public static List<FighterScript> blueFighters = new List<FighterScript>();
    public static List<FighterScript> redFighters = new List<FighterScript>();

    public static bool electionsEnded;
    public static bool fightEnded;

    public static LevelManager levelManager;

}
