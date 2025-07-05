import React, { useState } from "react";
import Calendar from "react-calendar";
import type { Lesson } from "../models/lessonModel";
import type { User } from "../models/userModel";
import type { Subject } from "../models/subjectModel";

interface Props {
  lessons: Lesson[];
  teachers: User[];
  subjects: Subject[];
  onRegister: (lesson: Lesson) => void;
}

const CalendarView: React.FC<Props> = ({ lessons, teachers, subjects, onRegister }) => {
  const [selectedDate, setSelectedDate] = useState<string | null>(null);

  const lessonsByDate = lessons.reduce((acc, lesson) => {
    const date = lesson.lessonDate;
    if (!acc[date]) acc[date] = [];
    acc[date].push(lesson);
    return acc;
  }, {} as Record<string, Lesson[]>);

  const formatDate = (date: Date) =>
  date.toLocaleDateString("sv-SE"); 

  const getTeacherName = (id: number) => {
    const teacher = teachers.find((t) => t.userId === id);
    return teacher ? `${teacher.firstName} ${teacher.lastName}` : "לא ידוע";
  };

  const getSubjectName = (id: number) => {
    const subject = subjects.find((s) => s.subjectId === id);
    return subject ? subject.name : "לא ידוע";
  };

  return (
    <div style={{ flex: 1 }}>
      <h2>לוח שיעורים</h2>
      <Calendar
        calendarType="hebrew"
        onClickDay={(value) => {
          const dateStr = formatDate(value);
          setSelectedDate(lessonsByDate[dateStr] ? dateStr : null);
        }}
        tileContent={({ date }) => {
          const dateStr = formatDate(date);
          const lessonsForDate = lessonsByDate[dateStr];
          if (!lessonsForDate) return null;
          const hasAvailable = lessonsForDate.some((l) => l.status === "Available");
          return hasAvailable ? <span title="פנוי">📌</span> : <span title="תפוס">❌</span>;
        }}
      />

      {selectedDate && (
        <div style={{ marginTop: "1em" }}>
          <h3>שיעורים בתאריך {new Date(selectedDate).toLocaleDateString("he-IL")}</h3>
          <ul>
            {lessonsByDate[selectedDate].map((lesson, index) => (
              <li key={index}>
                {lesson.startTime} - {lesson.endTime} | מורה: {getTeacherName(lesson.teacherId)} | מקצוע: {getSubjectName(lesson.subjectId)}{" "}
                {lesson.status === "Available" ? (
                  <button onClick={() => onRegister(lesson)}>הירשם</button>
                ) : (
                  <span style={{ color: "gray", fontWeight: "bold", marginRight: "10px" }}>
                    תפוס ❌
                  </span>
                )}
              </li>
            ))}
          </ul>
        </div>
      )}
    </div>
  );
};

export default CalendarView;
