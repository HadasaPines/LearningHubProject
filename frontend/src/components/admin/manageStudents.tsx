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
import type { User, StudentDetails, Gender } from "../../models/userModel";

import { parseApiError } from "../../utils/apiErrorParser";

const ManageStudents = () => {
  const [students, setStudents] = useState<(User)[]>([]);
  const [newStudent, setNewStudent] = useState<Omit<StudentDetails, "studentId">>({
    gender: "M",
    age: 0,
    birthDate: "",
  });
  const [newUser, setNewUser] = useState<User>({
    userId: 0,
    firstName: "",
    lastName: "",
    email: "",
    phone: "",
    password: "",
    role: "Student",
    student: newStudent


  });

  const [editingUserId, setEditingUserId] = useState<number | null>(null);
  const [editUser, setEditUser] = useState<Partial<User>>({});

  const [editStudent, setEditStudent] = useState<Partial<StudentDetails>>({});
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
      const studentUsers = userRes.data.filter((u: User) => u.role === "Student");
      const fullList = studentUsers.map((user: User) => {

        const student = studentRes.data.find((s: StudentDetails) => s.studentId === user.userId);
        return {
          ...user,
          ...student,
        };
      });
      setStudents(fullList);
    } catch (error: any) {
      showMessage(extractErrorMessage(error, "Error loading student list"), "error");
    }
  };

  const handleAdd = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await addUser(newUser);
      await addStudent({
        studentId: newUser.userId,
        ...newStudent,
        birthDate: newStudent.birthDate ? new Date(newStudent.birthDate).toISOString() : "",
      });
      resetForm();
      loadStudents();
      showMessage("Student added successfully", "success");
    } catch (error: any) {
      showMessage(parseApiError(error), "error");
    }
  };

  const handleDelete = async (id: number) => {
    if (!window.confirm("Are you sure you want to delete this student?")) return;
    try {
      await deleteUser(id);
      loadStudents();
      showMessage("Student deleted successfully", "success");
    } catch (error: any) {
      showMessage(parseApiError(error), "error");
    }
  };

  const handleEditClick = (student: User) => {

    setEditingUserId(student.userId);
    setEditUser({ ...student });
    setEditStudent({
      gender: student.student?.gender,
      age: student.student?.age,
      birthDate: student.student?.birthDate,
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
      showMessage("Student updated successfully", "success");
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
      student: {
        gender: "M",
        age: 0,
        birthDate: ""
      }
    });

  };

  return (
    <div>
      <h2>Student Management</h2>

      {errorMessage && <div style={{ color: "red", marginBottom: 10 }}>{errorMessage}</div>}
      {successMessage && <div style={{ color: "green", marginBottom: 10 }}>{successMessage}</div>}

      <form onSubmit={handleAdd}>
        <h3>Add New Student</h3>
        <input
          type="number"
          name="userId"
          placeholder="ID"
          value={newUser.userId}
          required
          onChange={(e) => setNewUser({ ...newUser, userId: parseInt(e.target.value) })}
        />
        <input
          name="firstName"
          placeholder="First Name"
          value={newUser.firstName}
          required
          onChange={(e) => setNewUser({ ...newUser, firstName: e.target.value })}
        />
        <input
          name="lastName"
          placeholder="Last Name"
          value={newUser.lastName}
          required
          onChange={(e) => setNewUser({ ...newUser, lastName: e.target.value })}
        />
        <input
          name="email"
          placeholder="Email"
          value={newUser.email}
          onChange={(e) => setNewUser({ ...newUser, email: e.target.value })}
        />
        <input
          name="phone"
          placeholder="Phone"
          value={newUser.phone}
          onChange={(e) => setNewUser({ ...newUser, phone: e.target.value })}
        />
        <input
          name="password"
          placeholder="Password"
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
          <option value="M">Male</option>
          <option value="F">Female</option>
        </select>
        <input
          type="number"
          name="age"
          placeholder="Age"
          value={newStudent.age}
          onChange={(e) => setNewStudent({ ...newStudent, age: parseInt(e.target.value) })}
        />
        <button type="submit">Add</button>
      </form>

      <hr />

      <h3>Student List</h3>
      <table cellPadding="8">
        <thead>
          <tr>
            <th>ID</th>
            <th>First Name</th>
            <th>Last Name</th>
            <th>Phone</th>
            <th>Email</th>
            <th>Gender</th>
            <th>Age</th>
            <th>Birth Date</th>
            <th>Actions</th>
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
                    <option value="M">Male</option>
                    <option value="F">Female</option>
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
                <td>{s.student?.birthDate?.substring(0, 10)}</td>
                <td>
                  <button onClick={handleSaveEdit} title="Save">
                    💾
                  </button>
                  <button onClick={() => setEditingUserId(null)} title="Cancel">
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
                <td>{s.student?.gender}</td>
                <td>{s.student?.age}</td>
                <td>{s.student?.birthDate?.substring(0, 10)}</td>
                <td>
                  <button onClick={() => handleEditClick(s)} title="Edit">
                    ✏️
                  </button>
                  <button onClick={() => handleDelete(s.userId)} title="Delete">
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