using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogic;

namespace Status
{
    
        public static class StatusScene
        {
            public static void ShowStatus(Job player)
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("[상태 보기]");
                    Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");
                    Console.WriteLine($"Lv. {player.level}");
                    Console.WriteLine($"{player.name}({player.chad})");
                    Console.WriteLine($"공격력 : {player.atk}");
                    Console.WriteLine($"방어력 : {player.def}");
                    Console.WriteLine($"체  력 : {player.health}");
                    Console.WriteLine($"Gold : {player.gold} G");
                    Console.WriteLine("0. 나가기");
                    Console.WriteLine("\n원하시는 행동을 입력해주세요.");
                    Console.WriteLine(">>");

                    string stat_choice = Console.ReadLine();
                    if (stat_choice == "0")
                    {
                        Console.Clear();
                        break;
                    }
                }
            }
        }
    
}
