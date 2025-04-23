using GameLogic;
using GameService;
using Manager;

namespace Shop
{
    public class Shops
    {
        public static void Show()
        {
            var _player = GameManager.player;
            int input = 0;
            Item[] gameItem = GameManager.ItemPooling;
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
                        ShopBuyItem();
                    }
                    else if (input == 2)
                    {
                        // 판매
                        ShopSaleItem();
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
        public static void ShopBuyItem()
        {
            var _player = GameManager.player;
            int input = 0;
            Item[] gameItem = GameManager.ItemPooling;
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
                    Console.WriteLine($"- {i + 1}. {item.name} | {item.Ability()} | {item.itemInfo} | {isBuy}");
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

                    else if (input > 0 && input <= gameItem.Length)
                    {
                        TryShopBuyItem(gameItem[input - 1]);
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
        public static void TryShopBuyItem(GameLogic.Item item)
        {
            int price = item.gold;
            var _player = GameManager.player;

            if(item.itemId != (int)ItemCode.Potion)
            {
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
            } 
            else
            {
                if (_player.gold >= price)
                {
                    _player.gold -= price;
                    Mathod.PotionItemPlus();
                    Console.WriteLine($"구매를 완료했습니다! 남은 Gold : {_player.gold} G");
                }
                else
                {
                    Console.WriteLine("Gold가 부족합니다.");
                }
            }

            Thread.Sleep(1000);
        }


        // 아이템 판매 메뉴
        public static void ShopSaleItem()
        {
            var _player = GameManager.player;
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

                Console.WriteLine("\n0. 나가기");
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
                        TryShopSaleItem(_player.item[input - 1]);
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
        public static void TryShopSaleItem(GameLogic.Item _playerItem)
        {
            var _player = GameManager.player;
            int price = (int)(_playerItem.gold * 0.85);

            if (_playerItem.itemId != (int)ItemCode.Potion)
            {
                // 장착 중이라면 장착 해제
                if (_playerItem.equipped)
                {
                    _playerItem.EquippedItem(!_playerItem.equipped);
                }

                _player.item.Remove(_playerItem);
            } 
            else
            {
                Mathod.PotionItemMinus();
            }
            
            _player.gold += price;

            Console.WriteLine($"판매 완료! 판매 Gold : {price} -> 남은 Gold : {_player.gold} G");
            Thread.Sleep(2000);
        }
    }
}