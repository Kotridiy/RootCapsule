using System;
using UnityEngine;

namespace RootCapsule.Core
{
    // Developing: loading state;
    public class WorldTime : MonoBehaviour
    {
        [SerializeField] private float tickLength = 9f;
        [SerializeField] private int dayLenght = 30;

        private TimeSpan timePassed;
        private int ticksPassed;
        private bool timeGo;
        private bool initialized;

        private static WorldTime worldTime;

        public static WorldTime GetWorldTime()
        {
            if (worldTime == null) worldTime = FindObjectOfType<WorldTime>();
            return worldTime;
        }

        public bool IsDayOver { get => ticksPassed == dayLenght; }

        public bool TimeGo
        { 
            get => timeGo;
            set 
            { 
                if (!IsDayOver) timeGo = value; 
            }
        }

        public float DayLeftoverSeconds
        {
            get
            {
                if (IsDayOver) return 0;
                return (dayLenght - ticksPassed) * tickLength - (float)timePassed.TotalSeconds;
            }
        }

        public float DayLeftoverTicks => dayLenght - ticksPassed - 1;

        public float DayLeftoverPersentage => dayLenght * tickLength / DayLeftoverSeconds;

        public event Action Tick;
        public event Action DayOver;


        void Awake()
        {
            // TODO loading state
            initialized = false;
            Initialize();

        }

        void Update()
        {
            if (TimeGo)
            {
                timePassed += TimeSpan.FromSeconds(Time.deltaTime);
                if (timePassed.TotalSeconds > tickLength)
                {
                    Tick?.Invoke();
                    ticksPassed++;
                    timePassed -= TimeSpan.FromSeconds(tickLength);

                    if (IsDayOver)
                    {
                        DayOver?.Invoke();
                        TimeGo = false;
                    }
                }
            }

            if (ticksPassed > dayLenght) 
                throw new Exception("Time more than day length!");
        }


        public void Initialize()
        {
            if (initialized) throw new InvalidOperationException("World Time initialized! Don't do it again.");

            timePassed = default;
            ticksPassed = default;
            TimeGo = true;
        }

        public void StartNewDay()
        {
            if (IsDayOver)
            {
                timePassed = default;
                ticksPassed = default;
            }
        }

        public void SkipTime(int ticks)
        {
            if (ticks <= DayLeftoverTicks)
            {
                ticksPassed += ticks;
                if (Tick != null)
                {
                    for (int i = 0; i < ticks; i++)
                    {
                        Tick.Invoke();
                    }
                }
            }
            else
            {
                throw new ArgumentException(nameof(ticks) + " must be less or equal that " + nameof(DayLeftoverTicks));
            }
        }

        public void SkipTime(float seconds)
        {
            if (seconds <= DayLeftoverSeconds)
            {
                timePassed += TimeSpan.FromSeconds(seconds);
                int ticks = (int)Math.Floor(timePassed.TotalSeconds / tickLength);
                timePassed -= TimeSpan.FromSeconds(ticks * tickLength);
                SkipTime(ticks);
            }
            else
            {
                throw new ArgumentException(nameof(seconds) + " must be less or equal that " + nameof(DayLeftoverSeconds));
            }
        }

        public void SkipRestDay()
        {
            SkipTime(DayLeftoverTicks);
            Tick?.Invoke();
            DayOver?.Invoke();
            TimeGo = false;
            ticksPassed = dayLenght;
        }
    }
}