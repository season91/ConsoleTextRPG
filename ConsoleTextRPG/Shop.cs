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
                Console.Clear();
                Mathod.ChangeFontColor(ColorCode.Yellow);
                Console.WriteLine("상점");
                Mathod.ChangeFontColor(ColorCode.None);
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
                Console.WriteLine("\n[보유 골드]");
                Console.WriteLine($"{_player.gold} G");
                Console.WriteLine("\n[아이템 목록]");

                Item[] gameItem = {
                    new Item("수련자 갑옷", "수련에 도움을 주는 갑옷입니다.", 1000, "방어력", 5),
                    new Item("무쇠갑옷", "무쇠로 만들어져 튼튼한 갑옷입니다.", 1500, "방어력", 9),
                    new Item("스파르타의 갑옷", "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500, "방어력", 15),
                    new Item("낡은 검", "쉽게 볼 수 있는 낡은 검 입니다.", 600, "공격력", 2),
                    new Item("청동 도끼", "어디선가 사용됐던거 같은 도끼입니다.", 1500, "공격력", 5),
                    new Item("스파르타의 창", "쉽게 볼 수 있는 낡은 검 입니다.", 3500, "공격력", 7)
                };

                for (int i = 0; i < gameItem.Length; i++)
                {
                    var item = gameItem[i];
                    var isBuy = item.IsSameItem(_player, item) ? "구매 완료" : $"{item.gold} G";
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
                        ShopBuyItem(_player, gameItem);
                    }
                    else if (input == 2)
                    {
                        // 판매
                        ShopSaleItem(_player);
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        Thread.Sleep(1000);
                    }
                }
            }
        }

        // 아이템 구매 메뉴
        public static void ShopBuyItem(Job _player, Item[] gameItem)
        {
            int input = 0;
            while (true)
            {
                Console.Clear();
                Mathod.ChangeFontColor(ColorCode.Yellow);
                Console.WriteLine("상점 - 아이템 구매");
                Mathod.ChangeFontColor(ColorCode.None);
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
                Console.WriteLine("\n[보유 골드]");
                Console.WriteLine($"{_player.gold} G");
                Console.WriteLine("\n[아이템 목록]");

                for (int i = 0; i < gameItem.Length; i++)
                {
                    var item = gameItem[i];
                    var isBuy = item.IsSameItem(_player, item) ? "구매 완료" : $"{item.gold} G";
                    Console.WriteLine($"- {i+1}. {item.name} | {item.Ability()} | {item.itemInfo} | {isBuy}");
                }

                Console.WriteLine("0. 나가기");
                Console.WriteLine("\n원하시는 행동을 입력해주세요.");
                Console.Write(">>");

                if (Mathod.CheckInput(out input))
                {
                    if (input == 0)
                    {
                        break;
                    }

                    else if (input > 0 && input <= gameItem.Length)
                    {
                        TryShopBuyItem(_player, gameItem[input-1]);
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        Thread.Sleep(1000);
                    }
                }
            }
        }

        // 아이템 구매 시도
        public static void TryShopBuyItem(Job _player, Item item)
        {
            int price = item.gold;

            if (_player.item.Contains(item))
            {
                Console.WriteLine("이미 구매한 아이템입니다.");

            }
            else if (_player.gold >= price)
            {
                _player.gold -= price;
                _player.item.Add(item);
                Console.WriteLine($"구매를 완료했습니다! 남은 Gold : {_player.gold} G");
            }
            else
            {
                Console.WriteLine("Gold가 부족합니다.");
            }

            Thread.Sleep(1000);
        }



        // 아이템 판매 메뉴
        public static void ShopSaleItem(Job _player)
        {
            int input = 0;
            while (true)
            {
                Console.Clear();
                Mathod.ChangeFontColor(ColorCode.Yellow);
                Console.WriteLine("상점 - 아이템 판매");
                Mathod.ChangeFontColor(ColorCode.None);
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
                Console.WriteLine("\n[보유 골드]");
                Console.WriteLine($"{_player.gold} G");
                Console.WriteLine("\n[아이템 목록]");

                for (int i = 0; i < _player.item.Count; i++)
                {
                    var playerItem = _player.item[i];
                    Console.WriteLine($"- {i + 1}. {playerItem.name} | {playerItem.Ability()} | {playerItem.itemInfo}");
                }

                Console.WriteLine("0. 나가기");
                Console.WriteLine("\n원하시는 행동을 입력해주세요.");
                Console.Write(">>");

                if (Mathod.CheckInput(out input))
                {
                    if (input == 0)
                    {
                        break;
                    }

                    else if (input > 0 && input <= _player.item.Count)
                    {
                        TryShopSaleItem(_player, _player.item[input - 1]);
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        Thread.Sleep(1000);
                    }
                }
            }
        }

        // 아이템 판매 시도
        public static void TryShopSaleItem(Job _player, Item _playerItem)
        {
            int price = (int) (_playerItem.gold * 0.85);
            
            // 장착 중이라면 장착 해제
            if (_playerItem.equipped)
            {
                _playerItem.EquippedItem(!_playerItem.equipped);
            }

            _player.item.Remove(_playerItem);
            _player.gold += price;

            Console.WriteLine($"판매 완료! 판매 Gold : {price} -> 남은 Gold : {_player.gold} G");
            Thread.Sleep(2000);
        }
    }
}
