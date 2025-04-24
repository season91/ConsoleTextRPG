using GameService;
using Manager;

namespace SaveWindow
{
    public class SaveWindows
    {
        public static void Show()
        {
            var input = 0;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("게임을 저장하시겠습니까?\n\n");

                Mathod.MenuFont("1", "네\n", ColorCode.Green);
                Mathod.MenuFont("2", "아니오", ColorCode.DarkGray);

                Console.Write("\n>>");

                if (Mathod.CheckInput(out input))
                {
                    if (input == 1)
                    {
                        GameManager.quest.Save();
                        GameManager.player.SaveData();
                        GameManager.data.Save();

                        Console.Clear();
                        Mathod.ChangeFontColor(ColorCode.Green);
                        Console.Write("저장이 완료되었습니다.");
                        Mathod.ChangeFontColor(ColorCode.None);
                        Thread.Sleep(1000);
                    }

                    break;
                }
            }
        }
    }
}
