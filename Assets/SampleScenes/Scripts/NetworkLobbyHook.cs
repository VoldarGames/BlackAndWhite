using UnityEngine;
using UnityStandardAssets.Network;
using System.Collections;
using Assets.Scripts.Managers.Net;
using Assets.Scripts.TestScripts.Net;
using UnityEngine.Networking;

public class NetworkLobbyHook : LobbyHook
{
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
        //NetworkSpaceship spaceship = gamePlayer.GetComponent<NetworkSpaceship>();

        NetSpawnerManager netCoreSpawnerManager = gamePlayer.GetComponent<NetSpawnerManager>();
        var lobbyslot = lobby.slot;
        if(lobbyslot == 0) netCoreSpawnerManager.MyTeam = 1;
        if (lobbyslot == 1) netCoreSpawnerManager.MyTeam = 0;
    }
}
