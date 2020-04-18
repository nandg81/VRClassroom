namespace PlayoVR {
    using UnityEngine;

    public class NetworkConnectManager : Photon.PunBehaviour {
        public static string gameVersion = "0.4";

        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        public byte MaxPlayersPerRoom = 4;
        public PhotonLogLevel Loglevel = PhotonLogLevel.Informational;
        public FirstScreen fs;
        public void TeacherConnect()
        {
            Debug.Log("Teacher connect method");
            if (!PhotonNetwork.connecting && !PhotonNetwork.connected)
            {
                PhotonNetwork.autoJoinLobby = false;
                PhotonNetwork.automaticallySyncScene = false;
                PhotonNetwork.logLevel = Loglevel;
                PhotonNetwork.ConnectUsingSettings(gameVersion);
            }
        }

        public void StudentConnect()
        {
            Debug.Log("Student connect method");
            if (!PhotonNetwork.connecting && !PhotonNetwork.connected)
            {
                PhotonNetwork.autoJoinLobby = false;
                PhotonNetwork.automaticallySyncScene = false;
                PhotonNetwork.logLevel = Loglevel;
                PhotonNetwork.ConnectUsingSettings(gameVersion);
            }
        }
        //void Awake() {
         //   if (!PhotonNetwork.connecting && !PhotonNetwork.connected) {
          //      PhotonNetwork.autoJoinLobby = false;    // we join randomly. always. no need to join a lobby to get the list of rooms.
           //     PhotonNetwork.automaticallySyncScene = false;
           //     PhotonNetwork.logLevel = Loglevel;
            //    PhotonNetwork.ConnectUsingSettings(gameVersion);
          //  }
       // }

        public override void OnConnectedToMaster() {
            fs.resultt.text = "Connected to master";
            if(fs.teacher==true)
            {
                var options = new RoomOptions();
                PhotonNetwork.CreateRoom(fs.ccn, options, null);
                Debug.Log("Creating room");
                fs.resultt.text = "Creating room";
            }
            else
            {
                PhotonNetwork.JoinRoom(fs.jcn);
                fs.resultt.text = "Joining room";
            }

        }

        public override void OnJoinedLobby() {
            Debug.Log("Joined lobby");

            Debug.Log("Joining random room...");
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnLeftLobby() {
            Debug.Log("Left lobby");
        }

        public override void OnPhotonJoinRoomFailed(object[] codeAndMsg) {
            fs.resultt.text = "Can't join room!";
            fs.NCall();
        }

        public override void OnPhotonCreateRoomFailed(object[] codeAndMsg)
        {
            fs.resultt.text = "Can't create room!";
            fs.NCall();
        }
        public override void OnCreatedRoom() {
            fs.resultt.text = "Created room";
        }

        public override void OnJoinedRoom() {
            fs.resultt.text = "Joined room";
        }

        public override void OnLeftRoom() {
            Debug.Log("Left room");
        }

        public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer) {
            Debug.Log("Player connected");
        }

        public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer) {
            Debug.Log("Player disconnected");
        }

        public override void OnFailedToConnectToPhoton(DisconnectCause cause) {
            Debug.Log("Couldn't connect to Photon network");
        }

        public override void OnConnectionFail(DisconnectCause cause) {
            Debug.Log("Connection failed to the Photon network");
        }

        public override void OnDisconnectedFromPhoton() {
            Debug.Log("We got disconnected form the Photon network");
        }
    }
}
