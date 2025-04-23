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
        }
        
        public static int DungeonFloor { get; set; } = 1;
        public static void NextFloor()
        {
            DungeonFloor++;
            if (DungeonFloor == 10)
            {
                Console.WriteLine("던전 클리어!");
                Console.WriteLine("축하합니다!");
                Console.WriteLine("무한모드로 진입합니다.");
                Console.ReadKey();
            }
            else if (DungeonFloor > 10)
            {
                Console.WriteLine("무한모드입니다.");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine($"던전 {DungeonFloor}층으로 이동합니다...");
                Thread.Sleep(1000);
                Console.Clear();
                BattleSystems.Start();
            }
        }
    }
}
