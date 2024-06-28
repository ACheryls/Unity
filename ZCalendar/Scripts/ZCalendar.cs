using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Events;

namespace ZTools
{
    [RequireComponent(typeof(ZCalendarModel))]
    public class ZCalendar : MonoBehaviour
    {
        /// <summary>
        /// ���ݸ���ʱ���ɻ�ȡ��ÿһ�����ڣ���������в���
        /// </summary>
        public class UpdateDateEvent : UnityEvent<DateTime> { }
        public UpdateDateEvent m_DayRefresh = new UpdateDateEvent();
        /// <summary>
        /// ���Ի�ȡ�������ĳһ��
        /// </summary>
        public class ChoiceDayEvent : UnityEvent<DateTime> { }
        public ChoiceDayEvent m_DayValueChanged = new ChoiceDayEvent();
        /// <summary>
        /// ѡ������ʱ���¼�
        /// </summary>
        public class RangeTimeEvent : UnityEvent<DateTime, DateTime> { }
        public RangeTimeEvent m_RangeTimeEvent = new RangeTimeEvent();
        /// <summary>
        /// �������ؽ���
        /// </summary>
        public class CompleteEvent : UnityEvent { }
        public CompleteEvent m_completeEvent = new CompleteEvent();
        /// <summary>
        /// ��ȡ��ǰѡ�е������
        /// </summary>
        public DateTime CrtTime { get; set; }
        /// <summary>
        /// model
        /// </summary>
        private ZCalendarModel zCalendarModel;
        /// <summary>
        /// controller
        /// </summary>
        private ZCalendarController zCalendarController;
        public CompleteEvent onComplete
        {
            set { m_completeEvent = value; }
            get { return m_completeEvent; }
        }
        public RangeTimeEvent onRangeTimeValueChanged
        {
            set { m_RangeTimeEvent = value; }
            get { return m_RangeTimeEvent; }
        }
        public ChoiceDayEvent onDayValueChanged
        {
            set { m_DayValueChanged = value; }
            get { return m_DayValueChanged; }
        }
        public UpdateDateEvent onDayRefresh
        {
            set { m_DayRefresh = value; }
            get { return m_DayRefresh; }
        }
        public event Action<DateTime> dayValueChangedDayItemListenerEvent;
        public event Action<DateTime, DateTime> RangeTimeChangedDayItemListenerEvent;
        /// <summary>
        /// ���
        /// </summary>
        private void Start()
        {
            zCalendarModel = this.GetComponent<ZCalendarModel>();
            // ����ʱ�Զ���ʼ��
            if (zCalendarModel.awake2Init)
            {
                Init();
            }
        }
        /// <summary>
        /// ��ʼ��
        /// </summary>
        public void Init()
        {
            zCalendarController = new ZCalendarController()
            {
                zCalendar = this,
                zCalendarModel = zCalendarModel,
                pos = this.transform.localPosition
            };
            zCalendarController.Init();
            RefreshDate();
        }
        /// <summary>
        /// ��������ʱ���ʼ��
        /// </summary>
        public void RefreshDate()
        {
            zCalendarController.RefreshDate(DateTime.Today);
        }
        /// <summary>
        /// ����DateTime��ʽ��ʼ������
        /// </summary>
        public void RefreshDate(DateTime dateTime)
        {
            if (dateTime.Hour != 0 || dateTime.Month != 0 || dateTime.Second != 0)
            {
                dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
            }
            zCalendarController.RefreshDate(dateTime);
        }
        /// <summary>
        /// ����YYYY-MM-DD��ʽ��ʼ������
        /// </summary>
        public void RefreshDate(string dateTime)
        {
            if (!dateTime.Contains("-") || dateTime.Contains(":"))
            {
                Debug.LogError("wrong format: Time divided by '-' and do not contains hour minute or second");
                return;
            }
            string[] dateTimes = dateTime.Split('-');
            zCalendarController.RefreshDate(new DateTime(int.Parse(dateTimes[0]), int.Parse(dateTimes[1]), int.Parse(dateTimes[2])));
        }
        /// <summary>
        /// ��ʼ����������
        /// </summary>
        public void RefreshDate(DateTime startDateTime, DateTime endDateTime)
        {
            if (!zCalendarModel.rangeCalendar)
            {
                Debug.LogError("ZCalendar Init Error��The config is not RangeCalendar!!!");
                return;
            }
            if (startDateTime.Hour != 0 || startDateTime.Minute != 0 || startDateTime.Second != 0)
            {
                startDateTime = new DateTime(startDateTime.Year, startDateTime.Month, startDateTime.Day, 0, 0, 0);
            }
            if (endDateTime.Hour != 0 || endDateTime.Minute != 0 || endDateTime.Second != 0)
            {
                endDateTime = new DateTime(endDateTime.Year, endDateTime.Month, endDateTime.Day, 0, 0, 0);
            }
            zCalendarController.RefreshDate(startDateTime, endDateTime);
        }
        /// <summary>
        /// ��ʼ����������
        /// </summary>
        /// <param name="startDateTime">��ʼʱ�䣬�ԡ�-���ָ�</param>
        /// <param name="endDateTime">����ʱ�䣬�ԡ�-���ָ�</param>
        public void RefreshDate(string startDateTime, string endDateTime)
        {
            if (!startDateTime.Contains("-") || !endDateTime.Contains("-"))
            {
                Debug.LogError("wrong format: Time divided by '-' and do not contains hour minute or second");
                return;
            }
            string[] startDateTimes = startDateTime.Split('-');
            string[] endDataTimes = endDateTime.Split('-');
            zCalendarController.RefreshDate(new DateTime(int.Parse(startDateTimes[0]), int.Parse(startDateTimes[1]), int.Parse(startDateTimes[2])), new DateTime(int.Parse(endDataTimes[0]), int.Parse(endDataTimes[1]), int.Parse(endDataTimes[2])));
        }
        /// <summary>
        /// ��ʾ����
        /// </summary>
        public void Show()
        {
            zCalendarController.Show();
        }
        /// <summary>
        /// ���ص���
        /// </summary>
        public void Hide()
        {
            zCalendarController.Hide();
        }

        /// <summary>
        /// �л�ʱ��
        /// </summary>
        /// <param name="obj"></param>
        [Obsolete("�¼�����������ʹ��UpdateDateEvent��ȡ�л��·�ʱ���ص�ʱ�����")]
        public void UpdateDate(DateTime obj)
        {
            m_DayRefresh.Invoke(obj);
        }
        DateTime choiceTime;
        /// <summary>
        /// ���ڵ��
        /// </summary>
        [Obsolete("�¼�����������ʹ��ChoiceDayEvent��ȡ��ǰѡ���ʱ��")]
        public void DayClick(DateTime dayItem)
        {
            choiceTime = new DateTime(dayItem.Year, dayItem.Month, dayItem.Day, zCalendarModel.hour.choiceTime, zCalendarModel.min.choiceTime, zCalendarModel.second.choiceTime);
            dayValueChangedDayItemListenerEvent.Invoke(dayItem);
            m_DayValueChanged.Invoke(dayItem.AddHours(zCalendarModel.hour.choiceTime).AddMinutes(zCalendarModel.min.choiceTime).AddSeconds(zCalendarModel.second.choiceTime));
            CrtTime = dayItem;
        }
        public void TimeChoice()
        {
            if (zCalendarModel.rangeCalendar)
            {
                RangeCalendar(rangeStartTime, rangeEndTime);
            }
            else
            {
                DayClick(CrtTime);
            }
        }
        /// <summary>
        /// ���ؽ���
        /// </summary>
        [Obsolete("�¼�����������ʹ��CompleteEvent��ȡ������������¼�")]
        public void DateComplete()
        {
            onComplete?.Invoke();
        }
        DateTime rangeStartTime, rangeEndTime;
        /// <summary>
        /// ��������ѡ��
        /// </summary>
        /// <param name="firstDay"></param>
        /// <param name="secondDay"></param>
        [Obsolete("�¼�����������ʹ��RangeTimeEvent��ȡ����ʱ��")]
        public void RangeCalendar(DateTime firstDay, DateTime secondDay )
        {
            RangeTimeChangedDayItemListenerEvent?.Invoke(firstDay, secondDay);
            m_RangeTimeEvent?.Invoke(firstDay.AddHours(zCalendarModel.hour.choiceTime).AddMinutes(zCalendarModel.min.choiceTime).AddSeconds(zCalendarModel.second.choiceTime), secondDay.AddHours(zCalendarModel.hour.choiceTime).AddMinutes(zCalendarModel.min.choiceTime).AddSeconds(zCalendarModel.second.choiceTime));
            rangeStartTime = firstDay;
            rangeEndTime = secondDay;
        }
        private void OnDestroy()
        {
            zCalendarController = null;
            GC.Collect();
        }
    }
}
