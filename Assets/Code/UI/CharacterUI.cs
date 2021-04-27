namespace RunlingRun.UI
{
    using Character;
    using Managers;
    using Photon.Pun;
    using TMPro;
    using UnityEngine;

    [RequireComponent(typeof(CharacterBehaviour))]
    public class CharacterUI : MonoBehaviourPun
    {
        public TMP_Text NameText;
        private CharacterBehaviour _behav;
        private void Awake()
        {
            _behav = GetComponent<CharacterBehaviour>();
        }

        private void Start()
        {
            if (photonView.IsMine)
            {
                photonView.RPC("RPCSetName", RpcTarget.AllBuffered, _behav.Name);
            }
        }

        [PunRPC]
        private void RPCSetName(string name)
        {
            NameText.text = name;
        }
    }
}