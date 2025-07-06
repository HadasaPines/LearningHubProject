import React, { useState, useEffect } from "react";
import {
  addLesson,
  getAllTeachers,
  getAllSubjects,
  getAllLessons,
  updateLesson,
} from "../../services/api";
import type { Lesson } from "../../models/lessonModel";
import type { User } from "../../models/userModel";
import type { Subject } from "../../models/subjectModel";
import { parseApiError } from "../../utils/apiErrorParser";


const ManageLessons = () => {
  const [lessons, setLessons] = useState<Lesson[]>([]);
  const [teachers, setTeachers] = useState<User[]>([]);
  const [subjects, setSubjects] = useState<Subject[]>([]);
  const [newLesson, setNewLesson] = useState<Omit<Lesson, "lessonId">>({
    teacherId: 0,
    subjectId: 0,
    lessonDate: "",
    startTime: "",
    endTime: "",
    minAge: 0,
    maxAge: 0,
    gender: "M",  
    status: "Available",
  });
  const [editingLessonId, setEditingLessonId] = useState<number | null>(null);
  const [editLesson, setEditLesson] = useState<Partial<Lesson>>({});
  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);

  const showMessage = (msg: string, type: "success" | "error") => {
    type === "success" ? setSuccessMessage(msg) : setErrorMessage(msg);
    setTimeout(() => {
      setSuccessMessage(null);
      setErrorMessage(null);
    }, 3000);
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

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>,
    isEdit = false
  ) => {
    const { name, value } = e.target;
    const parsedValue = ["teacherId", "subjectId", "minAge", "maxAge"].includes(name)
      ? parseInt(value)
      : value;

    if (isEdit) {
      setEditLesson((prev) => ({ ...prev, [name]: parsedValue }));
    } else {
      setNewLesson((prev) => ({ ...prev, [name]: parsedValue }));
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await addLesson(newLesson);
      showMessage("השיעור נוסף בהצלחה", "success");
      setNewLesson({
        teacherId: 0,
        subjectId: 0,
        lessonDate: "",
        startTime: "",
        endTime: "",
        minAge: 0,
        maxAge: 0,
        gender: "M",
        status: "Available",
      });
      const updated = await getAllLessons();
      setLessons(updated.data);
    } catch (error) {
      showMessage("שגיאה בהוספת שיעור " + parseApiError(error), "error");
    }
  };

  const handleEditClick = (lesson: Lesson) => {
    setEditingLessonId(lesson.lessonId);
    setEditLesson({ ...lesson });
  };
const formatDateToDateOnly = (dateString: any) => {
  const date = new Date(dateString);
  return `${date.getFullYear()}-${(date.getMonth() + 1).toString().padStart(2, '0')}-${date.getDate().toString().padStart(2, '0')}`;
};

const formatTimeToTimeOnly = (timeString: any) => {
  const [hours, minutes] = timeString.split(":");
  return `${hours}:${minutes}`;
};


 const handleSaveEdit = async () => {
  if (!editingLessonId) return;

 console.log("נתונים שנשלחים ל-API:", {
    teacherId: editLesson.teacherId,
    subjectId: editLesson.subjectId,
    //lessonDate: formatDateToDateOnly(editLesson.lessonDate),
    startTime: formatTimeToTimeOnly(editLesson.startTime),
    endTime: formatTimeToTimeOnly(editLesson.endTime),
    minAge: editLesson.minAge,
    maxAge: editLesson.maxAge,
  });

  try {
    const lessonPatch = [
      { op: "replace", path: "/teacherId", value: editLesson.teacherId },
      { op: "replace", path: "/subjectId", value: editLesson.subjectId },
      { op: "replace", path: "/lessonDate", value: formatDateToDateOnly(editLesson.lessonDate) },
     { op: "replace", path: "/startTime", value: formatTimeToTimeOnly(editLesson.startTime) },
      { op: "replace", path: "/endTime", value: formatTimeToTimeOnly(editLesson.endTime) },
      { op: "replace", path: "/minAge", value: editLesson.minAge },
      { op: "replace", path: "/maxAge", value: editLesson.maxAge },
    ];

    await updateLesson(editingLessonId, lessonPatch); 
    setSuccessMessage("השיעור עודכן בהצלחה");

    const updated = await getAllLessons(); 
    setLessons(updated.data);

    setEditingLessonId(null);
    setEditLesson({});
  } catch (error) {
    setErrorMessage("שגיאה בעדכון השיעור: " + parseApiError(error));
  }
};


  return (
    <div>
      <h2>ניהול שיעורים</h2>

      {errorMessage && <div style={{ color: "red" }}>{errorMessage}</div>}
      {successMessage && <div style={{ color: "green" }}>{successMessage}</div>}

      <form onSubmit={handleSubmit}>
        <h3>הוספת שיעור חדש</h3>
        <select name="teacherId" value={newLesson.teacherId} onChange={handleChange}>
          <option value="">בחר מורה</option>
          {teachers.map((t) => (
            <option key={t.userId} value={t.userId}>
              {t.firstName} {t.lastName}
            </option>
          ))}
        </select>
        <select name="subjectId" value={newLesson.subjectId} onChange={handleChange}>
          <option value="">בחר מקצוע</option>
          {subjects.map((s) => (
            <option key={s.subjectId} value={s.subjectId}>
              {s.name}
            </option>
          ))}
        </select>
        <input name="lessonDate" type="date" value={newLesson.lessonDate} onChange={handleChange} />
        <input name="startTime" type="time" value={newLesson.startTime} onChange={handleChange} />
        <input name="endTime" type="time" value={newLesson.endTime} onChange={handleChange} />
        <input name="minAge" type="number" placeholder="גיל מינ'" value={newLesson.minAge} onChange={handleChange} />
        <input name="maxAge" type="number" placeholder="גיל מקס'" value={newLesson.maxAge} onChange={handleChange} />
        <select name="gender" value={newLesson.gender} onChange={handleChange}>
          <option value="M">זכר</option>
          <option value="F">נקבה</option>
        </select>
        <button type="submit">הוסף שיעור</button>
      </form>

      <hr />
      <h3>רשימת שיעורים</h3>
      <table>
        <thead>
          <tr>
            <th>תאריך</th>
            <th>שעה</th>
            <th>מורה</th>
            <th>מקצוע</th>
            <th>גילאים</th>
            <th>מין</th>
            <th>סטטוס</th>
            <th>פעולות</th>
          </tr>
        </thead>
        <tbody>
          {lessons.map((lesson) =>
            editingLessonId === lesson.lessonId ? (
              <tr key={lesson.lessonId}>
                <td>
                  <input type="date" name="lessonDate" value={editLesson.lessonDate || ""} onChange={(e) => handleChange(e, true)} />
                </td>
                <td>
                  <input type="time" name="startTime" value={editLesson.startTime || ""} onChange={(e) => handleChange(e, true)} />
                  -
                  <input type="time" name="endTime" value={editLesson.endTime || ""} onChange={(e) => handleChange(e, true)} />
                </td>
                <td>
                  <select name="teacherId" value={editLesson.teacherId || 0} onChange={(e) => handleChange(e, true)}>
                    {teachers.map((t) => (
                      <option key={t.userId} value={t.userId}>
                        {t.firstName} {t.lastName}
                      </option>
                    ))}
                  </select>
                </td>
                <td>
                  <select name="subjectId" value={editLesson.subjectId || 0} onChange={(e) => handleChange(e, true)}>
                    {subjects.map((s) => (
                      <option key={s.subjectId} value={s.subjectId}>
                        {s.name}
                      </option>
                    ))}
                  </select>
                </td>
                <td>
                  <input type="number" name="minAge" value={editLesson.minAge || 0} onChange={(e) => handleChange(e, true)} />
                  -
                  <input type="number" name="maxAge" value={editLesson.maxAge || 0} onChange={(e) => handleChange(e, true)} />
                </td>
                <td>
               
                </td>
                <td>{lesson.status}</td>
                <td>
                  <button onClick={handleSaveEdit}>💾</button>
                  <button onClick={() => setEditingLessonId(null)}>❌</button>
                </td>
              </tr>
            ) : (
              <tr key={lesson.lessonId}>
                <td>{lesson.lessonDate}</td>
                <td>
                  {lesson.startTime} - {lesson.endTime}
                </td>
                <td>{teachers.find((t) => t.userId === lesson.teacherId)?.firstName}</td>
                <td>{subjects.find((s) => s.subjectId === lesson.subjectId)?.name}</td>
                <td>
                  {lesson.minAge} - {lesson.maxAge}
                </td>
                <td>{lesson.gender === "M" ? "זכר" : "נקבה"}</td>
                <td>{lesson.status}</td>
                <td>
                 <button
  onClick={() => handleEditClick(lesson)}
  disabled={lesson.status !== "Available"}
  style={{
    cursor: lesson.status === "Available" ? "pointer" : "not-allowed",
    border: "none",
    padding: "5px 10px",
  }}
>
  ✏️
</button>
                </td>
              </tr>
            )
          )}
        </tbody>
      </table>
    </div>
  );
};

export default ManageLessons;
