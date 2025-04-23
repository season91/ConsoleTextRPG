using GameLogic;
using GameService;
using Manager;

namespace Recovery
{
    public class RecoveryItem
    {

        public static void Show()
        {
            var _player = GameManager.player;
            int input = 0;
            Item HPotion = (from item in GameManager.ItemPooling
                            where item.itemId == "32001"
                            select item).First();
            while (true)
            {
                Console.Clear();
                Mathod.ChangeFontColor(ColorCode.Yellow);
                Console.WriteLine("회복");
                Mathod.ChangeFontColor(ColorCode.None);
                Console.WriteLine($"포션을 사용하면 체력을 30 회복 할 수 있습니다. (남은 포션 : {HPotion.count} )");
                Console.WriteLine("\n1. 사용하기");
                Console.WriteLine("0. 나가기");
                Console.WriteLine("\n원하시는 행동을 입력해주세요.");
                Console.Write(">>");

                if (Mathod.CheckInput(out input))
                {
                    if (input == 0)
                    {
                        break;
                    }
                    else if (input == 1)
                    {
                        TryUseRecoveryItem();
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        Thread.Sleep(1000);
                    }
                }
            }
        }

        // 회복 아이템 사용
        public static void TryUseRecoveryItem()
        {
            var playerHealth = GameManager.player.health;
            
            if(playerHealth < 100)
            {
                if (playerHealth > 70)
                {
                    playerHealth = 100;
                }
                else
                {
                    playerHealth += 30;
                }
                Console.WriteLine($"체력을 30 회복 했습니다. 현재 체력 {playerHealth}");
            } else
            {
                Console.WriteLine("체력이 이미 100 입니다.");
            }
            Thread.Sleep(1000);
        }
    }
}
