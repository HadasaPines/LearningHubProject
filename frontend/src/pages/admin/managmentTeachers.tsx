import React, { useState, useEffect } from "react";
import type { User } from "../../models/userModel";
import { addUser, getAllTeachers, addTeacher } from "../../services/api";
import { parseApiError } from "../../utils/apiErrorParser";

const ManageTeachers = () => {
  const [teachers, setTeachers] = useState<User[]>([]);
  const [newTeacher, setNewTeacher] = useState<Omit<User, "teacherId">>({
    userId: 0,
    firstName: "",
    lastName: "",
    password: "",
    phone: "",
    email: "",
    role: "Teacher",
    teacher: {
      gender: "M",
      bio: "",
      birthDate: "",
    },
  });

  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);

  useEffect(() => {
    const fetchTeachers = async () => {
      try {
        const res = await getAllTeachers();
        setTeachers(res.data);
      } catch (error) {
        setErrorMessage(parseApiError(error));
      }
    };
    fetchTeachers();
  }, []);


    setTimeout(() => {
      setErrorMessage(null);
      setSuccessMessage(null);
    }, 4000);
  

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>
  ) => {
    const { name, value } = e.target;

    if (name in newTeacher) {
      setNewTeacher((prev) => ({
        ...prev,
        [name]: name === "userId" ? parseInt(value) : value,
      }));
    } else {

      setNewTeacher((prev) => ({
        ...prev,
        teacher: {
          ...prev.teacher!,
          [name]: value,
        },
      }));
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {

      await addUser(newTeacher);

      await addTeacher({
        teacherId: newTeacher.userId,
        gender: newTeacher.teacher?.gender || "M",
        bio: newTeacher.teacher?.bio || "",
        birthDate: newTeacher.teacher?.birthDate || "",
      });

      setSuccessMessage("המורה נוסף בהצלחה ✅");

      setNewTeacher({
        userId: 0,
        firstName: "",
        lastName: "",
        password: "",
        phone: "",
        email: "",
        role: "Teacher",
        teacher: {
          gender: "M",
          bio: "",
          birthDate: "",
        },
      });


      const updated = await getAllTeachers();
      setTeachers(updated.data);
    } catch (error) {
      setErrorMessage(parseApiError(error));
    }
  };

  return (
    <div>
      <h2>ניהול מורים</h2>

      {errorMessage && <div style={{ color: "red" }}>{errorMessage}</div>}
      {successMessage && <div style={{ color: "green" }}>{successMessage}</div>}

      <form onSubmit={handleSubmit}>
        <h3>הוספת מורה חדש</h3>
        <input
          type="number"
          name="userId"
          placeholder="תעודת זהות"
          value={newTeacher.userId}
          required
          onChange={handleChange}
        />
        <input
          name="firstName"
          placeholder="שם פרטי"
          value={newTeacher.firstName}
          onChange={handleChange}
        />
        <input
          name="lastName"
          placeholder="שם משפחה"
          value={newTeacher.lastName}
          onChange={handleChange}
        />
        <input
          name="phone"
          placeholder="טלפון"
          value={newTeacher.phone}
          onChange={handleChange}
        />
        <input
          name="email"
          placeholder="אימייל"
          value={newTeacher.email}
          onChange={handleChange}
        />
        <input
          name="password"
          type="password"
          placeholder="סיסמה"
          value={newTeacher.password}
          onChange={handleChange}
        />
        <select
          name="gender"
          value={newTeacher.teacher?.gender || "M"}
          onChange={handleChange}
        >
          <option value="M">זכר</option>
          <option value="F">נקבה</option>
        </select>
        <input
          name="birthDate"
          type="date"
          value={newTeacher.teacher?.birthDate || ""}
          onChange={handleChange}
        />
        <textarea
          name="bio"
          placeholder="ביוגרפיה"
          value={newTeacher.teacher?.bio || ""}
          onChange={handleChange}
        />
        <button type="submit">הוסף מורה</button>
      </form>

      <hr />

      <h3>רשימת מורים</h3>
      <ul>
        {teachers.map((t) => (
          <li key={t.userId}>
            {t.firstName} {t.lastName} ({t.email})
          </li>
        ))}
      </ul>
    </div>
  );
};

export default ManageTeachers;
