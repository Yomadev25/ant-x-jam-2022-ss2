using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

namespace Multiplayer.Online
{
    public class RoomManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject _roomPanel;
        [SerializeField] private GameObject _lobbyPanel;

        [SerializeField] private TMP_Text _roomCode;

        [SerializeField] private GameObject _playButton;

        public void JoinedRoom()
        {
            _roomCode.text = PhotonNetwork.CurrentRoom.Name;
        }

        private void Update()
        {
            if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= 2)
            {
                _playButton.SetActive(true);
            }
            else
            {
                _playButton.SetActive(false);
            }
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        public override void OnLeftRoom()
        {
            _lobbyPanel.SetActive(true);
            _roomPanel.SetActive(false);
        }

        public void Play()
        {
            PhotonNetwork.LoadLevel("Online");
        }
    }
}
