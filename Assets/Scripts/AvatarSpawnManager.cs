namespace PlayoVR {
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using VRTK;
    using Hashtable = ExitGames.Client.Photon.Hashtable;

    public class AvatarSpawnManager : Photon.PunBehaviour {
        [Tooltip("Reference to the player avatar prefab")]
        public GameObject playerAvatar;
        public GameObject playerAvatar2;
        public FirstScreen fs;
        private GameObject[] spawnPoints;
        private bool sceneLoaded = false;
        private bool connected = false;
        public bool spawned=false;

        void Awake() {
            if (playerAvatar == null||playerAvatar2==null) {
                Debug.LogError("AvatarSpawnManager is missing a reference to the player avatar prefab!");
            }
            spawnPoints = GameObject.FindGameObjectsWithTag("Respawn");
            if (spawnPoints.Length == 0) {
                Debug.LogError("No spawn points were found!");
            }
        }

        void OnEnable() {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnDisable() {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            Debug.Log("Scene loaded");
            sceneLoaded = true;
        }

        public override void OnJoinedRoom() {
            connected = true;
            // Player sets its own name when joining
            PhotonNetwork.playerName = fs.usernam;
            // Initialize the master client
            InitPlayer(PhotonNetwork.player);
        }

        public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer) {
            InitPlayer(newPlayer);
        }

        public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer) {
        }

        void InitPlayer(PhotonPlayer newPlayer) {
            if (PhotonNetwork.isMasterClient && connected && sceneLoaded) {
                // The master client tells everyone about the new player
                Hashtable props = new Hashtable();
                props[PlayerPropNames.PLAYER_NR] = playerNr(newPlayer);
                newPlayer.SetCustomProperties(props);
                photonView.RPC("SpawnAvatar", newPlayer);
            }
        }

        [PunRPC]
        void SpawnAvatar() {
            if (!PhotonNetwork.player.CustomProperties.ContainsKey(PlayerPropNames.PLAYER_NR)) {
                Debug.LogError("Player does not have a PLAYER_NR property!");
                return;
            }
            int nr = (int)PhotonNetwork.player.CustomProperties[PlayerPropNames.PLAYER_NR];
            // Create a new player at the appropriate spawn spot
            var trans = spawnPoints[nr].transform;
            var name = PhotonNetwork.playerName;
            if (fs.avi == 1)
            {
                var player = PhotonNetwork.Instantiate(playerAvatar.name, trans.position, trans.rotation, 0, new object[] { name });
                spawned = true;
            }
            else
            {
                var player = PhotonNetwork.Instantiate(playerAvatar2.name, trans.position, trans.rotation, 0, new object[] { name });
                spawned = true;
            }

        }

        private string playerName(PhotonPlayer ply) {
            return "Player " + ply.ID;
        }

        private int playerNr(PhotonPlayer ply) {
            // TODO: do something a bit more clever here
            // We want players to actually show up in an empty spot
            return PhotonNetwork.otherPlayers.Length;
        }
    }
}

