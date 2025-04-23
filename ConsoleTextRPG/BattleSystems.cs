using System.Security.Cryptography.X509Certificates;
using GameCharacter;
using GameLogic;
using GameService;
using Manager;

namespace BattleSystem
{
    public enum Monsters
    {
        고블린 = 1101,
        홉고블린 = 1102,
        오크 = 1103,
        하이오크 = 1104,
        해츨링 = 1105,
        와이번 = 1106,
        워울프 = 1107,
        만티코어 = 1108,
        드래곤 = 1109,
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

        public int monLevel { get; set; }
        public bool IsAlive => monHP > 0;
        public Monster(Monsters type)
        {
            MonsterType = type;
            (monMaxHp, monAtk, monGold, monLevel) = type switch
            {
                Monsters.고블린 => (15, 5, 75, 2),
                Monsters.홉고블린 => (20, 7, 140, 4),
                Monsters.오크 => (20, 8, 160, 6),
                Monsters.하이오크 => (25, 10, 250, 8),
                Monsters.해츨링 => (50, 20, 1000, 30),
                Monsters.와이번 => (30, 13, 300, 12),
                Monsters.워울프 => (17, 17, 350, 14),
                Monsters.만티코어 => (25, 20, 500, 16),
                Monsters.드래곤 => (100, 40, 5000, 100),
            };
            monHP = monMaxHp;
        }

        public int TakeDamage(int playerAtk, Job job)
        {
            Random random = new Random();
            // 0.9 ~ 1.1 사이의 랜덤 배율 생성
            double variation = random.NextDouble() * 0.2 + 0.9;
            int dmg = (int)Math.Round(playerAtk * variation);
            monHP = Math.Max(monHP - dmg, 0); // HP가 0보다 작아지지 않도록 조정
            if (monHP <= 0)
            {
                GameManager.quest.CheckCondition(Name);
            }
            return dmg;
        }
    }

    public class BattleSystems
    {
        // 전투 진입점
        public static void Start()
        {
            var player = GameManager.player;
            var monsters = SpawnMonsters(GameManager.DungeonFloor);

            while (player.health > 0 && monsters.Any(m => m.IsAlive))
            {
                if (!PlayerTurn(player, monsters))
                    return;
                if (!monsters.Any(m => m.IsAlive)) 
                    break;
                EnemyTurn(player, monsters);
            }
            Console.Clear();
            if (player.health > 0)
            {
                Console.WriteLine("[전투 결과]\n");
                Console.WriteLine("전투에서 승리했습니다!");
                Console.WriteLine($"{GameManager.DungeonFloor}층 던전에서 몬스터 {monsters.Length}마리를 처치했습니다\n");

                Console.WriteLine("[전투 보상]");
                //레벨업 시 레벨 증가 출력
                int xpGain = monsters.Sum(m => m.monLevel);
                player.exp += xpGain;
                Console.WriteLine($"획득 경험치: {xpGain}");
                //Console.WriteLine("\n[획득 아이템]");
                //획득 아이템 출력
                int goldGain = monsters.Sum(m => m.monGold);
                player.gold += goldGain;
                Console.WriteLine($"획득 골드: {goldGain}");
                Console.WriteLine($"현재 골드: {player.gold}");

                Console.WriteLine("1. 다음층으로");
                Console.WriteLine("0. 던전 나가기");
                Console.Write("\n>> ");
                if (!Mathod.CheckInput(out int sel))
                    return;
                if (sel == 1)
                {
                    GameManager.DungeonFloor++;
                    if (GameManager.DungeonFloor == 10)
                    {
                        Console.WriteLine("던전 클리어!");
                        Console.WriteLine("축하합니다!");
                        Console.WriteLine("무한모드로 진입합니다.");
                        Console.ReadKey();
                    }
                    else if (GameManager.DungeonFloor > 10)
                    {
                        Console.WriteLine("무한모드입니다.");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine($"던전 {GameManager.DungeonFloor}층으로 이동합니다...");
                        Thread.Sleep(1000);
                        Console.Clear();
                        BattleSystems.Start();
                    }

                }
                else if (sel == 0)
                {
                    GameManager.DungeonFloor++;
                    Console.WriteLine("던전을 나갑니다...");
                    Thread.Sleep(1000);
                    Console.Clear();
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
        private static Monster[] SpawnMonsters(int Floor)//Floor에 따라 몬스터 생성
        {
            int monsterCount;
            if (Floor == 5 || Floor == 10)
                monsterCount = 1; // 보스 몬스터
            else if (Floor < 10)
                monsterCount = GameManager.rd.Next(1, 5);
            else
                monsterCount = GameManager.rd.Next(3, 7);

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
                types = new[] { Monsters.고블린, Monsters.홉고블린, Monsters.오크, Monsters.하이오크, Monsters.해츨링, Monsters.와이번, Monsters.워울프, Monsters.만티코어, Monsters.드래곤 };

            var monsters = new Monster[monsterCount];
            for (int i = 0; i < monsterCount; i++)
            {
                monsters[i] = new Monster(types[GameManager.rd.Next(types.Length)]);
                if (Floor > 10)
                {
                    monsters[i].monMaxHp = monsters[i].monMaxHp*(1 + Floor / 10);
                    monsters[i].monAtk = monsters[i].monAtk * (1 + Floor / 20);
                }
            }
            return monsters;
        }
        private static bool PlayerTurn(Job player, Monster[] monsters)
        {
            while (true)
            {
                RenderStatus(player, monsters);
                Console.Write("\n\n1. 공격 ");
                Console.WriteLine("2. 스킬 ");
                Console.WriteLine("0. 던전 나가기");
                Console.WriteLine("\n 원하시는 행동을 입력해주세요.");
                Console.Write(">> ");
                if (!Mathod.CheckInput(out int sel))
                    continue;

                Console.Clear();
                if (sel == 1) // 공격 
                {
                    int idx = GameManager.SelectMonster(monsters);
                    var target = monsters[idx];
                    int dmg = player.Attack(target);

                    if (GameManager.NextFloor(monsters))
                        return false;
                    return true;
                }
                else if (sel == 2) // 스킬
                {
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine($"현재 마나량: {player.Mp}");
                        Console.WriteLine("사용할 스킬을 선택하세요.");
                        Console.WriteLine($"1. {player.Skill1Name}");
                        Console.WriteLine($"2. {player.Skill2Name}");
                        Console.WriteLine("0. 취소");
                        Console.Write(">> ");
                        if (!Mathod.CheckInput(out int skill))
                            continue;
                        Console.Clear();
                        if (skill == 1)
                        {
                            if (player.Mp < player.Skill1Cost)
                            {
                                Console.WriteLine("MP가 부족합니다.");
                                Thread.Sleep(1000);
                                Console.Clear();
                                continue;
                            }
                            int idx = GameManager.SelectMonster(monsters);
                            var target = monsters[idx];
                            int dmg = player.Skill1(target);

                            if (GameManager.NextFloor(monsters))
                                return false;
                            return true;
                        }
                        else if (skill == 2)
                        {
                            if (player.Mp < player.Skill2Cost)
                            {
                                Console.WriteLine("MP가 부족합니다.");
                                Thread.Sleep(1000);
                                continue;
                            }
                            player.Skill2(monsters);

                            if (GameManager.NextFloor(monsters))
                                return false;
                            return true;
                        }
                        else if (skill == 0)
                        {
                            Console.WriteLine("스킬 사용을 취소합니다.");
                            Thread.Sleep(500);
                            Console.Clear();
                            break;
                        }
                        else
                        {
                            Console.WriteLine("잘못된 입력입니다.");
                            Thread.Sleep(500);
                        }
                        continue;
                    }
                }
                else if (sel == 0)
                {
                    Console.WriteLine("체력이 1/5 감소합니다");
                    Console.WriteLine("던전을 나갑니다.");
                    player.health -= player.health / 5;
                    Thread.Sleep(1000);
                    return false;
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Thread.Sleep(1000);
                    Console.Clear();
                    continue;
                }

            }
        }
        private static void EnemyTurn(Job player, Monster[] monsters)
        {
            Console.Clear();
            Console.WriteLine("적 턴");
            Mathod.ChangeFontColor(ColorCode.Green);
            Console.WriteLine($"플레이어 체력: {player.health}\n");
            Mathod.ChangeFontColor(ColorCode.None);
            for (int i = 0; i < monsters.Length; i++)
            {
                var m = monsters[i];
                if (!m.IsAlive)
                    continue; //죽은 몬스터는 건너뛰기
                else
                {
                    Mathod.ChangeFontColor(ColorCode.Red);
                    Console.WriteLine($"\n{m.MonsterType}의 공격");
                    Console.WriteLine($"→ {m.monAtk - player.def / 2} 데미지!");
                    player.health -= m.monAtk - player.def / 2;//방어력의 절반만큼 데미지 감소
                    Mathod.ChangeFontColor(ColorCode.Green);
                    Console.WriteLine($"남은 체력: {player.health}");
                    Console.WriteLine("");
                    Thread.Sleep(1000);
                    Mathod.ChangeFontColor(ColorCode.None);
                    if (player.health <= 0)
                        return; // 플레이어가 사망하면 전투 종료

                }
            }
            Console.WriteLine("적 턴 종료");
            Console.WriteLine("아무키나 눌러서 계속 진행하세요.");
            Console.ReadKey();
        }
        public static void RenderStatus(Job player, Monster[] monsters)
        {
            Console.Clear();
            Console.WriteLine($"[던전 {GameManager.DungeonFloor}층]");
            Mathod.ChangeFontColor(ColorCode.Green);
            Console.WriteLine("플레이어 턴\n");
            Console.WriteLine($"체력: {player.health}");
            Console.WriteLine($"공격력: {player.atk}");
            Console.WriteLine($"현재 마나량:{player.Mp}\n\n");
            Mathod.ChangeFontColor(ColorCode.None);
            for (int i = 0; i < monsters.Length; i++)
            {
                Mathod.ChangeFontColor(ColorCode.Red);
                var m = monsters[i];
                if (m.IsAlive)
                {
                    Console.WriteLine($"{i + 1} ({m.MonsterType})");
                    Console.WriteLine($"체력: {m.monHP}/{m.monMaxHp}");
                    Console.WriteLine($"공격력: {m.monAtk}\n");
                }
                else
                {
                    Mathod.ChangeFontColor(ColorCode.DarkGray);
                    Console.WriteLine($"{i + 1} ({m.MonsterType})\n사망\n");
                    Mathod.ChangeFontColor(ColorCode.None);
                }
                Mathod.ChangeFontColor(ColorCode.None);
            }
        }
    }
}
