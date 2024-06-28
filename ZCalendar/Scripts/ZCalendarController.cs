/*
 * Created by JacobKay - 2022.08.24
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ZTools
{
    public class ZCalendarController
    {
        public int Year { set; get; }
        public int Month { set; get; }
        public int Day { set; get; }
        /// <summary>
        /// ��ǰ�Ƿ�������ѡ��״̬
        /// </summary>
        private bool isInRange = false;
        public bool IsInRange { get { return isInRange; } }
        private string week;
        private DateTime now;
        private int days;
        /// <summary>
        /// ��ǰѡ�е�λ��
        /// </summary>
        public Vector3 pos;
        private int lastMonthDays;
        private int nextMonthDays;
        public ZCalendar zCalendar;
        public ZCalendarModel zCalendarModel;
        public DateTime nowTime = DateTime.Today;
        private int lastMonthEmptyDays;
        bool isShow = true;
        public bool isInit = false;
        /// <summary>
        /// ����������ɫ
        /// </summary>
        public Color greyColor;

        public System.Globalization.ChineseLunisolarCalendar cncld = new System.Globalization.ChineseLunisolarCalendar();
        /// <summary>
        /// ũ����
        /// </summary>
        public string[] lunarMonths = { "��", "��", "��", "��", "��", "��", "��", "��", "��", "ʮ", "ʮһ", "��" };

        public string[] lunarDaysT = { "��", "ʮ", "إ", "��" };

        /// <summary>
        /// ũ����
        /// </summary>
        public string[] lunarDays = { "һ", "��", "��", "��", "��", "��", "��", "��", "��", "ʮ" };
        DateTime monthFirstDay;
        public int crtDayIndex = 0;
        /// <summary>
        /// ��ʼ��
        /// </summary>
        /// <param name="date"></param>
        public void Init()
        {
            zCalendarModel.zCalendarController = this;
            zCalendarModel.Init();
            if (zCalendarModel.isStaticCalendar) return;
            // ��̬�������ɹر�
            if (zCalendarModel.isPopupCalendar)
            {
                zCalendarModel.btnClose.onClick.AddListener(() =>
                {
                    Hide();
                });
            }
            zCalendarModel.btnLastYear.onClick.AddListener(LastYear);
            zCalendarModel.btnNextYear.onClick.AddListener(NextYear);
            zCalendarModel.btnLastMonth.onClick.AddListener(LastMonth);
            zCalendarModel.btnNextMonth.onClick.AddListener(NextMonth);
        }

        /// <summary>
        /// ���չ涨ʱ���ʼ������
        /// </summary>
        public void RefreshDate(DateTime date)
        {
            now = date;
            DestroyAllChildren();
            UpdateYear();
            UpdateMonth();
            UpdateDays();
            UpdateDate();
            if (!isInit)
            {
                isInit = true;
                zCalendar.DateComplete();
            }
        }
        /// <summary>
        /// ����ָ��֮���ʼ������ʱ��
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        public void RefreshDate(DateTime startDate, DateTime endDate)
        {
            now = DateTime.Today;
            DestroyAllChildren();
            ChangeRangeType(startDate);
            ChangeRangeType(endDate);
            UpdateYear();
            UpdateMonth();
            UpdateDays();
            UpdateDate();
            if (!isInit)
            {
                isInit = true;
                zCalendar.DateComplete();
            }
        }
        void LastYear()
        {
            now = now.AddYears(-1);
            DestroyAllChildren();
            UpdateYear();
            UpdateMonth();
            UpdateDays();
            UpdateDate();
        }
        void NextYear()
        {
            now = now.AddYears(1);
            DestroyAllChildren();
            UpdateYear();
            UpdateMonth();
            UpdateDays();
            UpdateDate();
        }
        void LastMonth()
        {
            now = now.AddMonths(-1);
            DestroyAllChildren();
            UpdateYear();
            UpdateMonth();
            UpdateDays();
            UpdateDate();
        }
        void NextMonth()
        {
            now = now.AddMonths(1);
            DestroyAllChildren();
            UpdateYear();
            UpdateMonth();
            UpdateDays();
            UpdateDate();
        }

        List<DateTime> dayItemList = new List<DateTime>();

        /// <summary>
        /// ���������������ѡ��ʱ��ʱ����Ҫ�жϵ�ǰ����ѡ��״̬
        /// </summary>
        /// <returns></returns>
        public void ChangeRangeType(DateTime dayItem)
        {
            isInRange = !isInRange;
            if (dayItemList.Count >= 2)
            {
                dayItemList.Clear();
            }
            if (dayItemList.Count == 0)
            {
                dayItemList.Add(dayItem);
            }
            else
            {
                int compare = DateTime.Compare(dayItemList[0], dayItem);
                if (compare <= 0)
                {
                    dayItemList.Add(dayItem);
                }
                else
                {
                    dayItemList.Insert(0, dayItem);
                }
            }
            if (!isInRange)
            {
                zCalendar.RangeCalendar(dayItemList[0], dayItemList[1]);
            }
        }
        /// <summary>
        /// ��ʾ����
        /// </summary>
        public void Show()
        {
            if (pos != null && !isShow)
            {
                isShow = true;
                zCalendar.transform.localPosition = pos;
            }
        }
        /// <summary>
        /// ��������
        /// </summary>
        public void Hide()
        {
            if (isShow && !isInRange)
            {
                isShow = false;
                Debug.Log("hide");
                zCalendar.transform.localPosition = new Vector3(pos.x, 5000, pos.z);
            }
        }
        /// <summary>
        /// ��ѯ������
        /// </summary>
        void UpdateYear()
        {
            Year = now.Year;
        }
        /// <summary>
        /// ��ѯ������
        /// </summary>
        void UpdateMonth()
        {
            Month = int.Parse(now.Month.ToString("00"));
        }
        /// <summary>
        /// ����Ҫ��ѯ����
        /// </summary>
        /// <returns></returns>
        void UpdateDays()
        {
            days = DateTime.DaysInMonth(Year, Month);
            if (Day == 0)
            {
                Day = now.Day;
            }
            else if (Day > days)
            {
                Day = days;
            }
        }
        /// <summary>
        /// ������ʾ�·�
        /// </summary>
        void UpdateDate()
        {
            zCalendarModel.SetTimeTxt(Year, Month);
            FillLastMonth();
            for (int i = 0; i < days; i++)
            {
                AddDayItem(monthFirstDay.AddDays(i));
            }
            FillNextMonth();
        }
        /// <summary>
        /// �Զ�����ϸ�������
        /// </summary>
        void FillLastMonth()
        {
            monthFirstDay = new DateTime(Year, Month, 1);
            lastMonthEmptyDays = GetLastMonthDays();
            if (zCalendarModel.autoFillDate)
            {
                for (int i = lastMonthEmptyDays; i > 0; i--)
                {
                    AddDayItem(monthFirstDay.AddDays(-i));
                }
            }
            else
            {
                for (int i = 0; i < lastMonthEmptyDays; i++)
                {
                    crtDayItem = zCalendarModel.Instantiate();
                    crtDayItem.zCalendarController = this;
                    crtDayItem.CloseClickAble();
                }
            }
        }
        /// <summary>
        /// ����¸��µ�ʱ��
        /// </summary>
        void FillNextMonth()
        {
            int nextMonthDays = 42 - (lastMonthEmptyDays + days);
            if (nextMonthDays != 0)
            {
                if (zCalendarModel.autoFillDate)
                {
                    DateTime lastDay = monthFirstDay.AddDays(days);
                    for (int i = 0; i < nextMonthDays; i++)
                    {
                        AddDayItem(lastDay.AddDays(i));
                    }
                }
            }
        }
        ZCalendarDayItem crtDayItem;
        /// <summary>
        /// ������ڶ���
        /// </summary>
        void AddDayItem(DateTime dateTime)
        {
            crtDayItem = zCalendarModel.Instantiate();
            crtDayItem.zCalendarController = this;
            crtDayItem.Init(dateTime, now);
            zCalendar.UpdateDate(crtDayItem.dateTime);
            // ����ʱ��ѡ��һ���Ժ��ڷ�ҳ��ʱ����Ҫ����
            if (!isInRange && dayItemList.Count > 0)
            {
                crtDayItem.IsRangeDayItem(dayItemList[0], dayItemList[1]);
            }
        }
        /// <summary>
        /// �ж���һ�����м���
        /// </summary>
        /// <returns></returns>
        int GetLastMonthDays()
        {
            string firstWeek = new DateTime(Year, Month, 1).DayOfWeek.ToString();
            return (int)Enum.Parse(typeof(DayOfWeek), firstWeek);
        }
        void DestroyAllChildren()
        {
            crtDayIndex = 0;
            for (int i = 0; i < zCalendarModel.dayItemList.Count; i++)
            {
                zCalendarModel.dayItemList[i].OnDestroy();
            }
        }
    }
}
