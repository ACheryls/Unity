/*
 * JacobKay --20220903
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZTools;
/// <summary>
/// ʹ��ʾ��
/// </summary>
public class ZCalendarDemo : MonoBehaviour
{
    public ZCalendar zCalendar;
    // Start is called before the first frame update
    void Awake()
    {
        zCalendar.onDayRefresh.AddListener(ZCalendar_UpdateDateEvent);
        zCalendar.onDayValueChanged.AddListener(ZCalendar_ChoiceDayEvent);
        zCalendar.onRangeTimeValueChanged.AddListener(ZCalendar_RangeTimeEvent);
        zCalendar.onComplete.AddListener(ZCalendar_CompleteEvent);
        //zCalendar.RefreshDate("2023-10-01", "2023-11-21");
        //zCalendar.RefreshDate(System.DateTime.Now);
        //zCalendar.RefreshDate("2022-02-02");
        //zCalendar.Show();
        //zCalendar.Hide();
    }
    /// <summary>
    /// ���ؽ���
    /// </summary>
    private void ZCalendar_CompleteEvent()
    {
        Debug.Log("ZCalendar���ؽ���");
        if (null != zCalendar.CrtTime)
        {
            Debug.Log($"��ǰʱ��{zCalendar.CrtTime.Day}");
        }
    }

    /// <summary>
    /// ����ʱ��
    /// </summary>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    private void ZCalendar_RangeTimeEvent(DateTime arg1, DateTime arg2)
    {
        if (GetComponent<ZCalendarModel>().timeChoice)
        {
            Debug.Log($"ѡ���ʱ�����䣺{arg1.ToString("yyyy-MM-dd HH:mm:ss")}��{arg2.ToString("yyyy-MM-dd HH:mm:ss")}");
        }
        else
        {
            Debug.Log($"ѡ����������䣺{arg1.ToString("yyyy-MM-dd")}��{arg2.ToString("yyyy-MM-dd")}");
        }
    }

    /// <summary>
    /// ��ȡѡ�������
    /// </summary>
    /// <param name="obj"></param>
    private void ZCalendar_ChoiceDayEvent(DateTime obj)
    {
        if (GetComponent<ZCalendarModel>().timeChoice)
        {
            Debug.Log($"ѡ���ʱ�䣺{obj.ToString("yyyy-MM-dd HH:mm:ss")}");
        }
        else
        {
            Debug.Log($"ѡ������ڣ�{obj.ToString("yyyy-MM-dd")}");
        }
    }

    /// <summary>
    /// �л��·�ʱ�����õ�ÿһ���item����
    /// </summary>
    /// <param name="obj"></param>
    private void ZCalendar_UpdateDateEvent(DateTime obj)
    {
        //Debug.Log($"�������ڣ�{obj.Day}");
    }
}
