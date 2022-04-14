using BlobIO.Services;
using UnityEngine;

namespace BlobIO.Infrastructure
{
    public class Bootstrapper : MonoBehaviour
    {
        private Game _game;

        private void Start()
        {
            _game = new Game(new AllServices());
            _game.Run();
            
            DontDestroyOnLoad(this);
        }
    }
}