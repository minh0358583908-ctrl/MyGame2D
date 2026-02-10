using System;
using System.Collections.Generic;
using BaseGame;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runner
{
    [Serializable]
    public class GameplayAsset
    {
        public CharacterBaseCtrl characterPrefabs;
        public List<ObstacleBaseCtrl> obstaclePrefabs;
        public List<Stack<ObstacleBaseCtrl>> obstaclePools;
        public Transform gameplayArea, poolArea;

        public void Init()
        {
            obstaclePools ??= new List<Stack<ObstacleBaseCtrl>>();
            foreach (var _ in obstaclePrefabs)
                obstaclePools.Add(new Stack<ObstacleBaseCtrl>());
        }

        // cách khởi tạo và xóa đối tượng mới đơn giản
        public CharacterBaseCtrl RespawnCharacter()
        {
            return GameObject.Instantiate(characterPrefabs, gameplayArea);
        }
        public void DespawnCharacter(CharacterBaseCtrl character)
        {
            GameObject.Destroy(character.gameObject);
        }

        // cách khởi tạo và xóa đối tượng mới tối ưu
        public ObstacleBaseCtrl RespawnObstacleRandom()
        {
            var randObstacleIndex = Random.Range(0, obstaclePrefabs.Count);
            var obstaclePrefab = obstaclePrefabs[randObstacleIndex];
            var obstaclePool = obstaclePools[randObstacleIndex];
            var obstacle = obstaclePool.Count > 0 ? obstaclePool.Pop()
                : GameObject.Instantiate(obstaclePrefab, gameplayArea);
            obstacle.gameObject.SetActive(true);
            obstacle.transform.SetParent(gameplayArea);
            return obstacle;
        }
        public void DespawnObstacle(ObstacleBaseCtrl obstacle)
        {
            obstacle.gameObject.SetActive(false);
            obstacle.transform.SetParent(poolArea);
            obstaclePools[obstacle.index].Push(obstacle);
        }
    }
    [Serializable]
    public class GameplayConfig
    {
        public float moveSpeedMin = 3f;
        public float moveSpeedModify = 0.1f; // 1s tăng bao nhiêu
        public float moveSpeedMax = 10f;
        public float spawnObstacleCappingTime;
        public Transform mainCharPos;
        public Transform obstacleSpawnPos;
        public Transform obstacleDespawnPos;
    }
    [Serializable]
    public class GameplayRuntime
    {
        public List<CharacterBaseCtrl> mainCharacters;
        public List<ObstacleBaseCtrl> listObstacleActive;
        public bool gameplayRunning = false;
        public float timeSpawnObstacle;
        public int maxCharHp, curCharHp;
    }

    public enum GameplayMode { One, Two, Three }

    public class GameplayController : SingletonX<GameplayController>
    {
        public GameplayAsset asset;
        public GameplayConfig config;
        public GameplayRuntime runtime;

        public static void Init()
        {
            Ins.asset.Init();
        }

        public static void StartGame(GameplayMode mode = GameplayMode.One)
        {
            Ins.ClearGame();
            Ins.runtime.gameplayRunning = true;
        }

        private void ClearGame()
        {
            runtime.listObstacleActive ??= new List<ObstacleBaseCtrl>();
            foreach (var obstacle in runtime.listObstacleActive)
                asset.DespawnObstacle(obstacle);
            runtime.listObstacleActive.Clear();
        }

        private void Update()
        {
            if (!Ins.runtime.gameplayRunning || PopupController.HasPopupActive()) return;

            UpdateMoveBackground();
            UpdateMoveObstacle();
        }

        private void UpdateMoveBackground()
        {
        }

        private void UpdateMoveObstacle()
        {
            for (var i = 0; i < runtime.listObstacleActive.Count;)
            {
                var obstacle = runtime.listObstacleActive[i];
                if (obstacle.UpdateMove(config.obstacleDespawnPos.position.x))
                {
                    asset.DespawnObstacle(obstacle);
                    runtime.listObstacleActive.RemoveAt(i);
                }
                else i++;
            }
        }

        public static void HandleCharacterCollide()
        {
            // Ins.runtime
        }
    }
}