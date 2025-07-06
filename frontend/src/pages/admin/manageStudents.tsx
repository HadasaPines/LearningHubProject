import React, { useEffect, useState } from "react";
import {
  getAllUsers,
  getAllStudents,
  addStudent,
  deleteUser,
  updateUser,
  updateStudent,
  addUser2,
} from "../../services/api";
import type { User2, Student, Gender } from "../../models/studentModel";
import { parseApiError } from "../../utils/apiErrorParser";

const ManageStudents = () => {
  const [students, setStudents] = useState<(User2 & Student)[]>([]);
  const [newUser, setNewUser] = useState<User2>({
    userId: 0,
    firstName: "",
    lastName: "",
    email: "",
    phone: "",
    password: "",
    role: "Student",
  });
  const [newStudent, setNewStudent] = useState<Omit<Student, "studentId">>({
    gender: "M",
    age: 0,
    birthDate: "",
  });
  const [editingUserId, setEditingUserId] = useState<number | null>(null);
  const [editUser, setEditUser] = useState<Partial<User2>>({});
  const [editStudent, setEditStudent] = useState<Partial<Student>>({});
  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);

  const showMessage = (msg: string, type: "error" | "success") => {
    if (type === "error") setErrorMessage(msg);
    else setSuccessMessage(msg);

    setTimeout(() => {
      setErrorMessage(null);
      setSuccessMessage(null);
    }, 4000);
  };

  const extractErrorMessage = (error: any, defaultMsg: string) => {
    return (
      error?.response?.data?.detail ||
      error?.response?.data?.message ||
      defaultMsg
    );
  };

  useEffect(() => {
    loadStudents();
  }, []);

  const loadStudents = async () => {
    try {
      const [userRes, studentRes] = await Promise.all([getAllUsers(), getAllStudents()]);
      const studentUsers = userRes.data.filter((u: User2) => u.role === "Student");
      const fullList = studentUsers.map((user: User2) => {
        const student = studentRes.data.find((s: Student) => s.studentId === user.userId);
        return {
          ...user,
          ...student,
        };
      });
      setStudents(fullList);
    } catch (error: any) {
      showMessage(extractErrorMessage(error, "שגיאה בטעינת רשימת התלמידים"), "error");
    }
  };

  const handleAdd = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await addUser2(newUser);
      await addStudent({
        studentId: newUser.userId,
        ...newStudent,
        birthDate: new Date(newStudent.birthDate).toISOString(),
      });
      resetForm();
      loadStudents();
      showMessage("תלמיד נוסף בהצלחה", "success");
    } catch (error: any) {
      showMessage(parseApiError(error), "error");
    }
  };

  const handleDelete = async (id: number) => {
    if (!window.confirm("האם למחוק את המשתמש?")) return;
    try {
      await deleteUser(id);
      loadStudents();
      showMessage("התלמיד נמחק בהצלחה", "success");
    } catch (error: any) {
      showMessage(parseApiError(error), "error");
    }
  };

  const handleEditClick = (student: User2 & Student) => {
    setEditingUserId(student.userId);
    setEditUser({ ...student });
    setEditStudent({
      gender: student.gender,
      age: student.age,
      birthDate: student.birthDate,
    });
  };

  const handleEditChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>,
    target: "user" | "student"
  ) => {
    const { name, value } = e.target;
    if (target === "user") {
      setEditUser({ ...editUser, [name]: value });
    } else {
      setEditStudent({ ...editStudent, [name]: name === "age" ? parseInt(value) : value });
    }
  };

  const handleSaveEdit = async () => {
    if (!editingUserId) return;
    try {
      const userPatch = [
        { op: "replace", path: "/firstName", value: editUser.firstName },
        { op: "replace", path: "/lastName", value: editUser.lastName },
        { op: "replace", path: "/email", value: editUser.email },
        { op: "replace", path: "/phone", value: editUser.phone },
      ];

      const studentPatch = [
        { op: "replace", path: "/gender", value: editStudent.gender },
        { op: "replace", path: "/age", value: editStudent.age },
      ];

      await updateUser(editingUserId, userPatch);
      await updateStudent(editingUserId, studentPatch);
      setEditingUserId(null);
      loadStudents();
      showMessage("העדכון נשמר בהצלחה", "success");
    } catch (error: any) {
      showMessage(parseApiError(error), "error");
    }
  };

  const resetForm = () => {
    setNewUser({
      userId: 0,
      firstName: "",
      lastName: "",
      email: "",
      phone: "",
      password: "",
      role: "Student",
    });
    setNewStudent({
      gender: "M",
      age: 0,
      birthDate: "",
    });
  };

  return (
    <div>
      <h2>ניהול תלמידים</h2>

      {errorMessage && <div style={{ color: "red", marginBottom: 10 }}>{errorMessage}</div>}
      {successMessage && <div style={{ color: "green", marginBottom: 10 }}>{successMessage}</div>}

      <form onSubmit={handleAdd}>
        <h3>הוספת תלמיד חדש</h3>
        <input
          type="number"
          name="userId"
          placeholder="ת.ז"
          value={newUser.userId}
          required
          onChange={(e) => setNewUser({ ...newUser, userId: parseInt(e.target.value) })}
        />
        <input
          name="firstName"
          placeholder="שם פרטי"
          value={newUser.firstName}
          required
          onChange={(e) => setNewUser({ ...newUser, firstName: e.target.value })}
        />
        <input
          name="lastName"
          placeholder="שם משפחה"
          value={newUser.lastName}
          required
          onChange={(e) => setNewUser({ ...newUser, lastName: e.target.value })}
        />
        <input
          name="email"
          placeholder="אימייל"
          value={newUser.email}
          onChange={(e) => setNewUser({ ...newUser, email: e.target.value })}
        />
        <input
          name="phone"
          placeholder="טלפון"
          value={newUser.phone}
          onChange={(e) => setNewUser({ ...newUser, phone: e.target.value })}
        />
        <input
          name="password"
          placeholder="סיסמה"
          value={newUser.password}
          required
          onChange={(e) => setNewUser({ ...newUser, password: e.target.value })}
        />
        <input
          type="date"
          name="birthDate"
          value={newStudent.birthDate}
          onChange={(e) => setNewStudent({ ...newStudent, birthDate: e.target.value })}
        />
        <select
          name="gender"
          value={newStudent.gender}
          onChange={(e) => setNewStudent({ ...newStudent, gender: e.target.value as Gender })}
        >
          <option value="M">זכר</option>
          <option value="F">נקבה</option>
        </select>
        <input
          type="number"
          name="age"
          placeholder="גיל"
          value={newStudent.age}
          onChange={(e) => setNewStudent({ ...newStudent, age: parseInt(e.target.value) })}
        />
        <button type="submit">הוסף</button>
      </form>

      <hr />

      <h3>רשימת תלמידים</h3>
      <table cellPadding="8">
        <thead>
          <tr>
            <th>ת.ז</th>
            <th>שם פרטי</th>
            <th>שם משפחה</th>
            <th>טלפון</th>
            <th>אימייל</th>
            <th>מין</th>
            <th>גיל</th>
            <th>ת. לידה</th>
            <th>פעולות</th>
          </tr>
        </thead>
        <tbody>
          {students.map((s) =>
            editingUserId === s.userId ? (
              <tr key={s.userId}>
                <td>{s.userId}</td>
                <td>
                  <input
                    name="firstName"
                    value={editUser.firstName || ""}
                    onChange={(e) => handleEditChange(e, "user")}
                  />
                </td>
                <td>
                  <input
                    name="lastName"
                    value={editUser.lastName || ""}
                    onChange={(e) => handleEditChange(e, "user")}
                  />
                </td>
                <td>
                  <input
                    name="phone"
                    value={editUser.phone || ""}
                    onChange={(e) => handleEditChange(e, "user")}
                  />
                </td>
                <td>
                  <input
                    name="email"
                    value={editUser.email || ""}
                    onChange={(e) => handleEditChange(e, "user")}
                  />
                </td>
                <td>
                  <select
                    name="gender"
                    value={editStudent.gender || "M"}
                    onChange={(e) => handleEditChange(e, "student")}
                  >
                    <option value="M">זכר</option>
                    <option value="F">נקבה</option>
                  </select>
                </td>
                <td>
                  <input
                    type="number"
                    name="age"
                    value={editStudent.age || 0}
                    onChange={(e) => handleEditChange(e, "student")}
                  />
                </td>
                <td>{s.birthDate?.substring(0, 10)}</td>
                <td>
                  <button onClick={handleSaveEdit} title="שמור">
                    💾
                  </button>
                  <button onClick={() => setEditingUserId(null)} title="ביטול">
                    ❌
                  </button>
                </td>
              </tr>
            ) : (
              <tr key={s.userId}>
                <td>{s.userId}</td>
                <td>{s.firstName}</td>
                <td>{s.lastName}</td>
                <td>{s.phone}</td>
                <td>{s.email}</td>
                <td>{s.gender}</td>
                <td>{s.age}</td>
                <td>{s.birthDate?.substring(0, 10)}</td>
                <td>
                  <button onClick={() => handleEditClick(s)} title="ערוך">
                    ✏️
                  </button>
                  <button onClick={() => handleDelete(s.userId)} title="מחק">
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

export default ManageStudents;