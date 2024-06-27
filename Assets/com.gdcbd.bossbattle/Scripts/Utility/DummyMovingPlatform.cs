using DG.Tweening;
using UnityEngine;

namespace com.gdcbd.bossbattle.utility
{
    public class DummyMovingPlatform: MonoBehaviour
    {
        public float duration = 1f; // Duration of each movement transition
        public float leftBound = -2f;
        public float rightBound = 2;

        private Vector3 _startPosition;
        void Start()
        {
            // Start the continuous movement loop
            MoveToLeft();
            _startPosition = transform.position;
        }

        void MoveToLeft()
        {
            transform.DOLocalMoveX(_startPosition.x+leftBound, duration).SetEase(Ease.InOutQuad).OnComplete(MoveToRight);
        }

        void MoveToRight()
        {
            transform.DOLocalMoveX(_startPosition.x+rightBound, duration).SetEase(Ease.InOutQuad).OnComplete(MoveToLeft);
        }
    }
}