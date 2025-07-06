import React, { useState,useEffect } from "react";
import { addLesson,getAllTeachers,getAllSubjects } from "../../services/api";
import type { Lesson } from "../../models/lessonModel";
import type { User } from "../../models/userModel";
import type { Subject } from "../../models/subjectModel";
import { parseApiError } from "../../utils/apiErrorParser";



const ManageLessons = () => {
const [errorMessages, setErrorMessages] = useState<string | null>(null);
const [successMessage, setSuccessMessage] = useState<string | null>(null);
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

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    const parsedValue = ["teacherId", "subjectId", "minAge", "maxAge"].includes(name)
      ? parseInt(value)
      : value;

    setNewLesson({ ...newLesson, [name]: parsedValue });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await addLesson(newLesson);
       setSuccessMessage("השיעור נוסף בהצלחה");

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
    } catch (error: any) {
      setErrorMessages("שגיאה בהוספת שיעור " + parseApiError(error));
    }
  };

  return (
    <div>
          {errorMessages && (
        <div role="alert" style={{ color: "red" }}>{errorMessages}</div>
      )}
      {successMessage && (
        <div style={{ color: "green" }}>{successMessage}</div>
      )}

      <h2>הוספת שיעור חדש</h2>
      <form onSubmit={handleSubmit}>
          
            <label>מורה:</label>
            <select name="teacherId" value={newLesson.teacherId ?? ""} onChange={handleChange}>
              <option value="">בחר מורה</option>
              {teachers.map((teacher) => (
                <option key={teacher.userId} value={teacher.userId}>
                  {teacher.firstName} {teacher.lastName}
                </option>
              ))}
            </select>
          

         
            <label>מקצוע:</label>
            <select name="subjectId" value={newLesson.subjectId ?? ""} onChange={handleChange}>
              <option value="">בחר מקצוע</option>
              {subjects.map((subject) => (
                <option key={subject.subjectId} value={subject.subjectId}>
                  {subject.name}
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
    </div>
  );
};

export default ManageLessons;
