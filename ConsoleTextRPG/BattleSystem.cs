using System;
using GameCharacter;
using GameLogic;
using GameService;
using System.Linq;
using Manager;

namespace BattleSystem
{
    public enum Monsters 
    {
        고블린=1101,
        홉고블린=1102,
        오크=1103,
        하이오크=1104,
        해츨링=1105,
        와이번=1106, 
        워울프=1107, 
        만티코어=1108,
        드래곤=1109,
    }
    public class Monster
    {
        public Monsters MonsterType { get; set; }
        public int ID => (int)MonsterType;
        public string Name => MonsterType.ToString();
        public int monMaxHp { get; set; }
        public int monHP { get; private set; }
        public int monAtk { get; set; }
        public int monGold { get; set; }
        public bool IsAlive => monHP > 0;
        public Monster(Monsters type)
        {
            MonsterType = type;
            (monMaxHp, monAtk) = type switch
            {
                Monsters.고블린 => (15, 5),
                Monsters.홉고블린 => (20, 7),
                Monsters.오크 => (20, 8),
                Monsters.하이오크 => (25, 10),
                Monsters.해츨링 => (50, 20),
                Monsters.와이번 => (30, 13),
                Monsters.워울프 => (17, 17),
                Monsters.만티코어 => (25, 20),
                Monsters.드래곤 => (100, 40),
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
        public static void Start()
        {
            var player = GameManager.player;
            var monsters = SpawnMonsters(Dungeon.Floor);

            while (player.health > 0 && monsters.Any(m => m.IsAlive))
            {
                PlayerTurn(player, monsters);
                if (!monsters.Any(m => m.IsAlive)) break;
                EnemyTurn(player, monsters);
            }
            Console.Clear();
            if (player.health > 0)
            {
                Console.WriteLine("[전투 결과]\n");
                Console.WriteLine("전투에서 승리했습니다!");
                Console.WriteLine($"{Dungeon.Floor}층 던전에서 몬스터 {monsters.Length}마리를 처치했습니다\n");

                Console.WriteLine("[전투 보상]");
                //레벨업 시 레벨 증가 출력
                //int xpGain = monsters.Sum(m => m.monAtk);
                //player.exp += xpGain;
                //Console.WriteLine($"획득 경험치: {xpGain}");
                //Console.WriteLine("\n[획득 아이템]");
                //획득 아이템 출력
                //int goldGain = monsters.Sum(m => m.monGold);
                //player.gold += goldGain;
                //Console.WriteLine($"획득 골드: {goldGain}");

                Console.WriteLine("1. 다음층으로");
                Console.WriteLine("0. 던전 나가기");
                Console.Write("\n>> ");
                if (!Mathod.CheckInput(out int sel))
                    return;
                if (sel == 1)
                {
                    Dungeon.NextFloor();
                    Console.WriteLine("다음 층으로 이동합니다...");
                }
                else if (sel == 0)
                {
                    Console.WriteLine("던전을 나갑니다...");
                }
            }
            else
            {
                Console.WriteLine("[전투 결과]\n");
                Console.WriteLine("전투에서 패배했습니다.");
                Console.WriteLine("게임 오버");
                Console.ReadKey();
            }
        }
        public static Monster[] SpawnMonsters(int Floor)
        {
            int monsterCount;
            if (Floor == 5 || Floor == 10)
                monsterCount = 1; // 보스 몬스터
            else
                monsterCount = _rand.Next(1, 5);

            Monsters[] types;
            if (Floor >= 1 && Floor <= 4)
                types = new[] { Monsters.고블린, Monsters.홉고블린, Monsters.오크, Monsters.하이오크 };
            else if (Floor == 5)
                types = new[] { Monsters.해츨링 };
            else if (Floor >= 6 && Floor <= 9)
                types = new[] { Monsters.와이번, Monsters.워울프, Monsters.만티코어 };
            else if (Floor == 10)
                types = new[] { Monsters.드래곤 };
            else
                types = new[] { Monsters.고블린 };

                var monsters = new Monster[monsterCount];
            for (int i = 0; i < monsterCount; i++)
            {
                monsters[i] = new Monster(types[_rand.Next(types.Length)]);
            }
            return monsters;
        }
        public static void PlayerTurn(Job player, Monster[] monsters)
        {
            while (true)
            {
                RenderStatus(player, monsters);
                Console.Write("\n\n공격할 몬스터 번호(0: 차례 넘기기) > ");
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

                // 몬스터 죽인 횟수 카운트
                //if (target.monHP == 0)
                //{
                // 
                //}

                Console.ReadKey();
                break;  // 한 번 공격 후 적 차례
            }
        }
        public static void EnemyTurn(Job player, Monster[] monsters)
        {
            Console.Clear();
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
                        return; // 플레이어가 사망하면 전투 종료
                }
            }
        }
        public static void RenderStatus(Job player, Monster[] monsters)
        {
            Console.Clear();
            Console.WriteLine("플레이어 턴\n");
            Console.WriteLine($"체력: {player.health}");
            Console.WriteLine($"공격력: {player.atk}\n\n");
            for (int i = 0; i < monsters.Length; i++)
            {
                var m = monsters[i];
                if (m.IsAlive)
                {
                    Console.WriteLine($"{i + 1} ({m.MonsterType})");
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
    public static class Dungeon
    {
        public static int Floor { get; private set; } = 1;
        public static void NextFloor() => Floor++;
    }
}
