using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// a pool for all the references of the player in the game, it's for displaying info and get a specified player and do stuff with it;
public class GameManager : MonoBehaviour {
		
	private static Dictionary<string, Player> players = new Dictionary<string, Player> ();

	public static void registerPlayer(string _playerId, Player player) {
		// add the player to the dictionary
		players.Add (Constants.PLAYER_ID_PREFIX + _playerId, player);
		// modify the name of the gameobject this player script component attached to, 
		// the name is used later for identifying player being shot
		player.transform.name = Constants.PLAYER_ID_PREFIX + _playerId;
	}

	public static void unregisterPlayer(string _playerId) {
		if (players.ContainsKey (Constants.PLAYER_ID_PREFIX + _playerId)) {
			players.Remove (Constants.PLAYER_ID_PREFIX + _playerId);
		}
	}

	void OnGUI() {
		GUILayout.BeginArea (new Rect (200, 200, 200, 500));
		GUILayout.BeginVertical ();

		foreach (string _key in players.Keys) {
			GUILayout.Label (players[_key].transform.name + " - " 
				+ players[_key].getCurrentHealth() + "/" 
				+ players[_key].getMaxmunHealth());
		}

		GUILayout.EndVertical ();
		GUILayout.EndArea ();
	}

	public static Player getPlayer(string _playerID) {
		return players[_playerID];
	} 
}
