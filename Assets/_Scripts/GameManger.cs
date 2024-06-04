using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _Scripts
{
    public class GameManger : Singleton<GameManger>
    {
        private AsyncOperationHandle<Level> level;
        private string levelKey = "level1";
        private GameObject oldRoom;
    
        [SerializeField] private bool _readyToPlay;
        [SerializeField] private bool _trashRemoved;
        [SerializeField] private bool _interiorRemoved;
        [SerializeField] private bool _floorReplaced;
        [SerializeField] private bool _wallPainted;
    
        public override void Awake()
        {
            base.Awake();
        
            Init();
        }

        private void Start()
        {
            OldRoomSpawn();
        }

        private void Update()
        {
            if (!_readyToPlay)
            {
                OldRoomSpawn();
                return;
            }
        }

        private void Init()
        {
            _readyToPlay = false;
            _trashRemoved = false;
            _interiorRemoved = false;
            _floorReplaced = false;
            _wallPainted = false;

            level = Addressables.LoadAssetAsync<Level>(levelKey);
        }

        public void TrashRemoved()
        {
            _trashRemoved = true;
        }

        public void InteriorRemoved()
        {
            _interiorRemoved = true;
        }

        public void FloorReplaced()
        {
            _floorReplaced = true;
        }

        public void WallPainted()
        {
            _wallPainted = true;
        }

        private void OldRoomSpawn()
        {
            if (level.Status == AsyncOperationStatus.Succeeded)
            {
                oldRoom = Instantiate(level.Result.oldRoom);
                oldRoom.name = "Old Room";

                _readyToPlay = true;
            }
        }
    }
}
