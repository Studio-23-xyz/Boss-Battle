using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Studio23.SS2
{
    public interface ISavableSettings
    { 
        /// <summary>
        ///     Must Return an Unique ID for save system to save file with this name
        /// </summary>
        public string GetUniqueID();

        /// <summary>
        ///     Return a serialized string for the save system to save
        /// </summary>
        /// <returns>String Data</returns>
        public UniTask<string> GetSerializedData();

        /// <summary>
        ///     Implement how your component will adjust on data load
        /// </summary>
        /// <param name="data">String data</param>
        public UniTask AssignSerializedData(string data);

        /// <summary>
        /// This should never happen. If anything comes here, You probably need to check the flow of loading.
        /// Override to manage this in edge case scenario. Strongly discouraged.
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public UniTask ManageSaveLoadException(Exception exception)
        {
            Debug.Log($"{exception.Message}");
            return UniTask.CompletedTask;
        }
    }
}
