using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class References
{
    public static PlayerBehaviour thePlayer;
    public static VotingPostScript votingPost;
    //public static CanvasBehaviour canvas;
    //public static CanvasInWorldBehaviour canvasInWorld;
    public static List<ElectorBehaviour> electors = new List<ElectorBehaviour>();
    public static List<ElectorBehaviour> targetElectors = new List<ElectorBehaviour>();

    public static List<SpawnPoint> spawnPoints = new List<SpawnPoint>();
    public static List<Useable> useables = new List<Useable>();

    public static int pointsForPlayerCandidate;
    public static int pointsForOppositeCandidate;
    public static LeaveArea leaveArea;
}
