using Cysharp.Threading.Tasks;
using Studio23.SS2.SceneLoadingSystem.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Studio23.SS2.Utility
{
    public class SceneTransitionController : MonoBehaviour
    {
       public List<SceneOperationAction> LoadActions;

        public async void DoSceneOperation()
        {
            await SceneOperation();
        }

        public async UniTask SceneOperation()
        {
            foreach (var loadAction in LoadActions)
            {
                await loadAction.DoSceneOperation();
            }
        }
    }
}
