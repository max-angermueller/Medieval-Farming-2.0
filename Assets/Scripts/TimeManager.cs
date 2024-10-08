using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public static Action OnSecondChanged;
    public static Action OnMinuteChanged;
    public static Action OnHourChanged;
    public static Action OnDayChanged;
    public static Action OnWeekChanged;

    [SerializeField]
    private TextMeshProUGUI UItext;

    public static int Second { get; private set; }
    public static int Minute { get; private set; }
    public static int Hour { get; private set; }
    public static int Day { get; private set; }
    public static int Week { get; private set; }

    public static float secondAquivalence { get; private set; }
    public static float minuteAquivalence { get; private set; }
    public static float hourAquivalence { get; private set; }
    public static float dayAquivalence { get; private set; }
    public static float weekAquivalence { get; private set; }

    public float secondToRealTime = 1f;
    public float timer;

    private static bool isRunning = true;

    public static string time;

    private void Awake()
    {
        secondAquivalence = secondToRealTime;
        minuteAquivalence = secondToRealTime * 60f;
        hourAquivalence = minuteAquivalence * 60;
        dayAquivalence = hourAquivalence * 24;
        weekAquivalence = dayAquivalence * 7;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Second = 0;
        Minute = 0;
        Hour = 11;
        Day = 1;
        Week = 0;
        timer = secondToRealTime;
    }

    public static void nextDay()
    {
        Day += 1;
        Minute = 0;
        Hour = 6;
        OnDayChanged.Invoke();
    }

    public static void pauseDayCycle()
    {
        isRunning = false;
    }

    public static void startDayCycle()
    {
        isRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        time = string.Format("Day {0} \n {1:D2}:{2:D2}:{3:D2}", Day, Hour, Minute, Second);
        UItext.text = time;
        if (isRunning)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                Second++;
                OnSecondChanged?.Invoke();
                if (Second >= 60)
                {
                    Minute++;
                    Second = 0;
                    OnMinuteChanged?.Invoke();

                    if (Minute >= 60)
                    {
                        Hour++;
                        Minute = 0;
                        OnHourChanged?.Invoke();

                        if (Hour >= 24)
                        {
                            Day++;
                            Hour = 0;
                            OnDayChanged?.Invoke();

                            if (Day >= 7)
                            {
                                Week++;
                                Day = 0;
                                OnWeekChanged?.Invoke();
                            }
                        }
                    }
                }
                timer = secondToRealTime;
            }
        }

    }
}
