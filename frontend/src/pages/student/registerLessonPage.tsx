import React, { useState, useEffect } from "react";
import Calendar from "react-calendar";
import 'react-calendar/dist/Calendar.css';

import type { Lesson } from "../../models/lessonModel";
import type { LessonDetails } from "../../models/lessonDetailsModel";
import type { Teacher } from "../../models/teacherModel";
import type { Subject } from "../../models/subjectModel";

import { parseApiError } from "../../utils/apiErrorParser";
import { getAllTechers, getLessonsByDetails, getAllSubjects, registerToLesson } from "../../services/api";

const RegisterLessonForm: React.FC = () => {
  const [errorMessages, setErrorMessages] = useState<string | null>(null);
  const [teachers, setTeachers] = useState<Teacher[]>([]);
  const [subjects, setSubjects] = useState<Subject[]>([]);
  const [lessons, setLessons] = useState<Lesson[]>([]);
  const [showForm, setShowForm] = useState(true);
  const [selectedDate, setSelectedDate] = useState<string | null>(null);

  const [formData, setFormData] = useState<LessonDetails>({
    teacherId: undefined,
    subjectId: undefined,
    startTime: "",
    endTime: "",
    specificDate: "",
    dateFrom: "",
    dateTo: "",
    age: undefined,
    status: "",
    gender: undefined,
  });

  useEffect(() => {
    const fetchTeachersAndSubjects = async () => {
      try {
        const teachers = (await getAllTechers()).data;
        const subjects = (await getAllSubjects()).data;
        setTeachers(teachers);
        setSubjects(subjects);
      } catch (error) {
        setErrorMessages(parseApiError(error));
      }
    };
    fetchTeachersAndSubjects();
  }, []);

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

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const response = await getLessonsByDetails(formData);
      setLessons(response.data);
      setShowForm(false);
      setErrorMessages(null);
    } catch (error: any) {
      setErrorMessages(parseApiError(error));
    }
  };

  // const registerToLessonn = async (lesson: Lesson) => {
  //   try {
  //     await registerToLesson(lesson); 
  //     alert("נרשמת בהצלחה לשיעור!");
  //   } catch (error) {
  //     alert("שגיאה בהרשמה: " + parseApiError(error));
  //   }
  // };

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
      {errorMessages && (
        <div role="alert" aria-live="assertive">
          {errorMessages}
        </div>
      )}

      {showForm ? (
        <form onSubmit={handleSubmit}>
          <div>
            <label>Teacher:</label>
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
            <label>Subject:</label>
            <select name="subjectId" value={formData.subjectId ?? ""} onChange={handleChange}>
              <option value="">בחר מקצוע</option>
              {subjects.map((subject) => (
                <option key={subject.subjectId} value={subject.subjectId}>
                  {subject.name}
                </option>
              ))}
            </select>
          </div>

          <div>
            <label>Start Time:</label>
            <input
              type="time"
              name="startTime"
              value={formData.startTime}
              onChange={handleChange}
            />
          </div>

          <div>
            <label>End Time:</label>
            <input
              type="time"
              name="endTime"
              value={formData.endTime}
              onChange={handleChange}
            />
          </div>

          <div>
            <label>Specific Date:</label>
            <input
              type="date"
              name="specificDate"
              value={formData.specificDate}
              onChange={handleChange}
            />
          </div>

          <div>
            <label>Date From:</label>
            <input
              type="date"
              name="dateFrom"
              value={formData.dateFrom}
              onChange={handleChange}
            />
          </div>

          <div>
            <label>Date To:</label>
            <input
              type="date"
              name="dateTo"
              value={formData.dateTo}
              onChange={handleChange}
            />
          </div>

          <div>
            <label>Age:</label>
            <input
              type="number"
              name="age"
              value={formData.age ?? ""}
              onChange={handleChange}
            />
          </div>

          <div>
            <label>Gender:</label>
            <select
              name="gender"
              value={formData.gender ?? ""}
              onChange={handleChange}
            >
              <option value="">בחר מגדר</option>
              <option value="M">זכר</option>
              <option value="F">נקבה</option>
            </select>
          </div>

          <button type="submit">חפש שיעורים</button>
        </form>
      ) : (
        <>
          <h2>לוח שיעורים</h2>
          <Calendar
            onClickDay={(value) => {
              const dateStr = formatDate(value);
              if (lessonsByDate[dateStr]) {
                setSelectedDate(dateStr);
              } else {
                setSelectedDate(null);
              }
            }}
            tileContent={({ date }) => {
              const dateStr = formatDate(date);
              return lessonsByDate[dateStr] ? <span>📌</span> : null;
            }}
          />

          {selectedDate && (
            <div>
              <h3>שיעורים בתאריך {new Date(selectedDate).toLocaleDateString("he-IL")}</h3>
              <ul>
                {lessonsByDate[selectedDate].map((lesson, index) => (
                  <li key={index}>
                    {lesson.startTime} - {lesson.endTime} | מורה: {getTeacherName(lesson.teacherId)} | מקצוע: {getSubjectName(lesson.subjectId)}{" "}
                    <button onClick={() => registerToLesson(lesson)}>הירשם</button>
                  </li>
                ))}
              </ul>
            </div>
          )}
        </>
      )}
    </>
  );
};

export default RegisterLessonForm;
