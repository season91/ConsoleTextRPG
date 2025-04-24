using System.Numerics;
using BattleSystem;
using GameLogic;
using GameService;
using Manager;
using static System.Net.Mime.MediaTypeNames;

namespace GameCharacter
{
    public class Warrior : Job
    {
        public Warrior()
        {
            level = 1;
            chad = "전사";
            atk = 10;
            def = 5;
            Mp = 30;
            health = 100;
            gold = 1500;
            exp = 0;
            critRate = 0.1;
            critDamage = 1.5;
            dodgeRate = 0.1;

        }
        public override int Skill1Cost => 10;
        public override int Skill2Cost => 15;
        public override string Skill1Name => "알파 스트라이크";
        public override string Skill2Name => "더블 스트라이크";
        public override int Attack(Monster target)
        {
            int dmg = target.TakeDamage(atk, this);
            if (dodgeRate > GameManager.rd.NextDouble())
            {
                Mathod.ChangeFontColor(ColorCode.Blue);
                Console.WriteLine($"→ {target.Name}이 공격을 회피했습니다!");
                Mathod.ChangeFontColor(ColorCode.None);
                Thread.Sleep(1000);
                return 0;
            }
            else if (critRate > GameManager.rd.NextDouble())
            {
                dmg = (int)(dmg * critDamage);
                Mathod.ChangeFontColor(ColorCode.Yellow);
                Console.WriteLine($"→ {target.Name}에게 ({dmg})크리티컬 데미지! (남은 HP {target.monHP}/{target.monMaxHp})");
                Mathod.ChangeFontColor(ColorCode.None);
                Thread.Sleep(1000);
            }
            else
            {
                Mathod.ChangeFontColor(ColorCode.Red);
                Console.WriteLine($"→ {target.Name}에게 {dmg} 데미지! (남은 HP {target.monHP}/{target.monMaxHp})");
                Mathod.ChangeFontColor(ColorCode.None);
                Thread.Sleep(1000);

            }
            return dmg;
        }
        public override int Skill1(Monster target)
        {
            Mp -= Skill1Cost;
            int dmg = target.TakeDamage(atk * 2, this);
            Mathod.ChangeFontColor(ColorCode.Red);
            Console.WriteLine($"→ {target.Name}에게 {dmg} 데미지! (남은 HP {target.monHP}/{target.monMaxHp})");
            Mathod.ChangeFontColor(ColorCode.None);
            Thread.Sleep(1000);


            return dmg;
        }
        public override void Skill2(Monster[] targets)
        {
            var Rdtarget = GameManager.rd;
            int targetDamage = (int)(atk * 1.5);

            for (int i = 0; i < 2; i++)
            {
                var targetalive = targets.Where(m=>m.IsAlive).ToArray();
                if (targetalive.Length == 0)
                    break;
                var target = targetalive[Rdtarget.Next(0, targetalive.Length)];
                target.TakeDamage(targetDamage, this);
                Mathod.ChangeFontColor(ColorCode.Red);
                Console.WriteLine($"→ {target.Name}에게 {targetDamage} 데미지! (남은 HP {target.monHP}/{target.monMaxHp})");
                Mathod.ChangeFontColor(ColorCode.None);
                Thread.Sleep(1000);
            }
            Mp -= Skill2Cost; //스킬 사용 시 MP 소모
        }
    }

    public class Wizard : Job
    {
        public Wizard()
        {
            level = 1;
            chad = "마법사";
            atk = 10;
            def = 5;
            Mp = 50;
            health = 100;
            gold = 1500;
            exp = 0;
            critRate = 0.15;
            critDamage = 1.6;
            dodgeRate = 0.1;
        }
        public override int Skill1Cost => 10;
        public override int Skill2Cost => 15;
        public override string Skill1Name => "파이어볼";
        public override string Skill2Name => "블리자드";
        public override int Attack(Monster target)
        {
            int dmg = target.TakeDamage(atk/10, this);
            Mathod.ChangeFontColor(ColorCode.Red);
            Console.WriteLine($"→ {target.Name}에게 {dmg} 데미지! (남은 HP {target.monHP}/{target.monMaxHp})");
            Mathod.ChangeFontColor(ColorCode.None);
            Thread.Sleep(1000);

            return dmg;
        }
        public override int Skill1(Monster target)
        {
            Mp -= Skill1Cost;
            int dmg = target.TakeDamage(atk * 3, this);
            Mathod.ChangeFontColor(ColorCode.Red);
            Console.WriteLine($"→ {target.Name}에게 {dmg} 데미지! (남은 HP {target.monHP}/{target.monMaxHp})");
            Mathod.ChangeFontColor(ColorCode.None);
            Thread.Sleep(1000);


            return dmg;
        }
        public override void Skill2(Monster[] targets)
        {
            var Rdtarget = GameManager.rd;
            int targetDamage = (int)(atk * 2.5);

            for (int i = 0; i < 3; i++)
            {
                var targetalive = targets.Where(m => m.IsAlive).ToArray();
                if (targetalive.Length == 0)
                    break;
                var target = targetalive[Rdtarget.Next(0, targetalive.Length)];
                int dmg = target.TakeDamage(targetDamage, this);
                Mathod.ChangeFontColor(ColorCode.Red);
                Console.WriteLine($"→ {target.Name}에게 {dmg} 데미지! (남은 HP {target.monHP}/{target.monMaxHp})");
                Mathod.ChangeFontColor(ColorCode.None);
                Thread.Sleep(1000);
            }
            Mp -= Skill2Cost; //스킬 사용 시 MP 소모
        }

    }
}
