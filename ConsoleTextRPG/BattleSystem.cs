using System;
using GameCharacter;
using GameLogic;
using GameService;
using System.Linq;

namespace BattleSystem
{
    public enum Monsters { Minion, VoidInsect, CannonMinion }

    public class Monster
    {
        public Monsters MonsterType { get; set; }
        public int monMaxHp { get; set; }
        public int monHP { get; private set; }
        public int monAtk { get; set; }
        public int monLevel { get; set; }
        public bool IsAlive => monHP > 0; // IsAlive 불리언을 통해 생존 여부 확인

        public Monster(Monsters type)
        {
            MonsterType = type;
            // switch 식으로 타입별 기본 스탯 정의
            (monMaxHp, monAtk, monLevel) = type switch
            {
                Monsters.Minion => (15, 5, 2),
                Monsters.VoidInsect => (15, 9, 3),
                Monsters.CannonMinion => (25, 8, 5),

            };
            monHP = monMaxHp;
        }

        public int TakeDamage(int playerAtk)
        {
            Random random = new Random();
            // 0.9 ~ 1.1 사이의 랜덤 배율 생성
            double variation = random.NextDouble() * 0.2 + 0.9;
            int dmg = (int)Math.Round(playerAtk * variation);
            monHP = Math.Max(monHP - dmg, 0); // HP가 0보다 작아지지 않도록 조정
            return dmg;
        }
    }

    public static class BattleSystem
    {
        private static readonly Random _rand = new Random();

        // 전투 진입점
        public static void Start(Job player)
        {
            var monsters = SpawnMonsters();

            while (player.health > 0 && monsters.Any(m => m.IsAlive))
            {
                PlayerTurn(player, monsters);
                if (!monsters.Any(m => m.IsAlive)) break;
                EnemyTurn(player, monsters);
            }

            Mathod.BufferClear();
            Console.WriteLine(player.health > 0 ? " 승리! " : " 패배...");
            Console.ReadKey();
        }
        public static Monster[] SpawnMonsters()
        {
            int monsterCount = _rand.Next(1, 5); // 1~4마리 랜덤 생성
            Monsters[] monsterTypes = (Monsters[])Enum.GetValues(typeof(Monsters));
            Monster[] monsters = new Monster[monsterCount];
            for (int i = 0; i < monsterCount; i++)
            {
                Monsters randomType = monsterTypes[_rand.Next(monsterTypes.Length)];
                monsters[i] = new Monster(randomType);
            }
            return monsters;
        }
        public static void PlayerTurn(Job player, Monster[] monsters)
        {
            while (true)
            {
                RenderStatus(player, monsters);
                Console.Write("공격할 몬스터 번호(0: 차례 넘기기) > ");
                if (!Mathod.CheckInput(out int sel)) 
                    continue;
                if (sel == 0) 
                    break;
                if (sel < 1 || sel > monsters.Length || !monsters[sel - 1].IsAlive)
                {
                    Console.WriteLine("잘못된 입력입니다."); 
                    continue;
                }

                var target = monsters[sel - 1];
                int dmg = target.TakeDamage(player.atk);
                Console.WriteLine($"→ 몬스터 {sel}번에 {dmg} 데미지! (남은 HP {target.monHP}/{target.monMaxHp})");
                Console.ReadKey();
                break;  // 한 번 공격 후 적 차례로
            }
        }
        public static void EnemyTurn(Job player, Monster[] monsters)
        {
            Mathod.BufferClear();
            Console.WriteLine("적 턴");
            Console.WriteLine($"플레이어 체력: {player.health}\n");
            for (int i = 0; i < monsters.Length; i++)
            {
                var m = monsters[i];
                if (!m.IsAlive) 
                    continue; //죽은 몬스터는 건너뛰기
                else
                {
                    Console.WriteLine($"몬스터 {i + 1} {m.MonsterType}의 공격");
                    Console.WriteLine($"→ {m.monAtk} 데미지!");
                    player.health -= m.monAtk;
                    Console.WriteLine($"남은 체력: {player.health}");

                    Console.WriteLine("계속하시려면 아무 키나 누르세요.");
                    Console.ReadKey();

                    if (player.health <= 0)
                    {
                        Console.WriteLine("플레이어가 사망했습니다.");
                        Console.WriteLine("전투가 종료됩니다.");
                        Console.WriteLine("게임 오버");
                        Console.WriteLine("다시 시작하려면 아무 키나 누르세요.");
                        Console.ReadKey();
                        return; // 플레이어가 사망하면 전투 종료
                    }
                }
            }
        }
        public static void RenderStatus(Job player, Monster[] monsters)
        {
            Mathod.BufferClear();
            Console.WriteLine("플레이어 상태");
            Console.WriteLine($"체력: {player.health}");
            Console.WriteLine($"공격력: {player.atk}");
            for (int i = 0; i < monsters.Length; i++)
            {
                var m = monsters[i];
                if (m.IsAlive)
                {
                    Console.WriteLine($"몬스터 {i + 1} ({m.MonsterType})");
                    Console.WriteLine($"체력: {m.monHP}/{m.monMaxHp}");
                    Console.WriteLine($"공격력: {m.monAtk}");
                }
                else
                {
                    Console.WriteLine($"몬스터 {i + 1} ({m.MonsterType}) 사망");
                }
            }
        }
    }
}
