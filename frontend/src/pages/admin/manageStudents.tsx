import { useEffect, useState } from "react";
import {
  getAllUsers,
  addUser,
  deleteUser,
  addStudent,
  getAllStudents,
  updateUser,
  updateStudent,
} from "../../services/api";
import type { User, Student, Gender } from "../../models/studentModel";

const ManageStudents = () => {
  const [students, setStudents] = useState<(User & Student)[]>([]);
  const [newUser, setNewUser] = useState<User>({
    userId: 0,
    firstName: "",
    lastName: "",
    password: "",
    phone: "",
    email: "",
    role: "Student",
  });
  const [newStudent, setNewStudent] = useState<Omit<Student, "studentId">>({
    gender: "M",
    age: 0,
    birthDate: "",
  });
  const [isEditing, setIsEditing] = useState(false);

  useEffect(() => {
    loadStudents();
  }, []);

  const loadStudents = async () => {
    const allUsers = await getAllUsers();
    const allStudents = await getAllStudents();
    const studentUsers = allUsers.data.filter((u: User) => u.role === "Student");

    const fullList = studentUsers.map((user: User) => {
      const studentData = allStudents.data.find((s: Student) => s.studentId === user.userId);
      return {
        ...user,
        ...studentData,
      };
    });

    setStudents(fullList);
  };

  const handleAdd = async () => {
    try {
      await addUser(newUser);
      await addStudent({
        studentId: newUser.userId,
        ...newStudent,
        birthDate: new Date(newStudent.birthDate).toISOString(),
      });
      await loadStudents();
      resetForm();
    } catch (error) {
      alert("שגיאה בהוספת תלמיד. ייתכן שתעודת הזהות כבר קיימת.");
    }
  };

  const handleEdit = (student: User & Student) => {
    setIsEditing(true);
    setNewUser({
      userId: student.userId,
      firstName: student.firstName,
      lastName: student.lastName,
      password: student.password || "",
      phone: student.phone,
      email: student.email,
      role: "Student",
    });
    setNewStudent({
      gender: student.gender,
      age: student.age,
      birthDate: student.birthDate?.substring(0, 10) || "",
    });
  };

  const handleSaveEdit = async () => {
    const userPatch = [
      { op: "replace", path: "/firstName", value: newUser.firstName },
      { op: "replace", path: "/lastName", value: newUser.lastName },
      { op: "replace", path: "/email", value: newUser.email },
      { op: "replace", path: "/phone", value: newUser.phone },
      { op: "replace", path: "/password", value: newUser.password },
    ];
    const studentPatch = [
      { op: "replace", path: "/gender", value: newStudent.gender },
      { op: "replace", path: "/age", value: newStudent.age },
      { op: "replace", path: "/birthDate", value: new Date(newStudent.birthDate).toISOString() },
    ];

    await updateUser(newUser.userId, userPatch);
    await updateStudent(newUser.userId, studentPatch);
    await loadStudents();
    resetForm();
    setIsEditing(false);
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
    setIsEditing(false);
  };

  const handleDelete = async (studentId: number) => {
    await deleteUser(studentId);
    await loadStudents();
  };

  return (
    <div>
      <h2>ניהול תלמידים</h2>

      <div>
        <h3>{isEditing ? "עריכת תלמיד" : "הוספת תלמיד"}</h3>

        <input
          type="number"
          placeholder="תעודת זהות"
          value={newUser.userId}
          disabled={isEditing}
          onChange={(e) =>
            setNewUser({ ...newUser, userId: parseInt(e.target.value) })
          }
        />
        <input
          placeholder="שם פרטי"
          value={newUser.firstName}
          onChange={(e) =>
            setNewUser({ ...newUser, firstName: e.target.value })
          }
        />
        <input
          placeholder="שם משפחה"
          value={newUser.lastName}
          onChange={(e) =>
            setNewUser({ ...newUser, lastName: e.target.value })
          }
        />
        <input
          placeholder="אימייל"
          value={newUser.email}
          onChange={(e) =>
            setNewUser({ ...newUser, email: e.target.value })
          }
        />
        <input
          placeholder="טלפון"
          value={newUser.phone}
          onChange={(e) =>
            setNewUser({ ...newUser, phone: e.target.value })
          }
        />
        <input
          placeholder="סיסמה"
          value={newUser.password}
          onChange={(e) =>
            setNewUser({ ...newUser, password: e.target.value })
          }
        />

        <select
          value={newStudent.gender}
          onChange={(e) =>
            setNewStudent({
              ...newStudent,
              gender: e.target.value as Gender,
            })
          }
        >
          <option value="M">זכר</option>
          <option value="F">נקבה</option>
        </select>
        <input
          type="number"
          placeholder="גיל"
          value={newStudent.age}
          onChange={(e) =>
            setNewStudent({ ...newStudent, age: parseInt(e.target.value) })
          }
        />
        <input
          type="date"
          placeholder="תאריך לידה"
          value={newStudent.birthDate}
          onChange={(e) =>
            setNewStudent({ ...newStudent, birthDate: e.target.value })
          }
        />

        {isEditing ? (
          <button onClick={handleSaveEdit}>שמור עריכה</button>
        ) : (
          <button onClick={handleAdd}>הוסף</button>
        )}
        {isEditing && <button onClick={resetForm}>ביטול</button>}
      </div>

      <hr />

      <h3>רשימת תלמידים</h3>
      <table>
        <thead>
          <tr>
            <th>ת.ז</th>
            <th>שם פרטי</th>
            <th>שם משפחה</th>
            <th>אימייל</th>
            <th>טלפון</th>
            <th>מין</th>
            <th>גיל</th>
            <th>תאריך לידה</th>
            <th>פעולות</th>
          </tr>
        </thead>
        <tbody>
          {students.map((s) => (
            <tr key={s.userId}>
              <td>{s.userId}</td>
              <td>{s.firstName}</td>
              <td>{s.lastName}</td>
              <td>{s.email}</td>
              <td>{s.phone}</td>
              <td>{s.gender}</td>
              <td>{s.age}</td>
              <td>{s.birthDate?.substring(0, 10)}</td>
              <td>
                <button onClick={() => handleEdit(s)}>ערוך</button>
                <button onClick={() => handleDelete(s.userId)}>מחק</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default ManageStudents;
