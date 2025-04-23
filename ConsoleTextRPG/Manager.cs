using BattleSystem;
using System.Drawing;
using GameLogic;
using GameQuest;
using GameService;

namespace Manager
{
    public static class GameManager
    {
        public static readonly Random rd = new Random();

        public static Item[] ItemPooling { get; set; }
        public static Job player { get; private set; }
        public static QuestManager quest { get; private set; } = new QuestManager();
        public static GameData data { get; private set; } = new GameData();

        public static void SpawnPlayer()
        {
            data.Load();

            var chadName = data.stringMap.GetData("Chad");
            int jobID = Mathod.JobToIndex(chadName);

            player = Mathod.JobToClass(jobID);
            player.LoadData();
            quest.Load(true);
        }

        public static void SpawnPlayer(string _playerName, Job _playerJob)
        {
            player = _playerJob;
            player.SetName(_playerName);
            quest.Load(false);
            
            Item potionItem = (from item in ItemPooling
                            where item.itemId == (int)ItemCode.Potion
                            select item).First();

            potionItem.count = 3;
            player.item.Add(potionItem);
        }
        
        public static int DungeonFloor { get; set; } = 1;
    }
}
