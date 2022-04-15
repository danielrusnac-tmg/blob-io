using System.Collections;
using BlobIO.Services;
using UnityEngine;

namespace BlobIO.Infrastructure
{
    public interface ICoroutineRunner : IService
    {
        Coroutine StartCoroutine(IEnumerator enumerator);
        void StopCoroutine(Coroutine coroutine);
    }
}