using GameLogic;
using GameService;

namespace StartScene
{
    public class StartScenes
    {
        public static void ShowStartScene(Job _player)
        {
            int valueInput = 0;
            string nameInput = "";

            while (true)
            {
                Console.Clear();
                Console.WriteLine("원하시는 이름을 입력해주세요:");
                Console.Write(">> ");

                nameInput = Console.ReadLine();

                if (string.IsNullOrEmpty(nameInput))
                {
                    Console.WriteLine("최소 한 글자 이상 입력하셔야 합니다.");
                    continue;
                }

                Console.WriteLine("1. 저장");
                Console.WriteLine("2. 취소");

                if (Mathod.CheckInput(out valueInput))
                {
                    //저장시
                    if (valueInput == 1)
                    {
                        _player.SetName(nameInput);
                        break;
                    }

                    //취소시 다시 루프
                    else if (valueInput == 2)
                    {
                        continue;
                    }

                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                    }
                }
            }

            Console.WriteLine($"\n환영합니다, {_player.name}님!");
            Console.WriteLine("아무 키나 눌러 스파르타 마을로 이동합니다...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
