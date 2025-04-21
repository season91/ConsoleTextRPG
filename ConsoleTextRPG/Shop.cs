using GameLogic;
using GameService;

namespace Shop
{
    public class Shops
    {
        public static void Show(Job _player)
        {
            int input = 0;

            while (true)
            {
                Mathod.ChangeFontColor(ColorCode.Yellow);
                Console.WriteLine("상점");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
                Console.WriteLine("]n[보유 골드]");
                Console.WriteLine($"{_player.gold} G");
                Console.WriteLine("\n[아이템 목록]");

                Item[] gameItem = {
                //방어구
                new Item("수련자 갑옷", "수련에 도움을 주는 갑옷입니다.", 1000, "방어력", 5),
                new Item("무쇠갑옷", "무쇠로 만들어져 튼튼한 갑옷입니다.", 1500, "방어력", 9),
                new Item("스파르타의 갑옷", "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500, "방어력", 15),

                //무기
                new Item("낡은 검", "쉽게 볼 수 있는 낡은 검 입니다.", 600, "공격력", 2),
                new Item("청동 도끼", "어디선가 사용됐던거 같은 도끼입니다.", 1500, "공격력", 5),
                new Item("스파르타의 창", "쉽게 볼 수 있는 낡은 검 입니다.", 3500, "공격력", 7)
                };

                for (int i = 0; i < gameItem.Length; i++)
                {
                    var item = gameItem[i];
                    var isBuy = item.IsSameItem(_player, item) ? "구매 완료\n" : $"{item.gold} G\n";
                    Console.WriteLine($"- {item.name} | {item.Ability()} | {item.itemInfo} | {isBuy}");
                }

                Console.WriteLine("\n1. 아이템 구매");
                Console.WriteLine("2. 아이템 판매");
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
                        // 구매
                    }
                    else if (input == 2)
                    {
                        // 판매

                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        Thread.Sleep(1000);
                    }
                }
            }
        }
    }
}
