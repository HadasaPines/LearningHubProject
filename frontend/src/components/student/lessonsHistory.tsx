import { useEffect, useState } from "react";
import {
  getLessonsByStudentId,
  getAllTeachers,
  getAllSubjects,
  deleteRegistrationByLessonId,
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
  const [loadingLessonId, setLoadingLessonId] = useState<number | null>(null);
  const [alert, setAlert] = useState<string>("");

  useEffect(() => {
    const fetchData = async () => {
      if (!studentId) return;

      try {
        const lessonsRes = await getLessonsByStudentId(studentId);
        const lessonsData = lessonsRes.data as Lesson[];
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

const handleCancelClick = async (lesson: Lesson) => {
  const lessonDateTime = new Date(`${lesson.lessonDate}T${lesson.startTime}`);
  const now = new Date();
  const diffMs = lessonDateTime.getTime() - now.getTime();
  const diffHours = diffMs / (1000 * 60 * 60);

  let message = "האם את/ה בטוח/ה שברצונך לבטל את ההשתתפות בשיעור?";
  if (diffHours < 24) {
    message += "\nשימו לב: ביטול פחות מ-24 שעות לפני השיעור יחייב בתשלום מלא.";
  }

  const confirmed = window.confirm(message);
  if (!confirmed) return;

  try {
    setAlert("");
    setLoadingLessonId(lesson.lessonId);

    await deleteRegistrationByLessonId(lesson.lessonId);

    // מסירים את השיעור מהרשימה לגמרי
    setLessons((prev) => prev.filter((l) => l.lessonId !== lesson.lessonId));

  } catch (err) {
    console.error("Error cancelling registration:", err);
    setAlert("אירעה שגיאה בעת ביטול השיעור.");
  } finally {
    setLoadingLessonId(null);
  }
};


  const renderLesson = (l: Lesson, withCancel = false) => (
    <li key={l.lessonId}>
      <strong>תאריך:</strong> {l.lessonDate} |
      <strong> שעה:</strong> {l.startTime} - {l.endTime} |
      <strong> מורה:</strong> {getTeacherName(l.teacherId)} |
      <strong> מקצוע:</strong> {getSubjectName(l.subjectId)} |
      <strong> גילאים:</strong> {l.minAge}-{l.maxAge} |
      <strong> מגדר:</strong> {l.gender === "M" ? "זכר" : "נקבה"}{" "}
      {withCancel && (
        <button
          onClick={() => handleCancelClick(l)}
          disabled={loadingLessonId === l.lessonId}
          style={{ marginRight: "10px" }}
        >
          {loadingLessonId === l.lessonId ? "מבטל..." : "בטל השתתפות"}
        </button>
      )}
    </li>
  );

  return (
    <div>
      <h2>שיעורים עתידיים</h2>
      <ul>{upcomingLessons.map((l) => renderLesson(l, true))}</ul>

      <h2>שיעורים שעברו</h2>
      <ul>{pastLessons.map((l) => renderLesson(l))}</ul>

      <h2>שיעורים שבוטלו</h2>
      <ul>{canceledLessons.map((l) => renderLesson(l))}</ul>

      {alert && <p style={{ color: "red" }}>{alert}</p>}
    </div>
  );
};

export default StudentLessonHistory;
