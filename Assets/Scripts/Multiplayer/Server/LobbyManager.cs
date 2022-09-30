using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

namespace Multiplayer.Online
{
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TMP_InputField _codeInputText;
        [SerializeField] private GameObject _roomPanel;

        public void CreateRoom()
        {
            int roomCode = Random.Range(1000, 9999);
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 2;

            PhotonNetwork.CreateRoom(roomCode.ToString(), roomOptions);
        }

        public void JoinRoom()
        {
            if (_codeInputText.text.Length >= 1)
            {
                PhotonNetwork.JoinRoom(_codeInputText.text);
            }
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Joined " + PhotonNetwork.CurrentRoom.Name);
            _roomPanel.SetActive(true);
            _roomPanel.GetComponent<RoomManager>().JoinedRoom();
            this.gameObject.SetActive(false);
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.LogWarning(returnCode + message);
        }
    }
}
