import React, { useState, useEffect } from "react";
import Calendar from "react-calendar";
import 'react-calendar/dist/Calendar.css';

import type { Lesson } from "../../models/lessonModel";
import type { LessonDetails } from "../../models/lessonDetailsModel";
//import type { Teacher } from "../../models/teacherModel";
import type { Subject } from "../../models/subjectModel";
import type { StudentDetails, User } from "../../models/userModel";
import type { Registration } from "../../models/registerationModel";

import { parseApiError } from "../../utils/apiErrorParser";
import {
  getAllTeachers,
  getAllSubjects,
  getLessonsByDetails,
  addRegistration
} from "../../services/api";

const RegisterLessonForm: React.FC = () => {
  const [teachers, setTeachers] = useState<User[]>([]);
  const [subjects, setSubjects] = useState<Subject[]>([]);
  const [lessons, setLessons] = useState<Lesson[]>([]);
  const [selectedDate, setSelectedDate] = useState<string | null>(null);
  const [errorMessages, setErrorMessages] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);

  const user = localStorage.getItem("user");
  if (!user) return <div>משתמש לא מחובר</div>;
  const userData: User = JSON.parse(user);
  const student: StudentDetails = userData.student;

  const [formData, setFormData] = useState<LessonDetails>({
    teacherId: undefined,
    subjectId: undefined,
    startTime: "",
    endTime: "",
    specificDate: "",
    dateFrom: "",
    dateTo: "",
    age: student.age,
    status: "",
    gender: student.gender,
  });

  useEffect(() => {
    const fetchData = async () => {
      try {
        const teachersRes = await getAllTeachers();
        const subjectsRes = await getAllSubjects();
        setTeachers(teachersRes.data);
        setSubjects(subjectsRes.data);
      } catch (error) {
        setErrorMessages(parseApiError(error));
      }
    };
    fetchData();
  }, []);

  useEffect(() => {
    const fetchLessons = async () => {
      try {
        const response = await getLessonsByDetails(formData);
        console.log("Fetched lessons:", response.data);
        setLessons(response.data);
        setErrorMessages(null);
      } catch (error) {
        setErrorMessages(parseApiError(error));
      }
    };
    fetchLessons();
  }, [formData]);

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: ["teacherId", "subjectId", "age"].includes(name)
        ? value === "" ? undefined : Number(value)
        : value,
    }));
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    setSelectedDate(null);
  };

  const registerToLesson = async (lesson: Lesson) => {
    const registration: Registration = {
      studentId: userData.userId,
      lessonId: lesson.lessonId,
    };
    try {
      await addRegistration(registration);
      setSuccessMessage("נרשמת בהצלחה לשיעור!");
    } catch (error) {
      setErrorMessages("שגיאה בהרשמה: " + parseApiError(error));
    }
  };

  const lessonsByDate = lessons.reduce((acc, lesson) => {
    const date = lesson.lessonDate;
    if (!acc[date]) acc[date] = [];
    acc[date].push(lesson);
    return acc;
  }, {} as Record<string, Lesson[]>);

  const formatDate = (date: Date) => date.toISOString().split("T")[0];

  const getTeacherName = (id: number) => {
    const teacher = teachers.find((t) => t.userId === id);
    return teacher ? `${teacher.firstName} ${teacher.lastName}` : "לא ידוע";
  };

  const getSubjectName = (id: number) => {
    const subject = subjects.find((s) => s.subjectId === id);
    return subject ? subject.name : "לא ידוע";
  };

  return (
    <>
      {errorMessages && <div role="alert" style={{ color: "red" }}>{errorMessages}</div>}
      {successMessage && <div style={{ color: "green" }}>{successMessage}</div>}

      <div dir="rtl" style={{ display: "flex", gap: "2rem", alignItems: "flex-start" }}>

        <form onSubmit={handleSubmit} style={{ width: "320px" }}>
          <h2>סנן שיעורים</h2>

          <div>
            <label>מורה:</label>
            <select name="teacherId" value={formData.teacherId ?? ""} onChange={handleChange}>
              <option value="">בחר מורה</option>
              {teachers.map((teacher) => (
                <option key={teacher.userId} value={teacher.userId}>
                  {teacher.firstName} {teacher.lastName}
                </option>
              ))}
            </select>
          </div>

          <div>
            <label>מקצוע:</label>
            <select name="subjectId" value={formData.subjectId ?? ""} onChange={handleChange}>
              <option value="">בחר מקצוע</option>
              {subjects.map((subject) => (
                <option key={subject.subjectId} value={subject.subjectId}>
                  {subject.name}
                </option>
              ))}
            </select>
          </div>

          <div><label>שעת התחלה:</label><input type="time" name="startTime" value={formData.startTime} onChange={handleChange} /></div>
          <div><label>שעת סיום:</label><input type="time" name="endTime" value={formData.endTime} onChange={handleChange} /></div>
          <div><label>תאריך מסוים:</label><input type="date" name="specificDate" value={formData.specificDate} onChange={handleChange} /></div>
          <div><label>מתאריך:</label><input type="date" name="dateFrom" value={formData.dateFrom} onChange={handleChange} /></div>
          <div><label>עד תאריך:</label><input type="date" name="dateTo" value={formData.dateTo} onChange={handleChange} /></div>

          {/* <button type="submit" style={{ marginTop: "1em" }}>עדכן סינון</button> */}
        </form>

  
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
              return <span>{hasAvailable ? "📌" : "❌"}</span>;
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
                      <button onClick={() => registerToLesson(lesson)}>הירשם</button>
                    ) : (
                      <span style={{ color: "gray", fontWeight: "bold", marginRight: "10px" }}>תפוס ❌</span>
                    )}
                  </li>
                ))}
              </ul>
            </div>
          )}
        </div>
      </div>
    </>
  );
};

export default RegisterLessonForm;
