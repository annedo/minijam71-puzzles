using UnityEngine;

namespace Assets.Scripts
{
    public class RandomIconHelper : MonoBehaviour
    {
        private readonly System.Random _random = new System.Random();
        private GameObject _cornTileRef;
        private GameObject _chickenTileRef;
        private GameObject _tireTileRef;
        private GameObject _hayTileRef;

        public void Start()
        {
            _cornTileRef = (GameObject)Instantiate(Resources.Load("corn_obj"));
            _chickenTileRef = (GameObject)Instantiate(Resources.Load("chicken_obj"));
            _tireTileRef = (GameObject)Instantiate(Resources.Load("tire_obj"));
            _hayTileRef = (GameObject)Instantiate(Resources.Load("hay_obj"));

            // Move prefabs off screen
            _cornTileRef.transform.position = new Vector2(-100, -100);
            _chickenTileRef.transform.position = new Vector2(-100, -100);
            _tireTileRef.transform.position = new Vector2(-100, -100);
            _hayTileRef.transform.position = new Vector2(-100, -100);
        }

        public GameObject GetRandomTile()
        {
            var currentBarnTier = TierTracker.CurrentTier[TierTracker.TierTypes.Barn];

            int spawnType;
            if (currentBarnTier == 2)
                spawnType = _random.Next(1, 6); // 1,2,3,4,5
            else if (currentBarnTier == 3)
                spawnType = _random.Next(1, 7); // 1,2,3,4,5,6
            else
                spawnType = _random.Next(1, 5); // 1,2,3,4

            switch (spawnType)
            {
                case 1:                
                    return Instantiate(_cornTileRef, transform);
                case 2:
                case 5:
                case 6:
                    return Instantiate(_chickenTileRef, transform);
                case 3:
                    return Instantiate(_hayTileRef, transform);
                case 4:
                    return Instantiate(_tireTileRef, transform);
                default:
                    return null;
            }
        }
    }
}