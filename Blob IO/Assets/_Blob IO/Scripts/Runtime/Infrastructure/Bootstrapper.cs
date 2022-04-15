using BlobIO.Services;
using UnityEngine;

namespace BlobIO.Infrastructure
{
    public class Bootstrapper : MonoBehaviour, ICoroutineRunner
    {
        private Game _game;

        private void Start()
        {
            _game = new Game(new AllServices(), this);
            _game.Run();
            
            DontDestroyOnLoad(this);
        }
    }
}