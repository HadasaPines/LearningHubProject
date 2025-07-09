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
    return teacher ? `${teacher.firstName} ${teacher.lastName}` : "Unknown";
  };

  const getSubjectName = (id: number) => {
    const subject = subjects.find((s) => s.subjectId === id);
    return subject ? subject.name : "Unknown";
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

  let message = "Are you sure you want to cancel your participation in the lesson?";
  if (diffHours < 24) {
    message += "\nNote: Cancelling less than 24 hours before the lesson will incur a full charge.";
  }

  const confirmed = window.confirm(message);
  if (!confirmed) return;

  try {
    setAlert("");
    setLoadingLessonId(lesson.lessonId);

    await deleteRegistrationByLessonId(lesson.lessonId);

    // Remove the lesson from the list entirely
    setLessons((prev) => prev.filter((l) => l.lessonId !== lesson.lessonId));

  } catch (err) {
    console.error("Error cancelling registration:", err);
    setAlert("An error occurred while canceling the lesson.");
  } finally {
    setLoadingLessonId(null);
  }
};


  const renderLesson = (l: Lesson, withCancel = false) => (
    <li key={l.lessonId}>
      <strong>Date:</strong> {l.lessonDate} |
      <strong> Time:</strong> {l.startTime} - {l.endTime} |
      <strong> Teacher:</strong> {getTeacherName(l.teacherId)} |
      <strong> Subject:</strong> {getSubjectName(l.subjectId)} |
      <strong> Ages:</strong> {l.minAge}-{l.maxAge} |
      <strong> Gender:</strong> {l.gender === "M" ? "Male" : "Female"}{" "}
      {withCancel && (
        <button
          onClick={() => handleCancelClick(l)}
          disabled={loadingLessonId === l.lessonId}
          style={{ marginRight: "10px" }}
        >
          {loadingLessonId === l.lessonId ? "Cancelling..." : "Cancel Participation"}
        </button>
      )}
    </li>
  );

  return (
    <div>
      <h2>Upcoming Lessons</h2>
      <ul>{upcomingLessons.map((l) => renderLesson(l, true))}</ul>

      <h2>Past Lessons</h2>
      <ul>{pastLessons.map((l) => renderLesson(l))}</ul>

      <h2>Canceled Lessons</h2>
      <ul>{canceledLessons.map((l) => renderLesson(l))}</ul>

      {alert && <p style={{ color: "red" }}>{alert}</p>}
    </div>
  );
};

export default StudentLessonHistory;
