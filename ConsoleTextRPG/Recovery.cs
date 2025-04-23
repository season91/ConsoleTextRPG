using GameLogic;
using GameService;
using Manager;

namespace Recovery
{
    public class RecoveryItem
    {
        public static void Show()
        {
            int input = 0;
            while (true)
            {
                Item potion = (from item in GameManager.player.item
                               where item.itemId == (int)ItemCode.Potion
                               select item).FirstOrDefault();

                int potionCount = 0;
                if (potion != null)
                {
                    potionCount = potion.count;
                }

                Console.Clear();
                Mathod.ChangeFontColor(ColorCode.Yellow);
                Console.WriteLine("회복");
                Mathod.ChangeFontColor(ColorCode.None);
                Console.WriteLine($"포션을 사용하면 체력을 30 회복 할 수 있습니다. (남은 포션 : {potionCount} )");
                Console.WriteLine($"현재 체력 : {GameManager.player.health}");
                if (potionCount > 0)
                {
                    Console.WriteLine("\n1. 사용하기");
                }
                
                Console.WriteLine("\n0. 나가기");
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
            var player = GameManager.player;
            
            if(player.health < 100)
            {
                if (player.health > 70)
                {
                    player.health = 100;
                }
                else
                {
                    player.health += 30;
                }
                Item potionItem = (from item in GameManager.player.item
                                   where item.itemId == (int)ItemCode.Potion
                                   select item).FirstOrDefault();

                Mathod.PotionItemMinus();
                Console.WriteLine($"체력을 30 회복 했습니다. 현재 체력 {player.health}");
            } else
            {
                Console.WriteLine("체력이 이미 100 입니다.");
            }
            Thread.Sleep(1000);
        }
    }
}
