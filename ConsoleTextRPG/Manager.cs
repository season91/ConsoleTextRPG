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

        public static int SelectMonster(Monster[] monsters)
        {
            while (true)
            {
                Console.WriteLine("공격할 몬스터를 선택하세요.");
                for (int i = 0; i < monsters.Length; i++)
                {
                    var m = monsters[i];
                    string status = m.IsAlive ? $"(남은 HP {m.monHP}/{m.monMaxHp})" : "(사망)";

                    Mathod.ChangeFontColor(m.IsAlive ? ColorCode.Red : ColorCode.DarkGray);
                    Console.WriteLine($"{i + 1}. {m.Name} {status}");
                }
                Mathod.ChangeFontColor(ColorCode.None);
                Console.Write(">> ");

                if (Mathod.CheckInput(out int input))
                {
                    if (input > 0 && input <= monsters.Length && monsters[input - 1].IsAlive)
                    {
                        return input - 1;
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        Thread.Sleep(1000);
                        Console.Clear();
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Thread.Sleep(1000);
                    Console.Clear();
                }
            }
        }

        public static bool NextFloor(Monster[] monsters)
        {
            if (monsters.Any(m => m.IsAlive))
                return false;

            Console.Clear();
            DungeonFloor++;

            Console.WriteLine("모든 몬스터를 처치했습니다!\n");
            Console.WriteLine("1. 다음층으로 이동");
            Console.WriteLine("0. 던전 나가기");
            Console.Write("\n>> ");

            if (Mathod.CheckInput(out int sel))
            if (sel == 1)
            {
                Console.WriteLine($"\n던전 {DungeonFloor}층으로 이동합니다...");
                Thread.Sleep(1000);
                Console.Clear();
                BattleSystems.Start();
            }
            else if (sel == 0)
            {
                Console.WriteLine("\n던전을 나갑니다...");
                Thread.Sleep(1000);
                Console.Clear();
            }
            else
            {  Console.WriteLine("잘못된 입력입니다."); }

                return true;
        }
    }
}
