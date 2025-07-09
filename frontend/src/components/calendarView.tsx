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
    return teacher ? `${teacher.firstName} ${teacher.lastName}` : "Unknown";
  };

  const getSubjectName = (id: number) => {
    const subject = subjects.find((s) => s.subjectId === id);
    return subject ? subject.name : "Unknown";
  };

  return (
    <div style={{ flex: 1 }}>
      <h2>Lesson Calendar</h2>
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
          return hasAvailable ? <span title="Available">📌</span> : <span title="Booked">❌</span>;
        }}
      />

{selectedDate && (
  <div style={{ marginTop: "1em" }}>
    <h3>Lessons on {new Date(selectedDate).toLocaleDateString("en-US")}</h3>
    <ul>
      {lessonsByDate[selectedDate].map((lesson, index) => (
        <li
          key={index}
          style={{
            display: "flex",
            justifyContent: "space-between",
            alignItems: "center",
            direction: "rtl"   
          }}
        >
          <span>
            {lesson.startTime} - {lesson.endTime} | Teacher: {getTeacherName(lesson.teacherId)} | Subject: {getSubjectName(lesson.subjectId)}
          </span>

          {lesson.status === "Available" ? (
            <button onClick={() => onRegister(lesson)}>Register</button>
          ) : (
            <span style={{ color: "gray", fontWeight: "bold", marginLeft: "10px" }}>
              Booked ❌
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
