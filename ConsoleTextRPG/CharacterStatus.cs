using Manager;

namespace Status
{

    public static class StatusScene
    {
        public static void ShowStatus()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("[상태 보기]");
                Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");
                Console.WriteLine($"Lv. {GameManager.player.level}");
                Console.WriteLine($"{GameManager.player.name}({GameManager.player.chad})");
                Console.WriteLine($"공격력 : {GameManager.player.atk}" + (GameManager.player.bonusAtk > 0 ? $" (+{GameManager.player.bonusAtk})" : ""));
                Console.WriteLine($"방어력 : {GameManager.player.def}" + (GameManager.player.bonusDef > 0 ? $" (+{GameManager.player.bonusDef})" : ""));
                Console.WriteLine($"체  력 : {GameManager.player.health}");
                Console.WriteLine($"마  나 : {GameManager.player.Mp}");
                Console.WriteLine($"경험치 : {GameManager.player.exp}");
                Console.WriteLine($"Gold : {GameManager.player.gold} G");
                Console.WriteLine("0. 나가기");
                Console.WriteLine("\n원하시는 행동을 입력해주세요.");
                Console.Write(">>");

                string stat_choice = Console.ReadLine();

                if (stat_choice == "0")
                {
                    Console.Clear();
                    break;
                }

                else
                {
                    Console.Write("잘못된 입력입니다.");
                    Thread.Sleep(1000);
                }
            }
        }
    }

}
