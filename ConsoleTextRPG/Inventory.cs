using System.Numerics;
using GameLogic;
using GameService;

namespace Inventory
{
    public class Inventorys
    {
        public static void Show(Job _player)
        {
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
                        EquippedItemManage(_player);
                    }

                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        Thread.Sleep(1000);
                    }
                }
            }
        }
        public static void EquippedItemManage(Job _player)
        {
            int input = 0;
            while (true)
            {
                Console.Clear();
                Mathod.ChangeFontColor(ColorCode.Yellow);
                Console.WriteLine("인벤토리 - 장착관리");
                Mathod.ChangeFontColor(ColorCode.None);
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
                Console.WriteLine("\n[아이템 목록]");

                for (int i = 0; i < _player.item.Count; i++)
                {
                    var item = _player.item[i];
                    Console.WriteLine($"- {i + 1}. {(item.equipped ? "[E] " : "")} {item.name} | {item.Ability()} | {item.itemInfo}");
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
                    else if (input >= 1 && input <= _player.item.Count)
                    {
                        // 
                        if(_player.item[input - 1].atk > 0)
                        {

                        }

                        _player.item[input-1].EquippedItem(!_player.item[input-1].equipped);
                        break;
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
