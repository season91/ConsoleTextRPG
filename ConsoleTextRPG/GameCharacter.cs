using System.Numerics;
using BattleSystem;
using GameLogic;
using GameService;
using Manager;


namespace GameCharacter
{
    public class Warrior : Job
    {
        public Warrior()
        {
            level = 1;
            chad = "전사";
            atk = 10;
            def = 10;
            Mp = 30;
            health = 100;
            gold = 1500;
            exp = 0;
            critRate = 0.1; // 치명타 확률
            critDamage = 1.5; // 치명타 배율
            dodgeRate = 0.1; // 회피율
            Skill1Cost = 10; // 스킬 MP 소모량
            Skill2Cost = 15;
            Skill1Name = "알파 스트라이크"; // 스킬 이름
            Skill2Name = "더블 스트라이크";
        }

        public override int Attack(Monster target)
        {
            int dmg = target.TakeDamage(atk, this); // 일반 공격 데미지
            return dmg;
        }
        public override int Skill1(Monster target)
        {
            Mp -= Skill1Cost;
            int dmg = target.TakeDamage(atk * 2, this); // 스킬 데미지

            return dmg;
        }
        public override void Skill2(Monster[] targets)
        {
            var Rdtarget = GameManager.rd; //랜덤 타겟팅
            int targetDamage = (int)(atk * 1.5); // 스킬 데미지

            for (int i = 0; i < 2; i++) // 2회 공격
            {
                var targetalive = targets.Where(m => m.IsAlive).ToArray(); // 살아있는 몬스터만 필터링
                if (targetalive.Length == 0)
                    break;
                var target = targetalive[Rdtarget.Next(0, targetalive.Length)]; // 랜덤 타겟팅
                int dmg = target.TakeDamage(targetDamage, this);
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
            def = 2;
            Mp = 50;
            health = 100;
            gold = 1500;
            exp = 0;
            critRate = 0.15;
            critDamage = 1.6;
            dodgeRate = 0.1;
            Skill1Cost = 10;
            Skill2Cost = 15;
            Skill1Name = "파이어볼";
            Skill2Name = "블리자드";
        }

        public override int Attack(Monster target)
        {
            int dmg = target.TakeDamage(atk, this);
            return dmg;
        }
        public override int Skill1(Monster target)
        {
            Mp -= Skill1Cost;
            int dmg = target.TakeDamage(atk * 3, this);

            return dmg;
        }
        public override void Skill2(Monster[] targets)
        {
            var Rdtarget = GameManager.rd;
            int targetDamage = (int)(atk * 1.8);

            for (int i = 0; i < 3; i++)
            {
                var targetalive = targets.Where(m => m.IsAlive).ToArray();
                if (targetalive.Length == 0)
                    break;
                var target = targetalive[Rdtarget.Next(0, targetalive.Length)];
                int dmg = target.TakeDamage(targetDamage, this);
            }
            Mp -= Skill2Cost; //스킬 사용 시 MP 소모
        }

    }
    public class Archer : Job
    {
        public Archer()
        {
            level = 1;
            chad = "궁수";
            atk = 10;
            def = 3;
            Mp = 30;
            health = 100;
            gold = 1500;
            exp = 0;
            critRate = 0.5;
            critDamage = 2;
            dodgeRate = 0.1;
            Skill1Cost = 10;
            Skill2Cost = 15;
            Skill1Name = "더블샷";
            Skill2Name = "멀티샷";
        }

        public override int Attack(Monster target)
        {
            int dmg = target.TakeDamage(atk, this);
            return dmg;
        }
        public override int Skill1(Monster target)
        {
            Mp -= Skill1Cost;
            int dmg = target.TakeDamage(atk * 2, this);

            return dmg;
        }
        public override void Skill2(Monster[] targets)
        {
            var Rdtarget = GameManager.rd;
            int targetDamage = (int)(atk * 2.0);

            for (int i = 0; i < 2; i++)
            {
                var targetalive = targets.Where(m => m.IsAlive).ToArray();
                if (targetalive.Length == 0)
                    break;
                var target = targetalive[Rdtarget.Next(0, targetalive.Length)];
                int dmg = target.TakeDamage(targetDamage, this);
            }
            Mp -= Skill2Cost; //스킬 사용 시 MP 소모
        }
    }
    public class Berserker : Job
    {
        public Berserker()
        {
            level = 1;
            chad = "광전사";
            atk = 12;
            def = 5;
            Mp = 5;
            health = 150;
            gold = 1500;
            exp = 0;
            critRate = 0.2;
            critDamage = 2;
            dodgeRate = 0.1;
            Skill1HpCost = 10;
            Skill2HpCost = 20;
            Skill1Name = "머리쪼개기";
            Skill2Name = "회전베기";
        }

        public override int Attack(Monster target)
        {
            int dmg = target.TakeDamage(atk, this);
            return dmg;
        }
        public override int Skill1(Monster target)
        {
            health -= Skill1HpCost;
            int dmg = target.TakeDamage(atk * 2, this);
            Mathod.FontColorOnce($"{Skill1Name} 사용으로 HP{Skill1HpCost} 소모", ColorCode.Red);
            Thread.Sleep(1000);
            return dmg;
        }
        public override void Skill2(Monster[] targets)
        {
            var Rdtarget = GameManager.rd;
            int targetDamage = atk * 1;

            for (int i = 0; i < 4; i++)
            {
                var targetalive = targets.Where(m => m.IsAlive).ToArray();
                if (targetalive.Length == 0)
                    break;
                var target = targetalive[Rdtarget.Next(0, targetalive.Length)];
                int dmg = target.TakeDamage(targetDamage, this);
            }
            health -= Skill2HpCost; //스킬 사용 시 HP 소모
            Mathod.FontColorOnce($"{Skill2Name} 사용으로 HP{Skill2HpCost} 소모", ColorCode.Red);
            Thread.Sleep(1000);
        }
    }

}
