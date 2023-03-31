using Audio;
using General;
using UnityEngine;

namespace Objects
{
    public class PaperBall : Interactable
    {
        private new void Start()
        {
            IsThrowable = true;
            base.Start();
            ShowArrow = false;
        }
        
        public override void Interact()
        {
            var player = GameManager.Instance.Player;

            if (player.HeldObject == gameObject) return;
            
            GameManager.Instance.Player.HoldObject(gameObject);
            AudioManager.PlayOneShot(FMODEvents.Instance.PaperBallGrab, player.transform.position);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player") || collision.gameObject.layer == LayerMask.NameToLayer("Arm"))
                Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
            
            AudioManager.PlayOneShot(FMODEvents.Instance.PaperBallHit, transform.position);
        }
    }
}
