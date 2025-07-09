import { useEffect, useState } from "react";
import {
  getLessonsByStudentId,
  getAllTeachers,
  getAllSubjects,
} from "../../services/api";

import type { Lesson } from "../../models/lessonModel";
import type { User } from "../../models/userModel";
import type { Subject } from "../../models/subjectModel";

const StudentLessonHistory = () => {
  const user = JSON.parse(localStorage.getItem("user") || "{}");
  const studentId = user?.userId;

  const [lessons, setLessons] = useState<Lesson[]>([]);
  const [teachers, setTeachers] = useState<User[]>([]);
  const [subjects, setSubjects] = useState<Subject[]>([]);

useEffect(() => {
  const fetchData = async () => {
    if (!studentId) return;

    try {
      const lessonsRes = await getLessonsByStudentId(studentId);
      const lessonsData = lessonsRes.data as Lesson[];
      console.log("lessons from API", lessonsData);
      setLessons(lessonsData);

      const teachersRes = await getAllTeachers();
      setTeachers(teachersRes.data);

      const subjectsRes = await getAllSubjects();
      setSubjects(subjectsRes.data);
    } catch (error) {
      console.error("Error fetching data:", error);
    }
  };

  fetchData();
}, [studentId]);


  const getTeacherName = (id: number) => {
    const teacher = teachers.find((t) => t.userId === id);
    return teacher ? `${teacher.firstName} ${teacher.lastName}` : "לא ידוע";
  };

  const getSubjectName = (id: number) => {
    const subject = subjects.find((s) => s.subjectId === id);
    return subject ? subject.name : "לא ידוע";
  };

  const now = new Date();
const validLessons = lessons.filter((l): l is Lesson => !!l);

const pastLessons = validLessons.filter(
  (l) =>
    l.status === "passed" &&
    new Date(`${l.lessonDate}T${l.endTime}`) < now
);

const canceledLessons = validLessons.filter(
  (l) => l.status === "canceled"
);

const upcomingLessons = validLessons.filter(
  (l) =>
    l.status === "booked" &&
    new Date(`${l.lessonDate}T${l.endTime}`) >= now
);
  

  const renderLesson = (l: Lesson) => (
    <li key={l.lessonId}>
      <strong>תאריך:</strong> {l.lessonDate} |
      <strong> שעה:</strong> {l.startTime} - {l.endTime} |
      <strong> מורה:</strong> {getTeacherName(l.teacherId)} |
      <strong> מקצוע:</strong> {getSubjectName(l.subjectId)} |
      <strong> גילאים:</strong> {l.minAge}-{l.maxAge} |
      <strong> מגדר:</strong> {l.gender === "M" ? "זכר" : "נקבה"}
    </li>
  );

  return (
    <div>
      <h2>שיעורים עתידיים</h2>
      <ul>{upcomingLessons.map(renderLesson)}</ul>

      <h2>שיעורים שעברו</h2>
      <ul>{pastLessons.map(renderLesson)}</ul>

      <h2>שיעורים שבוטלו</h2>
      <ul>{canceledLessons.map(renderLesson)}</ul>
    </div>
  );
};

export default StudentLessonHistory;
