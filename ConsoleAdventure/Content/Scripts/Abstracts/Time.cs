﻿namespace ConsoleAdventure
{
    public class Time
    {
        public int day { get; private set; } = 1;
        public int hour { get; private set; } = 8;
        public int minute { get; private set; }
        public int second { get; private set; }

        private int multiplier = 1;

        public void PassTime(int secondsPass)
        {
            second += secondsPass * multiplier;
            Stack();
        }

        public string GetTime()
        {
            return $"Day: {day} Time: {hour}:{minute.ToString("D2")}";
        }

        private void Stack()
        {
            while (hour >= 24)
            {
                day++;
                hour -= 24;
            }
            while (second >= 60)
            {
                minute++;
                second -= 60;
            }
            while (minute >= 60)
            {
                hour++;
                minute -= 60;
            }
        }
    }
}
