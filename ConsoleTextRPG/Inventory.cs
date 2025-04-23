using GameLogic;
using GameService;
using Manager;

namespace Inventory
{
    public class Inventorys
    {
        public static void Show()
        {
            var _player = GameManager.player;
            int input = 0;
            while (true)
            {
                Console.Clear();
                Mathod.ChangeFontColor(ColorCode.Yellow);
                Console.WriteLine("인벤토리");
                Mathod.ChangeFontColor(ColorCode.None);
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
                Console.WriteLine("\n[아이템 목록]");

                if(_player.item.Count <= 0)
                {
                    Console.WriteLine("\n보유 중인 아이템이 없습니다.");
                } else
                {
                    for (int i = 0; i < _player.item.Count; i++)
                    {
                        var item = _player.item[i];
                        Console.WriteLine($"- {(item.equipped ? "[E] " : "")}{item.name} | {item.Ability()} | {item.itemInfo}");
                    }
                }

                Console.WriteLine("\n1. 장착 관리");
                Console.WriteLine("0. 나가기");
                Console.WriteLine("\n원하시는 행동을 입력해주세요.");
                Console.Write(">>");

                if (Mathod.CheckInput(out input))
                {
                    if (input == 0)
                    {
                        break;
                    }
                    else if (input == 1 && _player.item.Count > 0)
                    {
                        EquippedItemManage();
                    }

                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        Thread.Sleep(1000);
                    }
                }
            }
        }
        public static void EquippedItemManage()
        {
            //List<Item> playerItem = GameManager.player.item.OfType<Item>().Where(x => x.itemId != (int)ItemCode.Potion).ToList();
            var playerItem = GameManager.player.item;
            int input = 0;
            while (true)
            {
                Console.Clear();
                Mathod.ChangeFontColor(ColorCode.Yellow);
                Console.WriteLine("인벤토리 - 장착관리");
                Mathod.ChangeFontColor(ColorCode.None);
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
                Console.WriteLine("\n[아이템 목록]");

                for (int i = 0; i < playerItem.Count; i++)
                {
                    var item = playerItem[i];
                    Console.WriteLine($"- {i + 1}. {(item.equipped ? "[E] " : "")}{item.name} | {item.Ability()} | {item.itemInfo}");
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
                    else if (input >= 1 && input <= playerItem.Count)
                    {
                        TryEquipItem(input-1);
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        Thread.Sleep(1000);
                    }
                }
            }
        }

        public static void TryEquipItem(int tryEquipIndex)
        {
            var _player = GameManager.player;
            var tryEquipItem = _player.item[tryEquipIndex];

            // 장착 시도 아이템이 포션이 아닌 경우만 시도
            if(tryEquipItem.itemId != (int)ItemCode.Potion)
            {
                if (tryEquipItem.atk > 0)
                {
                    // 장착된 무기가 있는지
                    var equippedAtkItem = (from x in _player.item.Select((item, index) => new { item, index })
                                           where x.item.equipped && x.item.atk > 0 && x.item.def == 0
                                           select (x.index, x.item)).FirstOrDefault();

                    // 기존 장착 해제 후 장착
                    if (equippedAtkItem.item != null)
                    {
                        _player.atk -= _player.item[equippedAtkItem.index].atk;
                        _player.bonusAtk -= _player.item[equippedAtkItem.index].atk;

                        _player.item[equippedAtkItem.index].EquippedItem(!_player.item[equippedAtkItem.index].equipped);
                    }

                    _player.atk += tryEquipItem.atk;
                    _player.bonusAtk += tryEquipItem.atk;

                    tryEquipItem.EquippedItem(!tryEquipItem.equipped);
                }
                else if (tryEquipItem.def > 0)
                {
                    // 장착된 방어구가 있는지
                    var equippedDefItem = (from x in _player.item.Select((item, index) => new { item, index })
                                           where x.item.equipped && x.item.def > 0 && x.item.atk == 0
                                           select (x.index, x.item)).FirstOrDefault();

                    // 기존 장착 해제 후 장착
                    if (equippedDefItem.item != null)
                    {
                        _player.def -= _player.item[equippedDefItem.index].def;
                        _player.bonusDef -= _player.item[equippedDefItem.index].def;

                        _player.item[equippedDefItem.index].EquippedItem(!_player.item[equippedDefItem.index].equipped);
                    }

                    _player.def += tryEquipItem.def;
                    _player.bonusDef += tryEquipItem.def;

                    tryEquipItem.EquippedItem(!tryEquipItem.equipped);
                }
            }
            else
            {
                Console.WriteLine("장착 가능한 아이템이 아닙니다.");
            }
            Thread.Sleep(1000);
        }
    }
}
