import React, { useState, useEffect } from "react";
import type { User } from "../../models/userModel";
import { addUser, getAllTeachers, addTeacher, updateUser, deleteUser, getLessonsByTeacherId, updateTeacher,deleteTeacher } from "../../services/api";
import { parseApiError } from "../../utils/apiErrorParser";

const ManageTeachers = () => {
  const [teachers, setTeachers] = useState<User[]>([]);
  const [newTeacher, setNewTeacher] = useState<Omit<User, "teacherId">>({
    userId: 0, firstName: "", lastName: "", password: "",
    phone: "", email: "", role: "Teacher",
    teacher: { gender: "M", bio: "", birthDate: "" },
  });
  const [editingTeacherId, setEditingTeacherId] = useState<number | null>(null);
  const [editTeacher, setEditTeacher] = useState<User | null>(null);
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
    (async () => {
      try {
        const res = await getAllTeachers();
        setTeachers(res.data);
      } catch (err) {
        showMessage(parseApiError(err), "error");
      }
    })();
  }, []);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    const teacherFields = ["gender", "bio", "birthDate"];
    if (teacherFields.includes(name)) {
      setNewTeacher(prev => ({
        ...prev,
        teacher: { ...prev.teacher!, [name]: value }
      }));
    } else {
      setNewTeacher(prev => ({ ...prev, [name]: value }));
    }
  };

  const handleEditChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => {
    if (!editTeacher) return;
    const { name, value } = e.target;
    const teacherFields = ["gender", "bio", "birthDate"];
    if (teacherFields.includes(name)) {
      setEditTeacher({
        ...editTeacher,
        teacher: { ...editTeacher.teacher!, [name]: value }
      });
    } else {
      setEditTeacher({ ...editTeacher, [name]: value });
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await addUser(newTeacher);
      await addTeacher({
        teacherId: newTeacher.userId,
        gender: newTeacher.teacher!.gender,
        bio: newTeacher.teacher!.bio,
        birthDate: newTeacher.teacher!.birthDate,
      });
      showMessage("המורה נוסף בהצלחה", "success");
      setNewTeacher({
        userId: 0, firstName: "", lastName: "", password: "",
        phone: "", email: "", role: "Teacher",
        teacher: { gender: "M", bio: "", birthDate: "" },
      });
      const res = await getAllTeachers();
      setTeachers(res.data);
    } catch (err) {
      showMessage(parseApiError(err), "error");
    }
  };

  const handleEditClick = (t: User) => {
    setEditingTeacherId(t.userId);
    setEditTeacher({ ...t });
  };

  const handleSaveEdit = async () => {
    if (!editTeacher) return;
    try {
      const userPatch = [
        { op: "replace", path: "/firstName", value: editTeacher.firstName },
        { op: "replace", path: "/lastName", value: editTeacher.lastName },
        { op: "replace", path: "/email", value: editTeacher.email },
        { op: "replace", path: "/phone", value: editTeacher.phone },
      ];
      const teacherPatch = [
        { op: "replace", path: "/bio", value: editTeacher.teacher!.bio },
      ];
      await updateUser(editTeacher.userId, userPatch);
      await updateTeacher(editTeacher.userId, teacherPatch);
      showMessage("המורה עודכן בהצלחה", "success");
      const res = await getAllTeachers();
      setTeachers(res.data);
      setEditingTeacherId(null);
      setEditTeacher(null);
    } catch (err) {
      showMessage(parseApiError(err), "error");
    }
  };

  const handleDeleteTeacher = async (userId: number) => {
    try {
      const lessons = await getLessonsByTeacherId(userId);
      if (lessons.data.length > 0) {
        showMessage("לא ניתן למחוק – יש שיעורים משוייכים למורה", "error");
        return;
      }

      if (!window.confirm("האם אתה בטוח שברצונך למחוק את המורה?")) return;
      await deleteUser(userId);
      await deleteTeacher(userId);
      showMessage("המורה נמחק בהצלחה", "success");
      setTeachers(prev => prev.filter(t => t.userId !== userId));
    } catch (err) {
      showMessage(parseApiError(err), "error");
    }
  };

  return (
    <div>
      <h2>ניהול מורים</h2>
      {errorMessage && <div style={{ color: "red" }}>{errorMessage}</div>}
      {successMessage && <div style={{ color: "green" }}>{successMessage}</div>}

      <form onSubmit={handleSubmit}>
        <h3>הוספת מורה חדש</h3>
        <input name="userId" type="number" placeholder="ID" value={newTeacher.userId} onChange={handleChange} required />
        <input name="firstName" placeholder="שם פרטי" value={newTeacher.firstName} onChange={handleChange} required />
        <input name="lastName" placeholder="שם משפחה" value={newTeacher.lastName} onChange={handleChange} required />
        <input name="phone" placeholder="טלפון" value={newTeacher.phone} onChange={handleChange} required />
        <input name="email" placeholder="אימייל" value={newTeacher.email} onChange={handleChange} required />
        <input name="password" type="password" placeholder="סיסמה" value={newTeacher.password} onChange={handleChange} required />
        <select name="gender" value={newTeacher.teacher!.gender} onChange={handleChange}>
          <option value="M">זכר</option>
          <option value="F">נקבה</option>
        </select>
        <input name="birthDate" type="date" value={newTeacher.teacher!.birthDate} onChange={handleChange} />
        <textarea name="bio" placeholder="ביוגרפיה" value={newTeacher.teacher!.bio} onChange={handleChange} />
        <button type="submit">הוסף מורה</button>
      </form>

      <hr />

      <h3>רשימת מורים</h3>
      <table>
        <thead>
          <tr>
            <th>שם</th><th>אימייל</th><th>טלפון</th><th>ביוגרפיה</th><th>פעולות</th>
          </tr>
        </thead>
        <tbody>
          {teachers.map(t =>
            editingTeacherId === t.userId && editTeacher ? (
              <tr key={t.userId}>
                <td>
                  <input name="firstName" value={editTeacher.firstName} onChange={handleEditChange} />
                  <input name="lastName" value={editTeacher.lastName} onChange={handleEditChange} />
                </td>
                <td><input name="email" value={editTeacher.email} onChange={handleEditChange} /></td>
                <td><input name="phone" value={editTeacher.phone} onChange={handleEditChange} /></td>
                <td><textarea name="bio" value={editTeacher.teacher!.bio} onChange={handleEditChange} /></td>
                <td>
                  <button onClick={handleSaveEdit}>💾</button>
                  <button onClick={() => setEditingTeacherId(null)}>❌</button>
                </td>
              </tr>
            ) : (
              <tr key={t.userId}>
                <td>{t.firstName} {t.lastName}</td>
                <td>{t.email}</td>
                <td>{t.phone}</td>
                <td>{t.teacher?.bio}</td>
                <td>
                  <button onClick={() => handleEditClick(t)}>✏️</button>
                <button
  onClick={() => handleDeleteTeacher(t.userId)}
  style={{
    border: "none",
    padding: "5px 10px",
  }}
>
   🗑️
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

export default ManageTeachers;
