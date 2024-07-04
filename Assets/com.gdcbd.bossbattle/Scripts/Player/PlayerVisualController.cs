using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace com.gdcbd.bossbattle.player
{
    public class PlayerVisualController : MonoBehaviour
    {
        // TODO : This feature will be handle by Player Animation Controller
        [SerializeField] private CapsuleCollider2D _colider;
        [SerializeField] private GameObject _body;
        [SerializeField] private Transform _gunContainer;
        private SpriteRenderer _bodySpriteRenderer;
        
        private Color originalColor;

        private void Awake()
        {
            _bodySpriteRenderer = _body.GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            
            originalColor = _bodySpriteRenderer.color;
        }

        public void Hurt()
        {
           // _bodySpriteRenderer.DOColor(Color.red, 0.1f).OnComplete(() => { _bodySpriteRenderer.DOColor(originalColor, 0.1f); });
        }

        public void PowerUP()
        {
          //  _bodySpriteRenderer.DOColor(Color.green, 0.1f).OnComplete(() =>
           // {
           //     _bodySpriteRenderer.DOColor(originalColor, 0.1f);
          //  });
        }

        public void Crouch()
        {
            _body.transform.DOScaleY(0.75f, 0.2f).SetEase(Ease.InOutQuad);
           
            _colider.size = new Vector2(0.7f, 0.7f);
            _colider.offset = new Vector2(0, 0.31f);
            _gunContainer.localPosition = new Vector3(0, 0.225f, 0);
        }
        public void StandUp()
        {
            _body.transform.DOScaleY(1.25f, 0.2f).SetEase(Ease.InOutQuad);
           
            _colider.size = new Vector2(0.7f, 1.25f);
            _colider.offset = new Vector2(0, 0.62f);
            _gunContainer.localPosition = new Vector3(0, 0.625f, 0);
        }
    }
}
