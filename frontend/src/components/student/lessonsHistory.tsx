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

import Toast from "../../components/toast";
import styles from "../../components/student/lessonsHistory.module.scss";

const StudentLessonHistory = () => {
  const user = JSON.parse(localStorage.getItem("user") || "{}");
  const studentId = user?.userId;

  const [lessons, setLessons] = useState<Lesson[]>([]);
  const [teachers, setTeachers] = useState<User[]>([]);
  const [subjects, setSubjects] = useState<Subject[]>([]);
  const [loadingLessonId, setLoadingLessonId] = useState<number | null>(null);

  const [errorMessages, setErrorMessages] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);

  const [filter, setFilter] = useState<"all" | "passed" | "upcoming" | "canceled">("all");

  useEffect(() => {
    const fetchData = async () => {
      if (!studentId) return;

      try {
        const lessonsRes = await getLessonsByStudentId(studentId);
        setLessons(lessonsRes.data);

        const teachersRes = await getAllTeachers();
        setTeachers(teachersRes.data);

        const subjectsRes = await getAllSubjects();
        setSubjects(subjectsRes.data);
      } catch (error) {
        setErrorMessages("Error fetching data");
      }
    };

    fetchData();
  }, [studentId]);


  useEffect(() => {
    if (errorMessages || successMessage) {
      const timeout = setTimeout(() => {
        setErrorMessages(null);
        setSuccessMessage(null);
      }, 4000);
      return () => clearTimeout(timeout);
    }
  }, [errorMessages, successMessage]);

  const getTeacherName = (id: number) => {
    const teacher = teachers.find((t) => t.userId === id);
    return teacher ? `${teacher.firstName} ${teacher.lastName}` : "Unknown";
  };

  const getSubjectName = (id: number) => {
    const subject = subjects.find((s) => s.subjectId === id);
    return subject ? subject.name : "Unknown";
  };

  const now = new Date();

  const filteredLessons = lessons.filter((l) => {
    const lessonEnd = new Date(`${l.lessonDate}T${l.endTime}`);
    switch (filter) {
      case "passed":
        return l.status === "passed" && lessonEnd < now;
      case "upcoming":
        return l.status === "booked" && lessonEnd >= now;
      case "canceled":
        return l.status === "canceled";
      case "all":
      default:
        return true;
    }
  });

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
      setErrorMessages(null);
      setSuccessMessage(null);
      setLoadingLessonId(lesson.lessonId);
      await deleteRegistrationByLessonId(lesson.lessonId);
      setLessons((prev) => prev.filter((l) => l.lessonId !== lesson.lessonId));
      setSuccessMessage("Lesson cancelled successfully");
    } catch (err) {
      console.error("Error cancelling registration:", err);
      setErrorMessages("An error occurred while canceling the lesson.");
    } finally {
      setLoadingLessonId(null);
    }
  };

  const getStatusLabel = (lesson: Lesson) => {
    const lessonEnd = new Date(`${lesson.lessonDate}T${lesson.endTime}`);
    if (lesson.status === "canceled") return "Canceled";
    if (lesson.status === "passed" && lessonEnd < now) return "Passed";
    if (lesson.status === "booked" && lessonEnd >= now) return "Upcoming";
    return lesson.status;
  };

  const getStatusClass = (lesson: Lesson) => {
    const label = getStatusLabel(lesson);
    return `${styles.status} ${styles[label.toLowerCase()]}`;
  };

  

  const renderLessonRow = (l: Lesson) => (
    <tr key={l.lessonId}>
      <td>{l.lessonDate}</td>
      <td>
        {l.startTime} - {l.endTime}
      </td>
      <td>{getTeacherName(l.teacherId)}</td>
      <td>{getSubjectName(l.subjectId)}</td>
      <td>
        {l.minAge}-{l.maxAge}
      </td>
      <td>{l.gender === "M" ? "Male" : "Female"}</td>
      <td>
        <span className={getStatusClass(l)}>{getStatusLabel(l)}</span>
      </td>
      <td>
        {l.status === "booked" && (
          <button
            className={styles.actionBtn}
            onClick={() => handleCancelClick(l)}
            disabled={loadingLessonId === l.lessonId}
          >
            {loadingLessonId === l.lessonId ? "Cancelling..." : "Cancel"}
          </button>
        )}
        <button
          className={styles.actionBtn}
          onClick={() => window.print()}
          title="Print this lesson"
        >
          Print
        </button>
      </td>
    </tr>
  );

  return (
    <div className={styles.container}>
      {errorMessages && <Toast type="error" message={errorMessages} />}
      {successMessage && <Toast type="success" message={successMessage} />}

      <h2>My lessons:</h2>

      <div className={styles.filters}>
        {["all", "passed", "upcoming", "canceled"].map((key) => (
          <button
            key={key}
            className={filter === key ? styles.active : ""}
            onClick={() => setFilter(key as typeof filter)}
          >
            {key === "all"
              ? "All"
              : key === "passed"
              ? "Past"
              : key === "upcoming"
              ? "Upcoming"
              : "Canceled"}
          </button>
        ))}
      </div>

      <div className={styles.tableWrapper}>
        <table className={styles.lessonTable}>
          <thead>
            <tr>
              <th>Date</th>
              <th>Time</th>
              <th>Teacher</th>
              <th>Subject</th>
              <th>Age</th>
              <th>Gender</th>
              <th>Status</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>{filteredLessons.map(renderLessonRow)}</tbody>
        </table>
      </div>

      <div className={styles.exportButtons}>
        <button onClick={() => window.print()}>Print Page</button>
      </div>
    </div>
  );
};

export default StudentLessonHistory;
