using Photon.Pun;

namespace Network
{
    public class NetworkClass : MonoBehaviourPun
    {
        public bool IsMine => photonView.IsMine && PhotonNetwork.IsConnected;
        public bool IsMasterClient => PhotonNetwork.IsMasterClient;
    }
}