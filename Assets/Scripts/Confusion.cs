namespace PlayoVR
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System.Diagnostics;
    using NetMQ;
    using NetMQ.Sockets;
    using AsyncIO;
    using NetBase;
    public class Confusion : Photon.MonoBehaviour
    {
        // Start is called before the first frame update
        public RequestSocket client;
        public string message;
        public bool gotMessage;
        public AvatarSpawnManager ph;
        public FirstScreen fh;
        void Start()
        {

                Process foo = new Process();
                foo.StartInfo.FileName = "C:\\Predictor\\PredictorUnity.py";
                foo.StartInfo.Arguments = "";
                foo.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                foo.Start();
                ForceDotNet.Force();
                client = new RequestSocket();
                client.Connect("tcp://localhost:55");
                client.SendFrame("Hello");
        }

        // Update is called once per frame
        void Update()
        {
            string y;
            if (ph.spawned==true)
            {
                gotMessage = client.TryReceiveFrameString(out message); // this returns true if it's successful
                if (gotMessage)
                {
                    y = fh.usernam;
                    photonView.RPC("x",PhotonTargets.All,message,y);
                }
            }
        }
        [PunRPC]
        public void x(string m,string n)
        {
            UnityEngine.Debug.Log("Received " + m);
            var con = GameObject.Find(n+"/Top/Conf");
            TMPro.TextMeshPro txt = con.GetComponent<TMPro.TextMeshPro>();
            txt.text = "Confusion: "+m;
            client.SendFrame("Hello");
        }

        private void OnDestroy()
        {
            NetMQConfig.Cleanup();
        }
    }
}