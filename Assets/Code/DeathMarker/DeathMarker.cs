namespace RunlingRun.DeathMarker
{
    using Photon.Pun;
    using RunlingRun.Character;
    using TMPro;
    using UnityEngine;
    public class DeathMarker : MonoBehaviourPun
    {
        public TMP_Text PlayerNameText;
        private CharacterBehaviour _behav;

        public void SetCharacter(CharacterBehaviour behav)
        {
            _behav = behav;
            _behav.OnRevive += PlayerHasRevived;
            photonView.RPC("RPCNameText", RpcTarget.All, _behav.Name);
        }

        [PunRPC]
        private void RPCNameText(string text)
        {
            PlayerNameText.text = text;
        }

        private void PlayerHasRevived()
        {
            PhotonNetwork.Destroy(gameObject);
        }

        private void OnDestroy()
        {
            if (_behav != null)
            {
                _behav.OnRevive -= PlayerHasRevived;
            }
        }
    }
}

