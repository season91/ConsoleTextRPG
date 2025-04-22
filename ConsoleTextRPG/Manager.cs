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

            string playerName = data.stringMap.GetData("PlayerName");
            int jobID = Mathod.JobToIndex(playerName);

            player = Mathod.JobToClass(jobID);
            player.LoadData();
        }

        public static void SpawnPlayer(string _playerName, Job _playerJob)
        {
            player.SetName(_playerName);
            player = _playerJob;
        }
    }
}
