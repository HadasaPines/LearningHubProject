import { useState, useEffect } from "react";
import {
  getAllTeachers,
  getAllSubjects,
  getAllLessons,
  deleteLesson,
  getStudentToLeeson,
  deleteRegistrationByLessonId,
} from "../../services/api";
import type { Lesson } from "../../models/lessonModel";
import type { User } from "../../models/userModel";
import type { Subject } from "../../models/subjectModel";
import { parseApiError } from "../../utils/apiErrorParser";
import Toast from "../../components/toast";
import { FaChalkboardTeacher } from "react-icons/fa";
import styles from "./manageLessons.module.scss";

const ManageLessons = () => {
  const [lessons, setLessons] = useState<Lesson[]>([]);
  const [teachers, setTeachers] = useState<User[]>([]);
  const [subjects, setSubjects] = useState<Subject[]>([]);
  const [filterTeacherId, setFilterTeacherId] = useState<number | null>(null);
  const [filterDate, setFilterDate] = useState<string>("");
  const [selectedLessons, setSelectedLessons] = useState<number[]>([]);
  const [toast, setToast] = useState<{ type: "success" | "error"; message: string } | null>(null);
  const [expandedLessonId, setExpandedLessonId] = useState<number | null>(null);

  const showMessage = (msg: string, type: "success" | "error") => {
    setToast({ message: msg, type });
    setTimeout(() => setToast(null), 3000);
  };

  const refreshLessons = async () => {
    try {
      const res = await getAllLessons();
      setLessons(res.data);
    } catch (error) {
      showMessage(parseApiError(error), "error");
    }
  };

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [teachersRes, subjectsRes, lessonsRes] = await Promise.all([
          getAllTeachers(),
          getAllSubjects(),
          getAllLessons(),
        ]);
        setTeachers(teachersRes.data);
        setSubjects(subjectsRes.data);
        setLessons(lessonsRes.data);
      } catch (error) {
        showMessage(parseApiError(error), "error");
      }
    };
    fetchData();
  }, []);

  const toggleSelectLesson = (lessonId: number) => {
    setSelectedLessons((prev) =>
      prev.includes(lessonId)
        ? prev.filter((id) => id !== lessonId)
        : [...prev, lessonId]
    );
  };

  const handleBulkDelete = async () => {
    if (
      selectedLessons.length &&
      window.confirm("Are you sure you want to delete the selected lessons?")
    ) {
      try {
        for (const lessonId of selectedLessons) {
          await handleDeleteLesson(lessonId, false);
        }
        await refreshLessons();
        setSelectedLessons([]);
        showMessage("Selected lessons deleted successfully", "success");
      } catch (error) {
        showMessage("Error deleting selected lessons: " + parseApiError(error), "error");
      }
    }
  };

  const handleDeleteLesson = async (lessonId: number, showConfirm = true) => {
    try {
      const response = await getStudentToLeeson(lessonId);
      const student: User = response.data;

      if (student) {
        const message = `${student.firstName} ${student.lastName} ${student.phone} ${student.email}`;
        if (showConfirm) {
          const confirmDelete = window.confirm(
            `Note: This lesson has a registered student. Are you sure you want to delete the lesson?\n${message}`
          );
          if (!confirmDelete) return;
        }
        await deleteRegistrationByLessonId(lessonId);
      } else {
        if (showConfirm) {
          const confirmDelete = window.confirm("Are you sure you want to delete this lesson?");
          if (!confirmDelete) return;
        }
      }

      await deleteLesson(lessonId);
      showMessage("Lesson deleted successfully", "success");
      const updatedLessons = await getAllLessons();
      setLessons(updatedLessons.data);
    } catch (error) {
      showMessage("Error deleting lesson: " + parseApiError(error), "error");
    }
  };

  const filteredLessons = lessons.filter((lesson) => {
    const matchTeacher = !filterTeacherId || lesson.teacherId === filterTeacherId;
    const matchDate = !filterDate || lesson.lessonDate === filterDate;
    return matchTeacher && matchDate;
  });


  return (
    <div className={styles.container}>
      {toast && <Toast type={toast.type} message={toast.message} />}
      <h2 className={styles.title}>Manage Lessons</h2>

      <div className={styles.filters}>
        <select
          value={filterTeacherId ?? ""}
          onChange={(e) => setFilterTeacherId(Number(e.target.value) || null)}
        >
          <option value="">All Teachers</option>
          {teachers.map((t) => (
            <option key={t.userId} value={t.userId}>
              {t.firstName} {t.lastName}
            </option>
          ))}
        </select>

        <input
          type="date"
          value={filterDate}
          onChange={(e) => setFilterDate(e.target.value)}
        />

        <button
          onClick={handleBulkDelete}
          disabled={!selectedLessons.length}
          className={styles.addBtn}
        >
          Delete Selected
        </button>
      </div>

      {filteredLessons.length === 0 ? (
        <p>No lessons found.</p>
      ) : (
        <div className={styles.cardGrid}>
          {filteredLessons.map((lesson) => {
            const teacher = teachers.find((t) => t.userId === lesson.teacherId);
            const subject = subjects.find((s) => s.subjectId === lesson.subjectId);
            const isExpanded = expandedLessonId === lesson.lessonId;

            return (
              <div
                key={lesson.lessonId}
                className={`${styles.card} ${isExpanded ? styles.expanded : ""}`}
              >
                <div className={styles.icon}>
                  <FaChalkboardTeacher />
                </div >
                <div className={styles.info}>
                <p><b>{lesson.lessonDate}</b></p>
                <p><strong>Subject:</strong> {subject ? subject.name : "-"}</p>
                <p><strong>Teacher:</strong> {teacher ? `${teacher.firstName} ${teacher.lastName}` : "-"}</p>
                </div>
                <div className={styles.content}>
                  {isExpanded && (
                    <>
                      <p><strong>Time:</strong> {lesson.startTime} - {lesson.endTime}</p>

                      <p><strong>Age Range:</strong> {lesson.minAge} - {lesson.maxAge}</p>
                      <p><strong>Gender:</strong> {lesson.gender === "M" ? "Male" : "Female"}</p>
                      <p><strong>Status:</strong> {lesson.status}</p>
                    </>
                  )}
                </div>

                <div className={styles.actions}>
                  <label>
                    <input
                      type="checkbox"
                      checked={selectedLessons.includes(lesson.lessonId)}
                      onChange={() => toggleSelectLesson(lesson.lessonId)}
                    />
                  </label>


                  <button onClick={() => handleDeleteLesson(lesson.lessonId)}>
                    Delete
                  </button>
                  <button
                    className={styles.detailsBtn}
                    onClick={() =>
                      setExpandedLessonId((prev) =>
                        prev === lesson.lessonId ? null : lesson.lessonId
                      )
                    }
                  >
                    {isExpanded ? "Collapse" : "Details"}
                  </button>

                </div>
              </div>
            );
          })}
        </div>
      )}
    </div>
  );

};

export default ManageLessons;
