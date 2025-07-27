import React, { useEffect, useState } from "react";
import {
  getAllUsers,
  getAllStudents,
  addStudent,
  deleteUser,
  updateUser,
  updateStudent,
  addUser,
} from "../../services/api";
import type { User, StudentDetails } from "../../models/userModel";
import styles from "./manageStudents.module.scss";
import Toast from "../../components/toast";
import { FaEdit, FaTrash, FaSave, FaTimes, FaUserPlus } from "react-icons/fa";
import { FiUser } from "react-icons/fi";

const formatDate = (dateStr: string | undefined): string => {
  if (!dateStr) return "-";
  const date = new Date(dateStr);
  const day = String(date.getDate()).padStart(2, "0");
  const month = String(date.getMonth() + 1).padStart(2, "0");
  const year = date.getFullYear();
  return `${day}/${month}/${year}`;
};

const ManageStudents = () => {
  const [students, setStudents] = useState<User[]>([]);
  const [newStudentVisible, setNewStudentVisible] = useState(false);
  const [newUser, setNewUser] = useState<Omit<User, "student">>({
    userId: 0,
    firstName: "",
    lastName: "",
    email: "",
    phone: "",
    password: "",
    role: "Student",
  });
  const [newStudent, setNewStudent] = useState<Omit<StudentDetails, "studentId">>({
    gender: "",
    age: 0,
    birthDate: "",
  });
  const [editingUserId, setEditingUserId] = useState<number | null>(null);
  const [editUser, setEditUser] = useState<User | null>(null);
  const [editStudent, setEditStudent] = useState<StudentDetails | null>(null);
  const [toast, setToast] = useState<{ type: "success" | "error"; message: string } | null>(null);
  const [expandedStudentId, setExpandedStudentId] = useState<number | null>(null);
  const [birthDateInputTypeAdd, setBirthDateInputTypeAdd] = useState<"text" | "date">("text");

  // חישוב גיל אוטומטי לפי תאריך לידה
  const calculateAge = (birthDateStr: string): number => {
    if (!birthDateStr) return 0;
    const birthDate = new Date(birthDateStr);
    const today = new Date();
    let age = today.getFullYear() - birthDate.getFullYear();
    const m = today.getMonth() - birthDate.getMonth();
    if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
      age--;
    }
    return age;
  };

  const showMessage = (msg: string, type: "success" | "error") => {
    setToast({ message: msg, type });
    setTimeout(() => setToast(null), 3000);
  };

  useEffect(() => {
    loadStudents();
  }, []);

  const loadStudents = async () => {
    try {
      const [userRes, studentRes] = await Promise.all([getAllUsers(), getAllStudents()]);
      const studentUsers = userRes.data.filter((u: User) => u.role === "Student");
      const fullList = studentUsers.map((user: User) => {
        const student = studentRes.data.find((s: StudentDetails) => s.studentId === user.userId);
        return {
          ...user,
          student,
        };
      });
      setStudents(fullList);
    } catch {
      showMessage("Failed to load students", "error");
    }
  };

  const toggleDetails = (id: number) => {
    setExpandedStudentId((prev) => (prev === id ? null : id));
  };

  const handleAddChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    const studentFields = ["gender", "age", "birthDate"];
    if (studentFields.includes(name)) {
      let newValue: string | number = value;
      if (name === "age") newValue = Number(value);

      let updatedStudent = {
        ...newStudent,
        [name]: newValue,
      };

      // עדכון גיל אוטומטי כאשר משנים תאריך לידה
      if (name === "birthDate") {
        updatedStudent.age = calculateAge(value);
      }

      setNewStudent(updatedStudent);
    } else {
      setNewUser((prev) => ({
        ...prev,
        [name]: name === "userId" ? Number(value) : value,
      }));
    }
  };

  const handleAdd = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await addUser({ ...newUser, student: undefined });
      await addStudent({
        studentId: newUser.userId,
        ...newStudent,
        birthDate: newStudent.birthDate ? new Date(newStudent.birthDate).toISOString() : "",
      });
      showMessage("Student added successfully", "success");
      setNewStudentVisible(false);
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
      setBirthDateInputTypeAdd("text");
      loadStudents();
    } catch {
      showMessage("Error adding student", "error");
    }
  };

  const handleEditClick = (student: User) => {
    setEditingUserId(student.userId);
    setEditUser(student);
    setEditStudent(student.student || { gender: "M", age: 0, birthDate: "" });
  };

  const handleEditChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>,
    target: "user" | "student"
  ) => {
    const { name, value } = e.target;

    if (target === "user" && editUser) {
      setEditUser({ ...editUser, [name]: name === "userId" ? Number(value) : value });
    }

    if (target === "student" && editStudent) {
      let newValue: string | number = value;
      if (name === "age") newValue = Number(value);

      let updatedStudent = {
        ...editStudent,
        [name]: newValue,
      };

      // עדכון גיל אוטומטי בעריכה לפי שינוי תאריך לידה
      if (name === "birthDate") {
        updatedStudent.age = calculateAge(value);
      }

      setEditStudent(updatedStudent);
    }
  };

  const handleSaveEdit = async () => {
    if (!editingUserId || !editUser || !editStudent) return;

    try {
      const userPatch = [
        { op: "replace", path: "/firstName", value: editUser.firstName },
        { op: "replace", path: "/lastName", value: editUser.lastName },
        { op: "replace", path: "/email", value: editUser.email },
        { op: "replace", path: "/phone", value: editUser.phone },
      ];
      const studentPatch = [
        { op: "replace", path: "/age", value: editStudent.age },

      ];
      await updateUser(editingUserId, userPatch);
      await updateStudent(editingUserId, studentPatch);
      showMessage("Student updated successfully", "success");
      setEditingUserId(null);
      loadStudents();
    } catch {
      showMessage("Failed to update student", "error");
    }
  };

  const handleDelete = async (userId: number) => {
    if (!window.confirm("Are you sure you want to delete this student?")) return;
    try {
      await deleteUser(userId);
      showMessage("Student deleted successfully", "success");
      loadStudents();
    } catch {
      showMessage("Failed to delete student", "error");
    }
  };

  return (
    <div>
      {toast && <Toast type={toast.type} message={toast.message} />}

      <div className={styles.container}>
        <h2 className={styles.title}>Manage Students</h2>

        <button className={styles.addBtn} onClick={() => setNewStudentVisible((prev) => !prev)}>
          <FaUserPlus /> {newStudentVisible ? "Cancel" : " Add New Student"}
        </button>

        {newStudentVisible && (
          <div className={styles.modalOverlay}>
            <div className={styles.modalBox}>
              <h3>Add New Student</h3>
              <form onSubmit={handleAdd} className={styles.form}>
                <input
                  type="number"
                  name="userId"
                  placeholder="Student ID"
                  value={newUser.userId || ""}
                  onChange={handleAddChange}
                  className={styles.singleInput}
                  required
                />
                <div className={styles.twoColumnForm}>
                  <input
                    name="firstName"
                    placeholder="First Name"
                    value={newUser.firstName}
                    onChange={handleAddChange}
                    required
                  />
                  <input
                    name="lastName"
                    placeholder="Last Name"
                    value={newUser.lastName}
                    onChange={handleAddChange}
                    required
                  />
                  <input
                    name="phone"
                    placeholder="Phone"
                    value={newUser.phone}
                    onChange={handleAddChange}
                    required
                  />
                  <input
                    name="email"
                    placeholder="Email"
                    value={newUser.email}
                    onChange={handleAddChange}
                    required
                  />
                  <input
                    name="password"
                    type="password"
                    placeholder="Password"
                    value={newUser.password}
                    onChange={handleAddChange}
                    required
                  />
                  <select name="gender" value={newStudent.gender} onChange={handleAddChange} required>
                    <option value="" disabled>
                      Select Gender
                    </option>
                    <option value="M">Male</option>
                    <option value="F">Female</option>
                  </select>
                  <input
                    name="birthDate"
                    type={birthDateInputTypeAdd}
                    placeholder="Birth Date"
                    value={newStudent.birthDate}
                    onFocus={() => setBirthDateInputTypeAdd("date")}
                    onBlur={() => {
                      if (!newStudent.birthDate) setBirthDateInputTypeAdd("text");
                    }}
                    onChange={handleAddChange}
                    required
                  />
                  <input
                    name="age"
                    type="number"
                    placeholder="Age"
                    value={newStudent.age || ""}
                    disabled
                  />
                </div>
                <div className={styles.actions}>
                  <button type="submit">Save</button>
                  <button type="button" onClick={() => setNewStudentVisible(false)}>
                    Cancel
                  </button>
                </div>
              </form>
            </div>
          </div>
        )}

        <div className={styles.cardGrid}>
          {students.map((student) => {
            const isExpanded = expandedStudentId === student.userId;
            if (editingUserId === student.userId && editUser && editStudent) {
              return (
                <div
                  key={student.userId}
                  className={`${styles.card} ${isExpanded ? styles.expanded : ""}`}
                >
                  <input
                    name="firstName"
                    value={editUser.firstName || ""}
                    onChange={(e) => handleEditChange(e, "user")}
                  />
                  <input
                    name="lastName"
                    value={editUser.lastName || ""}
                    onChange={(e) => handleEditChange(e, "user")}
                  />
                  <input
                    name="email"
                    value={editUser.email || ""}
                    onChange={(e) => handleEditChange(e, "user")}
                  />
                  <input
                    name="phone"
                    value={editUser.phone || ""}
                    onChange={(e) => handleEditChange(e, "user")}
                  />
        
                  <div className={styles.actions}>
                    <button onClick={handleSaveEdit} title="Save">
                      <FaSave />
                    </button>
                    <button onClick={() => setEditingUserId(null)} title="Cancel">
                      <FaTimes />
                    </button>
                    <button onClick={() => toggleDetails(student.userId)}>
                      {isExpanded ? "Collapse" : "Details"}
                    </button>
                  </div>
                </div>
              );
            }

            return (
              <div key={student.userId} className={`${styles.card} ${isExpanded ? styles.expanded : ""}`}>
                <div className="content">
                  <div className={styles.icon}>
                    <FiUser />
                  </div>
                  <h4>
                    {student.firstName} {student.lastName}
                  </h4>
                </div>
                <div className={styles.actions}>
                  <button onClick={() => handleEditClick(student)} title="Edit">
                    <FaEdit />
                  </button>
                  <button onClick={() => handleDelete(student.userId)} title="Delete">
                    <FaTrash />
                  </button>
                  <button onClick={() => toggleDetails(student.userId)}>
                    {isExpanded ? "Collapse" : "Details"}
                  </button>
                </div>
                {isExpanded && (
                  <>
                    <p>
                      <b>ID:</b> {student.userId}
                    </p>
                    <p>
                      <b>Email:</b> {student.email}
                    </p>
                    <p>
                      <b>Phone:</b> {student.phone}
                    </p>
                    <p>
                      <b>Gender:</b> {student.student?.gender === "M" ? "Male" : "Female"}
                    </p>
                    <p>
                      <b>Age:</b> {student.student?.age}
                    </p>
                    <p>
                      <b>Birth Date:</b> {formatDate(student.student?.birthDate)}
                    </p>
                  </>
                )}
              </div>
            );
          })}
        </div>
      </div>
    </div>
  );
};

export default ManageStudents;
