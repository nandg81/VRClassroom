namespace PlayoVR
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using PlayFab;
    using PlayFab.ClientModels;
    public class FirstScreen : MonoBehaviour
    {
        public GameObject[] buttons;
        public GameObject[] RegComp;
        public GameObject[] LogComp;
        public GameObject[] SucComp;
        public GameObject[] LS;
        public GameObject[] TS;
        public bool IsAuthenticated = false;
        public InputField userl;
        public InputField passl;
        public InputField email;
        public InputField user;
        public InputField pass;
        public InputField cclass;
        public InputField jclass;
        public Text resultt;
        public string usernam;
        public bool teacher = false;
        public string ccn;
        public string jcn;
        public int avi;
        public NetworkConnectManager pho;
        // Start is called before the first frame update
        LoginWithPlayFabRequest loginRequest;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnClickLogin()
        {
            foreach (GameObject button in buttons)
            {
                button.SetActive(false);
            }
            foreach (GameObject obj in LogComp)
            {
                obj.SetActive(true);
            }
        }
        public void OnClickHomeLog()
        {
            foreach (GameObject button in buttons)
            {
                button.SetActive(true);
            }
            foreach (GameObject obj in LogComp)
            {
                obj.SetActive(false);
            }
        }
        public void OnClickRegister()
        {
            foreach (GameObject button in buttons)
            {
                button.SetActive(false);
            }
            foreach (GameObject obj in RegComp)
            {
                obj.SetActive(true);
            }
            resultt.text = "";
        }
        public void OnClickHomeReg()
        {
            foreach (GameObject button in buttons)
            {
                button.SetActive(true);
            }
            foreach (GameObject obj in RegComp)
            {
                obj.SetActive(false);
            }
            resultt.text = "";
        }

        public void OnClickSEL()
        {
            loginRequest = new LoginWithPlayFabRequest();
            loginRequest.Username = userl.text;
            loginRequest.Password = passl.text;
            Debug.Log(loginRequest.Username);
            Debug.Log(loginRequest.Password);
            PlayFabClientAPI.LoginWithPlayFab(loginRequest, result =>
            {
                usernam = loginRequest.Username;
                resultt.text = "Logged in as " + usernam;
                IsAuthenticated = true;
                OnLogin();
            }, error =>
            {
                IsAuthenticated = false;
                resultt.text = error.ErrorMessage;
                Debug.Log(error.ErrorMessage);
            }, null);
        }

        public void OnLogin()
        {
            foreach (GameObject obj in LogComp)
            {
                obj.SetActive(false);
            }
            foreach (GameObject obj in SucComp)
            {
                obj.SetActive(true);
            }
        }

        public void OnLearn()
        {
            teacher = false;
            foreach (GameObject obj in SucComp)
            {
                obj.SetActive(false);
            }
            foreach (GameObject obj in LS)
            {
                obj.SetActive(true);
            }
        }

        public void OnTeach()
        {
            teacher = true;
            foreach (GameObject obj in SucComp)
            {
                obj.SetActive(false);
            }
            foreach (GameObject obj in TS)
            {
                obj.SetActive(true);
            }
        }

        public void OnClickSER()
        {
            RegisterPlayFabUserRequest request = new RegisterPlayFabUserRequest();
            request.Email = email.text;
            request.Username = user.text;
            request.Password = pass.text;
            PlayFabClientAPI.RegisterPlayFabUser(request, result =>
            {
                resultt.text = "User Registered Successfully";
            }, error =>
            {
                resultt.text = "Failed to create account" + error.ErrorMessage;
            });

        }

        public void OnClickA1()
        {
            avi = 1;
        }

        public void OnClickA2()
        {
            avi = 2;
        }

        public void OnJoin()
        {
            jcn=jclass.text;
            pho.StudentConnect();
            foreach (GameObject obj in LS)
            {
                obj.SetActive(false);
            }
        }

        public void OnCreate()
        {
            ccn=cclass.text;
            pho.TeacherConnect();
            foreach (GameObject obj in TS)
            {
                obj.SetActive(false);
            }
        }

        public void NCall()
        {
            foreach (GameObject obj in SucComp)
            {
                obj.SetActive(true);
            }
        }

    }
}