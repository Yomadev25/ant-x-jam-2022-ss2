using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

namespace Multiplayer.Online
{
    public class ConnectToServer : MonoBehaviourPunCallbacks
    {
        [Header("LOGIN INTERFACE")]
        [SerializeField] private TMP_InputField _name;
        [SerializeField] private TMP_Text _buttonText;
        [SerializeField] private GameObject _lobbyPanel;

        #region SERVER CONNECTION
        public void OnConnect()
        {
            if (_name.text.Length >= 1)
            {
                PhotonNetwork.NickName = _name.text;
                _buttonText.text = "Connecting..";
                PhotonNetwork.AutomaticallySyncScene = true;
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            _lobbyPanel.SetActive(true);
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogError(cause);
        }
        #endregion
    }
}
