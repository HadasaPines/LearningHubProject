import React, { useState } from "react";
import type { Lesson } from "../models/lessonModel";
import type { User } from "../models/userModel";
import type { Subject } from "../models/subjectModel";
import styles from "./calendarView.module.scss";
import dayjs from "dayjs";
import clsx from "clsx";
import { BsPinAngleFill } from "react-icons/bs";
import { FaAngleDoubleLeft,FaAngleDoubleRight} from "react-icons/fa";
import { FaChevronLeft, FaChevronRight } from "react-icons/fa";

interface Props {
  lessons: Lesson[];
  teachers: User[];
  subjects: Subject[];
  onRegister: (lesson: Lesson) => void;
}

const CalendarView: React.FC<Props> = ({
  lessons,
  teachers,
  subjects,
  onRegister,
}) => {
  const today = dayjs();
  const [currentMonth, setCurrentMonth] = useState(dayjs().startOf("month"));
  const [selectedDate, setSelectedDate] = useState<dayjs.Dayjs | null>(null);
  const [popupOpen, setPopupOpen] = useState(true); 

  const lessonsByDate = lessons.reduce((acc, lesson) => {
    const date = dayjs(lesson.lessonDate).format("YYYY-MM-DD");
    if (!acc[date]) acc[date] = [];
    acc[date].push(lesson);
    return acc;
  }, {} as Record<string, Lesson[]>);

  const startOfMonth = currentMonth.startOf("month");
  const endOfMonth = currentMonth.endOf("month");
  const startDay = startOfMonth.startOf("week");
  const endDay = endOfMonth.endOf("week");

  const calendarDays = [];
  let date = startDay;

  while (date.isBefore(endDay, "day") || date.isSame(endDay, "day")) {
    calendarDays.push(date);
    date = date.add(1, "day");
  }

  const getTeacherName = (id: number) => {
    const teacher = teachers.find((t) => t.userId === id);
    return teacher ? `${teacher.firstName} ${teacher.lastName}` : "Unknown";
  };

  const getSubjectName = (id: number) => {
    const subject = subjects.find((s) => s.subjectId === id);
    return subject ? subject.name : "Unknown";
  };

  const handlePrevMonth = () => {
    setCurrentMonth(currentMonth.subtract(1, "month"));
    setSelectedDate(null);
  };

  const handleNextMonth = () => {
    setCurrentMonth(currentMonth.add(1, "month"));
    setSelectedDate(null);
  };

  const handlePrevYear = () => {
    setCurrentMonth(currentMonth.subtract(1, "year"));
    setSelectedDate(null);
  };

  const handleNextYear = () => {
    setCurrentMonth(currentMonth.add(1, "year"));
    setSelectedDate(null);
  };

  const isToday = (date: dayjs.Dayjs) => date.isSame(today, "day");
  const isSelected = (date: dayjs.Dayjs) => selectedDate?.isSame(date, "day");
  const hasLessons = (date: dayjs.Dayjs) => {
    const dateStr = date.format("YYYY-MM-DD");
    return lessonsByDate[dateStr]?.length > 0;
  };

  return (
    <div className={styles.calendar}>
      <div className={styles.monthNavigation}>
        <button
          className={styles["action-button"]}
          onClick={handlePrevYear}
          aria-label="Previous Year"
          title="Previous Year"
        >
           <FaAngleDoubleLeft />
        </button>
        <button
          className={styles["action-button"]}
          onClick={handlePrevMonth}
          aria-label="Previous Month"
          title="Previous Month"
        >
         <FaChevronLeft />
        </button>

        <div className={styles.monthYearLabel}>
          {currentMonth.format("MMMM YYYY")}
        </div>

        <button
          className={styles["action-button"]}
          onClick={handleNextMonth}
          aria-label="Next Month"
          title="Next Month"
        >
          <FaChevronRight />
        </button>
        <button
          className={styles["action-button"]}
          onClick={handleNextYear}
          aria-label="Next Year"
          title="Next Year"
        >
          <FaAngleDoubleRight/>
        </button>
      </div>

      <div className={styles.header}>
  {["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"].map((day) => (
    <div key={day} className={styles.headerCell}>
      {day}
    </div>
  ))}
</div>


      <div className={styles.days}>
        {calendarDays.map((day) => {
          const dateStr = day.format( "YYYY-MM-DD");
          const has = hasLessons(day);
          const isSel = isSelected(day);

          return (
            <div
              key={dateStr}
              className={clsx(styles.day, {
                [styles.today]: isToday(day),
                [styles.selected]: isSel,
                [styles.clickable]: has,
              })}
              onClick={() => {
                if (has) {
                  if (selectedDate?.isSame(day, "day")) {
                    setSelectedDate(null);
                    setPopupOpen(false);
                  } else {
                    setSelectedDate(day);
                    setPopupOpen(true);
                  }
                }
              }}
            >
              <div className={styles.dayNumber}>{day.date()}</div>

           {has && <BsPinAngleFill  className={styles["task-pin"]} />}

              {isSel && has && popupOpen && (
                <div className={styles.lessonPopup}>
                  {lessonsByDate[dateStr].map((lesson, idx) => (
                    <div key={idx} className={styles.lessonItem}>
                      <span>
                        {lesson.startTime}–{lesson.endTime} |{" "}
                        {getSubjectName(lesson.subjectId)} |{" "}
                        {getTeacherName(lesson.teacherId)}
                      </span>

                      {lesson.status === "Available" ? (
                        <button onClick={() => onRegister(lesson)}>Register</button>
                      ) : (
                        <span className={styles.booked}>Booked ❌</span>
                      )}
                    </div>
                  ))}
                </div>
              )}
            </div>
          );
        })}
      </div>
    </div>
  );
};

export default CalendarView;
