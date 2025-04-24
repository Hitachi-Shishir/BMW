using System;
public class ExeRes
{

    public DateTime GetDueDate(DateTime created, string deskRef, string severity, string orgStaffFK)
    {
        string sqlHoursCovered = $"SELECT top 1 HoursCovered FROM vSDOrgDeskDef WITH(NOLOCK) WHERE Deskref='{deskRef}' AND OrgFk='{orgStaffFK}'";
        string hoursCovered = Convert.ToString(database.GetScalarValue(sqlHoursCovered));
        string sqlBeginHour = $"SELECT top 1 BeginHour FROM vSDOrgDeskDef WITH(NOLOCK)  WHERE Deskref='{deskRef}' AND OrgFk='{orgStaffFK}'";
        string beginHour = Convert.ToString(database.GetScalarValue(sqlBeginHour));
        string sqlEndHour = $"SELECT top 1 EndHour FROM vSDOrgDeskDef WITH(NOLOCK)  WHERE Deskref='{deskRef}' AND OrgFk='{orgStaffFK}'";
        string endHour = Convert.ToString(database.GetScalarValue(sqlEndHour));
        string sqlHoliday = $"SELECT top 1 COUNT(*) FROM SD_Holidays WITH(NOLOCK)  WHERE CAST(HolidayDate AS DATE) = CAST('{created:yyyy-MM-dd}' AS DATE) AND OrgID='{orgStaffFK}'";
        int holidayCount = Convert.ToInt32(database.GetScalarValue(sqlHoliday));
        if (holidayCount > 0)
        {
            DateTime nextDay = created.Date.AddDays(1);
            string sqlNextDayTime = $"SELECT DATEADD(MINUTE,0,(CAST('{nextDay:yyyy-MM-dd}' AS DATETIME) + CAST('{beginHour}' AS DATETIME)))";
            string nextDayTimeStr = Convert.ToString(database.GetScalarValue(sqlNextDayTime));
            return Convert.ToDateTime(nextDayTimeStr);
        }
        string ticketDay = created.ToString("dddd");
        string sqlDayCovered = $"SELECT top 1 COUNT(*) FROM vSDOrgDeskDef  WITH(NOLOCK) WHERE DaysCovered LIKE '%{ticketDay}%' AND Deskref='{deskRef}' AND OrgFk='{orgStaffFK}'";
        int dayCoveredCount = Convert.ToInt32(database.GetScalarValue(sqlDayCovered));
        string sqlDueMinutes = $"SELECT top 1 ISNULL(ResponseTime,0) FROM SD_Severity WITH(NOLOCK)  WHERE Deskref='{deskRef}' AND id='{severity}' AND OrgDeskRef='{orgStaffFK}'";
        int dueMinutes = Convert.ToInt32(database.GetScalarValue(sqlDueMinutes));

        string ticketTime = created.ToString("HH:mm:ss");

        if (dayCoveredCount > 0)
        {
            switch (hoursCovered)
            {
                case "UseTheseHours":
                    if (Convert.ToDateTime(ticketTime) >= Convert.ToDateTime(beginHour) && Convert.ToDateTime(ticketTime) <= Convert.ToDateTime(endHour) && (Convert.ToDateTime(endHour) - created).TotalMinutes <= dueMinutes)
                    {
                        DateTime nextDay = created.Date.AddDays(1);
                        string sqlNextDayTime = $"SELECT  DATEADD(MINUTE,0,(CAST('{nextDay:yyyy-MM-dd}' AS DATETIME) + CAST('{beginHour}' AS DATETIME)))";
                        string nextDayTimeStr = Convert.ToString(database.GetScalarValue(sqlNextDayTime));
                        return Convert.ToDateTime(nextDayTimeStr);
                    }
                    else if (Convert.ToDateTime(ticketTime) < Convert.ToDateTime(beginHour))
                    {
                        string sqlSameDayTime = $"SELECT DATEADD(MINUTE,0,(CAST('{created:yyyy-MM-dd}' AS DATETIME) + CAST('{beginHour}' AS DATETIME)))";
                        string sameDayTimeStr = Convert.ToString(database.GetScalarValue(sqlSameDayTime));
                        return Convert.ToDateTime(sameDayTimeStr);
                    }
                    else if (Convert.ToDateTime(ticketTime) > Convert.ToDateTime(endHour))
                    {
                        DateTime nextDay = created.Date.AddDays(1);
                        string sqlNextDayTime = $"SELECT DATEADD(MINUTE,0,(CAST('{nextDay:yyyy-MM-dd}' AS DATETIME) + CAST('{beginHour}' AS DATETIME)))";
                        string nextDayTimeStr = Convert.ToString(database.GetScalarValue(sqlNextDayTime));
                        return Convert.ToDateTime(nextDayTimeStr);
                    }
                    else if (Convert.ToDateTime(ticketTime) >= Convert.ToDateTime(beginHour) && Convert.ToDateTime(ticketTime) <= Convert.ToDateTime(endHour) && (Convert.ToDateTime(endHour) - created).TotalMinutes > dueMinutes)
                    {
                        string sqlAddMinutesWithinHours = $"SELECT DATEADD(MINUTE,{dueMinutes},(CAST('{created:yyyy-MM-dd HH:mm:ss}' AS DATETIME)))";
                        string addMinutesWithinHoursStr = Convert.ToString(database.GetScalarValue(sqlAddMinutesWithinHours));
                        return Convert.ToDateTime(addMinutesWithinHoursStr);
                    }
                    break;

                case "24hrCoverage":
                case "NoCoverage":
                    string sqlAddMinutes24hr = $"SELECT DATEADD(MINUTE,{dueMinutes},(CAST('{created:yyyy-MM-dd HH:mm:ss}' AS DATETIME)))";
                    string addMinutes24hrStr = Convert.ToString(database.GetScalarValue(sqlAddMinutes24hr));
                    return Convert.ToDateTime(addMinutes24hrStr);
            }
        }
        else if (hoursCovered == "NoCoverage")
        {
            string sqlAddMinutesNoCoverage = $"SELECT DATEADD(MINUTE,{dueMinutes},(CAST('{created:yyyy-MM-dd HH:mm:ss}' AS DATETIME)))";
            string addMinutesNoCoverageStr = Convert.ToString(database.GetScalarValue(sqlAddMinutesNoCoverage));
            return Convert.ToDateTime(addMinutesNoCoverageStr);
        }

        DateTime nextDayDefault = created.Date.AddDays(1);
        string sqlDefaultTime = $"SELECT DATEADD(MINUTE,0,(CAST('{nextDayDefault:yyyy-MM-dd}' AS DATETIME) + CAST('{beginHour}' AS DATETIME)))";
        string defaultTimeStr = Convert.ToString(database.GetScalarValue(sqlDefaultTime));
        return Convert.ToDateTime(defaultTimeStr);
    }
    public DateTime GetDueDateForResolution(DateTime created, string deskRef, string severity, string orgStaffFK)
    {
        string sqlHoursCovered = $"SELECT HoursCovered FROM vSDOrgDeskDef WITH(NOLOCK)  WHERE Deskref='{deskRef}' AND OrgFk='{orgStaffFK}'";
        string hoursCovered = Convert.ToString(database.GetScalarValue(sqlHoursCovered));
        string sqlBeginHour = $"SELECT BeginHour FROM vSDOrgDeskDef WITH(NOLOCK)  WHERE Deskref='{deskRef}' AND OrgFk='{orgStaffFK}'";
        string beginHour = Convert.ToString(database.GetScalarValue(sqlBeginHour));
        string sqlEndHour = $"SELECT EndHour FROM vSDOrgDeskDef WITH(NOLOCK)  WHERE Deskref='{deskRef}' AND OrgFk='{orgStaffFK}'";
        string endHour = Convert.ToString(database.GetScalarValue(sqlEndHour));
        string ticketDay = created.ToString("dddd");
        string sqlDayCovered = $"SELECT COUNT(*) FROM vSDOrgDeskDef WITH(NOLOCK)  WHERE DaysCovered LIKE '%{ticketDay}%' AND Deskref='{deskRef}' AND OrgFk='{orgStaffFK}'";
        int dayCoveredCount = Convert.ToInt32(database.GetScalarValue(sqlDayCovered));
        string sqlDueMinutes = $"SELECT ISNULL(ResolutionTime,0) FROM SD_Severity WITH(NOLOCK)  WHERE Deskref='{deskRef}' AND id='{severity}' AND OrgDeskRef='{orgStaffFK}'";
        int dueMinutes = Convert.ToInt32(database.GetScalarValue(sqlDueMinutes));

        string ticketTime = created.ToString("HH:mm:ss");

        if (dayCoveredCount > 0)
        {
            switch (hoursCovered)
            {
                case "UseTheseHours":
                    if (Convert.ToDateTime(ticketTime) >= Convert.ToDateTime(beginHour) && Convert.ToDateTime(ticketTime) <= Convert.ToDateTime(endHour) && (Convert.ToDateTime(endHour) - created).TotalMinutes <= dueMinutes)
                    {
                        DateTime nextDay = created.Date.AddDays(1);
                        string sqlNextDayTime = $"SELECT DATEADD(MINUTE,0,(CAST('{nextDay:yyyy-MM-dd}' AS DATETIME) + CAST('{beginHour}' AS DATETIME)))";
                        string nextDayTimeStr = Convert.ToString(database.GetScalarValue(sqlNextDayTime));
                        return Convert.ToDateTime(nextDayTimeStr);
                    }
                    else if (Convert.ToDateTime(ticketTime) < Convert.ToDateTime(beginHour))
                    {
                        string sqlSameDayTime = $"SELECT DATEADD(MINUTE,0,(CAST('{created:yyyy-MM-dd}' AS DATETIME) + CAST('{beginHour}' AS DATETIME)))";
                        string sameDayTimeStr = Convert.ToString(database.GetScalarValue(sqlSameDayTime));
                        return Convert.ToDateTime(sameDayTimeStr);
                    }
                    else if (Convert.ToDateTime(ticketTime) > Convert.ToDateTime(endHour))
                    {
                        DateTime nextDay = created.Date.AddDays(1);
                        string sqlNextDayTime = $"SELECT DATEADD(MINUTE,0,(CAST('{nextDay:yyyy-MM-dd}' AS DATETIME) + CAST('{beginHour}' AS DATETIME)))";
                        string nextDayTimeStr = Convert.ToString(database.GetScalarValue(sqlNextDayTime));
                        return Convert.ToDateTime(nextDayTimeStr);
                    }
                    else if (Convert.ToDateTime(ticketTime) >= Convert.ToDateTime(beginHour) && Convert.ToDateTime(ticketTime) <= Convert.ToDateTime(endHour) && (Convert.ToDateTime(endHour) - created).TotalMinutes > dueMinutes)
                    {
                        string sqlAddMinutesWithinHours = $"SELECT DATEADD(MINUTE,{dueMinutes},(CAST('{created:yyyy-MM-dd HH:mm:ss}' AS DATETIME)))";
                        string addMinutesWithinHoursStr = Convert.ToString(database.GetScalarValue(sqlAddMinutesWithinHours));
                        return Convert.ToDateTime(addMinutesWithinHoursStr);
                    }
                    break;

                case "24hrCoverage":
                case "NoCoverage":
                    string sqlAddMinutes24hr = $"SELECT DATEADD(MINUTE,{dueMinutes},(CAST('{created:yyyy-MM-dd HH:mm:ss}' AS DATETIME)))";
                    string addMinutes24hrStr = Convert.ToString(database.GetScalarValue(sqlAddMinutes24hr));
                    return Convert.ToDateTime(addMinutes24hrStr);
            }
        }
        else if (hoursCovered == "NoCoverage")
        {
            string sqlAddMinutesNoCoverage = $"SELECT DATEADD(MINUTE,{dueMinutes},(CAST('{created:yyyy-MM-dd HH:mm:ss}' AS DATETIME)))";
            string addMinutesNoCoverageStr = Convert.ToString(database.GetScalarValue(sqlAddMinutesNoCoverage));
            return Convert.ToDateTime(addMinutesNoCoverageStr);
        }
        DateTime nextDayDefault = created.Date.AddDays(1);
        string sqlDefaultTime = $"SELECT DATEADD(MINUTE,0,(CAST('{nextDayDefault:yyyy-MM-dd}' AS DATETIME) + CAST('{beginHour}' AS DATETIME)))";
        string defaultTimeStr = Convert.ToString(database.GetScalarValue(sqlDefaultTime));
        return Convert.ToDateTime(defaultTimeStr);
    }
    public DateTime GetDueDateForCategory(DateTime created, string deskRef, string categoryRef, string orgStaffFK)
    {
        TimeSpan ticketTime = created.TimeOfDay;
        string hoursCovered = string.Empty;
        TimeSpan beginHour = TimeSpan.Zero;
        TimeSpan endHour = TimeSpan.Zero;
        DateTime nextWorkingDay;
        int dueMinutes = 0;
        int minutesLeftToday = 0;
        int remainingMinutes = 0;
        string sqlConfig = $"SELECT TOP 1 HoursCovered, BeginHour, EndHour FROM vSDOrgDeskDef WITH(NOLOCK)  WHERE Deskref = '{deskRef}' AND OrgFk = '{orgStaffFK}'";
        var config = database.GetDataTable(sqlConfig);
        if (config.Rows.Count > 0)
        {
            hoursCovered = Convert.ToString(config.Rows[0]["HoursCovered"]);
            beginHour = TimeSpan.Parse(Convert.ToString(config.Rows[0]["BeginHour"]));
            endHour = TimeSpan.Parse(Convert.ToString(config.Rows[0]["EndHour"]));
        }
        string sqlDueMinutes = $"SELECT top 1 ISNULL(ResponseTime, 0) FROM SD_Category WITH(NOLOCK)  WHERE DeskRef = '{deskRef}' AND CategoryRef = '{categoryRef}' AND OrgDeskRef = '{orgStaffFK}'";
        dueMinutes = Convert.ToInt32(database.GetScalarValue(sqlDueMinutes));
        if (ticketTime > endHour)
        {
            nextWorkingDay = created.Date.AddDays(1);
            while (IsHolidayOrNonWorkingDay(nextWorkingDay, deskRef, orgStaffFK))
            {
                nextWorkingDay = nextWorkingDay.AddDays(1);
            }
            DateTime nextWorkingDayWithTime = nextWorkingDay.Add(beginHour);
            return nextWorkingDayWithTime.AddMinutes(dueMinutes);
        }
        if (ticketTime >= beginHour && ticketTime <= endHour)
        {
            TimeSpan remainingTime = endHour - ticketTime;
            minutesLeftToday = (int)remainingTime.TotalMinutes;
            if (minutesLeftToday >= dueMinutes)
            {
                return created.AddMinutes(dueMinutes);
            }
            else
            {
                remainingMinutes = dueMinutes - minutesLeftToday;
                nextWorkingDay = created.Date.AddDays(1);

                while (IsHolidayOrNonWorkingDay(nextWorkingDay, deskRef, orgStaffFK))
                {
                    nextWorkingDay = nextWorkingDay.AddDays(1);
                }
                DateTime nextWorkingDayWithTime = nextWorkingDay.Add(beginHour);
                return nextWorkingDayWithTime.AddMinutes(remainingMinutes);
            }
        }
        DateTime startOfDayWithBeginHour = created.Date.Add(beginHour);
        return startOfDayWithBeginHour.AddMinutes(dueMinutes);
    }
    private bool IsHolidayOrNonWorkingDay(DateTime date, string deskRef, string orgStaffFK)
    {
        string day = Convert.ToDateTime(date).ToString("yyyy-MM-dd");
        string holidayQuery = $"SELECT COUNT(1) FROM SD_Holidays WITH(NOLOCK)  WHERE CAST(HolidayDate AS DATE) = CAST('" + day + "'as Date) AND OrgID = '" + orgStaffFK + "'";
        int isHoliday = Convert.ToInt32(database.GetScalarValue(holidayQuery));

        if (isHoliday > 0)
        {
            return true;
        }
        string workingDayQuery = $"SELECT COUNT(1) FROM vSDOrgDeskDef WITH(NOLOCK)  WHERE DaysCovered LIKE '%{date.DayOfWeek}%' AND Deskref = '{deskRef}' AND OrgFk = '{orgStaffFK}'";
        int isWorkingDay = Convert.ToInt32(database.GetScalarValue(workingDayQuery));

        return isWorkingDay == 0;
    }
    public DateTime GetDueDateForCategoryResolution(DateTime created, string deskRef, string categoryRef, string orgStaffFK)
    {
        DateTime nextWorkingDay;
        TimeSpan ticketTime = created.TimeOfDay;
        TimeSpan beginHour = TimeSpan.Zero;
        TimeSpan endHour = TimeSpan.Zero;
        int dueMinutes = 0;
        int minutesLeftToday = 0;
        int remainingMinutes = 0;
        string sqlConfig = $"SELECT TOP 1 HoursCovered, BeginHour, EndHour FROM vSDOrgDeskDef WITH(NOLOCK)  WHERE Deskref = '{deskRef}' AND OrgFk = '{orgStaffFK}'";
        var config = database.GetDataTable(sqlConfig);
        if (config.Rows.Count > 0)
        {
            beginHour = TimeSpan.Parse(config.Rows[0]["BeginHour"].ToString());
            endHour = TimeSpan.Parse(config.Rows[0]["EndHour"].ToString());
        }
        string sqlDueMinutes = $"SELECT top 1 ISNULL(ResolutionTime, 0) FROM SD_Category WITH(NOLOCK) WHERE DeskRef = '{deskRef}' AND CategoryRef = '{categoryRef}' AND OrgDeskRef = '{orgStaffFK}'";
        dueMinutes = Convert.ToInt32(database.GetScalarValue(sqlDueMinutes));

        if (ticketTime > endHour)
        {
            nextWorkingDay = created.Date.AddDays(1);
            while (IsHolidayOrNonWorkingDay(nextWorkingDay, deskRef, orgStaffFK))
            {
                nextWorkingDay = nextWorkingDay.AddDays(1);
            }

            DateTime nextWorkingDayWithTime = nextWorkingDay.Add(beginHour);
            return nextWorkingDayWithTime.AddMinutes(dueMinutes);
        }

        if (ticketTime >= beginHour && ticketTime <= endHour)
        {
            TimeSpan remainingTime = endHour - ticketTime;
            minutesLeftToday = (int)remainingTime.TotalMinutes;
            if (minutesLeftToday >= dueMinutes)
            {
                return created.AddMinutes(dueMinutes);
            }
            else
            {
                remainingMinutes = dueMinutes - minutesLeftToday;
                nextWorkingDay = created.Date.AddDays(1);

                while (IsHolidayOrNonWorkingDay(nextWorkingDay, deskRef, orgStaffFK))
                {
                    nextWorkingDay = nextWorkingDay.AddDays(1);
                }

                DateTime nextWorkingDayWithTime = nextWorkingDay.Add(beginHour);
                return nextWorkingDayWithTime.AddMinutes(remainingMinutes);
            }
        }

        DateTime startOfDayWithBeginHour = created.Date.Add(beginHour);
        return startOfDayWithBeginHour.AddMinutes(dueMinutes);
    }
    public DateTime GetDueDateForPriority(DateTime created, string deskRef, string PriorityID, string orgStaffFK)
    {
        TimeSpan ticketTime = created.TimeOfDay;
        string hoursCovered = string.Empty;
        TimeSpan beginHour = TimeSpan.Zero;
        TimeSpan endHour = TimeSpan.Zero;
        DateTime nextWorkingDay;
        int dueMinutes = 0;
        int minutesLeftToday = 0;
        int remainingMinutes = 0;
        string sqlConfig = $"SELECT TOP 1 HoursCovered, BeginHour, EndHour FROM vSDOrgDeskDef WITH(NOLOCK)  WHERE Deskref = '{deskRef}' AND OrgFk = '{orgStaffFK}'";
        var config = database.GetDataTable(sqlConfig);
        if (config.Rows.Count > 0)
        {
            hoursCovered = Convert.ToString(config.Rows[0]["HoursCovered"]);
            beginHour = TimeSpan.Parse(Convert.ToString(config.Rows[0]["BeginHour"]));
            endHour = TimeSpan.Parse(Convert.ToString(config.Rows[0]["EndHour"]));
        }

        string sqlDueMinutes = $"SELECT ISNULL(ResponseTime, 0) FROM SD_Priority WITH(NOLOCK)  WHERE DeskRef = '{deskRef}' AND ID = '{PriorityID}' AND OrgDeskRef = '{orgStaffFK}'";
        dueMinutes = Convert.ToInt32(database.GetScalarValue(sqlDueMinutes));

        if (ticketTime > endHour)
        {
            nextWorkingDay = created.Date.AddDays(1);
            while (IsHolidayOrNonWorkingDay(nextWorkingDay, deskRef, orgStaffFK))
            {
                nextWorkingDay = nextWorkingDay.AddDays(1);
            }
            DateTime nextWorkingDayWithTime = nextWorkingDay.Add(beginHour);
            return nextWorkingDayWithTime.AddMinutes(dueMinutes);
        }

        if (ticketTime >= beginHour && ticketTime <= endHour)
        {
            TimeSpan remainingTime = endHour - ticketTime;
            minutesLeftToday = (int)remainingTime.TotalMinutes;
            if (minutesLeftToday >= dueMinutes)
            {
                return created.AddMinutes(dueMinutes);
            }
            else
            {
                remainingMinutes = dueMinutes - minutesLeftToday;
                nextWorkingDay = created.Date.AddDays(1);

                while (IsHolidayOrNonWorkingDay(nextWorkingDay, deskRef, orgStaffFK))
                {
                    nextWorkingDay = nextWorkingDay.AddDays(1);
                }
                DateTime nextWorkingDayWithTime = nextWorkingDay.Add(beginHour);
                return nextWorkingDayWithTime.AddMinutes(remainingMinutes);
            }
        }

        DateTime startOfDayWithBeginHour = created.Date.Add(beginHour);
        return startOfDayWithBeginHour.AddMinutes(dueMinutes);
    }
    public DateTime GetDueDateForPriorityResolution(DateTime created, string deskRef, string PriorityID, string orgStaffFK)
    {
        TimeSpan ticketTime = created.TimeOfDay;
        string hoursCovered = string.Empty;
        TimeSpan beginHour = TimeSpan.Zero;
        TimeSpan endHour = TimeSpan.Zero;
        DateTime nextWorkingDay;
        int dueMinutes = 0;
        int minutesLeftToday = 0;
        int remainingMinutes = 0;
        string sqlConfig = $"SELECT TOP 1 HoursCovered, BeginHour, EndHour FROM vSDOrgDeskDef WITH(NOLOCK)  WHERE Deskref = '{deskRef}' AND OrgFk = '{orgStaffFK}'";
        var config = database.GetDataTable(sqlConfig);
        if (config.Rows.Count > 0)
        {
            hoursCovered = Convert.ToString(config.Rows[0]["HoursCovered"]);
            beginHour = TimeSpan.Parse(Convert.ToString(config.Rows[0]["BeginHour"]));
            endHour = TimeSpan.Parse(Convert.ToString(config.Rows[0]["EndHour"]));
        }

        string sqlDueMinutes = $"SELECT ISNULL(ResolutionTime, 0) FROM SD_Priority WITH(NOLOCK)  WHERE DeskRef = '{deskRef}' AND ID = '{PriorityID}' AND OrgDeskRef = '{orgStaffFK}'";
        dueMinutes = Convert.ToInt32(database.GetScalarValue(sqlDueMinutes));

        if (ticketTime > endHour)
        {
            nextWorkingDay = created.Date.AddDays(1);
            while (IsHolidayOrNonWorkingDay(nextWorkingDay, deskRef, orgStaffFK))
            {
                nextWorkingDay = nextWorkingDay.AddDays(1);
            }
            DateTime nextWorkingDayWithTime = nextWorkingDay.Add(beginHour);
            return nextWorkingDayWithTime.AddMinutes(dueMinutes);
        }

        if (ticketTime >= beginHour && ticketTime <= endHour)
        {
            TimeSpan remainingTime = endHour - ticketTime;
            minutesLeftToday = (int)remainingTime.TotalMinutes;
            if (minutesLeftToday >= dueMinutes)
            {
                return created.AddMinutes(dueMinutes);
            }
            else
            {
                remainingMinutes = dueMinutes - minutesLeftToday;
                nextWorkingDay = created.Date.AddDays(1);

                while (IsHolidayOrNonWorkingDay(nextWorkingDay, deskRef, orgStaffFK))
                {
                    nextWorkingDay = nextWorkingDay.AddDays(1);
                }
                DateTime nextWorkingDayWithTime = nextWorkingDay.Add(beginHour);
                return nextWorkingDayWithTime.AddMinutes(remainingMinutes);
            }
        }

        DateTime startOfDayWithBeginHour = created.Date.Add(beginHour);
        return startOfDayWithBeginHour.AddMinutes(dueMinutes);
    }

}
